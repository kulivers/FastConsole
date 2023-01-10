using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FastScratchMVC
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider( System.Text.CodePagesEncodingProvider.Instance);
            var encodingInfos = Encoding.GetEncodings();
        }
    }
}