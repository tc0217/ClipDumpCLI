using System;
using System.CommandLine;
using System.CommandLine.DragonFruit;
using System.CommandLine.Invocation;
using System.IO;
using System.Windows;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace clipdump
{
    static class dumpCLI
    {
        ///<summary>simple CLI tool which allows you to pass your clipboard directly to a file from the terminal</summary>
        ///<param name="fileOut">the name + extension of the file to save clipboard to</param>
        public static void Main(FileInfo fileOut = null)
        {
#if DEBUG
            //fileOut = new FileInfo("testvows.txt");
#endif
            var TextList = new List<string>();
            Thread t = new Thread(() => TextList = dumpCLI.ClipHandler());
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            //if (!fileOut.Exists)
            //{
            //    fileOut.Create();
            //}
            var curDir = Environment.CurrentDirectory;
            var fullPath = curDir + fileOut.Name;
            Console.WriteLine($"Writing to {fileOut.FullName}");
            t.Join();
            using (StreamWriter sw = new StreamWriter(fileOut.FullName))
            {
                foreach (var l in TextList)
                {
                    sw.WriteLine(l);
                }
            }

        }
        [STAThread]
        public static List<string> ClipHandler()
        {
            var result = new List<string>();
            var text = string.Empty;
            if (!Clipboard.ContainsText())
            {
                return result;
            }
            text = Clipboard.GetText();
            if (text.Length > 1)
            {
                //text.Replace("\r\n", "\r");
                //text.Replace("\n", "\r");
                string[] asArray = text.Split(System.Environment.NewLine);
                foreach (var l in asArray)
                {
                    result.Add(l);
                }
                return result;
            }

            result.Add(text);
            return result;
        }
    }
}
