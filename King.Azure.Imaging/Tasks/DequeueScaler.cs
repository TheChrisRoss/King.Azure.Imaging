﻿namespace King.Azure.Imaging.Tasks
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using King.Service;
    using King.Service.Data;
    using System.Collections.Generic;

    /// <summary>
    /// Dequeue Auto Scaler
    /// </summary>
    public class DequeueScaler : AutoScaler<ITaskConfiguration>
    {
        #region Constructors
        public DequeueScaler(ITaskConfiguration configuration)
            : base(configuration)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Scale Unit (Dequeue)
        /// </summary>
        /// <param name="config">Configruation</param>
        /// <returns>Scalable Tasks</returns>
        public override IEnumerable<IScalable> ScaleUnit(ITaskConfiguration config)
        {
            var elements = config.StorageElements;
            var queue = new StorageQueue(elements.Queue, config.ConnectionString);

            //Queue Poller
            var poller = new StorageQueuePoller<ImageQueued>(queue);
            //Image Processor
            var processor = new Processor(new DataStore(config.ConnectionString), config.Versions.Images);
            //Image Processing Task
            yield return new BackoffRunner(new DequeueBatch<ImageQueued>(poller, processor));
        }
        #endregion
    }
}