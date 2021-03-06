﻿namespace SiRFLive.GUI
{
    using CommonClassLibrary;
    using SiRFLive.Communication;
    using SiRFLive.General;
    using SiRFLive.GUI.DlgsInputMsg;
    using SiRFLive.Properties;
    using SiRFLive.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    public class frmCommSiRFaware : Form
    {
        private DateTime _currTime = new DateTime();
        private bool _gotTTFF = true;
        private bool _inAwareMode;
        private DateTime _lastUpdateTime = new DateTime();
        private DateTime _resetTime = new DateTime();
        private List<string> _SiRFAwareScanResultList = new List<string>();
        private DataGridView _SiRFAwareStatsDataGridView;
        private Button buttonExit;
        private Button buttonGetFix;
        private Button buttonStart;
        private Button buttonUpdate;
        private DataGridView clkLearnStatusView;
        private CommunicationManager comm;
        private IContainer components;
        private int cTempLoc = 14;
        private int currEphRow = 1;
        private string currTempValue = "";
        private int curTempRow;
        private int freqUncRow = 3;
        private string freqUncValue = "";
        private Label label_CurrentTime;
        private Label label_LastUpdate;
        private Label label_TimeSinceUpdate;
        private Label label_TTFF;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private const string MINUS_FIFTEEN = "-15";
        private const string MINUS_FIVE = " -5 ";
        private const string MINUS_FORTY = "- 40";
        private const string MINUS_TEN = "-10";
        private const string MINUS_THIRTY = "-30";
        private const string MINUS_THIRTYFIVE = "-35";
        private const string MINUS_TWENTY = "-20";
        private const string MINUS_TWENTYFIVE = "-25";
        private DataGridViewTextBoxColumn minus10;
        private DataGridViewTextBoxColumn minus15;
        private DataGridViewTextBoxColumn minus20;
        private DataGridViewTextBoxColumn minus25;
        private DataGridViewTextBoxColumn minus30;
        private DataGridViewTextBoxColumn minus35;
        private DataGridViewTextBoxColumn minus40;
        private DataGridViewTextBoxColumn minus5;
        private ToolStripButton mpmConfigBtn;
        private bool msgid68Update;
        private string numEphValue = "";
		private System.Windows.Forms.Timer OneSecondTimer;
        public string paramList = " , Ephemeris, Time Uncertainty, Frequency Uncertainty";
        private const string PLUS_EIGHTY = "80";
        private const string PLUS_EIGHTYFIVE = "85";
        private const string PLUS_FIFTEEN = "15";
        private const string PLUS_FIFTY = "50";
        private const string PLUS_FIFTYFIVE = "55";
        private const string PLUS_FIVE = " 5 ";
        private const string PLUS_FORTY = "40";
        private const string PLUS_FORTYFIVE = "45";
        private const string PLUS_SEVENTY = "70";
        private const string PLUS_SEVENTYFIVE = "75";
        private const string PLUS_SIXTY = "60";
        private const string PLUS_SIXTYFIVE = "65";
        private const string PLUS_TEN = "10";
        private const string PLUS_THIRTY = "30";
        private const string PLUS_THIRTYFIVE = "35";
        private const string PLUS_TWENTY = "20";
        private const string PLUS_TWENTYFIVE = "25";
        private DataGridViewTextBoxColumn plus10;
        private DataGridViewTextBoxColumn plus15;
        private DataGridViewTextBoxColumn plus20;
        private DataGridViewTextBoxColumn plus25;
        private DataGridViewTextBoxColumn plus30;
        private DataGridViewTextBoxColumn plus35;
        private DataGridViewTextBoxColumn plus40;
        private DataGridViewTextBoxColumn plus45;
        private DataGridViewTextBoxColumn plus5;
        private DataGridViewTextBoxColumn plus50;
        private DataGridViewTextBoxColumn plus55;
        private DataGridViewTextBoxColumn plus60;
        private DataGridViewTextBoxColumn plus65;
        private DataGridViewTextBoxColumn plus70;
        private DataGridViewTextBoxColumn plus75;
        private DataGridViewTextBoxColumn plus80;
        private DataGridViewTextBoxColumn plus85;
        private int SA_PARAM_ROWS = 4;
        private DataGridViewTextBoxColumn SA_Parameter;
        private DataGridViewTextBoxColumn SA_Status;
        private DataGridViewTextBoxColumn SA_Value;
        private ToolStrip sirfAwareToolStrip;
        private int timeUncRow = 2;
        private string timeUncValue = "";
        private string valueCaution = "  Caution";
        private string valueCenterDash = "      --";
        private string valueGood = "  Good";
        private string valueUpdateIn60 = "";
        private string valueWarning = "  Warning";
        private const string WINDOW_TITLE_LABEL = ": SiRFaware";
        public int WinHeight;
        public int WinLeft;
        public int WinTop;
        public int WinWidth;
        private DataGridViewTextBoxColumn zero;
        private const string ZERO = " 0 ";

        public event updateParentEventHandler updateMainWindow;

        public event UpdateWindowEventHandler UpdatePortManager;

        public frmCommSiRFaware(CommunicationManager mainComWin)
        {
            this.InitializeComponent();
            int num = 0;
            base.MdiParent = clsGlobal.g_objfrmMDIMain;
            this.CommWindow = mainComWin;
            string[] strArray = this.paramList.Split(new char[] { ',' });
            this.SA_PARAM_ROWS = strArray.Length;
            foreach (string str in strArray)
            {
                this._SiRFAwareStatsDataGridView.Rows.Add();
                this._SiRFAwareStatsDataGridView["SA_Parameter", num].Value = str;
                num++;
            }
        }

        public void _SiRFAwareStatsDataGridView_Paint(int index)
        {
			this._SiRFAwareStatsDataGridView.BeginInvoke((MethodInvoker)delegate
			{
                switch (index)
                {
                    case 1:
                        try
                        {
                            this._SiRFAwareStatsDataGridView["SA_Value", this.currEphRow].Value = this.numEphValue;
                            double num = Convert.ToDouble(this.numEphValue);
                            if (num >= 5.0)
                            {
                                this._SiRFAwareStatsDataGridView["SA_Status", this.currEphRow].Style.BackColor = Color.Green;
                                this._SiRFAwareStatsDataGridView["SA_Status", this.currEphRow].Value = this.valueGood;
                            }
                            else if (num > 0.0)
                            {
                                this._SiRFAwareStatsDataGridView["SA_Status", this.currEphRow].Style.BackColor = Color.Yellow;
                                this._SiRFAwareStatsDataGridView["SA_Status", this.currEphRow].Value = this.valueCaution;
                            }
                            else
                            {
                                this._SiRFAwareStatsDataGridView["SA_Status", this.currEphRow].Style.BackColor = Color.Red;
                                this._SiRFAwareStatsDataGridView["SA_Status", this.currEphRow].Value = this.valueWarning;
                            }
                        }
                        catch (Exception exception)
                        {
                            string text1 = "### SiRFaware: Ephemeris handler error -- " + exception.Message;
                        }
                        break;

                    case 2:
                        try
                        {
                            if (this.timeUncValue == this.valueCenterDash)
                            {
                                this._SiRFAwareStatsDataGridView["SA_Status", this.timeUncRow].Style.BackColor = Color.Red;
                                this._SiRFAwareStatsDataGridView["SA_Status", this.timeUncRow].Value = this.valueWarning;
                                this._SiRFAwareStatsDataGridView["SA_Value", this.timeUncRow].Value = this.timeUncValue;
                            }
                            else
                            {
                                this._SiRFAwareStatsDataGridView["SA_Value", this.timeUncRow].Value = this.timeUncValue;
                                if (Convert.ToDouble(this.timeUncValue) > 2.1E-05)
                                {
                                    this._SiRFAwareStatsDataGridView["SA_Status", this.timeUncRow].Style.BackColor = Color.Yellow;
                                    this._SiRFAwareStatsDataGridView["SA_Status", this.timeUncRow].Value = this.valueCaution;
                                }
                                else
                                {
                                    this._SiRFAwareStatsDataGridView["SA_Status", this.timeUncRow].Style.BackColor = Color.Green;
                                    this._SiRFAwareStatsDataGridView["SA_Status", this.timeUncRow].Value = this.valueGood;
                                }
                            }
                        }
                        catch (Exception exception2)
                        {
                            string text2 = "### SiRFaware: TimeUnc handler error -- " + exception2.Message;
                        }
                        try
                        {
                            if (this.freqUncValue == this.valueCenterDash)
                            {
                                this._SiRFAwareStatsDataGridView["SA_Status", this.freqUncRow].Style.BackColor = Color.Red;
                                this._SiRFAwareStatsDataGridView["SA_Status", this.freqUncRow].Value = this.valueWarning;
                                this._SiRFAwareStatsDataGridView["SA_Value", this.freqUncRow].Value = this.freqUncValue;
                            }
                            else
                            {
                                double num3 = Convert.ToDouble(this.freqUncValue);
                                if (num3 > 27.0)
                                {
                                    this._SiRFAwareStatsDataGridView["SA_Status", this.freqUncRow].Style.BackColor = Color.Yellow;
                                    this._SiRFAwareStatsDataGridView["SA_Status", this.freqUncRow].Value = this.valueCaution;
                                }
                                else
                                {
                                    this._SiRFAwareStatsDataGridView["SA_Status", this.freqUncRow].Style.BackColor = Color.Green;
                                    this._SiRFAwareStatsDataGridView["SA_Status", this.freqUncRow].Value = this.valueGood;
                                }
                                this.freqUncValue = (num3 / 16.369).ToString("N3");
                                this._SiRFAwareStatsDataGridView["SA_Value", this.freqUncRow].Value = this.freqUncValue;
                            }
                        }
                        catch (Exception exception3)
                        {
                            string text3 = "### SiRFaware: FreqUnc handler error -- " + exception3.Message;
                        }
                        break;

                    default:
                        return;
                }
            });
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.StopListeners();
            base.Close();
        }

        private void buttonGetFix_Click(object sender, EventArgs e)
        {
            if ((this.comm != null) && !this.comm.IsSourceDeviceOpen())
            {
                MessageBox.Show("Port is not connected!", "SiRFaware Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                this.buttonStart.Enabled = true;
                this.SendInputMessage("0002", "DA 00");
                this.ClearSiRFAwareWindowValues();
                if (this._inAwareMode && (this.comm.ProductFamily == CommonClass.ProductType.GSD4e))
                {
                    new frmCommSiRFAwarePic().ShowDialog();
                }
                this.comm.Toggle4eWakeupPort();
                this._inAwareMode = false;
                this.label_TTFF.Visible = true;
                this.DisableLastUpdate();
                this.comm.dataGui._PMODE = 0;
                this.ClearTTFFTime();
                this.ClearLastUpdatedTime();
                this._resetTime = DateTime.Now;
                this._gotTTFF = false;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
                if (!this.comm.IsSourceDeviceOpen())
                {
                    MessageBox.Show("Port is not connected!", "SiRFaware Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                if (this.comm.RxCtrl != null)
                {
                    this.comm.RxCtrl.SetMessageRateForFactory();
                }
            }
            this.InitialSiRFAwareWindowRefresh();
            this.ClearTTFFTime();
            this.SendInputMessage("000A", "DD 00 10 10 00 00 00 00 00 00");
            this.comm.RxCtrl.SendMPM_V2(this.comm.LowPowerParams.MPM_Timeout, this.comm.LowPowerParams.MPM_Control);
            this.UpdateLastUpdatedTime();
            this.UpdateLastUpdatedTimeGUI();
            this.buttonStart.Enabled = false;
            this._inAwareMode = true;
            this.label_TTFF.Visible = false;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
        }

        public void ClearLastUpdatedTime()
        {
            this.label_LastUpdate.Text = "Last Update: ";
        }

        public void ClearSiRFAwareWindowValues()
        {
            this._SiRFAwareStatsDataGridView["SA_Status", this.curTempRow].Style.BackColor = Color.White;
            this._SiRFAwareStatsDataGridView["SA_Status", this.currEphRow].Style.BackColor = Color.White;
            this._SiRFAwareStatsDataGridView["SA_Status", this.timeUncRow].Style.BackColor = Color.White;
            this._SiRFAwareStatsDataGridView["SA_Status", this.freqUncRow].Style.BackColor = Color.White;
            this._SiRFAwareStatsDataGridView["SA_Status", this.currEphRow].Value = this.valueCenterDash;
            this._SiRFAwareStatsDataGridView["SA_Status", this.timeUncRow].Value = this.valueCenterDash;
            this._SiRFAwareStatsDataGridView["SA_Status", this.freqUncRow].Value = this.valueCenterDash;
            this._SiRFAwareStatsDataGridView["SA_Value", this.currEphRow].Value = "";
            this._SiRFAwareStatsDataGridView["SA_Value", this.timeUncRow].Value = "";
            this._SiRFAwareStatsDataGridView["SA_Value", this.freqUncRow].Value = "";
        }

        public void ClearTTFFTime()
        {
            this.label_TTFF.Text = "TTFF: ";
        }

        public void DisableLastUpdate()
        {
            DateTime time = new DateTime();
            this._lastUpdateTime = time;
        }

        public void DisplayCurrentTempResults(string ScanResultsStr)
        {
            try
            {
                string[] strArray = ScanResultsStr.Split(new char[] { ',' });
                this.currTempValue = strArray[this.cTempLoc];
                this._SiRFAwareStatsDataGridView_Paint(0);
            }
            catch (Exception exception)
            {
                string text1 = "### SiRFaware: Current Temp handler error -- " + exception.Message;
            }
        }

        public void DisplayEphemerisResults(string ScanResultsStr)
        {
            string pattern = @"ns:(?<numEphValue>\d+)";
            if (this.comm.ProductFamily == CommonClass.ProductType.GSD4e)
            {
                pattern = @"MPM: (?<numEphValue>\d+)";
            }
            Regex regex = new Regex(pattern, RegexOptions.Compiled);
            if (regex.IsMatch(ScanResultsStr))
            {
                this.numEphValue = regex.Match(ScanResultsStr).Result("${numEphValue}");
            }
            else
            {
                this.numEphValue = this.valueCenterDash;
            }
            this._SiRFAwareStatsDataGridView_Paint(1);
        }

        public void DisplaySiRFAwareResults(string ScanResultsStr)
        {
            string pattern = @"(status:\s+(?<statusValue>\d+))";
            string str2 = @"freqCorrUnc:\s+(?<freqUncValue>[-]?\d+\.\d+)";
            string str3 = @"timeCorrUnc:\s+(?<timeUncValue>[-]?\d+\.\d+)";
            Regex regex = new Regex(pattern, RegexOptions.Compiled);
            Regex regex2 = new Regex(str2, RegexOptions.Compiled);
            Regex regex3 = new Regex(str3, RegexOptions.Compiled);
            if (regex.IsMatch(ScanResultsStr))
            {
                regex.Match(ScanResultsStr).Result("${statusValue}");
                if (regex2.IsMatch(ScanResultsStr))
                {
                    this.freqUncValue = regex2.Match(ScanResultsStr).Result("${freqUncValue}");
                }
                if (regex3.IsMatch(ScanResultsStr))
                {
                    this.timeUncValue = regex3.Match(ScanResultsStr).Result("${timeUncValue}");
                }
            }
            else
            {
                this.freqUncValue = this.valueCenterDash;
                this.timeUncValue = this.valueCenterDash;
            }
            this._SiRFAwareStatsDataGridView_Paint(2);
            this.UpdateLastUpdatedTime();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmCommSiRFAware_Load(object sender, EventArgs e)
        {
            base.Height = 0xca;
            this.OneSecondTimer.Start();
            if (this._inAwareMode && (this.comm.ProductFamily == CommonClass.ProductType.GSD4e))
            {
                this.label_TTFF.Visible = false;
            }
            else
            {
                this.label_TTFF.Visible = true;
            }
            base.Top = this.WinTop;
            base.Left = this.WinLeft;
            if (this.WinWidth != 0)
            {
                base.Width = this.WinWidth;
            }
            if (this.WinHeight != 0)
            {
                base.Height = this.WinHeight;
            }
            else
            {
                base.Height = this.label1.Location.Y + 50;
            }
        }

        private void frmSiRFAwareDisplayCurrentTempHandler(object sender, DoWorkEventArgs myQContent)
        {
            if (this.comm.MessageProtocol == "OSP")
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
                    MessageQData argument = (MessageQData) myQContent.Argument;
                    if (argument.MessageText != string.Empty)
                    {
                        string scanResultsStr = this.comm.m_Protocols.ConvertRawToFields(HelperFunctions.HexToByte(argument.MessageText));
                        if (scanResultsStr != string.Empty)
                        {
                            this.DisplayCurrentTempResults(scanResultsStr);
                        }
                    }
                }
                catch (Exception exception)
                {
                    string msg = "### SiRFaware Current Temp GUI handler error -- " + exception.Message;
                    this.comm.WriteApp(msg);
                }
            }
        }

        private void frmSiRFAwareDisplayEphemerisHandler(object sender, DoWorkEventArgs myQContent)
        {
            if (this.comm.MessageProtocol == "OSP")
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
                    MessageQData argument = (MessageQData) myQContent.Argument;
                    if (argument.MessageText != string.Empty)
                    {
                        string scanResultsStr = this.comm.m_Protocols.ConvertRawToFields(HelperFunctions.HexToByte(argument.MessageText));
                        if ((scanResultsStr != string.Empty) && (scanResultsStr.Contains("MEI:MPM:SVD1") || scanResultsStr.Contains("valid sats")))
                        {
                            this.DisplayEphemerisResults(scanResultsStr);
                        }
                    }
                }
                catch (Exception exception)
                {
                    string msg = "### SiRFaware Ephemeris GUI handler error -- " + exception.Message;
                    this.comm.WriteApp(msg);
                }
            }
        }

        private void frmSiRFAwareDisplayQueueHandler(object sender, DoWorkEventArgs myQContent)
        {
            if (this.comm.MessageProtocol == "OSP")
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
                    MessageQData argument = (MessageQData) myQContent.Argument;
                    if (argument.MessageText != string.Empty)
                    {
                        string scanResultsStr = this.comm.m_Protocols.ConvertRawToFields(HelperFunctions.HexToByte(argument.MessageText));
                        if ((scanResultsStr != string.Empty) && scanResultsStr.Contains("MPM:navState1"))
                        {
                            this.DisplaySiRFAwareResults(scanResultsStr);
                        }
                    }
                }
                catch (Exception exception)
                {
                    string msg = "### SiRFaware GUI handler error -- " + exception.Message;
                    this.comm.WriteApp(msg);
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            DataGridViewCellStyle style4 = new DataGridViewCellStyle();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmCommSiRFaware));
            this.label_TTFF = new Label();
            this.clkLearnStatusView = new DataGridView();
            this.minus40 = new DataGridViewTextBoxColumn();
            this.minus35 = new DataGridViewTextBoxColumn();
            this.minus30 = new DataGridViewTextBoxColumn();
            this.minus25 = new DataGridViewTextBoxColumn();
            this.minus20 = new DataGridViewTextBoxColumn();
            this.minus15 = new DataGridViewTextBoxColumn();
            this.minus10 = new DataGridViewTextBoxColumn();
            this.minus5 = new DataGridViewTextBoxColumn();
            this.zero = new DataGridViewTextBoxColumn();
            this.plus5 = new DataGridViewTextBoxColumn();
            this.plus10 = new DataGridViewTextBoxColumn();
            this.plus15 = new DataGridViewTextBoxColumn();
            this.plus20 = new DataGridViewTextBoxColumn();
            this.plus25 = new DataGridViewTextBoxColumn();
            this.plus30 = new DataGridViewTextBoxColumn();
            this.plus35 = new DataGridViewTextBoxColumn();
            this.plus40 = new DataGridViewTextBoxColumn();
            this.plus45 = new DataGridViewTextBoxColumn();
            this.plus50 = new DataGridViewTextBoxColumn();
            this.plus55 = new DataGridViewTextBoxColumn();
            this.plus60 = new DataGridViewTextBoxColumn();
            this.plus65 = new DataGridViewTextBoxColumn();
            this.plus70 = new DataGridViewTextBoxColumn();
            this.plus75 = new DataGridViewTextBoxColumn();
            this.plus80 = new DataGridViewTextBoxColumn();
            this.plus85 = new DataGridViewTextBoxColumn();
            this.buttonUpdate = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.buttonStart = new Button();
            this.buttonExit = new Button();
            this.label3 = new Label();
            this.buttonGetFix = new Button();
            this._SiRFAwareStatsDataGridView = new DataGridView();
            this.SA_Parameter = new DataGridViewTextBoxColumn();
            this.SA_Status = new DataGridViewTextBoxColumn();
            this.SA_Value = new DataGridViewTextBoxColumn();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label_LastUpdate = new Label();
            this.label_CurrentTime = new Label();
            this.label_TimeSinceUpdate = new Label();
            this.OneSecondTimer = new System.Windows.Forms.Timer(this.components);
            this.sirfAwareToolStrip = new ToolStrip();
            this.mpmConfigBtn = new ToolStripButton();
            ((ISupportInitialize) this.clkLearnStatusView).BeginInit();
            ((ISupportInitialize) this._SiRFAwareStatsDataGridView).BeginInit();
            this.sirfAwareToolStrip.SuspendLayout();
            base.SuspendLayout();
            this.label_TTFF.AutoSize = true;
            this.label_TTFF.Location = new Point(0x2d7, 0x5f);
            this.label_TTFF.Name = "label_TTFF";
            this.label_TTFF.Size = new Size(0x33, 13);
            this.label_TTFF.TabIndex = 0x11;
            this.label_TTFF.Text = "TTFF:     ";
            this.clkLearnStatusView.AllowUserToAddRows = false;
            this.clkLearnStatusView.AllowUserToDeleteRows = false;
            this.clkLearnStatusView.AllowUserToResizeColumns = false;
            this.clkLearnStatusView.AllowUserToResizeRows = false;
            this.clkLearnStatusView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.clkLearnStatusView.Columns.AddRange(new DataGridViewColumn[] { 
                this.minus40, this.minus35, this.minus30, this.minus25, this.minus20, this.minus15, this.minus10, this.minus5, this.zero, this.plus5, this.plus10, this.plus15, this.plus20, this.plus25, this.plus30, this.plus35, 
                this.plus40, this.plus45, this.plus50, this.plus55, this.plus60, this.plus65, this.plus70, this.plus75, this.plus80, this.plus85
             });
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Window;
            style.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            style.ForeColor = SystemColors.ControlText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.False;
            this.clkLearnStatusView.DefaultCellStyle = style;
            this.clkLearnStatusView.GridColor = SystemColors.ControlLight;
            this.clkLearnStatusView.Location = new Point(0x18, 0xc0);
            this.clkLearnStatusView.Name = "clkLearnStatusView";
            this.clkLearnStatusView.ReadOnly = true;
            this.clkLearnStatusView.Size = new Size(0x337, 0x37);
            this.clkLearnStatusView.TabIndex = 0;
            this.clkLearnStatusView.Visible = false;
            this.minus40.HeaderText = "-40";
            this.minus40.MaxInputLength = 3;
            this.minus40.Name = "minus40";
            this.minus40.ReadOnly = true;
            this.minus40.Resizable = DataGridViewTriState.False;
            this.minus40.Width = 30;
            this.minus35.HeaderText = "-35";
            this.minus35.MaxInputLength = 3;
            this.minus35.Name = "minus35";
            this.minus35.ReadOnly = true;
            this.minus35.Resizable = DataGridViewTriState.False;
            this.minus35.Width = 30;
            this.minus30.HeaderText = "-30";
            this.minus30.MaxInputLength = 3;
            this.minus30.Name = "minus30";
            this.minus30.ReadOnly = true;
            this.minus30.Resizable = DataGridViewTriState.False;
            this.minus30.Width = 30;
            this.minus25.HeaderText = "-25";
            this.minus25.MaxInputLength = 3;
            this.minus25.Name = "minus25";
            this.minus25.ReadOnly = true;
            this.minus25.Resizable = DataGridViewTriState.False;
            this.minus25.Width = 30;
            this.minus20.HeaderText = "-20";
            this.minus20.MaxInputLength = 3;
            this.minus20.Name = "minus20";
            this.minus20.ReadOnly = true;
            this.minus20.Resizable = DataGridViewTriState.False;
            this.minus20.Width = 30;
            this.minus15.HeaderText = "-15";
            this.minus15.MaxInputLength = 3;
            this.minus15.Name = "minus15";
            this.minus15.ReadOnly = true;
            this.minus15.Resizable = DataGridViewTriState.False;
            this.minus15.Width = 30;
            this.minus10.HeaderText = "-10";
            this.minus10.MaxInputLength = 3;
            this.minus10.Name = "minus10";
            this.minus10.ReadOnly = true;
            this.minus10.Resizable = DataGridViewTriState.False;
            this.minus10.Width = 30;
            this.minus5.HeaderText = " -5";
            this.minus5.MaxInputLength = 3;
            this.minus5.Name = "minus5";
            this.minus5.ReadOnly = true;
            this.minus5.Resizable = DataGridViewTriState.False;
            this.minus5.Width = 30;
            this.zero.HeaderText = "  0";
            this.zero.MaxInputLength = 3;
            this.zero.Name = "zero";
            this.zero.ReadOnly = true;
            this.zero.Resizable = DataGridViewTriState.False;
            this.zero.Width = 30;
            this.plus5.HeaderText = "  5";
            this.plus5.MaxInputLength = 3;
            this.plus5.Name = "plus5";
            this.plus5.ReadOnly = true;
            this.plus5.Resizable = DataGridViewTriState.False;
            this.plus5.Width = 30;
            this.plus10.HeaderText = "10";
            this.plus10.MaxInputLength = 3;
            this.plus10.Name = "plus10";
            this.plus10.ReadOnly = true;
            this.plus10.Resizable = DataGridViewTriState.False;
            this.plus10.Width = 30;
            this.plus15.HeaderText = "15";
            this.plus15.MaxInputLength = 3;
            this.plus15.Name = "plus15";
            this.plus15.ReadOnly = true;
            this.plus15.Resizable = DataGridViewTriState.False;
            this.plus15.Width = 30;
            this.plus20.HeaderText = "20";
            this.plus20.MaxInputLength = 3;
            this.plus20.Name = "plus20";
            this.plus20.ReadOnly = true;
            this.plus20.Resizable = DataGridViewTriState.False;
            this.plus20.Width = 30;
            this.plus25.HeaderText = "25";
            this.plus25.MaxInputLength = 3;
            this.plus25.Name = "plus25";
            this.plus25.ReadOnly = true;
            this.plus25.Resizable = DataGridViewTriState.False;
            this.plus25.Width = 30;
            this.plus30.HeaderText = "30";
            this.plus30.MaxInputLength = 3;
            this.plus30.Name = "plus30";
            this.plus30.ReadOnly = true;
            this.plus30.Resizable = DataGridViewTriState.False;
            this.plus30.Width = 30;
            this.plus35.HeaderText = "35";
            this.plus35.MaxInputLength = 3;
            this.plus35.Name = "plus35";
            this.plus35.ReadOnly = true;
            this.plus35.Resizable = DataGridViewTriState.False;
            this.plus35.Width = 30;
            this.plus40.HeaderText = "40";
            this.plus40.MaxInputLength = 3;
            this.plus40.Name = "plus40";
            this.plus40.ReadOnly = true;
            this.plus40.Resizable = DataGridViewTriState.False;
            this.plus40.Width = 30;
            this.plus45.HeaderText = "45";
            this.plus45.MaxInputLength = 3;
            this.plus45.Name = "plus45";
            this.plus45.ReadOnly = true;
            this.plus45.Resizable = DataGridViewTriState.False;
            this.plus45.Width = 30;
            this.plus50.HeaderText = "50";
            this.plus50.MaxInputLength = 3;
            this.plus50.Name = "plus50";
            this.plus50.ReadOnly = true;
            this.plus50.Resizable = DataGridViewTriState.False;
            this.plus50.Width = 30;
            this.plus55.HeaderText = "55";
            this.plus55.MaxInputLength = 3;
            this.plus55.Name = "plus55";
            this.plus55.ReadOnly = true;
            this.plus55.Resizable = DataGridViewTriState.False;
            this.plus55.Width = 30;
            this.plus60.HeaderText = "60";
            this.plus60.MaxInputLength = 3;
            this.plus60.Name = "plus60";
            this.plus60.ReadOnly = true;
            this.plus60.Resizable = DataGridViewTriState.False;
            this.plus60.Width = 30;
            this.plus65.HeaderText = "65";
            this.plus65.MaxInputLength = 3;
            this.plus65.Name = "plus65";
            this.plus65.ReadOnly = true;
            this.plus65.Resizable = DataGridViewTriState.False;
            this.plus65.Width = 30;
            this.plus70.HeaderText = "70";
            this.plus70.MaxInputLength = 3;
            this.plus70.Name = "plus70";
            this.plus70.ReadOnly = true;
            this.plus70.Resizable = DataGridViewTriState.False;
            this.plus70.Width = 30;
            this.plus75.HeaderText = "75";
            this.plus75.MaxInputLength = 3;
            this.plus75.Name = "plus75";
            this.plus75.ReadOnly = true;
            this.plus75.Resizable = DataGridViewTriState.False;
            this.plus75.Width = 30;
            this.plus80.HeaderText = "80";
            this.plus80.MaxInputLength = 3;
            this.plus80.Name = "plus80";
            this.plus80.ReadOnly = true;
            this.plus80.Resizable = DataGridViewTriState.False;
            this.plus80.Width = 30;
            this.plus85.HeaderText = "85";
            this.plus85.MaxInputLength = 3;
            this.plus85.Name = "plus85";
            this.plus85.ReadOnly = true;
            this.plus85.Resizable = DataGridViewTriState.False;
            this.plus85.Width = 30;
            this.buttonUpdate.Enabled = false;
            this.buttonUpdate.Location = new Point(0x192, 0x114);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new Size(0x4b, 0x17);
            this.buttonUpdate.TabIndex = 1;
            this.buttonUpdate.Text = "&Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Visible = false;
            this.buttonUpdate.Click += new EventHandler(this.buttonUpdate_Click);
            this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0x18, 160);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x337, 0x1d);
            this.label1.TabIndex = 2;
            this.label1.Text = "Clock Learning Status (over temp)";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            this.label2.Location = new Point(0x14b, 250);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0xd1, 0x17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Y = Yes   N = No   I = Interpolated";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.Visible = false;
            this.buttonStart.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.buttonStart.Location = new Point(0x2d8, 0x22);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new Size(0x4b, 0x18);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "&Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new EventHandler(this.buttonStart_Click);
            this.buttonExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonExit.Location = new Point(0x2d8, 0x7d);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new Size(0x4b, 0x17);
            this.buttonExit.TabIndex = 6;
            this.buttonExit.Text = "E&xit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new EventHandler(this.buttonExit_Click);
            this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(0xe4, 12);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x19e, 0x17);
            this.label3.TabIndex = 7;
            this.label3.Text = "SiRFaware";
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonGetFix.Location = new Point(0x2d8, 0x40);
            this.buttonGetFix.Name = "buttonGetFix";
            this.buttonGetFix.Size = new Size(0x4b, 0x17);
            this.buttonGetFix.TabIndex = 8;
            this.buttonGetFix.Text = "Get &Position";
            this.buttonGetFix.UseVisualStyleBackColor = true;
            this.buttonGetFix.Click += new EventHandler(this.buttonGetFix_Click);
            this._SiRFAwareStatsDataGridView.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this._SiRFAwareStatsDataGridView.AllowUserToAddRows = false;
            this._SiRFAwareStatsDataGridView.AllowUserToDeleteRows = false;
            this._SiRFAwareStatsDataGridView.AllowUserToResizeColumns = false;
            this._SiRFAwareStatsDataGridView.AllowUserToResizeRows = false;
            this._SiRFAwareStatsDataGridView.CausesValidation = false;
            this._SiRFAwareStatsDataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            style2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style2.BackColor = SystemColors.Control;
            style2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            style2.ForeColor = SystemColors.WindowText;
            style2.SelectionBackColor = SystemColors.ActiveCaptionText;
            style2.SelectionForeColor = SystemColors.WindowText;
            style2.WrapMode = DataGridViewTriState.True;
            this._SiRFAwareStatsDataGridView.ColumnHeadersDefaultCellStyle = style2;
            this._SiRFAwareStatsDataGridView.ColumnHeadersHeight = 20;
            this._SiRFAwareStatsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._SiRFAwareStatsDataGridView.Columns.AddRange(new DataGridViewColumn[] { this.SA_Parameter, this.SA_Status, this.SA_Value });
            this._SiRFAwareStatsDataGridView.Cursor = Cursors.No;
            style3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style3.BackColor = SystemColors.Window;
            style3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            style3.ForeColor = SystemColors.ControlText;
            style3.SelectionBackColor = SystemColors.ActiveCaptionText;
            style3.SelectionForeColor = SystemColors.ControlText;
            style3.WrapMode = DataGridViewTriState.False;
            this._SiRFAwareStatsDataGridView.DefaultCellStyle = style3;
            this._SiRFAwareStatsDataGridView.Location = new Point(0xe4, 0x26);
            this._SiRFAwareStatsDataGridView.MultiSelect = false;
            this._SiRFAwareStatsDataGridView.Name = "_SiRFAwareStatsDataGridView";
            this._SiRFAwareStatsDataGridView.ReadOnly = true;
            style4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style4.BackColor = SystemColors.Control;
            style4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            style4.ForeColor = SystemColors.WindowText;
            style4.SelectionBackColor = SystemColors.ActiveCaptionText;
            style4.SelectionForeColor = SystemColors.WindowText;
            style4.WrapMode = DataGridViewTriState.True;
            this._SiRFAwareStatsDataGridView.RowHeadersDefaultCellStyle = style4;
            this._SiRFAwareStatsDataGridView.RowHeadersWidth = 4;
            this._SiRFAwareStatsDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._SiRFAwareStatsDataGridView.RowTemplate.ReadOnly = true;
            this._SiRFAwareStatsDataGridView.RowTemplate.Resizable = DataGridViewTriState.False;
            this._SiRFAwareStatsDataGridView.ScrollBars = ScrollBars.None;
            this._SiRFAwareStatsDataGridView.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            this._SiRFAwareStatsDataGridView.ShowCellErrors = false;
            this._SiRFAwareStatsDataGridView.ShowCellToolTips = false;
            this._SiRFAwareStatsDataGridView.ShowEditingIcon = false;
            this._SiRFAwareStatsDataGridView.ShowRowErrors = false;
            this._SiRFAwareStatsDataGridView.Size = new Size(0x19e, 110);
            this._SiRFAwareStatsDataGridView.TabIndex = 9;
            this._SiRFAwareStatsDataGridView.TabStop = false;
            this.SA_Parameter.Frozen = true;
            this.SA_Parameter.HeaderText = "Parameter";
            this.SA_Parameter.Name = "SA_Parameter";
            this.SA_Parameter.ReadOnly = true;
            this.SA_Parameter.Resizable = DataGridViewTriState.False;
            this.SA_Parameter.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.SA_Parameter.Width = 0xaf;
            this.SA_Status.Frozen = true;
            this.SA_Status.HeaderText = "Status";
            this.SA_Status.Name = "SA_Status";
            this.SA_Status.ReadOnly = true;
            this.SA_Status.Resizable = DataGridViewTriState.False;
            this.SA_Status.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.SA_Status.Width = 60;
            this.SA_Value.Frozen = true;
            this.SA_Value.HeaderText = "Value";
            this.SA_Value.Name = "SA_Value";
            this.SA_Value.ReadOnly = true;
            this.SA_Value.Resizable = DataGridViewTriState.False;
            this.SA_Value.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.SA_Value.Width = 0xaf;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x284, 0x6b);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1a, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Sec";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x284, 0x80);
            this.label5.Name = "label5";
            this.label5.Size = new Size(30, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "PPM";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x284, 0x56);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x24, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "# SVs";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x284, 0x41);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x12, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "\x00b0C";
            this.label7.Visible = false;
            this.label_LastUpdate.AutoSize = true;
            this.label_LastUpdate.Location = new Point(0x24, 0x3a);
            this.label_LastUpdate.Name = "label_LastUpdate";
            this.label_LastUpdate.Size = new Size(110, 13);
            this.label_LastUpdate.TabIndex = 14;
            this.label_LastUpdate.Text = "Last Update:              ";
            this.label_CurrentTime.AutoSize = true;
            this.label_CurrentTime.Location = new Point(0x24, 0x5d);
            this.label_CurrentTime.Name = "label_CurrentTime";
            this.label_CurrentTime.Size = new Size(70, 13);
            this.label_CurrentTime.TabIndex = 15;
            this.label_CurrentTime.Text = "Current Time:";
            this.label_TimeSinceUpdate.AutoSize = true;
            this.label_TimeSinceUpdate.Location = new Point(0x24, 0x80);
            this.label_TimeSinceUpdate.Name = "label_TimeSinceUpdate";
            this.label_TimeSinceUpdate.Size = new Size(120, 13);
            this.label_TimeSinceUpdate.TabIndex = 0x10;
            this.label_TimeSinceUpdate.Text = "Seconds Since Update:";
            this.OneSecondTimer.Interval = 0x3e8;
            this.OneSecondTimer.Tick += new EventHandler(this.OneSecondTimer_Tick);
            this.sirfAwareToolStrip.Dock = DockStyle.Bottom;
            this.sirfAwareToolStrip.Items.AddRange(new ToolStripItem[] { this.mpmConfigBtn });
            this.sirfAwareToolStrip.Location = new Point(0, 0x138);
            this.sirfAwareToolStrip.Name = "sirfAwareToolStrip";
            this.sirfAwareToolStrip.Size = new Size(880, 0x19);
            this.sirfAwareToolStrip.Stretch = true;
            this.sirfAwareToolStrip.TabIndex = 0x12;
            this.sirfAwareToolStrip.Text = "Configuration";
            this.mpmConfigBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.mpmConfigBtn.Image = Resources.Config;
            this.mpmConfigBtn.ImageTransparentColor = Color.Magenta;
            this.mpmConfigBtn.Name = "mpmConfigBtn";
            this.mpmConfigBtn.Size = new Size(0x17, 0x16);
            this.mpmConfigBtn.Text = "Configuration";
            this.mpmConfigBtn.Click += new EventHandler(this.mpmConfigBtn_Click);
            base.AcceptButton = this.buttonStart;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonExit;
            base.ClientSize = new Size(880, 0x151);
            base.Controls.Add(this.sirfAwareToolStrip);
            base.Controls.Add(this.label_TTFF);
            base.Controls.Add(this.label_TimeSinceUpdate);
            base.Controls.Add(this.label_CurrentTime);
            base.Controls.Add(this.label_LastUpdate);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this._SiRFAwareStatsDataGridView);
            base.Controls.Add(this.buttonGetFix);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.buttonExit);
            base.Controls.Add(this.buttonStart);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.buttonUpdate);
            base.Controls.Add(this.clkLearnStatusView);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "frmCommSiRFaware";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "SiRFaware";
            base.Load += new EventHandler(this.frmCommSiRFAware_Load);
            ((ISupportInitialize) this.clkLearnStatusView).EndInit();
            ((ISupportInitialize) this._SiRFAwareStatsDataGridView).EndInit();
            this.sirfAwareToolStrip.ResumeLayout(false);
            this.sirfAwareToolStrip.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void InitialSiRFAwareWindowRefresh()
        {
            this._SiRFAwareStatsDataGridView["SA_Status", this.curTempRow].Style.BackColor = Color.White;
            this._SiRFAwareStatsDataGridView["SA_Parameter", this.curTempRow].Style.BackColor = Color.White;
            this._SiRFAwareStatsDataGridView["SA_Status", this.currEphRow].Style.BackColor = Color.White;
            this._SiRFAwareStatsDataGridView["SA_Status", this.timeUncRow].Style.BackColor = Color.White;
            this._SiRFAwareStatsDataGridView["SA_Status", this.freqUncRow].Style.BackColor = Color.White;
            this._SiRFAwareStatsDataGridView["SA_Status", this.curTempRow].Value = "";
            this._SiRFAwareStatsDataGridView["SA_Status", this.currEphRow].Value = this.valueCenterDash;
            this._SiRFAwareStatsDataGridView["SA_Status", this.timeUncRow].Value = this.valueCenterDash;
            this._SiRFAwareStatsDataGridView["SA_Status", this.freqUncRow].Value = this.valueCenterDash;
            this._SiRFAwareStatsDataGridView["SA_Value", this.curTempRow].Style.BackColor = Color.White;
            this._SiRFAwareStatsDataGridView["SA_Value", this.curTempRow].Value = "";
            this._SiRFAwareStatsDataGridView["SA_Value", this.currEphRow].Value = this.valueUpdateIn60;
            this._SiRFAwareStatsDataGridView["SA_Value", this.timeUncRow].Value = this.valueUpdateIn60;
            this._SiRFAwareStatsDataGridView["SA_Value", this.freqUncRow].Value = this.valueUpdateIn60;
        }

        private void mpmConfigBtn_Click(object sender, EventArgs e)
        {
            if (this.comm != null)
            {
				new frmMPMConfigure(ref this.comm._lowPowerParams).ShowDialog();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.comm != null)
            {
                this.StopListeners();
            }
            if (this.updateMainWindow != null)
            {
                this.updateMainWindow(base.Name);
            }
            if (this.UpdatePortManager != null)
            {
                this.UpdatePortManager(base.Name, base.Left, base.Top, base.Width, base.Height, false);
            }
        }

        private void OneSecondTimer_Tick(object sender, EventArgs e)
        {
            this.UpdateCurrentTime();
        }

        private void SendInputMessage(string payload, string hexString)
        {
            string msg = "A0A2 " + payload + " " + hexString + " " + this.comm.m_Protocols.GetChecksum(hexString, true) + " B0B3";
            this.comm.WriteData(msg);
        }

        public void StartListen()
        {
            if ((this.comm.ListenersCtrl != null) && (this.comm.MessageProtocol == "OSP"))
            {
                string listenerName = "MPM_navState1_GUI";
                if (this.comm.ProductFamily == CommonClass.ProductType.GSD4e)
                {
                    listenerName = "MPM_navState1:status_GUI";
                }
                if (!this.comm.ListenersCtrl.Exists(listenerName, this.comm.PortName))
                {
                    ListenerContent content = this.comm.ListenersCtrl.Create(listenerName, this.comm.PortName);
                    if (content != null)
                    {
                        content.DoUserWork.DoWork += new DoWorkEventHandler(this.frmSiRFAwareDisplayQueueHandler);
                        this._SiRFAwareScanResultList.Add(content.ListenerName);
                        this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                    }
                }
                else
                {
                    this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                }
                listenerName = "MPM_navState_V2_GUI";
                if (!this.comm.ListenersCtrl.Exists(listenerName, this.comm.PortName))
                {
                    ListenerContent content2 = this.comm.ListenersCtrl.Create(listenerName, this.comm.PortName);
                    if (content2 != null)
                    {
                        content2.DoUserWork.DoWork += new DoWorkEventHandler(this.frmSiRFAwareDisplayQueueHandler);
                        this._SiRFAwareScanResultList.Add(content2.ListenerName);
                        this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                    }
                }
                else
                {
                    this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                }
                listenerName = "MPM_SVD1_GUI";
                if (this.comm.ProductFamily == CommonClass.ProductType.GSD4e)
                {
                    listenerName = "valid sats_GUI";
                }
                if (!this.comm.ListenersCtrl.Exists(listenerName, this.comm.PortName))
                {
                    ListenerContent content3 = this.comm.ListenersCtrl.Create(listenerName, this.comm.PortName);
                    if (content3 != null)
                    {
                        content3.DoUserWork.DoWork += new DoWorkEventHandler(this.frmSiRFAwareDisplayEphemerisHandler);
                        this._SiRFAwareScanResultList.Add(content3.ListenerName);
                        this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                    }
                }
                else
                {
                    this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                }
                listenerName = "XOLearning_CurrentTemp_GUI";
                if (!this.comm.ListenersCtrl.Exists(listenerName, this.comm.PortName))
                {
                    ListenerContent content4 = this.comm.ListenersCtrl.Create(listenerName, this.comm.PortName);
                    if (content4 != null)
                    {
                        content4.DoUserWork.DoWork += new DoWorkEventHandler(this.frmSiRFAwareDisplayCurrentTempHandler);
                        this._SiRFAwareScanResultList.Add(content4.ListenerName);
                        this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                    }
                }
                else
                {
                    this.comm.ListenersCtrl.Start(listenerName, this.comm.PortName);
                }
            }
        }

        internal void StopListeners()
        {
            foreach (string str in this._SiRFAwareScanResultList)
            {
                try
                {
                    if (this.comm.ListenersCtrl != null)
                    {
                        this.comm.ListenersCtrl.Stop(str);
                        this.comm.ListenersCtrl.Delete(str);
                    }
                }
                catch
                {
                }
            }
            this._SiRFAwareScanResultList.Clear();
        }

        public void UpdateCurrentTime()
        {
            this._currTime = DateTime.Now;
            string str = string.Format("{0:D2}:{1:D2}:{2:D2}", this._currTime.Hour, this._currTime.Minute, this._currTime.Second);
            this.label_CurrentTime.Text = "Current Time: " + str;
            this.UpdateTimeSinceUpdate();
            this.UpdateLastUpdatedTimeGUI();
            this.UpdateTTFF();
        }

        public void UpdateLastUpdatedTime()
        {
            this._lastUpdateTime = DateTime.Now;
        }

        public void UpdateLastUpdatedTimeGUI()
        {
            DateTime time = new DateTime();
            if (!this._lastUpdateTime.Equals(time))
            {
                string str = string.Format("{0:D2}:{1:D2}:{2:D2}", this._lastUpdateTime.Hour, this._lastUpdateTime.Minute, this._lastUpdateTime.Second);
                this.label_LastUpdate.Text = "Last Update: " + str;
            }
            else
            {
                this.label_LastUpdate.Text = "Last Update: ";
            }
        }

        public void UpdateTimeSinceUpdate()
        {
            try
            {
                DateTime time = new DateTime();
                if (!this._lastUpdateTime.Equals(time))
                {
                    TimeSpan span = (TimeSpan) (this._currTime - this._lastUpdateTime);
                    string str = string.Format("{0} sec", (int) span.TotalSeconds);
                    this.label_TimeSinceUpdate.Text = "Time Since Update: " + str;
                }
                else
                {
                    this.label_TimeSinceUpdate.Text = "Time Since Update: ";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public void UpdateTTFF()
        {
            try
            {
                DateTime time = new DateTime();
                if (!this._resetTime.Equals(time))
                {
                    TimeSpan span = (TimeSpan) (this._currTime - this._resetTime);
                    if (!this._gotTTFF)
                    {
                        if (this.comm.dataGui._PMODE > 0)
                        {
                            if (span.TotalSeconds >= 1.0)
                            {
                                string str = string.Format("{0:F1} sec", span.TotalSeconds);
                                this.label_TTFF.Text = "TTFF: " + str;
                                this._gotTTFF = true;
                            }
                        }
                        else
                        {
                            this.ClearLastUpdatedTime();
                            this.ClearTTFFTime();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
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
                this.Text = this.comm.sourceDeviceName + ": SiRFaware";
            }
        }

        public bool MsgID68Update
        {
            get
            {
                return this.msgid68Update;
            }
            set
            {
                this.msgid68Update = value;
            }
        }

        public delegate void updateParentEventHandler(string titleString);

        public delegate void UpdateWindowEventHandler(string titleString, int left, int top, int width, int height, bool state);
    }
}

