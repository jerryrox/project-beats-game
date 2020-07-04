using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace PBGame.Configurations
{
    public class EnvConfiguration : IEnvConfiguration
    {
        public EnvType EnvironmentType { get; private set; }

        public bool IsDevelopment => EnvironmentType != EnvType.Production;

        public bool IsLoaded => Variables != null;

        public EnvVariables Variables { get; private set;}


        public EnvConfiguration(EnvType envType)
        {
            this.EnvironmentType = envType;
        }

        public void Load(string path)
        {
            if(string.IsNullOrEmpty(path))
                path = "";
            switch (EnvironmentType)
            {
                case EnvType.Development:
                    path += "Dev";
                    break;
                case EnvType.LocalDevelopment:
                    path += "Local";
                    break;
            }

            var textAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
            Variables = JsonConvert.DeserializeObject<EnvVariables>(textAsset.text);
            Resources.UnloadAsset(textAsset);
        }
    }
}