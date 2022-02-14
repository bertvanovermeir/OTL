using System;
using System.Windows.Forms;

namespace OTLWizard.ApplicationData
{
    /// <summary>
    /// Deze klasse start de applicatie op
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Alles werkt momenteel in de Main Thread, op termijn de calculaties omzetten naar een tweede thread.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // een instantie van application manager is noodzakelijk voor alle backend op te starten.
            ApplicationManager app = new ApplicationManager();           
        }
    }
}
