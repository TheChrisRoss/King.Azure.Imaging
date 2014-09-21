﻿namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using King.Service;
    using King.Service.Data;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Image Task Factory
    /// </summary>
    public class ImageTaskFactory : ITaskFactory<object>
    {
        #region Methods
        /// <summary>
        /// Connection String
        /// </summary>
        protected readonly string connectionString = null;

        /// <summary>
        /// Storage Elements
        /// </summary>
        protected readonly IStorageElements elements = null;

        /// <summary>
        /// Versions
        /// </summary>
        protected readonly IVersions versions = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public ImageTaskFactory(string connectionString)
            : this(connectionString, new Versions(), new StorageElements())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageTaskFactory(string connectionString, IVersions versions)
            : this(connectionString, versions, new StorageElements())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImageTaskFactory(string connectionString, IVersions versions, IStorageElements elements)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }
            if (null == versions)
            {
                throw new ArgumentNullException("versions");
            }
            if (null == elements)
            {
                throw new ArgumentNullException("elements");
            }

            this.connectionString = connectionString;
            this.versions = versions;
            this.elements = elements;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load Tasks
        /// </summary>
        /// <param name="passthrough">passthrough</param>
        /// <returns>Runnable Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks(object passthrough)
        {
            var tasks = new List<IRunnable>();

            //Storage
            var container = new Container(elements.Container, connectionString, true);
            var table = new TableStorage(elements.Table, connectionString);
            var queue = new StorageQueue(elements.Queue, connectionString);

            //Initialization Tasks
            tasks.Add(new InitializeStorageTask(container));
            tasks.Add(new InitializeStorageTask(table));
            tasks.Add(new InitializeStorageTask(queue));

            //Queue Poller
            var poller = new StorageQueuePoller<ImageQueued>(queue);
            //Image Processor
            var processor = new Processor(new DataStore(connectionString), this.versions.Images);
            //Image Processing Task
            tasks.Add(new BackoffRunner(new DequeueBatch<ImageQueued>(poller, processor)));

            return tasks;
        }
        #endregion
    }
}