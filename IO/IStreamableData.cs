namespace PBGame.IO
{
    public interface IStreamableData {

        /// <summary>
        /// Returns the data in a form which can be saved using the DataStreamWriter.
        /// </summary>
        string ToStreamData();

        /// <summary>
        /// Modifies the state based on the specified data given during write operation.
        /// </summary>
        void FromStreamData(string data);
    }
}