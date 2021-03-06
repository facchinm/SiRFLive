﻿namespace SiRFLive.GUI.Commmunication
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmCommErrorView : Form
    {
        private CommunicationManager comm;
        private IContainer components;
        private CommonClass.MyRichTextBox rtbDisplay_ErrorView;
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommErrorView()
        {
            this.InitializeComponent();
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCommErrorView_Load(object sender, EventArgs e)
        {
            if (this.WinTop != 0)
            {
                base.Top = this.WinTop;
            }
            if (this.WinLeft != 0)
            {
                base.Left = this.WinLeft;
            }
            if (this.WinWidth != 0)
            {
                base.Width = this.WinWidth;
            }
            if (this.WinHeight != 0)
            {
                base.Height = this.WinHeight;
            }
        }

        private void frmCommErrorView_LocationChanged(object sender, EventArgs e)
        {
            this.WinTop = base.Top;
            this.WinLeft = base.Left;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void frmCommErrorView_ResizeEnd(object sender, EventArgs e)
        {
            this.WinWidth = base.Width;
            this.WinHeight = base.Height;
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, true);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommErrorView));
            this.rtbDisplay_ErrorView = new CommonClass.MyRichTextBox();
            base.SuspendLayout();
            this.rtbDisplay_ErrorView.BackColor = SystemColors.Window;
            this.rtbDisplay_ErrorView.Dock = DockStyle.Fill;
            this.rtbDisplay_ErrorView.Location = new Point(0, 0);
            this.rtbDisplay_ErrorView.Name = "rtbDisplay_ErrorView";
            this.rtbDisplay_ErrorView.ReadOnly = true;
            this.rtbDisplay_ErrorView.Size = new Size(0x16c, 0x10f);
            this.rtbDisplay_ErrorView.TabIndex = 2;
            this.rtbDisplay_ErrorView.Text = "";
            this.rtbDisplay_ErrorView.DoubleClick += new EventHandler(this.rtbDisplay_ErrorView_DoubleClick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x16c, 0x10f);
            base.Controls.Add(this.rtbDisplay_ErrorView);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmCommErrorView";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Error View";
            base.Load += new EventHandler(this.frmCommErrorView_Load);
            base.LocationChanged += new EventHandler(this.frmCommErrorView_LocationChanged);
            base.ResizeEnd += new EventHandler(this.frmCommErrorView_ResizeEnd);
            base.ResumeLayout(false);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.updateMainWindow != null)
            {
                this.updateMainWindow(base.Name);
            }
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
            }
            base.OnClosed(e);
        }

        private void rtbDisplay_ErrorView_DoubleClick(object sender, EventArgs e)
        {
            this.rtbDisplay_ErrorView.Text = string.Empty;
        }

        public CommunicationManager CommWindow
        {
            get
            {
                return this.comm;
            }
            set
            {
                this.comm = value;
                this.comm.ErrorViewRTBDisplay.DisplayWindow = this.rtbDisplay_ErrorView;
                this.Text = this.comm.sourceDeviceName + ": Error View";
            }
        }

        public delegate void updateParentEventHandler(string titleString);

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

