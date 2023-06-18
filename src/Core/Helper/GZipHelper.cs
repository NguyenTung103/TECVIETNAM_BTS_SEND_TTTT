using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Core
{
    public interface IZipHelper
    {
        /// <summary>
        /// Zip a byte array data
        /// </summary>
        /// <param name="data">data to zio</param>
        /// <returns>byte array</returns>
        byte[] ZipByte(byte[] data);

        /// <summary>
        /// UnZip a byte array data
        /// </summary>
        /// <param name="data">data to unzio</param>
        /// <returns>byte array</returns>
        byte[] UnZipByte(byte[] data);

        byte[] Zip(string str);

        string Unzip(byte[] bytes);
    }
    public class GZipHelper : IZipHelper
    {
        public byte[] ZipByte(byte[] zipData)
        {
            using var inputStream = new MemoryStream(zipData);
            using var outputStream = new MemoryStream();
            using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
            {
                CopyTo(inputStream, gzipStream);
            }
            return outputStream.ToArray();
        }

        public byte[] UnZipByte(byte[] unZipData)
        {
            using var inputStream = new MemoryStream(unZipData);
            using var outputStream = new MemoryStream();
            using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            {
                CopyTo(gzipStream, outputStream);
            }
            return outputStream.ToArray();
        }

        public byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        private void CopyTo(Stream sourceStream, Stream destinationStream)
        {
            byte[] bytes = new byte[4096];
            int cnt;
            while ((cnt = sourceStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                destinationStream.Write(bytes, 0, cnt);
            }
        }
    }
}
