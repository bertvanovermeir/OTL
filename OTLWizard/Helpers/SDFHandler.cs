using OTLWizard.Helpers;
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

        public static List<string> loadClasses(string path)
        {
            string application = Settings.Get("sdfpath");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = application;
            startInfo.Arguments = "list-classes --from-file \"" + path + "\"";
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
            List<string> classnames = SDFHandler.loadClasses(path);

            var localPath = System.IO.Path.GetTempPath() + "otlsdftempconversion\\";
            if(Directory.Exists(localPath))
                Directory.Delete(localPath,true);
            Directory.CreateDirectory(localPath);
                foreach (string classname in classnames)
                {
                    string contents = SDFHandler.loadDataForClass(classname, path);
                    contents = contents.Replace('_', '.');
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
            List<string> classnames = SDFHandler.loadClasses(path);

            var localPath = saveTo;
            if (Directory.Exists(localPath))
                Directory.Delete(localPath, true);
            Directory.CreateDirectory(localPath);
            foreach (string classname in classnames)
            {
                string contents = SDFHandler.loadDataForClass(classname, path);
                contents = contents.Replace('_', '.');
                string filename = classname + ".csv";
                File.WriteAllText(localPath + filename, contents);
                files.Add(localPath + filename);
            }
            return files;
        }

        private static void ConvertCSVOTLConformity(string path)
        {
            string[] CSVlines = File.ReadAllLines(path);




        }
    }
}
