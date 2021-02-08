using System.IO;

namespace PBGame.IO
{
    public interface IStreamableData {

        /// <summary>
        /// Writes the stream data of this object to the specified writer.
        /// </summary>
        void WriteStreamData(BinaryWriter writer);

        /// <summary>
        /// Modifies the state based on the specified reader's data given during write operation.
        /// </summary>
        void ReadStreamData(BinaryReader reader);
    }
}