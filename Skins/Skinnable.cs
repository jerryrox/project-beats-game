using System.IO;

namespace PBGame.Skins
{
    public abstract class Skinnable<T> : ISkinnable<T>
    {

        public T Element { get; set; }

        public bool IsDefaultAsset { get; set; }

        public string LookupName { get; set; }

        public FileInfo File { get; set; }


        public abstract void Dispose();
    }
}