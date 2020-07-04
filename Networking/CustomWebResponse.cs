using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Networking;
using UnityEngine;

namespace PBGame.Networking
{
    /// <summary>
    /// A response object which simply allows getting/setting of response data freely.
    /// </summary>
    public class CustomWebResponse : IWebResponse {

        public long Code { get; set; }

		public bool IsSuccess { get; set; }

		public string ErrorMessage { get; set; }

		public string TextData { get; set; }

		public byte[] ByteData { get; set; }

		public AudioClip AudioData { get; set; }

		public AssetBundle AssetBundleData { get; set; }

		public Texture2D TextureData { get; set; }

		public Dictionary<string, string> Headers { get; set; }

		public string ContentType { get; set; }

		public long ContentLength { get; set; }

		public ulong BytesLoaded { get; set; }


        public void Dispose() {}
    }
}