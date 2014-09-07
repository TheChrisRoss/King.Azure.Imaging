﻿namespace King.Azure.Imaging
{
    using ImageResizer;
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Imaging Task
    /// </summary>
    public class ImagingProcessor : IProcessor<ImageQueued>
    {
        #region Members
        /// <summary>
        /// 
        /// </summary>
        private readonly IDictionary<string, string> versions;

        /// <summary>
        /// 
        /// </summary>
        private readonly IContainer container;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITableStorage table;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="versions"></param>
        public ImagingProcessor(IContainer container, IDictionary<string, string> versions)
        {
            this.container = container;
            this.versions = versions;
        }
        #endregion

        #region Methods
        public async Task<bool> Process(ImageQueued data)
        {
            var result = false;
            var fileName = string.Format(data.FileNameFormat, ImagePreprocessor.Original);

            try
            {
                var bytes = await container.Get(fileName);
                foreach (var key in this.versions.Keys)
                {
                    using (var input = new MemoryStream(bytes))
                    {
                        using (var output = new MemoryStream())
                        {
                            var job = new ImageJob(input, output, new Instructions(versions[key]));
                            job.Build();

                            var filename = string.Format(data.FileNameFormat, key.ToLowerInvariant());
                            await container.Save(filename, output.ToArray(), job.ResultMimeType);
                        }
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return result;
        }
        #endregion
    }
}