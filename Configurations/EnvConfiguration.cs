using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace PBGame.Configurations
{
    public class EnvConfiguration : IEnvConfiguration
    {

        public bool IsDevelopment { get; private set; }

        public bool IsLoaded => Variables != null;

        public EnvVariables Variables { get; private set;}


        public EnvConfiguration(bool isDevelopment)
        {
            this.IsDevelopment = isDevelopment;
        }

        public void Load(string path)
        {
            var textAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
            Variables = JsonConvert.DeserializeObject<EnvVariables>(textAsset.text);
            Resources.UnloadAsset(textAsset);
        }
    }
}