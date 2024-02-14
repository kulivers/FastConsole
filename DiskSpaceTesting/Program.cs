using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace DiskSpaceTesting
{
    internal class Program
    {
        private static long CalculateDirSize(DirectoryInfo d)
        {
            // Add file sizes.
            var fileInfos = d.GetFiles();
            var size = fileInfos.Sum(fi => fi.Length);
            // Add subdirectory sizes.
            var dis = d.GetDirectories();
            size += dis.Sum(CalculateDirSize);
            return size;
        }

        private const string DecimalWithSeparator = "#,##0.";
        private const string DecimalWithoutSeparator = "0.";
        private const string IntWithSeparator = "#,##0";
        private const string IntWithoutSeparator = "0";
        private const char ZeroNumber = '0';

        public static string ResolveNumericCustomFormat(int? decimalPlaces, bool isDigitGrouping)
        {
            if (decimalPlaces == 0)
            {
                return isDigitGrouping ? IntWithSeparator : IntWithoutSeparator;
            }

            return $"{(isDigitGrouping ? DecimalWithSeparator : DecimalWithoutSeparator)}{new string(ZeroNumber, (int)decimalPlaces)}";
        }

        public static void Main(string[] args)
        {
            var maxValue = long.MaxValue / 1024d / 1024d / 1024d;
        }

        public static int FindLast(byte[] haystack, byte[] needle)
        {
            // iterate backwards, stop if the rest of the array is shorter than needle (i >= needle.Length)
            for (var i = haystack.Length - 1; i >= needle.Length - 1; i--)
            {
                var found = true;
                // also iterate backwards through needle, stop if elements do not match (!found)
                for (var j = needle.Length - 1; j >= 0 && found; j--)
                {
                    // compare needle's element with corresponding element of haystack
                    found = haystack[i - (needle.Length - 1 - j)] == needle[j];
                }

                if (found)
                    // result was found, i is now the index of the last found element, so subtract needle's length - 1
                    return i - (needle.Length - 1);
            }

            // not found, return -1
            return -1;
        }

        public static int FindLast(int[] haystack, int[] needle)
        {
            // iterate backwards, stop if the rest of the array is shorter than needle (i >= needle.Length)
            for (var i = haystack.Length - 1; i >= needle.Length - 1; i--)
            {
                var found = true;
                // also iterate backwards through needle, stop if elements do not match (!found)
                for (var j = needle.Length - 1; j >= 0 && found; j--)
                {
                    // compare needle's element with corresponding element of haystack
                    found = haystack[i - (needle.Length - 1 - j)] == needle[j];
                }

                if (found)
                    // result was found, i is now the index of the last found element, so subtract needle's length - 1
                    return i - (needle.Length - 1);
            }

            // not found, return -1
            return -1;
        }

        public void ExtractFile(string sourceArchive, string destination)
        {
            string zPath = "7za.exe"; //add to proj and set CopyToOuputDir
            try
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.FileName = zPath;
                pro.Arguments = string.Format("x \"{0}\" -y -o\"{1}\"", sourceArchive, destination);
                Process x = Process.Start(pro);
                x.WaitForExit();
            }
            catch (System.Exception Ex)
            {
                //handle error
            }
        }
    }
}