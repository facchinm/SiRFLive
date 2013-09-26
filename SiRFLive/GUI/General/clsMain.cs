﻿namespace SiRFLive.GUI.General
{
    using SiRFLive.General;
    using System;
    using System.Windows.Forms;

    internal class clsMain
    {
        [STAThread]
        private static void Main()
        {
            try
            {
                new frmSplash().ShowDialog();
                clsGlobal.g_objfrmMDIMain = new frmMDIMain();
                Application.Run(clsGlobal.g_objfrmMDIMain);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "SiRFLive...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
    }
}

