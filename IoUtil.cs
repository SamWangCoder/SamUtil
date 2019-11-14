using System;
using System.IO;

namespace SamUtil
{
    /// <summary>
    /// IO实用工具
    /// </summary>
    public class IoUtil
    {
        /// <summary>
        /// 将流转为byte数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        /// <summary>
        /// 将byte数组转为流
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        /// <summary>
        /// 写入流到指定文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static bool Save2File(Stream stream, string strPath)
        {
            try
            {
                FileStream fs = new FileStream(strPath, FileMode.Create);
                byte[] bytes = StreamToBytes(stream);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                stream.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
