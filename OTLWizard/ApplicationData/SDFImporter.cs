using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using Microsoft.Win32;
using OTLWizard.Helpers;
using System.IO;

namespace OTLWizard.ApplicationData
{

    
    public class SDFImporter
    {
        private string path;
        private string application = "";

        public SDFImporter(string path)
        {
         this.path = path;
         
        }

        public bool checkDependencies()
        {
            application = Settings.Get("sdfpath");

            if(File.Exists(application))
            {
                return true;
            } else
            {

                return false;
            }


        }
        public string loadDataForClass(string otlname)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
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

        public List<string> loadClasses()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = application;
            startInfo.Arguments = "list-classes --from-file \"" + path + "\"";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd().Replace('\r',' ').Trim();
            process.WaitForExit();
            string[] listing  = output.Split('\n');

            return listing.ToList();
        }


    }
}
