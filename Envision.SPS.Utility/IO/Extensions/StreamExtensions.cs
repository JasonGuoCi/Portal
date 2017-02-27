using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.IO.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] GetAllBytes(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int sz = stream.Read(buffer, 0, 1024);
                    if (sz == 0) break;
                    memoryStream.Write(buffer, 0, sz);
                }
                memoryStream.Position = 0;

                return memoryStream.ToArray();
            }
        }
    }
}
