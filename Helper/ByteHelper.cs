using System.IO;

namespace Swagger2Docx.Helper
{
    public class ByteHelper
    {
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 設置當前 Stream 的位置為 Stream 的開始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// 將 byte[] 轉成 Stream
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
