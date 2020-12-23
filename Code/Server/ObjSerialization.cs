using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Server
{
    public class ObjSerialization
    {
        // Code of these conversions on GitHub : https://gist.github.com/LorenzGit/2cd665b6b588a8bb75c1a53f4d6b240a

        // Convert a byte array to an object
        public object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
        // Convert an object to a byte array (to perform the displays of messages)
        public byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
