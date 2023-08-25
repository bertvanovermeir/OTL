﻿using System;
using System.Windows.Forms;

namespace OTLWizard.Helpers
{
    /// <summary>
    /// Deze klasse start de applicatie op
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ApplicationHandler.Start();

        }
    }
}
