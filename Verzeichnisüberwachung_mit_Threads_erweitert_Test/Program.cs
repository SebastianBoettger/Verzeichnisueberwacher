using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Verzeichnisüberwachung_mit_Threads_erweitert_Test
{
    class Work
    {
        public static void Main()
        {
            // Überwachung erstes Verzeichnis
            Thread t1 = new Thread(Work.DoWork);
            t1.Start();

            Work w1 = new Work();
            t1 = new Thread(w1.DoMoreWork);
            t1.Start(@"J:\2021\Projects\Verzeichnisüberwachung_mit_Threads_erweitert_Test\Ueberwachtes_Verzeichnis&" + 
                        @"J:\2021\Projects\Verzeichnisüberwachung_mit_Threads_erweitert_Test\Verzeichnis_fuer_txts&" + 
                        @"J:\2021\Projects\Verzeichnisüberwachung_mit_Threads_erweitert_Test\Verzeichnis_fuer_pdfs");

            // Überwachung zweites Verzeichnis
            Thread t2 = new Thread(Work.DoWork);
            t2.Start();

            Work w2 = new Work();
            t2 = new Thread(w2.DoMoreWork);
            t2.Start(@"J:\2021\Projects\Verzeichnisüberwachung_mit_Threads_erweitert_Test\zweites_Ueberwachtes_Verzeichnis&" +
                        @"J:\2021\Projects\Verzeichnisüberwachung_mit_Threads_erweitert_Test\zweites_Verzeichnis_fuer_txts&" +
                        @"J:\2021\Projects\Verzeichnisüberwachung_mit_Threads_erweitert_Test\zweites_Verzeichnis_fuer_pdfs");
        }

        public static void DoWork(object data)
        {
        }

        public void DoMoreWork(object data)
        {
            while (true)
            {
                string[] sDreiStrings = (data.ToString()).Split('&');
                string[] sDateienFullPaths = Directory.GetFiles(sDreiStrings[0]);

                for (int i = 0; i < sDateienFullPaths.Length; i++)
                {
                    if (sDateienFullPaths[i].EndsWith(".txt") == true)
                    {
                        string filePath = Path.GetDirectoryName(sDateienFullPaths[i]);
                        string fileName = Path.GetFileName(sDateienFullPaths[i]);
                        string targetPath = sDreiStrings[1];
                        shift(filePath, fileName, targetPath, data);
                    }
                    else if (sDateienFullPaths[i].EndsWith(".pdf") == true)
                    {
                        string filePath = Path.GetDirectoryName(sDateienFullPaths[i]);
                        string fileName = Path.GetFileName(sDateienFullPaths[i]);
                        string targetPath = sDreiStrings[2];
                        shift(filePath, fileName, targetPath, data);
                    }
                }
            }
        }

        static private void shift(string filePath, string fileName, string targetPath, object data)
        {
            string sourceFile = Path.Combine(filePath, fileName);
            string destinationFile = Path.Combine(targetPath, fileName);
            try
            {
                System.IO.File.Move(sourceFile, destinationFile);
            }
            catch
            {
                var obj = new Work();
                obj.DoMoreWork(data);
            }
        }
    }
}