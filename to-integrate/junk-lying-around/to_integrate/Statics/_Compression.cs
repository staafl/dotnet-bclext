
using System.IO.Compression;
using System.Text;
namespace System.IO
{
    public static class CompressionHelper
    {

        public static string Compress(string data) {
            byte[] before = ASCIIEncoding.Unicode.GetBytes(data);
            byte[] compressed = Compress(ASCIIEncoding.Unicode.GetBytes(data));
            string ret = Convert.ToBase64String(compressed);
            return ret;
            //return Convert.ToBase64String(Compress(ASCIIEncoding.Unicode.GetBytes(data)));
        }
        public static string Decompress(string data) {
            byte[] before = Convert.FromBase64String(data);
            byte[] decompressed = Decompress(Convert.FromBase64String(data));
            string ret = ASCIIEncoding.Unicode.GetString(decompressed);
            return ret;
            
            //return ASCIIEncoding.Unicode.GetString(Decompress(Convert.FromBase64String(data)));
        }
        public static MemoryStream Compress(MemoryStream data) {
            MemoryStream output = new MemoryStream();
            using (GZipStream gzip = new GZipStream(output, CompressionMode.Compress, true)) {
                gzip.Write(data.ToArray(), 0, (int)data.Length);
                gzip.Close();
                return output;
            }
        }
        public static MemoryStream Decompress(MemoryStream data) {
            using (MemoryStream input = new MemoryStream()) {
                input.Write(data.ToArray(), 0, (int)data.Length);
                input.Position = 0;
                using (GZipStream gzip = new GZipStream(input, CompressionMode.Decompress, true)) {
                    MemoryStream output = new MemoryStream();
                    byte[] buff = new byte[64];
                    int read = -1;
                    read = gzip.Read(buff, 0, buff.Length);
                    while (read > 0) {
                        output.Write(buff, 0, read);
                        read = gzip.Read(buff, 0, buff.Length);
                    }
                    gzip.Close();
                    return output;
                }
            }
        }
        public static byte[] Compress(byte[] data) {
            using (MemoryStream output = new MemoryStream())
            using (GZipStream gzip = new GZipStream(output, CompressionMode.Compress, true)) {
                gzip.Write(data, 0, data.Length);
                gzip.Close();
                return output.ToArray();
            }
        }
        public static byte[] Decompress(byte[] data) {
            using (MemoryStream input = new MemoryStream()) {
                input.Write(data, 0, data.Length);
                input.Position = 0;
                using (GZipStream gzip = new GZipStream(input, CompressionMode.Decompress, true)) {
                    using (MemoryStream output = new MemoryStream()) {
                        byte[] buff = new byte[64];
                        int read = -1;
                        read = gzip.Read(buff, 0, buff.Length);
                        while (read > 0) {
                            output.Write(buff, 0, read);
                            read = gzip.Read(buff, 0, buff.Length);
                        }
                        gzip.Close();
                        return output.ToArray();
                    }
                }
            }
        }
    }
}
