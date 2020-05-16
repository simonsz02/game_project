using System.IO;

namespace TowerDefenseGame.Repository
{
    /// <summary>
    /// Handles serialization
    /// </summary>
    public static class SerializationAsBinary
    {
        /// <summary>
        /// Exports the model into a file
        /// </summary>
        /// <typeparam name="T">type of model</typeparam>
        /// <param name="filePath">path of file</param>
        /// <param name="objectToWrite">type of model</param>
        /// <param name="append">Append or overwrite</param>
        public static void Export<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
                stream.Close();
            }
        }
        /// <summary>
        /// Imports the model from a binary file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">path of the file</param>
        /// <returns>returns a "model" type result</returns>
        public static T Import<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
