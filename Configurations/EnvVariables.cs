using System;
using System.Collections;
using System.Collections.Generic;

namespace PBGame.Configurations
{
    public class EnvVariables {

        /// <summary>
        /// Whether the environment variables mode is development mode.
        /// </summary>
        public bool IsDevelopment { get; set; }

        /// <summary>
        /// The base url of the PB Api server.
        /// </summary>
        public string BaseApiUrl { get; set; }
    }
}