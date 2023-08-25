using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OTLWizard.Helpers
{
    public static class SDFHandler
    {


        public static bool checkDependencies()
        {
            string application = Settings.Get("sdfpath");

            if (File.Exists(application))
            {
                return true;
            }
            else
            {

                return false;
            }


        }

        public static string loadDataForClass(string otlname, string path)
        {
            string application = Settings.Get("sdfpath");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = application;
            startInfo.Arguments = "query-features --class " + otlname + " --from-file \"" + path + "\" --format CSV > ";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }


        public static string GetSchemaName(string path)
        {
            string application = Settings.Get("sdfpath");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = application;
            startInfo.Arguments = "list-schemas --from-file \"" + path + "\"";
            System.Console.WriteLine(startInfo.Arguments);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        public static string DumpSchemaFromFile(string name, string path, string schemapath)
        {
            string application = Settings.Get("sdfpath");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = application;
            startInfo.Arguments = "dump-schema --from-file \"" + path + "\" --schema " + name + " --schema-path \"" + schemapath + "\"";
            System.Console.WriteLine(startInfo.Arguments);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        public static string CreateNewFile(string path, string schemapath)
        {
            string application = Settings.Get("sdfpath");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = application;
            startInfo.Arguments = "create-file --file \"" + path + "\" --schema-path \"" + schemapath + "\"";
            System.Console.WriteLine(startInfo.Arguments);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
        public static string CopyClass(string fromFilePath, string toFilePath, string className, string[] assetIds, string fromFileSchemaName, string toFileSchemaName)
        {
            // assetId_identificator LIKE '%541d2271-%' OR assetId_identificator LIKE '%99e7b%'
            var filtering = "";

            foreach (string assetId in assetIds)
            {
                filtering += "assetId_identificator LIKE '" + assetId + "' OR ";
            }

            filtering = filtering.Substring(0, filtering.Length - 4);
            

            string application = Settings.Get("sdfpath");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = application;
            startInfo.Arguments = "copy-class --dst-class " + className + " --dst-connect-params File \"" + toFilePath + "\" --dst-schema " + toFileSchemaName + " --src-class " + className + " --src-connect-params File \"" + fromFilePath + "\" --src-schema " + fromFileSchemaName + " --dst-provider OSGeo.SDF --src-provider OSGeo.SDF --filter \"" + filtering + "\"";
            System.Console.WriteLine(startInfo.Arguments);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }



        public static List<string> GetClasses(string path)
        {
            string application = Settings.Get("sdfpath");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = application;
            startInfo.Arguments = "list-classes --from-file \"" + path + "\"";
            System.Console.WriteLine(startInfo.Arguments);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd().Replace('\r', ' ').Trim();
            process.WaitForExit();
            string[] listing = output.Split('\n');

            return listing.ToList();
        }

        public static List<string> GenerateCSVExportFiles(string path)
        {
            List<string> files = new List<string>();
            // convert SDF to CSV
            List<string> classnames = SDFHandler.GetClasses(path);

            var localPath = System.IO.Path.GetTempPath() + "otlsdftempconversion\\";
            if(Directory.Exists(localPath))
                Directory.Delete(localPath,true);
            Directory.CreateDirectory(localPath);
                foreach (string classname in classnames)
                {
                    string contents = SDFHandler.loadDataForClass(classname, path);
                    string[] contentArray = contents.Split(new String[] { "\r\n" }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if(contentArray.Length > 1)
                {
                    string header = contentArray[0].Replace('_', '.');
                    contents = header + "\r\n" + contentArray[1];
                } else
                {
                    contents = contents.Replace('_', '.');
                }                   
                    string filename = classname + ".csv";
                    File.WriteAllText(localPath + filename, contents);
                    files.Add(localPath + filename);
                }
            return files;
        }

        public static List<string> GenerateCSVExportFiles(string path, string saveTo)
        {
            List<string> files = new List<string>();
            // convert SDF to CSV
            List<string> classnames = GetClasses(path);

            var localPath = saveTo;
            foreach (string classname in classnames)
            {
                string contents = loadDataForClass(classname, path);
                contents = contents.Replace('_', '.');
                contents = contents.Replace("Geometry","geometry");
                contents = contents.Replace("XYZ", "Z");
                
                string filename = classname + ".csv";
                File.WriteAllText(localPath + filename, contents);
                files.Add(localPath + filename);
            }
            return files;
        }
    }
}
