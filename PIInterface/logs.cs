using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PIInterface
{
     public class logs
    {
         public static void writelog(string e2)
         {
             string filename = "数据操作日志";
             string files = @"数据操作日志\";
             string txtname = "数据操作" + DateTime.Now.ToString("yyyyMMdd") + ".log";
             FileInfo file = new FileInfo(System.AppDomain.CurrentDomain.BaseDirectory + files + txtname);
             StreamWriter sw = null;
             if (!Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + filename))
             {
                 Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + filename);
                 if (!file.Exists)
                 {
                     sw = file.CreateText();
                     sw.WriteLine("============================================================================");
                     sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":");
                     sw.WriteLine(e2.ToString());
                 }
                 else
                 {
                     sw = file.AppendText();
                     sw.WriteLine("============================================================================");
                     sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":");
                     sw.WriteLine(e2.ToString());

                 }
             }
             else
             {
                 if (!file.Exists)
                 {
                     sw = file.CreateText();
                     sw.WriteLine("============================================================================");
                     sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":");
                     sw.WriteLine(e2.ToString());
                 }
                 else
                 {
                     sw = file.AppendText();
                     sw.WriteLine("============================================================================");
                     sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":");
                     sw.WriteLine(e2.ToString());

                 }
             }
             sw.Close();

         }
    }
}
