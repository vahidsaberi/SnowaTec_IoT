using System;
using System.IO;

namespace SnowaTec.Test.Domain.Helper
{
    public class FileHelper
    {
        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                var basePath = Path.GetDirectoryName(fileName);

                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

        public static string MimeType(byte[] file)
        {
            byte[] buffer = new byte[256];
            var length = (file.Length > 256) ? 256 : file.Length;
            Array.Copy(file, buffer, length);
            string result = System.Text.Encoding.UTF8.GetString(buffer);
            if (result.Contains("<svg"))
            {
                return "image/svg+xml";
            }
            return "application/octet-stream";
        }
    }
}
