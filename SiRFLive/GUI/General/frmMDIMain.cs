﻿using CommMgrClassLibrary;
using CommonClassLibrary;
using CommonUtilsClassLibrary;
using IronPython.Runtime.Exceptions;
using LogManagerClassLibrary;
using PerformanceMonitorClassLibrary;
using SiRFLive.Communication;
using SiRFLive.Configuration;
using SiRFLive.DeviceControl;
using SiRFLive.General;
using SiRFLive.GUI;
using SiRFLive.GUI.Automation;
using SiRFLive.GUI.Commmunication;
using SiRFLive.GUI.DeviceControl;
using SiRFLive.GUI.DlgsInputMsg;
using SiRFLive.GUI.Python;
using SiRFLive.GUI.Reporting;
using SiRFLive.MessageHandling;
using SiRFLive.Properties;
using SiRFLive.Reporting;
using SiRFLive.TruthData;
using SiRFLive.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml;

namespace SiRFLive.GUI.General
{
    public class frmMDIMain : Form
    {
        private XmlDocument _appWindowsSettings = new XmlDocument();
        private Color _currenPlotColor = Color.Red;
        private string _defaultWindowsRestoredFilePath;
        private int _epochIndex;
        private List<long> _epochList = new List<long>();
        private LargeFileHandler _fileHdlr;
        private long _fileIndex;
        private CommonClass.TransmissionType _fileType = CommonClass.TransmissionType.GP2;
        private bool _isExpand;
        private bool _isFileOpen;
        private _playStates _lastPlayState;
        private string _lastWindowsRestoredFilePath;
        private string _logFileName = string.Empty;
        private frmAutomationTests _objFrmAutoTest;
        private frmRFCaptureCtrl _objFrmCaptureCtrl;
        private frmCommInputMessage _objFrmCommInputMessage;
        private frmCommLocationMap _objFrmCommLocationMap;
        private frmCommOpen _objFrmCommOpen;
        private frmCommSignalView _objFrmCommSignalView;
        private frmCompareWithRef _objFrmCompareWithRefReport;
        private frmE911Report _objFrmE911Report;
        private frmEncryCtrl _objFrmEncryCtrl;
        //! private frmGPIBCtrl _objFrmGPIBCtrl;
        private frmLSMTestReport _objFrmLSMReport;
        private frmNavPerformanceReport _objFrmNavPerformanceReport;
        private frmPerformanceMonitor _objFrmPerfMonitor;
        private frmPRReport _objFrmPRReport;
        private frmPython _objFrmPython;
        private frmRackCtrl _objFrmRackCtrl;
        private frmRFPlaybackConfig _objFrmRFPlaybackConfig;
        private frmRFPlaybackCtrl _objFrmRFPlaybackCtrl;
        private frmSimplexCtrl _objFrmSimplexCtrl;
        private frmSPAzCtrl _objFrmSPAzCtrl;
        private frmRFSynthesizer _objFrmSynthesizer;
        internal Thread _parseThread;
        private _playStates _playState;
        private string _processFileLog = string.Empty;
        private string _regMatchString = string.Empty;
        private int _speedDelay = 0x19;
        private frmCommSVTrackedVsTime _SVTrackedVsTimePlotWin;
        private string _testName = string.Empty;
        private static int _toolStripConnectBtnIdx = (_toolStripPortNameComboBoxIdx + 3);
        private static int _toolStripPortNameComboBoxIdx = 0;
        private long _totalFileSize;
        private string _userWindowsRestoredFilePath;
        private CommonClass.TransmissionType _viewType = CommonClass.TransmissionType.GP2;
        private ToolStripMenuItem aboutMenu;
        private ToolStripMenuItem addReceiverToolStripMenuItem;
        private ToolStripMenuItem aidingConfigureToolStripMenuItem;
        private ToolStripMenuItem aidingsDownloadServerAssistedDataToolStripMenuItem;
        private ToolStripMenuItem aidingSummaryToolStripMenuItem;
        private ToolStripMenuItem aidingToolStripMenuItem;
        private ToolStripMenuItem aidingTTBToolStripMenuItem;
        private ToolStripMenuItem altitudeMeterToolStripMenuItem;
        private ToolStripMenuItem analysisToolStripMenuItem;
        private ToolStripMenuItem autoTest3GPPToolStripMenuItem;
        public SiRFLiveEvent AutoTestAbortHdlr = clsGlobal.AbortEvent;
        private ToolStripMenuItem autoTestAbortToolStripMenuItem;
        private ToolStripMenuItem autoTestAdvancedTestsToolStripMenuItem;
        private ToolStripMenuItem autoTestLoopitToolStripMenuItem;
        private ToolStripMenuItem autoTestStandardTestsToolStripMenuItem;
        private ToolStripMenuItem autoTestStatusToolStripMenuItem;
        private ToolStripMenuItem autoTestTIA916ToolStripMenuItem;
        private ToolStripMenuItem autoTestToolStripMenuItem;
        private ToolStripMenuItem averageCNoToolStripMenuItem;
        private ToolStripMenuItem binGPSToolStripMenuItem;
        private ToolStripMenuItem cascadeMenu;
        private ToolStripMenuItem closeAllMenu;
        private ToolStripMenuItem commandToolStripMenuItem;
        private ToolStripMenuItem compassToolStripMenuItem;
        private WinLocation CompassViewLocation = new WinLocation();
        private IContainer components;
        private ToolStripMenuItem configureDebugErrorLogToolStripMenuItem;
        private ToolStripMenuItem consoleToolStripMenuItem;
        private ToolStripMenuItem convertToolStripMenuItem;
        public string CurrentUser;
        private WinLocation DebugViewLocation = new WinLocation();
        private ToolStripMenuItem debugViewToolStripMenuItem;
        private ToolStripMenuItem defaultLayoutMenu;
        private ToolStripMenuItem developerDocMenu;
        private ToolStripMenuItem disable5HzNavToolStripMenuItem;
        private ToolStripMenuItem disableABPToolStripMenuItem;
        private ToolStripMenuItem disableMEMSToolStripMenuItem;
        private ToolStripMenuItem disableSBASRangingToolStripMenuItem;
        private ToolStripMenuItem dOPMaskToolStripMenuItem;
        private ToolStripMenuItem dRSensorsToolStripMenuItem;
        private ToolStripMenuItem elevationMaskToolStripMenuItem;
        private ToolStripMenuItem enable5HzNavToolStripMenuItem;
        private ToolStripMenuItem enableABPToolStripMenuItem;
        private ToolStripMenuItem enableMEMSToolStripMenuItem;
        private ToolStripMenuItem enableSBASRangingToolStripMenuItem;
        private ToolStripMenuItem errorToolStripMenuItem;
        private WinLocation ErrorViewLocation = new WinLocation();
        private ToolStripMenuItem ExtracttoolStripMenuItem;
        private ToolStripMenuItem featuresCWDetectionToolStripMenuItem;
        private ToolStripMenuItem featuresSiRFawareToolStripMenuItem;
        private ToolStripMenuItem featuresToolStripMenuItem;
        private ToolStripMenuItem fileCloseToolStripMenuItem;
        private ToolStripMenuItem fileExitToolStripMenuItem;
        private ToolStripMenuItem fileOpenToolStripMenuItem;
        private TrackBar filePlayBackTrackBar;
        private ToolStripMenuItem filePrintPreviewToolStripMenuItem;
        private ToolStripMenuItem filePrintToolStripMenuItem;
        private ToolStripMenuItem fileSaveAsToolStripMenuItem;
        private ToolStripMenuItem fileSaveToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private System.Timers.Timer gcTimer = new System.Timers.Timer();
        private ToolStripMenuItem gP2GPSToolStripMenuItem;
        private ToolStripMenuItem gPSNMEAToolStripMenuItem;
        private ToolStripMenuItem gPSToKMLToolStripMenuItem;
        private ToolStripMenuItem gyroFactoryCalibrationToolStripMenuItem;
        private ToolStripMenuItem helpMenuItem;
        public sysCmdExec HostAppCtrl;
        private ToolStripMenuItem iCConfigureToolStripMenuItem;
        private ToolStripMenuItem iCPeekPokeToolStripMenuItem;
        private WinLocation InputCommandLocation = new WinLocation();
        private ToolStripMenuItem inputCommandsToolStripMenuItem;
        private ToolStripMenuItem instrumentControlMenuItem;
        private WinLocation InterferenceLocation = new WinLocation();
        private bool isLoading;
        private WinLocation LocationMapLocation = new WinLocation();
        private ToolStripMenuItem logFileToolStripMenuItem;
        private Label logManagerStatusLabel;
        private ToolStripMenuItem lowPowerCommandsBufferToolStripMenuItem;
        private ToolStripMenuItem mapToolStripMenuItem;
        private ToolStripMenuItem MEMSToolStripMenuItem;
        private ToolStripMenuItem mEMSViewToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem messageToolStripMenuItem;
        private WinLocation MessageViewLocation = new WinLocation();
        private ToolStripMenuItem modeMaskToolStripMenuItem;
        private ToolStripMenuItem mPMToolStripMenuItem;
        private ToolStripMenuItem navAccuracyVsTimeToolStripMenuItem;
        private ToolStripMenuItem navigationModeControlToolStripMenuItem;
        private ToolStripMenuItem navigationToolStripMenuItem;
        private ToolStripMenuItem NMEAtoGPStoolStripMenuItem;
        private ToolStripMenuItem plotsToolStripMenuItem;
        private ToolStripMenuItem pointToPointAnalysisReportToolStripMenuItem;
        private ToolStripMenuItem pollAlmanacToolStripMenuItem;
        private ToolStripMenuItem pollEphemerisToolStripMenuItem;
        private ToolStripMenuItem pollNavParametersToolStripMenuItem;
        private ToolStripMenuItem pollSoftwareVesrionToolStripMenuItem;
        public Hashtable PortManagerHash = new Hashtable();
        private ToolStripMenuItem powerMaskToolStripMenuItem;
        private ToolStripMenuItem powerModeToolStripMenuItem;
        private ToolStripMenuItem predefinedToolStripMenuItem;
        private ToolStripMenuItem previousSettingsLayoutMenu;
        private ToolStripMenuItem radarToolStripMenuItem;
        private ToolStripMenuItem receiverConnectToolStripMenuItem;
        private ToolStripMenuItem receiverDisconnectToolStripMenuItem;
        private ToolStripMenuItem receiverToolStripMenuItem;
        private ToolStripMenuItem receiverViewCWDetectionToolStripMenuItem;
        private ToolStripMenuItem receiverViewSiRFawareToolStripMenuItem;
        private ToolStripMenuItem removeReceiverToolStripMenuItem;
        private ToolStripMenuItem report3GPPMenu;
        public Report ReportCtrl;
        private ToolStripMenuItem reportE911Menu;
        private ToolStripMenuItem reportlSMResetMenu;
        private ToolStripMenuItem reportMenuItem;
        private ToolStripMenuItem reportPerformanceMenu;
        private ToolStripMenuItem reportPseudoRangeMenu;
        private ToolStripMenuItem reportResetMenu;
        private ToolStripMenuItem reportTIA916Menu;
        private ToolStripMenuItem resetToolStripMenuItem;
        private WinLocation ResponseViewLocation = new WinLocation();
        private ToolStripMenuItem responseViewToolStripMenuItem;
        private ToolStripMenuItem restoreLayoutMenuItem;
        private ToolStripMenuItem rfPlaybackCaptureMenu;
        private ToolStripMenuItem rfReplayConfigurationMenu;
        private ToolStripMenuItem rFReplayMenuItem;
        private ToolStripMenuItem rfReplayPlaybackMenu;
        private ToolStripMenuItem rfReplaySynthesizerMenu;
        private ToolStripMenuItem rinexToEphToolStripMenuItem;
        private ToolStripMenuItem satellitesStatisticsToolStripMenuItem;
        private WinLocation SatelliteStatsLocation = new WinLocation();
        private ToolStripMenuItem saveLayoutMenu;
        private ToolStripMenuItem sBASRangingToolStripMenuItem;
        private ToolStripMenuItem sDOGenerationToolStripMenuItem;
        private ToolStripMenuItem set5HzNavToolStripMenuItem;
        private ToolStripMenuItem setABPToolStripMenuItem;
        private ToolStripMenuItem setAlmanacToolStripMenuItem;
        private ToolStripMenuItem setDebugLevelsToolStripMenuItem;
        private ToolStripMenuItem setDGPSToolStripMenuItem;
        private ToolStripMenuItem setEEToolStripMenuItem;
        private ToolStripMenuItem setEphemerisToolStripMenuItem;
        private ToolStripMenuItem setMEMSToolStripMenuItem;
        private ToolStripMenuItem setPollToolStripMenuItem;
        private ToolStripMenuItem setReferenceLocationToolStripMenuItem;
        //! public GPIB_Mgr_Agilent_HP8648C SigGenCtrl;
        private ToolStripMenuItem signalGeneratorMenu;
        private ToolStripMenuItem signalToolStripMenuItem;
        private WinLocation SignalViewLocation = new WinLocation();
        public Simplex SimCtrl;
        private ToolStripMenuItem simplexMenu;
        private WinLocation SiRFawareLocation = new WinLocation();
        private ToolStripMenuItem siRFDRiveSensorToolStripMenuItem;
        private ToolStripMenuItem siRFDRiveStatusToolStripMenuItem;
        private ToolStripMenuItem siRFDRiveToolStripMenuItem;
        private List<Thread> sleepThreads = new List<Thread>();
        public SPAzMgr SpazCtrl;
        private ToolStripMenuItem sPAzMenu;
        private ToolStripMenuItem startLogToolStripMenuItem;
        private ToolStripMenuItem staticNavToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem stopLogToolStripMenuItem;
        private WinLocation SVsMapLocation = new WinLocation();
        private ToolStripMenuItem sVTrackedVsTimeToolStripMenuItem;
        private ToolStripMenuItem sVTrajectoryToolStripMenuItem;
        private ToolStripMenuItem switchOperationModeToolStripMenuItem;
        private ToolStripMenuItem switchPowerModeToolStripMenuItem;
        private ToolStripMenuItem switchProtocolsToolStripMenuItem;
        public TestRackMgr TestRackCtrl;
        private ToolStripMenuItem testRackMenu;
        private ToolStripMenuItem tileHorizontalMenu;
        private ToolStripMenuItem tileVerticalMenu;
        private ToolStripButton toolStripBackBtn;
        private ToolStripButton toolStripCloseFileBtn;
        private ToolStripButton toolStripCompassViewBtn;
        private ToolStripContainer toolStripContainer1;
        private ToolStripButton toolStripDebugViewBtn;
        private ToolStripButton toolStripErrorViewBtn;
        private ToolStripButton toolStripHelpBtn;
        private ToolStripStatusLabel toolStripLogStatusLabel;
        private ToolStrip toolStripMain;
        private ToolStripButton toolStripMapViewBtn;
        private ToolStripMenuItem toolStripMenuItem_Plot;
        private ToolStripButton toolStripMessageViewBtn;
        private ToolStripButton toolStripNextBtn;
        private ToolStripTextBox toolStripNumPortTxtBox;
        private ToolStripButton toolStripOpenFileBtn;
        private ToolStripButton toolStripPause;
        private ToolStripButton toolStripPauseBtn;
        private ToolStripButton toolStripPlayBtn;
        private ToolStripComboBox toolStripPortComboBox;
        private ToolStripButton toolStripPortConfigBtn;
        private ToolStripButton toolStripPortOpenBtn;
        private ToolStripButton toolStripRadarViewBtn;
        private ToolStripButton toolStripResetBtn;
        private ToolStripButton toolStripResponseViewBtn;
        private ToolStripButton toolStripSaveBtn;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripSeparator toolStripSeparator14;
        private ToolStripSeparator toolStripSeparator16;
        private ToolStripSeparator toolStripSeparator17;
        private ToolStripSeparator toolStripSeparator18;
        private ToolStripSeparator toolStripSeparator19;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator20;
        private ToolStripSeparator toolStripSeparator21;
        private ToolStripSeparator toolStripSeparator22;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripButton toolStripSignalViewBtn;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolStripButton toolStripStopBtn;
        private ToolStripButton toolStripTTFFViewBtn;
        private ToolStripButton toolStripUpDownArrowBtn;
        private ToolStripButton toolStripUserTextBtn;
        private ToolStripMenuItem TTBConfigureTimeAidingToolStripMenuItem;
        private ToolStripMenuItem TTBConnectToolStripMenuItem;
        private ToolStripMenuItem TTBViewToolStripMenuItem;
        private ToolStripMenuItem tTFFAndNavAccuracyToolStripMenuItem;
        private WinLocation TTFFDisplayLocation = new WinLocation();
        private ToolStripMenuItem tTFSToolStripMenuItem;
        public UserAccessMgr UserAccess;
        private ToolStripMenuItem userDefinedToolStripMenuItem;
        private ToolStripMenuItem userManualMenu;
        private ToolStripMenuItem userSettingsLayoutMenu;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem windowMenuItem;

        internal event updateParentEventHandler updateParent;

        public frmMDIMain()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            clsGlobal.MyCulture = Thread.CurrentThread.CurrentCulture;
            clsGlobal.SiRFLiveAppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            clsGlobal.InstalledDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
            clsGlobal.SiRFLiveAppConfig.AppSettings.Settings.Remove("InstalledDirectory");
            clsGlobal.SiRFLiveAppConfig.AppSettings.Settings.Add("InstalledDirectory", clsGlobal.InstalledDirectory);
            clsGlobal.SiRFLiveAppConfig.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            string xmlFile = clsGlobal.InstalledDirectory + @"\Config\UserAccessConfig.xml";

            UserAccess = new UserAccessMgr(xmlFile);
            CurrentUser = UserAccess.GetCurrentUser();
            clsGlobal.CurrentUser = CurrentUser;
            clsGlobal.userAccessNum = UserAccess.GetAccessNum();
            clsGlobal.SiRFLiveVersion = "SiRFLive " + SiRFLiveVersion.VersionNum + " " + CurrentUser;
            clsGlobal.SiRFLiveChangeNum = StripDollarSigns(SiRFLiveVersion.ChangeNum);
            clsGlobal.SiRFLiveChangeDate = StripDollarSigns(SiRFLiveVersion.DateTime);
            Text = clsGlobal.SiRFLiveVersion;
            _defaultWindowsRestoredFilePath = clsGlobal.InstalledDirectory + @"\Config\DefaultWindowsRestore.xml";
            _lastWindowsRestoredFilePath = clsGlobal.InstalledDirectory + @"\Config\LastWindowsRestore.xml";
            SearchMenu(CurrentUser);
            SearchToolStrip(CurrentUser);
            try
            {
                IniHelper helper = new IniHelper(clsGlobal.InstalledDirectory + @"\Config\SiRFLiveAutomation.cfg");
                clsGlobal.ResetPeriodRandomizationSec = int.Parse(helper.GetIniFileString("SETUP", "RESET_PERIOD_RANDOMIZATION_SEC", "5"));
            }
            catch
            {
                clsGlobal.ResetPeriodRandomizationSec = 5;
            }
            ReportCtrl = new Report();
            HostAppCtrl = new sysCmdExec();
            SpazCtrl = new SPAzMgr(0x378);
            SimCtrl = new Simplex();
            gcTimer.Elapsed += new ElapsedEventHandler(OnGCTimerEvent);
            gcTimer.Interval = 1800000.0;
            gcTimer.AutoReset = true;
            setDefaultWindowsLocation();
        }

        public void Abort()
        {
            clsGlobal.ScriptDone = true;
            AutoTestAbortHdlr.SiRFLiveEventSet();
        }

        public void AbortDelay()
        {
            foreach (Thread thread in sleepThreads)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
            }
            sleepThreads.Clear();
        }

        private void aboutMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("SiRFLive (c) 2009-2010\n\nSiRF Technology Inc.\nA CSR plc Company\n\nA tool for real time GPS data collection and interaction.\nFeatures:\n\tL)ogging\n\tI)interactivity\n\tV)erification\n\tE)valuation\n\n{0}\n{1}\n{2}", clsGlobal.SiRFLiveVersion, clsGlobal.SiRFLiveChangeNum, clsGlobal.SiRFLiveChangeDate), "About SiRFLive", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private ToolStrip AddPortToolbar(int yLocation, string portName)
        {
            ToolStripSeparator separator = new ToolStripSeparator();
            ToolStripSeparator separator2 = new ToolStripSeparator();
            ToolStripSeparator separator3 = new ToolStripSeparator();
            ToolStripSeparator separator4 = new ToolStripSeparator();
            ToolStripSeparator separator5 = new ToolStripSeparator();
            ToolStripComboBox box = new ToolStripComboBox();
            ToolStripButton button = new ToolStripButton();
            ToolStripButton button2 = new ToolStripButton();
            ToolStripButton button3 = new ToolStripButton();
            ToolStripButton button4 = new ToolStripButton();
            ToolStripButton button5 = new ToolStripButton();
            ToolStripButton button6 = new ToolStripButton();
            ToolStripButton button7 = new ToolStripButton();
            ToolStripButton button8 = new ToolStripButton();
            ToolStripButton button9 = new ToolStripButton();
            ToolStripButton button10 = new ToolStripButton();
            ToolStrip strip = new ToolStrip();
            ToolStripButton button11 = new ToolStripButton();
            separator.Name = "toolStripSeparator1";
            separator.Size = new Size(6, 0x19);
            separator2.Name = "toolStripSeparator2";
            separator2.Size = new Size(6, 0x19);
            separator3.Name = "toolStripSeparator3";
            separator3.Size = new Size(6, 0x19);
            separator4.Name = "toolStripSeparator4";
            separator4.Size = new Size(6, 0x19);
            separator5.Name = "toolStripSeparator5";
            separator5.Size = new Size(6, 0x19);
            box.Name = "toolStripPortComboBox_" + portName;
            box.Size = new Size(0x79, 0x19);
            button.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button.Image = Resources.disconnect;
            button.ImageTransparentColor = Color.Magenta;
            button.Name = "toolStripPortOpenBtn_" + portName;
            button.Size = new Size(0x17, 0x16);
            button.Text = "Connect";
            button.Click += new EventHandler(perPortToolStripPortOpenBtn_Click);
            button2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button2.Image = Resources.log;
            button2.ImageTransparentColor = Color.Magenta;
            button2.Name = "toolStripSaveBtn_" + portName;
            button2.Size = new Size(0x17, 0x16);
            button2.Text = "Log File";
            button3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button3.Image = Resources.Reset;
            button3.ImageTransparentColor = Color.Magenta;
            button3.Name = "toolStripResetBtn_" + portName;
            button3.Size = new Size(0x17, 0x16);
            button3.Text = "Reset";
            button4.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button4.Image = Resources.signal;
            button4.ImageTransparentColor = Color.Magenta;
            button4.Name = "toolStripSignalViewBtn_" + portName;
            button4.Size = new Size(0x17, 0x16);
            button4.Text = "Signal View";
            button4.Click += new EventHandler(perPortToolStripSignalViewBtn_Click);
            button5.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button5.Image = Resources.radar;
            button5.ImageTransparentColor = Color.Magenta;
            button5.Name = "toolStripRadarViewBtn_" + portName;
            button5.Size = new Size(0x17, 0x16);
            button5.Text = "Radar View";
            button5.Click += new EventHandler(perPortToolStripRadarViewBtn_Click);
            button6.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button6.Image = Resources.map;
            button6.ImageTransparentColor = Color.Magenta;
            button6.Name = "toolStripMapViewBtn_" + portName;
            button6.Size = new Size(0x17, 0x16);
            button6.Text = "Map View";
            button6.Click += new EventHandler(perPortToolStripLocationViewBtn_Click);
            button7.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button7.Image = Resources.ttff;
            button7.ImageTransparentColor = Color.Magenta;
            button7.Name = "toolStripTTFFViewBtn_" + portName;
            button7.Size = new Size(0x17, 0x16);
            button7.Text = "TTFF View";
            button7.Click += new EventHandler(perPortToolStripTTFFViewBtn_Click);
            button8.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button8.Image = Resources.ResponseViewHS;
            button8.ImageTransparentColor = Color.Magenta;
            button8.Name = "toolStripResponseViewBtn_" + portName;
            button8.Size = new Size(0x17, 0x16);
            button8.Text = "Response View";
            button8.Click += new EventHandler(perPortToolStripResponseViewBtn_Click);
            button9.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button9.Image = Resources.DebugViewHS;
            button9.ImageTransparentColor = Color.Magenta;
            button9.Name = "toolStripDebugViewBtn_" + portName;
            button9.Size = new Size(0x17, 0x16);
            button9.Text = "Debug View";
            button9.Click += new EventHandler(perPortToolStripDebugViewBtn_Click);
            button10.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button10.Image = Resources.ErrorHS;
            button10.ImageTransparentColor = Color.Magenta;
            button10.Name = "toolStripErrorViewBtn_" + portName;
            button10.Size = new Size(0x17, 0x16);
            button10.Text = "Error View";
            button10.Click += new EventHandler(perPortToolStripErrorViewBtn_Click);
            strip.Items.AddRange(new ToolStripItem[] { 
                box, separator, button11, button, separator2, button2, separator3, button3, separator4, button4, button5, button6, button7, button8, button9, button10, 
                separator5
             });
            strip.Location = new Point(0, yLocation + 0x19);
            strip.Name = "toolStripMain";
            strip.Size = new Size(0x242, 0x19);
            strip.TabIndex = 5;
            strip.Text = "Tool Bar";
            button11.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button11.Image = Resources.synchronization;
            button11.ImageTransparentColor = Color.Magenta;
            button11.Name = "toolStripPortConfigBtn_" + portName;
            button11.Size = new Size(0x17, 0x16);
            button11.Text = "Receiver Settings";
            strip.Dock = DockStyle.None;
            strip.Visible = false;
            base.Controls.Add(strip);
            return strip;
        }

        private void addReceiverMenu_Click(object sender, EventArgs e)
        {
            callPortConfig();
        }

        private ToolStripMenuItem AddToWindowTab(string name)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Name = name + "WinTabItem";
            item.Size = new Size(0x98, 0x16);
            item.Text = name;
            windowMenuItem.DropDownItems.Add(item);
            clsGlobal.WindowsTab.Add(item.Text, item);
            return item;
        }

        private void aidingConfigureClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    createAutoReplyWindow();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        createAutoReplyWindow(ref target);
                    }
                }
            }
        }

        private void aidingConfigureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aidingConfigureClickHandler();
        }

        private void aidingDownloadServerAssistedDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void aidingSummaryClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    createAutoReplySummaryWindow();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        createAutoReplySummaryWindow(target);
                    }
                }
            }
        }

        private void aidingSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aidingSummaryClickHandler();
        }

        private void aidingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0)) && (toolStripPortComboBox.Text != "All"))
            {
                clsGlobal.PerformOnAll = false;
                PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                {
                    if (!manager.TTBWinLocation.IsOpen)
                    {
                        aidingTTBToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        aidingTTBToolStripMenuItem.Enabled = false;
                    }
                    if (manager.comm.TTBPort.IsOpen)
                    {
                        TTBConnectToolStripMenuItem.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        TTBConnectToolStripMenuItem.CheckState = CheckState.Unchecked;
                    }
                }
            }
        }

        private void aidingTTBToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (!clsGlobal.ScriptDone || checkLoopit())
            {
                TTBViewToolStripMenuItem.Enabled = false;
            }
            else
            {
                TTBViewToolStripMenuItem.Enabled = true;
            }
        }

        private void autoTest3GPPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setup3GPPTest("3GPP");
        }

        private void autoTestAbortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult none = DialogResult.None;
            if (!clsGlobal.ScriptDone)
            {
                none = MessageBox.Show("Test is running. Abort?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else
            {
                none = DialogResult.Yes;
            }
            if (none != DialogResult.No)
            {
                clsGlobal.AbortSingle = true;
                clsGlobal.Abort = true;
                _objFrmPython.PythonEngineOutput.CloseFile();
                _objFrmPython.WriteLine("Test aborted!");
                try
                {
                    if (frmRFPlaybackCtrl.GetChildInstance() != null)
                    {
                        frmRFPlaybackCtrl.GetChildInstance().PlaybackStop();
                    }
                }
                catch
                {
                }
                clsGlobal.g_objfrmMDIMain.CancelDelay();
                clsGlobal.AbortEvent.SiRFLiveEventSet();
                _objFrmPython.engine.Shutdown();
                foreach (string str in clsGlobal.g_objfrmMDIMain.PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) clsGlobal.g_objfrmMDIMain.PortManagerHash[str];
                    if ((manager != null) && (manager.comm != null))
                    {
                        if (manager.comm.RxCtrl != null)
                        {
                            if (manager.comm.RxCtrl.ResetCtrl != null)
                            {
                                manager.comm.RxCtrl.ResetCtrl.ResetTimerStop(true);
                                manager.comm.RxCtrl.ResetCtrl.CloseTTFFLog();
                            }
                            manager.comm.RxCtrl.LogCleanup();
                        }
                        manager.comm.ClosePort();
                    }
                }
                clsGlobal.ScriptDone = true;
                _objFrmPython.engine.Dispose();
                UpdateGUIFromScript();
            }
        }

        private void autoTestAdvancedTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)
            {
                if (!clsGlobal.ScriptDone)
                    MessageBox.Show("Test is running. Please use \"Abort\" button on the Automation Test Window to abort", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else if (base.MdiChildren.Length <= 0)
                    CreateAutomationTestWindow();

                else if (MessageBox.Show("All windows will be closed. Proceed?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    EventHandler method = null;
                    foreach (Form childForm in base.MdiChildren)
                    {
                        if (childForm.Name != "frmAutomationTests")
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    childForm.Close();
                                };
                            }
                            base.Invoke(method);
                        }
                    }
                    CreateAutomationTestWindow();
                }
            }
        }

        private void autoTestLoopitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.ScriptDone)
            {
                MessageBox.Show("Automation test is running. Please abort the current test before proceeding!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (!checkLoopit())
            {
                if (MessageBox.Show(string.Format("*** {0}\n\t{1}\n\n*** {2}\n\t{3}\n\n\t\t\t     {4}", new object[] { "For position accuracy evaluation, the reference location needs to be set correctly.", "To set reference location: Receiver --> Set Reference Location", "For AGPS test, AGPS parameters need to be set correctly.", "To set AGPS configuration: AGPS --> Configure...", "Proceed?" }), "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
                {
                    new frmLoopit().ShowDialog();
                    foreach (string port in PortManagerHash.Keys)
                    {
                        if (!(port == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager manager = (PortManager) PortManagerHash[port];
                            if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                            {
                                manager.comm.m_TestSetup.RXCommIntf = manager.comm.PortName;
                                manager.comm.m_TestSetup.qosParams.LocMethod = manager.comm.AutoReplyCtrl.PositionRequestCtrl.LocMethod.ToString();
                                manager.comm.m_TestSetup.qosParams.NumFixes = manager.comm.AutoReplyCtrl.PositionRequestCtrl.NumFixes.ToString();
                                manager.comm.m_TestSetup.qosParams.TBFixes = manager.comm.AutoReplyCtrl.PositionRequestCtrl.TimeBtwFixes.ToString();
                                manager.comm.m_TestSetup.qosParams.TAccPriority = manager.comm.AutoReplyCtrl.PositionRequestCtrl.TimeAccPriority.ToString();
                                manager.comm.m_TestSetup.qosParams.RespTMax = manager.comm.AutoReplyCtrl.PositionRequestCtrl.RespTimeMax.ToString();
                                manager.comm.m_TestSetup.qosParams.Position2DError = manager.comm.AutoReplyCtrl.PositionRequestCtrl.HorrErrMax.ToString();
                                manager.comm.m_TestSetup.qosParams.Position3DError = manager.comm.AutoReplyCtrl.PositionRequestCtrl.VertErrMax.ToString();
                                clsGlobal.LoopitInProgress |= manager.comm.RxCtrl.ResetCtrl.LoopitInprogress;
                            }
                        }
                    }
                }
            }
            else if (MessageBox.Show("Loopit in progress - Abort?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                StopLoopit();
                MessageBox.Show("Loopit Aborted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void autoTestStart()
        {
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
            _objFrmPython.testResult = "Hi,\nTest Summary:\n\n";
            string machineName = Environment.MachineName;
            string.Format("Hi,\nTest Station: {0}\nTest Summary:\n\n", machineName);
            clsGlobal.ScriptDone = true;
            clsGlobal.Abort = false;
            clsGlobal.AbortSingle = false;
            bool flag = false;
            string fileName = clsGlobal.InstalledDirectory + @"\scripts\core\rxSetup.py";
            string str3 = clsGlobal.InstalledDirectory + @"\scripts\core\portSetup.py";
            try
            {
                _objFrmPython.engine.ExecuteFile(fileName);
                goto Label_00A4;
            }
            catch (Exception exception)
            {
                _objFrmPython.PythonEngineOutput.CloseFile();
                displaySingleTestError(exception.Message);
                return;
            }
        Label_009D:
            Thread.Sleep(100);
        Label_00A4:
            if (!clsGlobal.ScriptDone)
            {
                goto Label_009D;
            }
            if (clsGlobal.ScriptError || clsGlobal.Abort)
            {
                MessageBox.Show("Setup script encountered error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                flag = true;
            }
            if (flag)
            {
                goto Label_013A;
            }
            try
            {
                _objFrmPython.engine.ExecuteFile(str3);
                goto Label_0110;
            }
            catch (Exception exception2)
            {
                _objFrmPython.PythonEngineOutput.CloseFile();
                displaySingleTestError(exception2.Message);
                return;
            }
        Label_0109:
            Thread.Sleep(100);
        Label_0110:
            if (!clsGlobal.ScriptDone)
            {
                goto Label_0109;
            }
            if (clsGlobal.ScriptError || clsGlobal.Abort)
            {
                MessageBox.Show("Port setup script encountered error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                flag = true;
            }
        Label_013A:
            if (flag)
            {
                goto Label_023D;
            }
            try
            {
                _objFrmPython.engine.ExecuteFile(clsGlobal.TestScriptPath);
                goto Label_01FC;
            }
            catch (PythonSyntaxErrorException exception3)
            {
                _objFrmPython.WriteLine("Syntax exception:");
                _objFrmPython.WriteLine(string.Format("Message: {0}\nLineText: {1}\nLine:{2}\nColumn: {3}", new object[] { exception3.Message, exception3.LineText, exception3.Line, exception3.Column }));
                displaySingleTestError(exception3.Message);
                _objFrmPython.PythonEngineOutput.CloseFile();
                return;
            }
        Label_01E4:
            if (!clsGlobal.AbortSingle)
            {
                Thread.Sleep(150);
            }
            if (clsGlobal.Abort)
            {
                goto Label_0203;
            }
        Label_01FC:
            if (!clsGlobal.ScriptDone)
            {
                goto Label_01E4;
            }
        Label_0203:
            if (clsGlobal.Abort)
            {
                MessageBox.Show("Test Aborted!", "Automation Test", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Completed!", _testName), "Automation Test", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        Label_023D:
		_objFrmPython.BeginInvoke((MethodInvoker)delegate
		{
                _objFrmPython.PythonEngineOutput.CloseFile();
                _objFrmPython.Close();
                _objFrmPython.Dispose();
                _objFrmPython = null;
            });
            try
            {
                clsGlobal.TestsToRun.Clear();
                clsGlobal.CurrentRunningTest = string.Empty;
            }
            catch
            {
            }
        }

        private void autoTestStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Tests To Run: \n");
            foreach (string str in clsGlobal.TestsToRun)
            {
                builder.Append("\t" + str);
                builder.Append("\n");
            }
            builder.Append("\nCurrent Running Test: " + clsGlobal.CurrentRunningTest);
            builder.Append(string.Format("\n\nCompleted Tests: {0}/{1}\n", clsGlobal.DoneTests.Count, clsGlobal.TestsToRun.Count));
            foreach (string str2 in clsGlobal.DoneTests)
            {
                builder.Append("\t" + str2);
                builder.Append("\n");
            }
            MessageBox.Show(builder.ToString(), "Automation Test Status", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void autoTestTIA916ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setup3GPPTest("TIA916");
        }

        private void averageCNoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVCNoViewClickHandler();
        }

        private void binGPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFileConversionWindow(ConversionType.BinToGPS_GP2);
        }

        private PortManager callCreateRxSettingsWindow()
        {
            PortManager manager = new PortManager();
            if (createRxSettingsWindow(ref manager.comm))
            {
                if (PortManagerHash.ContainsKey(manager.comm.PortName))
                {
                    PortManager manager2 = (PortManager) PortManagerHash[manager.comm.PortName];
                    if (manager2 != null)
                    {
                        manager = manager2;
                    }
                }
                if (!manualConnect(ref manager.comm))
                {
                    return null;
                }
                if (manager != null)
                {
                    if (PortManagerHash.ContainsKey(manager.comm.PortName))
                    {
                        PortManagerHash[manager.comm.PortName] = manager;
                    }
                    else
                    {
                        PortManagerHash.Add(manager.comm.PortName, manager);
                    }
                    manager.RunAsyncProcess();
                    manager.comm.Log.DurationLoggingStatusLabel = logManagerStatusLabel;
                    manager.comm.Log.UpdateMainWindow += new LogManager.UpdateParentEventHandler(updateLogFileBtn);
                    manager.comm.UpdatePortMainWinTitle += new CommunicationManager.UpdateParentPortEventHandler(UpdateWinTitle);
                }
                return manager;
            }
            manager.comm.Dispose();
            manager.comm = null;
            manager = null;
            return null;
        }

        private PortManager callCreateRxSettingsWindow(PortManager target)
        {
            if (target == null)
            {
                return target;
            }
            if (target.comm != null)
            {
                string portName = target.comm.PortName;
                if (!createRxSettingsWindow(ref target.comm))
                {
                    return null;
                }
                if (manualConnect(ref target.comm))
                {
                    if (portName != target.comm.PortName)
                    {
                        PortManagerHash.Remove(portName);
                        updateToolStripPortComboBox(portName, false);
                    }
                    if (target != null)
                    {
                        if (PortManagerHash.ContainsKey(target.comm.PortName))
                        {
                            PortManagerHash[target.comm.PortName] = target;
                        }
                        else
                        {
                            PortManagerHash.Add(target.comm.PortName, target);
                        }
                        target.RunAsyncProcess();
                        target.comm.Log.DurationLoggingStatusLabel = logManagerStatusLabel;
                        target.comm.Log.UpdateMainWindow += new LogManager.UpdateParentEventHandler(updateLogFileBtn);
                        target.comm.UpdatePortMainWinTitle += new CommunicationManager.UpdateParentPortEventHandler(UpdateWinTitle);
                    }
                    updateToolStripPortComboBox(target.comm.PortName, true);
                    return target;
                }
                MessageBox.Show(target.comm.ConnectErrorString, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return null;
        }

        private void callPortConfig()
        {
            PortManager target = null;
            if (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)
            {
                if (clsGlobal.IsMarketingUser())
                {
                    if (PortManagerHash.Count <= 0)
                    {
                        target = callCreateRxSettingsWindow();
                    }
                    else if (!PortManagerHash.ContainsKey(toolStripPortComboBox.Text))
                    {
                        target = callCreateRxSettingsWindow();
                    }
                    else
                    {
                        target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                        if (target != null)
                        {
                            target.CloseAll();
                            target = callCreateRxSettingsWindow();
                        }
                    }
                }
                else
                {
                    target = callCreateRxSettingsWindow();
                }
                if (target != null)
                {
                    updateToolStripPortComboBox(target.comm.PortName, true);
                    target.UpdateMainWindow += new PortManager.updateParentEventHandler(updateMainWindowTitle);
                    toolStripNumPortTxtBox.Text = PortManagerHash.Count.ToString();
                    target.PerPortToolStrip = AddPortToolbar((toolStripMain.Location.Y + (0x19 * PortManagerHash.Count)) + 0x23, target.comm.PortName);
                    if (clsGlobal.IsMarketingUser())
                    {
                        restoreDefaultPortLayout(false);
                    }
                    else
                    {
                        if (!target.SignalViewLocation.IsOpen)
                        {
                            CreateSignalViewWin(target);
                        }
                        if (!target.DebugViewLocation.IsOpen)
                        {
                            CreateDebugViewWin(target);
                        }
                    }
                    setSubWindowsVisibilty(target, true);
                    updateGUIOnConnectNDisconnect(target);
                }
            }
        }

        public void CancelDelay()
        {
            try
            {
                List<int> list = new List<int>();
                for (int i = 0; i < sleepThreads.Count; i++)
                {
                    Thread thread = sleepThreads[i];
                    if (thread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
                    {
                        thread.Interrupt();
                        list.Add(i);
                    }
                }
                foreach (int num2 in list)
                {
                    if (num2 < sleepThreads.Count)
                    {
                        sleepThreads.RemoveAt(num2);
                    }
                }
                list.Clear();
            }
            catch
            {
            }
        }

        private void cascadeMenu_Click(object sender, EventArgs e)
        {
            base.LayoutMdi(MdiLayout.Cascade);
        }

        public void ChangeActionAutoTestState(bool state)
        {
            base.Invoke((MethodInvoker)delegate {
                clsGlobal.IsMarketingUser();
            });
        }

        public void ChangeRestoreLayoutState(bool state)
        {
            EventHandler method = null;
            try
            {
                if (method == null)
                {
                    method = delegate {
                        previousSettingsLayoutMenu.Enabled = state;
                        userSettingsLayoutMenu.Enabled = state;
                        clsGlobal.IsMarketingUser();
                        ChangeActionAutoTestState(!state);
                    };
                }
                base.Invoke(method);
            }
            catch
            {
            }
        }

        private bool checkLoopit()
        {
            bool flag = false;
            foreach (string str in PortManagerHash.Keys)
            {
                if (!(str == clsGlobal.FilePlayBackPortName))
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag |= manager.comm.RxCtrl.ResetCtrl.LoopitInprogress;
                        manager.comm.Log.DurationLoggingStatusLabel = logManagerStatusLabel;
                        if ((manager._ttbWin != null) && manager.TTBWinLocation.IsOpen)
                        {
                            manager._ttbWin.Close();
                            manager.comm.TTBPort.Open();
                            manager.ReconnectTTB = false;
                        }
                    }
                }
            }
            return flag;
        }

        public int CheckRxSetup(string configFilePath)
        {
            IniHelper helper = new IniHelper(configFilePath);
            return helper.IniSiRFLiveRxSetupErrorCheck(configFilePath);
        }

        public void Cleanup()
        {
            if (!clsGlobal.ScriptDone)
            {
                clsGlobal.ScriptDone = true;
                if ((_objFrmPython != null) && _objFrmPython.IsDisposed)
                {
                    _objFrmPython.Close();
                }
            }
            foreach (Thread thread in sleepThreads)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
            }
            sleepThreads.Clear();
            List<int> list = new List<int>();
            foreach (int num in sysCmdExec.RunProgramList.Keys)
            {
                list.Add(num);
            }
            foreach (int num2 in list)
            {
                Process process = (Process) sysCmdExec.RunProgramList[num2];
                try
                {
                    process.CloseMainWindow();
                }
                catch
                {
                }
            }
            sysCmdExec.RunProgramList.Clear();
            list.Clear();
            closeAllPort();
        }

        private void cleanupPortManager()
        {
            ClearPortList();
        }

        private void clearAutomationStatus()
        {
            clsGlobal.TestsToRun.Clear();
            clsGlobal.DoneTests.Clear();
            clsGlobal.CurrentRunningTest = string.Empty;
        }

        public void ClearPortList()
        {
            EventHandler method = null;
            if (!clsGlobal.IsMarketingUser())
            {
                if (method == null)
                {
                    method = delegate {
                        foreach (string str in PortManagerHash.Keys)
                        {
                            PortManager manager = (PortManager) PortManagerHash[str];
                            if (((manager != null) && (manager.comm != null)) && manager.comm.IsSourceDeviceOpen())
                            {
                                manager.comm.ClosePort();
                            }
                            manager.CloseAll();
                            if (manager.PerPortToolStrip != null)
                            {
                                manager.PerPortToolStrip.Dispose();
                            }
                            manager.PerPortToolStrip = null;
                            manager.comm.Dispose();
                            manager.comm = null;
                            manager = null;
                        }
                        int count = PortManagerHash.Count;
                        for (int j = 0; j < count; j++)
                        {
                            PortManagerHash[j] = null;
                        }
                        PortManagerHash.Clear();
                        toolStripPortComboBox.Items.Clear();
                    };
                }
                base.Invoke(method);
            }
            GC.GetTotalMemory(true);
        }

        private void closeAllMenu_Click(object sender, EventArgs e)
        {
            closeAllWindows();
        }

        private void closeAllPort()
        {
            foreach (string str in PortManagerHash.Keys)
            {
                PortManager manager = (PortManager) PortManagerHash[str];
                if (manager != null)
                {
                    manager.CloseAll();
                }
            }
            PortManagerHash.Clear();
        }

        private void closeAllWindows()
        {
            EventHandler method = null;
            foreach (Form childForm in base.MdiChildren)
            {
                if (childForm.Name == "frmPython")
                {
                    if (method == null)
                    {
                        method = delegate {
                            childForm.Close();
                        };
                    }
                    base.Invoke(method);
                }
            }
            EventHandler handler2 = null;
            foreach (Form childForm in base.MdiChildren)
            {
                if (handler2 == null)
                {
                    handler2 = delegate {
                        childForm.Close();
                    };
                }
                base.Invoke(handler2);
            }
            updateAllMainBtn();
        }

        private void commandToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (commandToolStripMenuItem.Enabled && ((toolStripPortComboBox.Text != "All") && (toolStripPortComboBox.Text != string.Empty)))
            {
                PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                {
                    if (manager.comm.MessageProtocol == "NMEA")
                    {
                        enableDisableMenuAndButtonPerProtocol(false);
                    }
                    else
                    {
                        enableDisableMenuAndButtonPerProtocol(true);
                    }
                    if (manager.comm.ProductFamily == CommonClass.ProductType.GSD4t)
                    {
                        enableDisableMenuAndButtonPerProductType(false);
                        switchProtocolsToolStripMenuItem.Enabled = false;
                    }
                    else if (manager.comm.ProductFamily == CommonClass.ProductType.GSD4e)
                    {
                        enableDisableMenuAndButtonPerProductType(true);
                        switchProtocolsToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        enableDisableMenuAndButtonPerProductType(false);
                        switchProtocolsToolStripMenuItem.Enabled = true;
                    }
                }
            }
        }

        private void commandToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != "All") && (toolStripPortComboBox.Text != string.Empty))
            {
                PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if (manager.comm.IsSourceDeviceOpen())
                {
                    if (manager.comm.ProductFamily == CommonClass.ProductType.GSD4t)
                    {
                        enableDisableMenuAndButtonPerProductType(false);
                        switchProtocolsToolStripMenuItem.Enabled = false;
                    }
                    else if (manager.comm.ProductFamily == CommonClass.ProductType.GSD4e)
                    {
                        enableDisableMenuAndButtonPerProductType(true);
                        switchProtocolsToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        enableDisableMenuAndButtonPerProductType(false);
                        switchProtocolsToolStripMenuItem.Enabled = true;
                    }
                    if (manager.comm.MessageProtocol == "NMEA")
                    {
                        enableDisableMenuAndButtonPerProtocol(false);
                    }
                    else
                    {
                        enableDisableMenuAndButtonPerProtocol(true);
                    }
                }
            }
        }

        private void compassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compassViewClickHandler();
        }

        private void compassViewClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target._compassView = CreateCompassViewWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._compassView = CreateCompassViewWin(manager2);
                    }
                }
                updateCompassViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void configureDebugErrorLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    createDebugErrorLogWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager != null) && (manager.comm != null))
                    {
                        string str = string.Format("{0}: Set Debug Log Error", manager.comm.PortName);
                        frmErrorLogConfig config = new frmErrorLogConfig(manager.comm, 0);
                        config.Text = str;
                        config.ShowDialog();
                    }
                }
            }
        }

        private void connectHandler()
        {
            if ((toolStripPortComboBox.Text == string.Empty) || (PortManagerHash.Count <= 0))
            {
                callPortConfig();
            }
            else
            {
                if (toolStripPortComboBox.Text == clsGlobal.FilePlayBackPortName)
                {
                    return;
                }
                portOpenBtnClickHandler(toolStripPortComboBox.Text);
            }
            updateAllMainBtn();
            menuBtnInit();
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreatePythonWindow();
        }

        private frmAutomationTests CreateAutomationTestWindow()
        {
            frmAutomationTests objfrmAutoTest = null;
			base.Invoke((MethodInvoker)delegate
			{
                objfrmAutoTest = frmAutomationTests.GetChildInstance();
                if ((objfrmAutoTest == null) || objfrmAutoTest.IsDisposed)
                {
                    objfrmAutoTest = new frmAutomationTests();
                }
                objfrmAutoTest.MdiParent = this;
                CreatePythonWindow();
                objfrmAutoTest.BringToFront();
                objfrmAutoTest.Show();
            });
            return objfrmAutoTest;
        }

        private void createAutoReplySummaryWindow()
        {
            if (PortManagerHash.Count > 0)
            {
                string str = "Auto Reply Summary: All";
                frmAutoReplySummary summary = new frmAutoReplySummary();
                summary.Text = str;
                summary.ShowDialog();
            }
        }

        private void createAutoReplySummaryWindow(PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = "Auto Reply Summary: " + target.comm.sourceDeviceName;
                    frmAutoReplySummary summary = new frmAutoReplySummary();
                    summary.CommWindow = target.comm;
                    summary.Text = str;
                    summary.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void createAutoReplyWindow()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    manager = (PortManager) PortManagerHash[str];
                    if (((manager != null) && !(manager.comm.MessageProtocol == "NMEA")) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
                DialogResult cancel = System.Windows.Forms.DialogResult.Cancel;
                if (flag && (manager != null))
                {
                    string str2 = "Auto Reply Configure: All";
                    frmAutoReply reply = new frmAutoReply();
                    if (reply != null)
                    {
                        reply.CommWindow = manager.comm;
                        reply.Text = str2;
                        cancel = reply.ShowDialog();
                    }
                }
                if (cancel != DialogResult.Cancel)
                {
                    foreach (string str3 in PortManagerHash.Keys)
                    {
                        manager = (PortManager) PortManagerHash[str3];
                        if ((manager != null) && (manager.comm.MessageProtocol != "NMEA"))
                        {
                            if (manager.comm.IsSourceDeviceOpen())
                            {
                                manager.comm.ReadAutoReplyData(clsGlobal.InstalledDirectory + @"\scripts\SiRFLiveAutomationSetupAutoReply.cfg");
                                if (manager.comm.Log.IsFileOpen())
                                {
                                    manager.comm.WriteApp(utils_AutoReply.getAutoReplySummary(manager.comm));
                                }
                            }
                            if ((manager._ttffDisplay != null) && manager.TTFFDisplayLocation.IsOpen)
                            {
                                manager._ttffDisplay.SetTTFFMsgIndication();
                            }
                        }
                    }
                }
                clsGlobal.PerformOnAll = false;
            }
        }

        private void createAutoReplyWindow(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": Auto Reply";
                    if (target.comm.IsSourceDeviceOpen())
                    {
                        frmAutoReply reply = new frmAutoReply();
                        if (reply != null)
                        {
                            reply.CommWindow = target.comm;
                            reply.Text = str;
                            if ((reply.ShowDialog() != DialogResult.Cancel) && target.comm.Log.IsFileOpen())
                            {
                                target.comm.WriteApp(utils_AutoReply.getAutoReplySummary(target.comm));
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                if ((target._ttffDisplay != null) && target.TTFFDisplayLocation.IsOpen)
                {
                    target._ttffDisplay.SetTTFFMsgIndication();
                }
            }
        }

        public void CreateChannelProtocolCoverageHTMLReport(string fn)
        {
            Report.ProtocolChannelCoverageReportHTMLGenerator(fn);
        }

        public frmCommOpen CreateCommWindow()
        {
            frmCommOpen objfrmMComm = null;
			base.Invoke((MethodInvoker)delegate
			{
                objfrmMComm = new frmCommOpen();
                objfrmMComm.MdiParent = this;
                objfrmMComm.Show();
            });
            return objfrmMComm;
        }

        public frmCompassView CreateCompassViewWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (target.comm != null)
                {
                    if (base.InvokeRequired)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                target._compassView = localCreateCompassViewWin(target);
                            };
                        }
                        base.Invoke(method);
                    }
                    else
                    {
                        target._compassView = localCreateCompassViewWin(target);
                    }
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            return target._compassView;
        }

        private void createDebugErrorLogWin()
        {
            if (PortManagerHash.Count > 0)
            {
                bool flag = false;
                PortManager manager = null;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Set Debug Log Error: All";
                    frmErrorLogConfig config = new frmErrorLogConfig(manager.comm, 0);
                    config.Text = str2;
                    config.ShowDialog();
                }
            }
        }

        public frmCommDebugView CreateDebugViewWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target.DebugView = localCreateDebugViewWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target.DebugView = localCreateDebugViewWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target.DebugView;
        }

        public void CreateDirectoryProtocolCoverageReport(string dirPath)
        {
            Report.CreateDirectoryProtocolCoverageReport(dirPath);
        }

        private void CreateDOPMaskWin()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "DOP Mask: All";
                    frmMaskDOP kdop = new frmMaskDOP(manager.comm);
                    kdop.Text = str2;
                    kdop.ShowDialog();
                }
            }
        }

        private void CreateDOPMaskWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": DOP Mask";
                    frmMaskDOP kdop = new frmMaskDOP(target.comm);
                    kdop.Text = str;
                    kdop.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public frmCommDRSensor CreateDRSensorDataWin(PortManager target)
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._DRSensorViewPanel = localCreateDRSensorDataWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target._DRSensorViewPanel = localCreateDRSensorDataWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._DRSensorViewPanel;
        }

        private void CreateDRSensorParamWin()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "DR Sensor Parameters";
                    frmSetDRSensParam param = new frmSetDRSensParam(manager.comm);
                    param.Text = str2;
                    param.ShowDialog();
                }
            }
        }

        private void CreateDRSensorParamWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": DR Sensor Parameters";
                    frmSetDRSensParam param = new frmSetDRSensParam(target.comm);
                    param.Text = str;
                    param.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public frmCommDRStatus CreateDRStatusWin(PortManager target)
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._DRNavStatusViewPanel = localCreateDRStatusWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target._DRNavStatusViewPanel = localCreateDRStatusWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._DRNavStatusViewPanel;
        }

        public frmE911Report CreateE911CtrlWindow(string test)
        {
            frmE911Report childInstance = frmE911Report.GetChildInstance(test);
            if (childInstance.IsDisposed)
            {
                childInstance = new frmE911Report(test);
            }
            childInstance.MdiParent = this;
            childInstance.Show();
            return childInstance;
        }

        private void CreateElevationMaskWin()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Elevation Mask: All";
                    frmMaskElevation elevation = new frmMaskElevation(manager.comm);
                    elevation.Text = str2;
                    elevation.ShowDialog();
                }
            }
        }

        private void CreateElevationMaskWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": Elevation Mask";
                    frmMaskElevation elevation = new frmMaskElevation(target.comm);
                    elevation.Text = str;
                    elevation.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public void CreateEncrypCtrlWin()
        {
            if (PortManagerHash.Count > 0)
            {
                bool flag = false;
                PortManager manager = null;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Set Debug Level: All";
                    frmEncryCtrl ctrl = new frmEncryCtrl(manager.comm);
                    ctrl.CommWindow = manager.comm;
                    ctrl.Text = str2;
                    ctrl.ShowDialog();
                }
            }
        }

        public void CreateEncrypCtrlWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": Set Developer Debug Levels";
                    frmEncryCtrl ctrl = new frmEncryCtrl(target.comm);
                    ctrl.CommWindow = target.comm;
                    ctrl.Text = str;
                    ctrl.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public frmCommErrorView CreateErrorViewWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._errorView = localCreateErrorViewWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target._errorView = localCreateErrorViewWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._errorView;
        }

        private frmFileAnalysis CreateFileAnalysisWindow(string type)
        {
            frmFileAnalysis childInstance = null;
            childInstance = frmFileAnalysis.GetChildInstance(type);
            if (childInstance.IsDisposed)
            {
                childInstance = new frmFileAnalysis(type);
            }
            childInstance.BringToFront();
            childInstance.MdiParent = this;
            childInstance.Show();
            return childInstance;
        }

        private void CreateFileConversionWindow(ConversionType type)
        {
            switch (type)
            {
                case ConversionType.GP2ToGPS:
                    gP2GPSToolStripMenuItem.Enabled = true;
                    binGPSToolStripMenuItem.Enabled = false;
                    gPSNMEAToolStripMenuItem.Enabled = false;
                    NMEAtoGPStoolStripMenuItem.Enabled = false;
                    gPSToKMLToolStripMenuItem.Enabled = false;
                    break;

                case ConversionType.BinToGPS_GP2:
                    gP2GPSToolStripMenuItem.Enabled = false;
                    binGPSToolStripMenuItem.Enabled = true;
                    gPSNMEAToolStripMenuItem.Enabled = false;
                    NMEAtoGPStoolStripMenuItem.Enabled = false;
                    gPSToKMLToolStripMenuItem.Enabled = false;
                    break;

                case ConversionType.GPSToNMEA:
                    gP2GPSToolStripMenuItem.Enabled = false;
                    binGPSToolStripMenuItem.Enabled = false;
                    gPSNMEAToolStripMenuItem.Enabled = true;
                    NMEAtoGPStoolStripMenuItem.Enabled = false;
                    gPSToKMLToolStripMenuItem.Enabled = false;
                    break;

                case ConversionType.GPSToKML:
                    gP2GPSToolStripMenuItem.Enabled = false;
                    binGPSToolStripMenuItem.Enabled = false;
                    gPSNMEAToolStripMenuItem.Enabled = false;
                    NMEAtoGPStoolStripMenuItem.Enabled = false;
                    gPSToKMLToolStripMenuItem.Enabled = true;
                    break;

                case ConversionType.NMEAToGPS:
                    gP2GPSToolStripMenuItem.Enabled = false;
                    binGPSToolStripMenuItem.Enabled = false;
                    gPSNMEAToolStripMenuItem.Enabled = false;
                    NMEAtoGPStoolStripMenuItem.Enabled = true;
                    gPSToKMLToolStripMenuItem.Enabled = false;
                    break;

                default:
                    gP2GPSToolStripMenuItem.Enabled = true;
                    binGPSToolStripMenuItem.Enabled = true;
                    gPSNMEAToolStripMenuItem.Enabled = true;
                    NMEAtoGPStoolStripMenuItem.Enabled = true;
                    gPSToKMLToolStripMenuItem.Enabled = true;
                    break;
            }
            frmFileConversion childInstance = null;
            childInstance = frmFileConversion.GetChildInstance(type);
            if (childInstance.IsDisposed)
            {
                childInstance = new frmFileConversion(type);
            }
            childInstance.BringToFront();
            childInstance.updateParent += new frmFileConversion.updateParentEventHandler(updateFileCovAvail);
            childInstance.MdiParent = this;
            childInstance.Show();
        }

        private void CreateFileExtractWindow()
        {
            frmFileExtract childInstance = null;
            childInstance = frmFileExtract.GetChildInstance();
            if (childInstance.IsDisposed)
            {
                childInstance = new frmFileExtract();
            }
            childInstance.BringToFront();
            childInstance.MdiParent = this;
            childInstance.Show();
        }

        private void CreateFilePlotWindow()
        {
            frmFilePlots childInstance = null;
            childInstance = frmFilePlots.GetChildInstance();
            if (childInstance.IsDisposed)
            {
                childInstance = new frmFilePlots();
            }
            childInstance.BringToFront();
            childInstance.MdiParent = this;
            childInstance.Show();
        }

        private frmFileReplay CreateFileReplayWindow()
        {
            frmFileReplay childInstance = frmFileReplay.GetChildInstance();
            sleepThreads.Add(childInstance._parseThread);
            childInstance.MdiParent = this;
            childInstance.Show();
            childInstance.StartParseThread();
            return childInstance;
        }

        public frmPerformanceMonitor CreatefrmPerformanceMonitorWindow()
        {
            _objFrmPerfMonitor = new frmPerformanceMonitor();
            _objFrmPerfMonitor.MdiParent = this;
            _objFrmPerfMonitor.Show();
            return _objFrmPerfMonitor;
        }
/*
 * //!
        public frmGPIBCtrl CreateGPIBCtrlWindow()
        {
            frmGPIBCtrl childInstance = frmGPIBCtrl.GetChildInstance();
            if (childInstance.IsDisposed)
            {
                childInstance = new frmGPIBCtrl();
            }
            childInstance.MdiParent = this;
            childInstance.Show();
            return childInstance;
        }
*/
        private void CreateGyroFacCalWin()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Gyro Factory Calibration";
                    frmSetDRGyroFacCal cal = new frmSetDRGyroFacCal(manager.comm);
                    cal.Text = str2;
                    cal.ShowDialog();
                }
            }
        }

        private void CreateGyroFacCalWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": Gyro Factory Calibration";
                    frmSetDRGyroFacCal cal = new frmSetDRGyroFacCal(target.comm);
                    cal.Text = str;
                    cal.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private frmCommInputMessage CreateInputCommandsWin()
        {
            if (PortManagerHash.Count <= 0)
            {
                return null;
            }
            bool flag = false;
            PortManager manager = null;
            foreach (string str in PortManagerHash.Keys)
            {
                if (!(str == clsGlobal.FilePlayBackPortName))
                {
                    manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (flag && (manager != null))
            {
                string str2 = "Predefined Message: All";
                if ((manager._inputCommands == null) || manager._inputCommands.IsDisposed)
                {
                    manager._inputCommands = new frmCommInputMessage();
                    if (manager.InputCommandLocation.Width != 0)
                    {
                        manager._inputCommands.Width = manager.InputCommandLocation.Width;
                        manager._inputCommands.WinWidth = manager.InputCommandLocation.Width;
                    }
                    if (manager.InputCommandLocation.Height != 0)
                    {
                        manager._inputCommands.Height = manager.InputCommandLocation.Height;
                        manager._inputCommands.WinHeight = manager.InputCommandLocation.Height;
                    }
                    if (manager.InputCommandLocation.Left != 0)
                    {
                        manager._inputCommands.Left = manager.InputCommandLocation.Left;
                        manager._inputCommands.WinLeft = manager.InputCommandLocation.Left;
                    }
                    if (manager.InputCommandLocation.Top != 0)
                    {
                        manager._inputCommands.Top = manager.InputCommandLocation.Top;
                        manager._inputCommands.WinTop = manager.InputCommandLocation.Top;
                    }
                    manager._inputCommands.UpdatePortManager += new frmCommInputMessage.UpdateWindowEventHandler(manager.UpdateSubWindowOnClosed);
                }
                manager._inputCommands.CommWindow = manager.comm;
                manager._inputCommands.Text = str2;
                manager._inputCommands.Show();
                manager.InputCommandLocation.IsOpen = true;
                manager._inputCommands.BringToFront();
            }
            return manager._inputCommands;
        }

        internal frmCommInputMessage CreateInputCommandsWin(PortManager target)
        {
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                string str = target.comm.sourceDeviceName + ": Predefined Message";
                if ((target._inputCommands == null) || target._inputCommands.IsDisposed)
                {
                    target._inputCommands = new frmCommInputMessage();
                    if (target.InputCommandLocation.Width != 0)
                    {
                        target._inputCommands.Width = target.InputCommandLocation.Width;
                        target._inputCommands.WinWidth = target.InputCommandLocation.Width;
                    }
                    if (target.InputCommandLocation.Height != 0)
                    {
                        target._inputCommands.Height = target.InputCommandLocation.Height;
                        target._inputCommands.WinHeight = target.InputCommandLocation.Height;
                    }
                    if (target.InputCommandLocation.Left != 0)
                    {
                        target._inputCommands.Left = target.InputCommandLocation.Left;
                        target._inputCommands.WinLeft = target.InputCommandLocation.Left;
                    }
                    if (target.InputCommandLocation.Top != 0)
                    {
                        target._inputCommands.Top = target.InputCommandLocation.Top;
                        target._inputCommands.WinTop = target.InputCommandLocation.Top;
                    }
                    target._inputCommands.UpdatePortManager += new frmCommInputMessage.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                }
                target._inputCommands.CommWindow = target.comm;
                target._inputCommands.Text = str;
                target._inputCommands.Show();
                target.InputCommandLocation.IsOpen = true;
                target._inputCommands.BringToFront();
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._inputCommands;
        }

        public frmInterferenceReport CreateInterferenceReportWindow(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (target.comm != null)
                {
                    if (base.InvokeRequired)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                target._interferenceReport = localCreateInterferenceReportWindow(target);
                            };
                        }
                        base.Invoke(method);
                    }
                    else
                    {
                        target._interferenceReport = localCreateInterferenceReportWindow(target);
                    }
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            return target._interferenceReport;
        }

        public frmCommLocationMap CreateLocationMapWin(PortManager target)
        {
            EventHandler method = null;
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._locationViewPanel = localCreateLocationMapWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target._locationViewPanel = localCreateLocationMapWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._locationViewPanel;
        }

        private void CreateLogFileWin()
        {
            if (PortManagerHash.Count > 0)
            {
                bool flag = false;
                bool flag2 = false;
                PortManager manager = null;
                foreach (string str in PortManagerHash.Keys)
                {
                    manager = (PortManager) PortManagerHash[str];
                    if (((manager != null) && (manager.comm != null)) && manager.comm.Log.IsFileOpen())
                    {
                        if (MessageBox.Show(string.Format("{0}: Logging in progress. Proceeding will close the current log.\n\n\t Continue?", manager.comm.PortName), "Log File Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            manager.comm.Log.CloseFile();
                        }
                        else
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
                if (!flag2)
                {
                    foreach (string str2 in PortManagerHash.Keys)
                    {
                        manager = (PortManager) PortManagerHash[str2];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag && (manager != null))
                    {
                        string str3 = "Log files: All";
                        if ((manager.comm != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            manager.LogFileWin = new frmLogDuration(manager.comm);
                            manager.LogFileWin.ShowDialog();
                        }
                        manager.LogFileWin.Text = str3;
                    }
                }
            }
        }

        private void CreateLogFileWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    if (target.comm != null)
                    {
                        if (target.comm.Log.IsFileOpen())
                        {
                            if (MessageBox.Show(string.Format("{0}: Logging in progress. Proceeding will close the current log.\n\n\t Continue?", target.comm.PortName), "Log File Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                            {
                                target.comm.Log.CloseFile();
                            }
                        }
                        else
                        {
                            string str = target.comm.sourceDeviceName + ": Log File";
                            frmLogDuration duration = new frmLogDuration(target.comm);
                            duration.Text = str;
                            duration.ShowDialog();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public void CreateLowPowerBufferWin()
        {
            if (PortManagerHash.Count > 0)
            {
                bool flag = false;
                PortManager manager = null;
                foreach (string str in PortManagerHash.Keys)
                {
                    manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Low Power Buffer: All";
                    if ((manager._lowPowerBufferView == null) || manager._lowPowerBufferView.IsDisposed)
                    {
                        manager._lowPowerBufferView = new frmLPBufferWindow(manager.comm);
                    }
                    manager._lowPowerBufferView.Text = str2;
                    manager._lowPowerBufferView.ShowDialog();
                }
            }
        }

        public void CreateLowPowerBufferWin(PortManager target)
        {
            if (target != null)
            {
                if ((target._lowPowerBufferView == null) || target._lowPowerBufferView.IsDisposed)
                {
                    target._lowPowerBufferView = new frmLPBufferWindow(target.comm);
                }
                target._lowPowerBufferView.ShowDialog();
            }
        }

        private void createLowPowerInputWindow()
        {
            if (PortManagerHash.Count > 0)
            {
                bool flag = false;
                PortManager manager = null;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Switch Operation Mode: All";
                    if ((manager.comm != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        frmLowPower power = new frmLowPower(manager.comm);
                        power.Text = str2;
                        power.ShowDialog();
                    }
                }
            }
        }

        private void createLowPowerInputWindow(ref PortManager target)
        {
            if ((target != null) && ((target.comm != null) && target.comm.IsSourceDeviceOpen()))
            {
                new frmLowPower(target.comm).ShowDialog();
            }
        }

        public frmLSMTestReport CreateLSMReportWindow()
        {
            if (_objFrmLSMReport == null)
            {
                _objFrmLSMReport = new frmLSMTestReport();
            }
            if (_objFrmLSMReport.IsDisposed)
            {
                _objFrmLSMReport = new frmLSMTestReport();
            }
            _objFrmLSMReport.MdiParent = this;
            _objFrmLSMReport.Show();
            return _objFrmLSMReport;
        }

        public frmMEMSView CreateMEMSViewWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (target.comm != null)
                {
                    if (base.InvokeRequired)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                target._memsView = localCreateMEMSViewWin(target);
                            };
                        }
                        base.Invoke(method);
                    }
                    else
                    {
                        target._memsView = localCreateMEMSViewWin(target);
                    }
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            return target._memsView;
        }

        public frmCommMessageFilter CreateMessageViewWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target.MessageView = localCreateMessageViewWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target.MessageView = localCreateMessageViewWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target.MessageView;
        }

        private void CreateModeMaskWin()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Mode Mask: All";
                    frmMaskMode mode = new frmMaskMode(manager.comm);
                    mode.Text = str2;
                    mode.ShowDialog();
                }
            }
        }

        private void CreateModeMaskWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": Mode Mask";
                    frmMaskMode mode = new frmMaskMode(target.comm);
                    mode.Text = str;
                    mode.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public void CreateMPMGenWin()
        {
            frmSimpleReportGen gen = new frmSimpleReportGen("MPM Report");
            gen.MdiParent = this;
            gen.Show();
        }

        private void CreateNavModeControlWin()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = ": DR Nav Mode Control";
                    frmSetGPSDRMode mode = new frmSetGPSDRMode(manager.comm);
                    mode.Text = str2;
                    mode.ShowDialog();
                }
            }
        }

        private void CreateNavModeControlWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": Nav Mode Control";
                    frmSetGPSDRMode mode = new frmSetGPSDRMode(target.comm);
                    mode.Text = str;
                    mode.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public frmCommNavAccVsTime CreateNavVsTimeWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target.NavVsTimeView = localCreateNavVsTimeWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target.NavVsTimeView = localCreateNavVsTimeWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target.NavVsTimeView;
        }

        public PortManager CreateNewPort(string portName)
        {
            PortManager manager = null;
            if (PortManagerHash.Count < 0)
            {
                manager = new PortManager();
                PortManagerHash.Add(portName, manager);
            }
            else if (PortManagerHash.ContainsKey(portName))
            {
                manager = (PortManager) PortManagerHash[portName];
            }
            else
            {
                manager = new PortManager();
                PortManagerHash.Add(portName, manager);
            }
            if ((manager != null) && (manager.comm != null))
            {
                manager.comm.Log.DurationLoggingStatusLabel = logManagerStatusLabel;
                manager.comm.Log.UpdateMainWindow += new LogManager.UpdateParentEventHandler(updateLogFileBtn);
                manager.comm.UpdatePortMainWinTitle += new CommunicationManager.UpdateParentPortEventHandler(UpdateWinTitle);
            }
            UpdateToolStripPortComboBoxItems(true);
            return manager;
        }

        private void createPeekPokeWin()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if ((flag && (manager != null)) && manager.comm.IsSourceDeviceOpen())
                {
                    string str2 = "Peek Poke: All";
                    if ((manager._peekPokeWin == null) || manager._peekPokeWin.IsDisposed)
                    {
                        manager._peekPokeWin = new frmPeekPokeMem(manager.comm);
                        if (manager.PeekPokeLocation.Width != 0)
                        {
                            manager._peekPokeWin.Width = manager.PeekPokeLocation.Width;
                            manager._peekPokeWin.WinWidth = manager.PeekPokeLocation.Width;
                        }
                        if (manager.PeekPokeLocation.Height != 0)
                        {
                            manager._peekPokeWin.Height = manager.PeekPokeLocation.Height;
                            manager._peekPokeWin.WinHeight = manager.PeekPokeLocation.Height;
                        }
                        if (manager.PeekPokeLocation.Left != 0)
                        {
                            manager._peekPokeWin.Left = manager.PeekPokeLocation.Left;
                            manager._peekPokeWin.WinLeft = manager.PeekPokeLocation.Left;
                        }
                        if (manager.PeekPokeLocation.Top != 0)
                        {
                            manager._peekPokeWin.Top = manager.PeekPokeLocation.Top;
                            manager._peekPokeWin.WinTop = manager.PeekPokeLocation.Top;
                        }
                        manager._peekPokeWin.UpdatePortManager += new frmPeekPokeMem.UpdateWindowEventHandler(manager.UpdateSubWindowOnClosed);
                    }
                    manager._peekPokeWin.Text = str2;
                    manager._peekPokeWin.Show();
                    manager._peekPokeWin.BringToFront();
                }
            }
        }

        private void createPeekPokeWin(PortManager target)
        {
            EventHandler method = null;
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            if ((target._peekPokeWin != null) && !target._peekPokeWin.IsDisposed)
                            {
                                target._peekPokeWin.Close();
                            }
                            else
                            {
                                string str = target.comm.sourceDeviceName + ": Peek Poke";
                                if ((target._peekPokeWin == null) || target._peekPokeWin.IsDisposed)
                                {
                                    target._peekPokeWin = new frmPeekPokeMem(target.comm);
                                    if (target.PeekPokeLocation.Width != 0)
                                    {
                                        target._peekPokeWin.Width = target.PeekPokeLocation.Width;
                                        target._peekPokeWin.WinWidth = target.PeekPokeLocation.Width;
                                    }
                                    if (target.PeekPokeLocation.Height != 0)
                                    {
                                        target._peekPokeWin.Height = target.PeekPokeLocation.Height;
                                        target._peekPokeWin.WinHeight = target.PeekPokeLocation.Height;
                                    }
                                    if (target.PeekPokeLocation.Left != 0)
                                    {
                                        target._peekPokeWin.Left = target.PeekPokeLocation.Left;
                                        target._peekPokeWin.WinLeft = target.PeekPokeLocation.Left;
                                    }
                                    if (target.PeekPokeLocation.Top != 0)
                                    {
                                        target._peekPokeWin.Top = target.PeekPokeLocation.Top;
                                        target._peekPokeWin.WinTop = target.PeekPokeLocation.Top;
                                    }
                                    target._peekPokeWin.UpdatePortManager += new frmPeekPokeMem.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                                }
                                target._peekPokeWin.Text = str;
                                target._peekPokeWin.Show();
                                target.PeekPokeLocation.IsOpen = true;
                                if (target._responseView != null)
                                {
                                    target._responseView.BringToFront();
                                }
                            }
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public frmNavPerformanceReport CreatePerformanceReportCtrlWindow()
        {
            frmNavPerformanceReport childInstance = frmNavPerformanceReport.GetChildInstance();
            if (childInstance.IsDisposed)
            {
                childInstance = new frmNavPerformanceReport();
            }
            childInstance.MdiParent = this;
            childInstance.Show();
            return childInstance;
        }

        private void CreatePowerMaskWin()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Power Mask: All";
                    frmMaskPower power = new frmMaskPower(manager.comm);
                    power.Text = str2;
                    power.ShowDialog();
                }
            }
        }

        private void CreatePowerMaskWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": Power Mask";
                    frmMaskPower power = new frmMaskPower(target.comm);
                    power.Text = str;
                    power.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public void CreateProtocolCoverageHTMLReport(string fn)
        {
            Report.ProtocolCoverageReportHTMLGenerator(fn);
        }

        public void CreateProtocolGP2ChannelCoverageFile(string fn)
        {
            Report.ProtocolGP2ChannelCoverageFile(fn);
        }

        public void CreateProtocolGP2CoverageFile(string fn)
        {
            Report.ProtocolGP2CoverageFile(fn);
        }

        public void CreateProtocolGPSCoverageXMLFile(string fn)
        {
            Report.ProtocolGPSCoverageXMLFile(fn);
        }

        public void CreateProtocolHTMLReport(string fn)
        {
            Report.ProtocolReportHTMLGenerator(fn);
        }

        public void CreateProtocolXMLReport(string fn)
        {
            Report.ProtocolReportXMLGenerator(fn);
        }

        public frmPRReport CreatePRReportCtrlWindow()
        {
            if (_objFrmPRReport == null)
            {
                _objFrmPRReport = new frmPRReport();
            }
            if (_objFrmPRReport.IsDisposed)
            {
                _objFrmPRReport = new frmPRReport();
            }
            _objFrmPRReport.MdiParent = this;
            _objFrmPRReport.Show();
            return _objFrmPRReport;
        }

        public frmPython CreatePythonWindow()
        {
            frmPython objfrmPython = null;
			base.Invoke((MethodInvoker)delegate
			{
                objfrmPython = frmPython.GetChildInstance();
                if ((objfrmPython == null) || objfrmPython.IsDisposed)
                {
                    objfrmPython = new frmPython();
                }
                objfrmPython.MdiParent = this;
                objfrmPython.Show();
                objfrmPython.BringToFront();
            });
            return objfrmPython;
        }

        public frmCommRadarMap CreateRadarViewWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._svsMapPanel = localCreateRadarViewWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target._svsMapPanel = localCreateRadarViewWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._svsMapPanel;
        }

        public void CreateResetWindow()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                string portName = string.Empty;
                foreach (string str2 in PortManagerHash.Keys)
                {
                    if (str2 != clsGlobal.FilePlayBackPortName)
                    {
                        manager = (PortManager) PortManagerHash[str2];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            portName = manager.comm.PortName;
                            if ((manager._ttbWin != null) && manager.TTBWinLocation.IsOpen)
                            {
                                manager._ttbWin.Close();
                            }
                            if (!manager.comm.TTBPort.IsOpen && manager.ReconnectTTB)
                            {
                                manager.comm.TTBPort.Open();
                                manager.ReconnectTTB = false;
                            }
                        }
                    }
                }
                if (flag)
                {
                    manager = (PortManager) PortManagerHash[portName];
                    if (manager != null)
                    {
                        string str3 = "Reset All";
                        if ((manager._resetCmd == null) || manager._resetCmd.IsDisposed)
                        {
                            manager._resetCmd = new frmRXInit_cmd();
                        }
                        manager._resetCmd.CommWindow = manager.comm;
                        manager._resetCmd.Text = str3;
                    }
                    if (manager._resetCmd.ShowDialog() != DialogResult.Cancel)
                    {
                        foreach (string str4 in PortManagerHash.Keys)
                        {
                            manager = (PortManager) PortManagerHash[str4];
                            if (manager != null)
                            {
                                System.Timers.Timer timer = new System.Timers.Timer();
                                timer.Elapsed += new ElapsedEventHandler(manager.clearGUIDataTimerHandler);
                                timer.Interval = 1000.0;
                                timer.AutoReset = false;
                                timer.Start();
                                manager.comm.dataGui.AGC_Gain = 0;
                                if ((manager._ttffDisplay != null) && manager.TTFFDisplayLocation.IsOpen)
                                {
                                    manager._ttffDisplay.SetTTFFMsgIndication();
                                }
                            }
                        }
                    }
                }
            }
        }

        public frmRXInit_cmd CreateResetWindow(PortManager target)
        {
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if ((target._ttbWin != null) && target.TTBWinLocation.IsOpen)
                {
                    target._ttbWin.Close();
                }
                if (!target.comm.TTBPort.IsOpen && target.ReconnectTTB)
                {
                    target.comm.TTBPort.Open();
                    target.ReconnectTTB = false;
                }
                string str = target.comm.sourceDeviceName + ": Reset";
                if ((target._resetCmd == null) || target._resetCmd.IsDisposed)
                {
                    target._resetCmd = new frmRXInit_cmd();
                }
                target._resetCmd.CommWindow = target.comm;
                target._resetCmd.Text = str;
                if (target._resetCmd.ShowDialog() != DialogResult.Cancel)
                {
                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Elapsed += new ElapsedEventHandler(target.clearGUIDataTimerHandler);
                    timer.Interval = 1000.0;
                    timer.AutoReset = false;
                    timer.Start();
                    target.comm.dataGui.AGC_Gain = 0;
                }
                if ((target._ttffDisplay != null) && target.TTFFDisplayLocation.IsOpen)
                {
                    target._ttffDisplay.SetTTFFMsgIndication();
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._resetCmd;
        }

        public frmCommResponseView CreateResponseViewWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._responseView = localCreateResponseViewWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target._responseView = localCreateResponseViewWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._responseView;
        }

        public frmRFCaptureCtrl CreateRFReplayCaptureWindow()
        {
            frmRFCaptureCtrl objfrmRFCaptureCtrl = null;
			base.Invoke((MethodInvoker)delegate
			{
                objfrmRFCaptureCtrl = frmRFCaptureCtrl.GetChildInstance();
                if (objfrmRFCaptureCtrl.IsDisposed)
                {
                    objfrmRFCaptureCtrl = new frmRFCaptureCtrl();
                }
                objfrmRFCaptureCtrl.MdiParent = this;
                objfrmRFCaptureCtrl.Show();
            });
            return objfrmRFCaptureCtrl;
        }

        public frmRFPlaybackConfig CreateRFReplayConfigWindow()
        {
            frmRFPlaybackConfig objfrmRFPlaybackConfig = null;
			base.Invoke((MethodInvoker)delegate
			{
                objfrmRFPlaybackConfig = frmRFPlaybackConfig.GetChildInstance();
                if (objfrmRFPlaybackConfig.IsDisposed)
                {
                    objfrmRFPlaybackConfig = new frmRFPlaybackConfig();
                }
                objfrmRFPlaybackConfig.MdiParent = this;
                objfrmRFPlaybackConfig.Show();
            });
            return objfrmRFPlaybackConfig;
        }

        public frmRFPlaybackCtrl CreateRFReplayPlaybackWindow()
        {
            frmRFPlaybackCtrl objfrmRFPlaybackCtrl = null;
			base.Invoke((MethodInvoker)delegate
			{
                objfrmRFPlaybackCtrl = frmRFPlaybackCtrl.GetChildInstance();
                if (objfrmRFPlaybackCtrl.IsDisposed)
                {
                    objfrmRFPlaybackCtrl = new frmRFPlaybackCtrl();
                }
                objfrmRFPlaybackCtrl.MdiParent = this;
                objfrmRFPlaybackCtrl.Show();
            });
            return objfrmRFPlaybackCtrl;
        }

        private bool createRxSettingsWindow(ref CommunicationManager comm)
        {
            bool flag = false;
            if (comm != null)
            {
                if (!comm.IsSourceDeviceOpen())
                {
                    frmCommSettings settings = new frmCommSettings(ref comm);
                    if (settings.ShowDialog() != DialogResult.Cancel)
                    {
                        flag = true;
                    }
                }
                return flag;
            }
            MessageBox.Show("Port is connected! Disconnect port before configuring.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return flag;
        }

        public frmSatelliteStats CreateSatelliteStatsWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (target.comm != null)
                {
                    if (base.InvokeRequired)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                target._SatelliteStats = localCreateSatelliteStatsWin(target);
                            };
                        }
                        base.Invoke(method);
                    }
                    else
                    {
                        target._SatelliteStats = localCreateSatelliteStatsWin(target);
                    }
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            return target._SatelliteStats;
        }

        public void CreateSDOGenWin()
        {
            frmSimpleReportGen gen = new frmSimpleReportGen("SDO File Generation");
            gen.MdiParent = this;
            gen.Show();
        }

        private void CreateSetPollGenericSensorParamWin()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Set/Poll Generic Sensor Parameters";
                    frmDRSetPollGenericSensParams @params = new frmDRSetPollGenericSensParams(manager.comm);
                    @params.Text = str2;
                    @params.ShowDialog();
                }
            }
        }

        private void CreateSetPollGenericSensorParamWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": Set/Poll Generic Sensor Parameters";
                    frmDRSetPollGenericSensParams @params = new frmDRSetPollGenericSensParams(target.comm);
                    @params.Text = str;
                    @params.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public frmCommSignalView CreateSignalViewWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._signalStrengthPanel = localCreateSignalViewWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target._signalStrengthPanel = localCreateSignalViewWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._signalStrengthPanel;
        }

        public frmSimplexCtrl CreateSimplexCtrlWindow()
        {
            frmSimplexCtrl objfrmSimplexCtrl = frmSimplexCtrl.GetChildInstance();
			base.Invoke((MethodInvoker)delegate
			{
                if (objfrmSimplexCtrl.IsDisposed)
                {
                    objfrmSimplexCtrl = new frmSimplexCtrl();
                }
                objfrmSimplexCtrl.MdiParent = this;
                objfrmSimplexCtrl.Show();
            });
            return objfrmSimplexCtrl;
        }

        public frmCommSiRFawareV2 CreateSiRFawareWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (target.comm != null)
                {
                    if (target.comm.MessageProtocol == "NMEA")
                    {
                        return null;
                    }
                    if (base.InvokeRequired)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                target._SiRFAware = localCreateSiRFawareWin(target);
                            };
                        }
                        base.Invoke(method);
                    }
                    else
                    {
                        target._SiRFAware = localCreateSiRFawareWin(target);
                    }
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            return target._SiRFAware;
        }

        public frmSPAzCtrl CreateSPAzCtrlWindow()
        {
            frmSPAzCtrl childInstance = frmSPAzCtrl.GetChildInstance();
            if (childInstance.IsDisposed)
            {
                childInstance = new frmSPAzCtrl(0x378);
            }
            childInstance.MdiParent = this;
            childInstance.Show();
            return childInstance;
        }

        private void CreateStaticNavWin()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Static Nav: All";
                    frmStaticNav nav = new frmStaticNav(manager.comm);
                    nav.Text = str2;
                    nav.ShowDialog();
                }
            }
        }

        private void CreateStaticNavWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": Static Nav";
                    frmStaticNav nav = new frmStaticNav(target.comm);
                    nav.Text = str;
                    nav.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public frmCommSVAvgCNo CreateSVCNoWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._svCNoView = localCreateSVCNoWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target._svCNoView = localCreateSVCNoWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._svCNoView;
        }

        public frmCommSVTrackedVsTime CreateSVTrackedVsTimeWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._svTrackedVsTimeView = localCreateSVTrackedVsTimeWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target._svTrackedVsTimeView = localCreateSVTrackedVsTimeWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._svTrackedVsTimeView;
        }

        public frmCommSVTrajectory CreateSVTrajWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._svTrajView = localCreateSVTrajWin(target);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    target._svTrajView = localCreateSVTrajWin(target);
                }
            }
            else
            {
                MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return target._svTrajView;
        }

        private void CreateSwitchOperationModeWin()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Switch Operation Mode: All";
                    frmSwitchOperationMode mode = new frmSwitchOperationMode(manager.comm);
                    mode.Text = str2;
                    mode.ShowDialog();
                }
            }
        }

        private void CreateSwitchOperationModeWin(ref PortManager target)
        {
            if (target != null)
            {
                if (!base.IsDisposed)
                {
                    string str = target.comm.sourceDeviceName + ": Switch Operation Mode";
                    frmSwitchOperationMode mode = new frmSwitchOperationMode(target.comm);
                    mode.Text = str;
                    mode.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private DialogResult createSwitchProtocolWindow()
        {
            DialogResult cancel = System.Windows.Forms.DialogResult.Cancel;
            if (PortManagerHash.Count > 0)
            {
                bool flag = false;
                PortManager manager = null;
                foreach (string str in PortManagerHash.Keys)
                {
                    manager = (PortManager) PortManagerHash[str];
                    if (((manager != null) && manager.comm.IsSourceDeviceOpen()) && (manager.comm.ProductFamily == CommonClass.ProductType.GSD4e))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag && (manager != null))
                {
                    string str2 = "Switch Protocol: All";
                    if ((manager.comm != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        frmSwitchProtocol protocol = new frmSwitchProtocol(manager.comm);
                        protocol.Text = str2;
                        cancel = protocol.ShowDialog();
                    }
                }
            }
            return cancel;
        }

        private DialogResult createSwitchProtocolWindow(ref CommunicationManager comm)
        {
            if ((comm != null) && comm.IsSourceDeviceOpen())
            {
                frmSwitchProtocol protocol = new frmSwitchProtocol(comm);
                return protocol.ShowDialog();
            }
            return DialogResult.Cancel;
        }

        public frmRFSynthesizer CreateSynthesizerWindow()
        {
            frmRFSynthesizer childInstance = frmRFSynthesizer.GetChildInstance();
            if (childInstance.IsDisposed)
            {
                childInstance = new frmRFSynthesizer();
            }
            childInstance.MdiParent = this;
            childInstance.Show();
            return childInstance;
        }

        public frmRackCtrl CreateTestRackCtrl()
        {
            frmRackCtrl childInstance = frmRackCtrl.GetChildInstance();
            if (childInstance.IsDisposed)
            {
                childInstance = new frmRackCtrl();
            }
            childInstance.MdiParent = this;
            childInstance.Show();
            return childInstance;
        }

        private DialogResult CreateTestStationWindow(string testName)
        {
            DialogResult myResult = System.Windows.Forms.DialogResult.Cancel;
			base.Invoke((MethodInvoker)delegate
			{
                myResult = new frmStationSetup(testName).ShowDialog();
            });
            return myResult;
        }

        public AppTimers CreateTimer(int interval, bool isContinuous)
        {
            return new AppTimers(interval, isContinuous);
        }

        private frmTransmitSerialMessage CreateTransmitSerialMessageWin()
        {
            if (PortManagerHash.Count <= 0)
            {
                return null;
            }
            bool flag = false;
            PortManager manager = null;
            foreach (string str in PortManagerHash.Keys)
            {
                if (!(str == clsGlobal.FilePlayBackPortName))
                {
                    manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (flag && (manager != null))
            {
                string str2 = "User Defined Message: All";
                if (((manager.comm != null) && manager.comm.IsSourceDeviceOpen()) && ((manager._transmitSerialMessageWin == null) || manager._transmitSerialMessageWin.IsDisposed))
                {
                    manager._transmitSerialMessageWin = new frmTransmitSerialMessage(manager.comm);
                    if (manager.TransmitSerialMessageLocation.Width != 0)
                    {
                        manager._transmitSerialMessageWin.Width = manager.TransmitSerialMessageLocation.Width;
                        manager._transmitSerialMessageWin.WinWidth = manager.TransmitSerialMessageLocation.Width;
                    }
                    if (manager.TransmitSerialMessageLocation.Height != 0)
                    {
                        manager._transmitSerialMessageWin.Height = manager.TransmitSerialMessageLocation.Height;
                        manager._transmitSerialMessageWin.WinHeight = manager.TransmitSerialMessageLocation.Height;
                    }
                    if (manager.TransmitSerialMessageLocation.Left != 0)
                    {
                        manager._transmitSerialMessageWin.Left = manager.TransmitSerialMessageLocation.Left;
                        manager._transmitSerialMessageWin.WinLeft = manager.TransmitSerialMessageLocation.Left;
                    }
                    if (manager.TransmitSerialMessageLocation.Top != 0)
                    {
                        manager._transmitSerialMessageWin.Top = manager.TransmitSerialMessageLocation.Top;
                        manager._transmitSerialMessageWin.WinTop = manager.TransmitSerialMessageLocation.Top;
                    }
                }
                manager._transmitSerialMessageWin.Text = str2;
                manager._transmitSerialMessageWin.UpdatePortManager += new frmTransmitSerialMessage.UpdateWindowEventHandler(manager.UpdateSubWindowOnClosed);
                manager._transmitSerialMessageWin.Show();
                manager._transmitSerialMessageWin.BringToFront();
                manager.TransmitSerialMessageLocation.IsOpen = true;
            }
            return manager._transmitSerialMessageWin;
        }

        private void CreateTransmitSerialMessageWin(PortManager target)
        {
            if (target != null)
            {
                if (target.comm.IsSourceDeviceOpen())
                {
                    string str = target.comm.sourceDeviceName + ": User Defined Message";
                    if ((target._transmitSerialMessageWin == null) || target._transmitSerialMessageWin.IsDisposed)
                    {
                        target._transmitSerialMessageWin = new frmTransmitSerialMessage(target.comm);
                        if (target.TransmitSerialMessageLocation.Width != 0)
                        {
                            target._transmitSerialMessageWin.Width = target.TransmitSerialMessageLocation.Width;
                            target._transmitSerialMessageWin.WinWidth = target.TransmitSerialMessageLocation.Width;
                        }
                        if (target.TransmitSerialMessageLocation.Height != 0)
                        {
                            target._transmitSerialMessageWin.Height = target.TransmitSerialMessageLocation.Height;
                            target._transmitSerialMessageWin.WinHeight = target.TransmitSerialMessageLocation.Height;
                        }
                        if (target.TransmitSerialMessageLocation.Left != 0)
                        {
                            target._transmitSerialMessageWin.Left = target.TransmitSerialMessageLocation.Left;
                            target._transmitSerialMessageWin.WinLeft = target.TransmitSerialMessageLocation.Left;
                        }
                        if (target.TransmitSerialMessageLocation.Top != 0)
                        {
                            target._transmitSerialMessageWin.Top = target.TransmitSerialMessageLocation.Top;
                            target._transmitSerialMessageWin.WinTop = target.TransmitSerialMessageLocation.Top;
                        }
                        target._transmitSerialMessageWin.UpdatePortManager += new frmTransmitSerialMessage.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                    }
                    target._transmitSerialMessageWin.Show();
                    target.TransmitSerialMessageLocation.IsOpen = true;
                    target._transmitSerialMessageWin.Text = str;
                    target._transmitSerialMessageWin.BringToFront();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public frmTTBOpen CreateTTBConnectWindow(PortManager target)
        {
            if (target == null)
            {
                return null;
            }
            frmTTBOpen open = null;
            if (!base.IsDisposed)
            {
                if (target.comm.IsSourceDeviceOpen())
                {
                    string str = "Connect TTB: " + target.comm.PortName;
                    open = new frmTTBOpen();
                    open.CommWindow = target.comm;
                    open.Text = str;
                    if (open.ShowDialog() != DialogResult.OK)
                    {
                        target.ReconnectTTB = false;
                        return open;
                    }
                    target.TTBPortProperties = new UART_Properties();
                    target.TTBPortProperties.PortName = target.comm.TTBPort.PortName;
                    target.TTBPortProperties.BaudRate = target.comm.TTBPort.BaudRate;
                    target.TTBPortProperties.DataBits = target.comm.TTBPort.DataBits;
                    target.TTBPortProperties.StopBits = target.comm.TTBPort.StopBits;
                    target.TTBPortProperties.Parity = target.comm.TTBPort.Parity;
                }
                return open;
            }
            MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return open;
        }

        public frm_TTBTimeAidingCfg CreateTTBTimeAidCfgWindow(PortManager target)
        {
            if (target == null)
            {
                return null;
            }
            frm_TTBTimeAidingCfg cfg = null;
            if (target.comm.IsSourceDeviceOpen())
            {
                string str = "Configure Time Aiding: " + target.comm.PortName;
                cfg = new frm_TTBTimeAidingCfg();
                cfg.CommWindow = target.comm;
                cfg.Text = str;
                cfg.ShowDialog();
                return cfg;
            }
            MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return cfg;
        }

        public frmTTFFDisplay CreateTTFFWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (target.comm != null)
                {
                    if (target.comm.MessageProtocol == "NMEA")
                    {
                        return null;
                    }
                    if (base.InvokeRequired)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                target._ttffDisplay = localCreateTTFFWin(target);
                            };
                        }
                        base.Invoke(method);
                    }
                    else
                    {
                        target._ttffDisplay = localCreateTTFFWin(target);
                    }
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            return target._ttffDisplay;
        }

        public frmTTFSView CreateTTFSWin(PortManager target)
        {
            EventHandler method = null;
            if (target == null)
            {
                return null;
            }
            if (!base.IsDisposed)
            {
                if (target.comm != null)
                {
                    if (target.comm.MessageProtocol == "NMEA")
                    {
                        return null;
                    }
                    if (base.InvokeRequired)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                target.TTFSView = localCreateTTFSWin(target);
                            };
                        }
                        base.Invoke(method);
                    }
                    else
                    {
                        target.TTFSView = localCreateTTFSWin(target);
                    }
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            return target.TTFSView;
        }

        private void cwDetectionClickHandler()
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (!(str == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager target = (PortManager) PortManagerHash[str];
                            if (target != null)
                            {
                                target._interferenceReport = CreateInterferenceReportWindow(target);
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._interferenceReport = CreateInterferenceReportWindow(manager2);
                    }
                }
                updateInterferenceViewBtn();
            }
        }

        private void cWDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cwDetectionClickHandler();
        }

        private void debugViewManualClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target.DebugView = CreateDebugViewWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2.DebugView = CreateDebugViewWin(manager2);
                    }
                }
                updateDebugViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void debugViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            debugViewManualClickHandler();
        }

        private static string decryptALine(string psText)
        {
            string str = "";
            for (int i = 0; i <= (psText.Length - 1); i += 3)
            {
                char ch = (char) ((psText[i + 1] ^ '}') ^ (0xffff - (i / 3)));
                str = str + ((char) ((psText[i] ^ '}') ^ ch)).ToString();
            }
            return str;
        }

        private void defaultLayoutMenu_Click(object sender, EventArgs e)
        {
            restoreDefaultPortLayout(true);
            updateAllMainBtn();
        }

        public void Delay(int interval)
        {
            if (!clsGlobal.Abort && (interval > 0))
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Elapsed += new ElapsedEventHandler(OnSleepThreadTimerEvent);
                timer.Interval = interval * 0x3e8;
                timer.AutoReset = false;
                sleepThreads.Add(Thread.CurrentThread);
                Thread.CurrentThread.Join((int) (interval * 0x3e8));
                timer.Start();
            }
        }

        private void developerDocMenu_Click(object sender, EventArgs e)
        {
            string url = @"..\Doc\Help\SiRFLive Documentation.chm";
            Help.ShowHelp(this, url);
        }

        private void disable5HzNavMode(ref CommunicationManager comm)
        {
            try
            {
                comm.FiveHzNavModeToSet = false;
                comm.FiveHzNavModePendingSet = true;
                comm.RxCtrl.PollNavigationParameters();
            }
            catch
            {
            }
        }

        private void disable5HzNavToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                MessageBox.Show("It might take a few seconds for SiRFLive status to be updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (!(str == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager manager = (PortManager) PortManagerHash[str];
                            if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                            {
                                disable5HzNavMode(ref manager.comm);
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                    {
                        disable5HzNavMode(ref manager2.comm);
                    }
                }
            }
        }

        private void disableABPMode(ref CommunicationManager comm)
        {
            try
            {
                comm.ABPModeToSet = false;
                comm.ABPModePendingSet = true;
                comm.RxCtrl.PollNavigationParameters();
            }
            catch
            {
            }
        }

        private void disableABPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (!(str == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager manager = (PortManager) PortManagerHash[str];
                            if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                            {
                                disableABPMode(ref manager.comm);
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                    {
                        disableABPMode(ref manager2.comm);
                    }
                }
            }
        }

        private void disableMEMSMode(ref CommunicationManager comm)
        {
            comm.MEMSModeToSet = false;
            if (comm.RxCtrl != null)
            {
                comm.RxCtrl.SetMEMSMode(0);
            }
            comm.dataGui.MEMS_State = -1;
            comm.dataGui.CalStatus = 0;
        }

        private void disableMEMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0)) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (!(str == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager manager = (PortManager) PortManagerHash[str];
                            if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                            {
                                disableMEMSMode(ref manager.comm);
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                    {
                        disableMEMSMode(ref manager2.comm);
                    }
                }
                updateDisableMEMSViewBtn();
            }
        }

        private void disableSBASRangingMode(ref CommunicationManager comm)
        {
            try
            {
                comm.SBASRangingToSet = false;
                comm.SBASRangingPendingSet = true;
                comm.RxCtrl.PollNavigationParameters();
            }
            catch
            {
            }
        }

        private void disableSBASRangingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0)) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (!(str == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager manager = (PortManager) PortManagerHash[str];
                            if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                            {
                                disableSBASRangingMode(ref manager.comm);
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                    {
                        disableSBASRangingMode(ref manager2.comm);
                    }
                }
                updateDisableSBASRangingViewBtn();
            }
        }

        public void DisconnectPort(ref CommunicationManager comm)
        {
            if (comm != null)
            {
                comm.ClosePort();
                Thread.Sleep(10);
                comm.UpdatePortMainWinTitle -= new CommunicationManager.UpdateParentPortEventHandler(UpdateWinTitle);
                if (clsGlobal.CommWinRef.Contains(comm.PortName))
                {
                    clsGlobal.CommWinRef.Remove(comm.PortName);
                }
                sysCmdExec.CloseWinByProcId(comm.HostAppCmdWinId);
            }
        }

        private void displaySingleTestError(string errorString)
        {
            MessageBox.Show(string.Format("Auto test encounters error\n{0}", errorString), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            clsGlobal.TestsToRun.Clear();
            clsGlobal.CurrentRunningTest = string.Empty;
            clsGlobal.ScriptDone = true;
            clsGlobal.Abort = false;
            clsGlobal.AbortSingle = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dopMaskClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateDOPMaskWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateDOPMaskWin(ref target);
                    }
                }
            }
        }

        private void dOPMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dopMaskClickHandler();
        }

        private void DRNavStatusClick()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            CreateDRStatusWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        CreateDRStatusWin(manager2);
                    }
                }
                updateDRStatusViewBtn();
            }
        }

        private void DRSensorDataClick()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            CreateDRSensorDataWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        CreateDRSensorDataWin(manager2);
                    }
                }
                updateDRSensorDataViewBtn();
            }
        }

        private void dRSensorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateDRSensorParamWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateDRSensorParamWin(ref target);
                    }
                }
            }
        }

        private void elevationMaskClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateElevationMaskWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateElevationMaskWin(ref target);
                    }
                }
            }
        }

        private void elevationMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            elevationMaskClickHandler();
        }

        private void enable5HzNavToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0)) && (MessageBox.Show(string.Format("{0}\n{1}\n\n{2}", "WARNING: Slower baud rates may drop messages", " since messages will come out 5x faster when 5Hz nav mode is enabled.", "Proceed?"), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str2 in PortManagerHash.Keys)
                    {
                        if (!(str2 == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager manager = (PortManager) PortManagerHash[str2];
                            if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                            {
                                set5HzNavModeHandler(ref manager.comm);
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                    {
                        set5HzNavModeHandler(ref manager2.comm);
                    }
                }
            }
        }

        private void enableABPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (!(str == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager manager = (PortManager) PortManagerHash[str];
                            if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                            {
                                setABPModeHandler(ref manager.comm);
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                    {
                        setABPModeHandler(ref manager2.comm);
                    }
                }
            }
        }

        private void enableDisableMenuAndButton(bool state)
        {
            EventHandler method = null;
            if (clsGlobal.IsMarketingUser())
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            localEnableDisableMenuAndButton(state);
                        };
                    }
                    base.BeginInvoke(method);
                }
                else
                {
                    localEnableDisableMenuAndButton(state);
                }
            }
        }

        private void EnableDisableMenuAndButtonForFilePlayback(bool state)
        {
            EventHandler method = null;
            if (clsGlobal.IsMarketingUser())
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            localEnableDisableMenuAndButtonForFilePlayback(state);
                        };
                    }
                    base.BeginInvoke(method);
                }
                else
                {
                    localEnableDisableMenuAndButtonForFilePlayback(state);
                }
            }
        }

        private void enableDisableMenuAndButtonPerProductType(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localEnableDisableMenuAndButtonPerProductType(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localEnableDisableMenuAndButtonPerProductType(state);
            }
        }

        private void enableDisableMenuAndButtonPerProtocol(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localEnableDisableMenuAndButtonPerProtocol(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localEnableDisableMenuAndButtonPerProtocol(state);
            }
        }

        private void enableMEMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0)) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (!(str == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager manager = (PortManager) PortManagerHash[str];
                            if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                            {
                                setMEMSModeHandler(ref manager.comm);
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                    {
                        setMEMSModeHandler(ref manager2.comm);
                    }
                }
                updateEnableMEMSViewBtn();
            }
        }

        private void enableSBASRangingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0)) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            setSBASRangingModeHandler(ref manager.comm);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                    {
                        setSBASRangingModeHandler(ref manager2.comm);
                    }
                }
                updateEnableSBASRangingBtn();
            }
        }

        private static string encryptALine(string psText)
        {
            int num;
            Random random = new Random();
            string str = "";
            for (num = 0; num <= (psText.Length - 1); num++)
            {
                char ch = (char) random.Next(0xffff);
                str = str + ((char) (psText[num] ^ ch)).ToString() + ((char) (ch ^ (0xffff - num))).ToString();
                str = str + ((char) random.Next(0xffff)).ToString();
            }
            string str2 = "";
            for (num = 0; num <= (str.Length - 1); num++)
            {
                str2 = str2 + ((char) (str[num] ^ '}')).ToString();
            }
            return str2;
        }

        private static void encryptAndDecryptAFile(string inputFile, string outputFile, bool isEnc)
        {
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("Input file does not exist");
            }
            else
            {
                StreamWriter writer;
                StreamReader reader;
                if (isEnc)
                {
                    reader = new StreamReader(inputFile);
                    writer = new StreamWriter(outputFile, false, Encoding.UTF7);
                }
                else
                {
                    reader = new StreamReader(inputFile, Encoding.UTF7);
                    writer = new StreamWriter(outputFile, false);
                }
                for (string str = reader.ReadLine(); str != null; str = reader.ReadLine())
                {
                    string str2 = string.Empty;
                    if (isEnc)
                    {
                        str2 = encryptALine(str);
                    }
                    else
                    {
                        str2 = decryptALine(str);
                    }
                    writer.WriteLine(str2);
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        private void errorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            errorViewManualClickHandler();
        }

        private void errorViewManualClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target._errorView = CreateErrorViewWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._errorView = CreateErrorViewWin(manager2);
                    }
                }
                updateErrorViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void exitMenu_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            base.Close();
        }

        private void featuresCWDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cwDetectionClickHandler();
        }

        private void featuresSiRFawareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            siRFawareClickHandler();
        }

        private void fileAnalysisMenu_Click(object sender, EventArgs e)
        {
            CreateFileAnalysisWindow(".gps");
        }

        private void fileCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileReplayCloseActionHandler();
        }

        private void fileMenuItem_Click(object sender, EventArgs e)
        {
            setAddReceiverMenuState();
        }

        private void fileOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filePlaybackOpenActionHandler();
        }

        private void filePlaybackOpenActionHandler()
        {
            fileReplayOpenHandler();
            restoreDefaultPortLayout(true);
            updateAllMainBtn();
            logManagerStatusLabel.Text = string.Format("Playback File: {0}", _logFileName);
        }

        private void filePlayBackTrackBar_Scroll(object sender, EventArgs e)
        {
            if (_playState == _playStates.PAUSE)
            {
                PortManager manager = (PortManager) PortManagerHash[clsGlobal.FilePlayBackPortName];
                if (manager != null)
                {
                    manager.comm.WriteApp("User marker: user changes file position");
                }
                _fileIndex = (long) ((filePlayBackTrackBar.Value * _totalFileSize) / 100.0);
                _epochList.Clear();
                _epochIndex = 0;
            }
        }

        private void fileReplayCloseActionHandler()
        {
            fileReplayCloseHandler();
            closeAllWindows();
            updateFilePlaybackBtn(false);
            updateAllMainBtn();
            menuBtnInit();
            logManagerStatusLabel.Text = string.Empty;
        }

        private void fileReplayCloseHandler()
        {
            if (_isFileOpen)
            {
                _playState = _playStates.IDLE;
                _playState = _playStates.QUIT;
                if (_fileHdlr != null)
                {
                    _fileHdlr.Close();
                }
                _fileHdlr = null;
                _isFileOpen = false;
                _logFileName = string.Empty;
                toolStripOpenFileBtn.Enabled = true;
                fileOpenToolStripMenuItem.Enabled = true;
                filePlayBackTrackBar.Enabled = true;
                filePlayBackTrackBar.Visible = false;
                Text = string.Format("{0}: File Playback Close", clsGlobal.SiRFLiveVersion);
            }
        }

        private void fileReplayMenu_Click(object sender, EventArgs e)
        {
            CreateFileReplayWindow();
        }

        private DialogResult fileReplayOpenHandler()
        {
            bool flag = false;
            foreach (string str in PortManagerHash.Keys)
            {
                if (!(str == clsGlobal.FilePlayBackPortName))
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (flag && (MessageBox.Show("Proceeding will close all open ports. Continue?", "File Playback Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No))
            {
                return DialogResult.Cancel;
            }
            foreach (string str2 in PortManagerHash.Keys)
            {
                if (str2 != clsGlobal.FilePlayBackPortName)
                {
                    PortManager tmpP = (PortManager) PortManagerHash[str2];
                    if (tmpP != null)
                    {
                        if (tmpP.comm.IsSourceDeviceOpen())
                        {
                            tmpP.comm.ClosePort();
                        }
                        tmpP.CloseAll();
                        updateGUIOnConnectNDisconnect(tmpP);
                    }
                }
            }
            updateAllMainBtn();
            menuBtnInit();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Specify log file name:";
            dialog.InitialDirectory = @"..\..\logs\";
            dialog.Filter = "GP2 (*.gp2)|*.gp2|GPS (*.gps)|*.gps|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.FilterIndex = 4;
            dialog.CheckPathExists = false;
            dialog.CheckFileExists = false;
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                goto Label_04AE;
            }
            _logFileName = dialog.FileName;
            if ((_logFileName == string.Empty) || !File.Exists(_logFileName))
            {
                MessageBox.Show(string.Format("Error Open file\n {0}", _logFileName), "File Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                _playState = _playStates.IDLE;
                return DialogResult.Cancel;
            }
            Text = string.Format("{0}: File Playback", clsGlobal.SiRFLiveVersion);
            _fileHdlr = new LargeFileHandler(_logFileName);
            _totalFileSize = _fileHdlr.Length;
            filePlayBackTrackBar.Value = 0;
            filePlayBackTrackBar.Maximum = 100;
            filePlayBackTrackBar.Minimum = 0;
            filePlayBackTrackBar.Enabled = true;
            filePlayBackTrackBar.Visible = true;
            _playState = _playStates.IDLE;
            _lastPlayState = _playState;
            FileInfo info = new FileInfo(_logFileName);
            if (info.Length == 0L)
            {
                MessageBox.Show(string.Format("0 length file dectected!\n{0}", _logFileName), "File Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return DialogResult.Cancel;
            }
            string str3 = info.Extension.ToUpper();
            if (str3 != null)
            {
                if (!(str3 == ".GP2") && !(str3 == ".GPX"))
                {
                    if (str3 == ".GPS")
                    {
                        _viewType = CommonClass.TransmissionType.GPS;
                        _fileType = CommonClass.TransmissionType.GPS;
                        goto Label_033C;
                    }
                    if (str3 == ".TXT")
                    {
                        _viewType = CommonClass.TransmissionType.Text;
                        _fileType = CommonClass.TransmissionType.Text;
                        goto Label_033C;
                    }
                    if (str3 == ".BIN")
                    {
                        _viewType = CommonClass.TransmissionType.Hex;
                        _fileType = CommonClass.TransmissionType.Hex;
                        goto Label_033C;
                    }
                }
                else
                {
                    _viewType = CommonClass.TransmissionType.GPS;
                    _fileType = CommonClass.TransmissionType.GP2;
                    goto Label_033C;
                }
            }
            _viewType = CommonClass.TransmissionType.Hex;
            _fileType = CommonClass.TransmissionType.Hex;
        Label_033C:
            _processFileLog = info.DirectoryName + info.Name + ".par";
            PortManager manager3 = setupFileReplayPort();
            if (manager3 != null)
            {
                manager3.UpdateMainWindow += new PortManager.updateParentEventHandler(updateMainWindowTitle);
                if (!toolStripPortComboBox.Items.Contains(clsGlobal.FilePlayBackPortName))
                {
                    toolStripPortComboBox.Items.Add(clsGlobal.FilePlayBackPortName);
                }
                toolStripPortComboBox.Text = clsGlobal.FilePlayBackPortName;
                if (_lastPlayState == _playStates.IDLE)
                {
                    manager3.comm.WriteApp("User marker: Open file: " + _logFileName);
                }
                manager3.PerPortToolStrip = AddPortToolbar((toolStripMain.Location.Y + (0x19 * PortManagerHash.Count)) + 0x23, manager3.comm.PortName);
                manager3.ClearSubWindowsData(true);
            }
            else
            {
                MessageBox.Show("Error Setup Port for File Playback", "File Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                _playState = _playStates.IDLE;
                return DialogResult.Cancel;
            }
            fileOpenToolStripMenuItem.Enabled = false;
            toolStripOpenFileBtn.Enabled = false;
            logManagerStatusLabel.Text = _logFileName;
            _isFileOpen = true;
            _parseThread = new Thread(new ThreadStart(parseFile));
            _parseThread.IsBackground = true;
            _parseThread.Start();
            EnableDisableMenuAndButtonForFilePlayback(true);
            updateFilePlaybackBtn(true);
        Label_04AE:
            return base.DialogResult;
        }

        private void frmMDIMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            clsGlobal.ScriptDone = true;
            try
            {
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
                Cleanup();
                cleanupPortManager();
            }
            catch
            {
            }
            Application.DoEvents();
        }

        private void frmMDIMain_Load(object sender, EventArgs e)
        {
            frmSaveSettingsLoad(_lastWindowsRestoredFilePath);
            ChangeRestoreLayoutState(true);

            filePlayBackTrackBar.Visible = false;
            filePlayBackTrackBar.Value = 0;
            filePlayBackTrackBar.Maximum = 100;
            filePlayBackTrackBar.Minimum = 0;
            filePlayBackTrackBar.LargeChange = 5;
            filePlayBackTrackBar.SmallChange = 1;
            filePlayBackTrackBar.TickFrequency = 10;

            toolStripSeparator3.Visible = false;
            toolStripSeparator13.Visible = false;
            toolStripSeparator16.Visible = false;

            menuBtnInit();
            updateFilePlaybackBtn(false);
            startLogToolStripMenuItem.Enabled = true;
            stopLogToolStripMenuItem.Enabled = false;

            if (toolStripPortComboBox.Text == clsGlobal.FilePlayBackPortName)
            {
                toolStripPortComboBox.Text = "";
            }

            localEnableDisableDRMenus(false);
            toolStripUpDownArrowBtn.Visible = false;
        }

        private void frmSaveSettingsLoad(string filePath)
        {
            CommonUtilsClass class2 = new CommonUtilsClass();
            if (!File.Exists(filePath))
            {
                MessageBox.Show(string.Format("{0}\n not found use default", filePath), "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                filePath = _defaultWindowsRestoredFilePath;
            }
            isLoading = true;
            if (File.Exists(filePath))
            {
                try
                {
                    _appWindowsSettings.Load(filePath);
                    XmlNodeList list = _appWindowsSettings.SelectNodes("/windows/mainWindow");
                    object messageView = null;
                    object obj3 = null;
                    foreach (XmlNode node in list)
                    {
                        PortManager manager;
                        frmCommOpen open;
                        string str3;
                        string str4;
                        string str5;
                        string str7;
                        string str8;
                        string str9;
                        obj3 = null;
                        switch (node.Attributes["name"].Value.ToString())
                        {
                            case "Port":
                                if (node.Attributes["comport"].Value.ToString() == clsGlobal.FilePlayBackPortName)
                                {
                                    continue;
                                }
                                manager = new PortManager();
                                manager.comm.PortName = node.Attributes["comport"].Value.ToString();
                                manager.comm.sourceDeviceName = manager.comm.PortName;
                                manager.comm.MessageProtocol = node.Attributes["messageProtocol"].Value.ToString();
                                manager.comm.BaudRate = node.Attributes["baud"].Value.ToString();
                                manager.comm.FlowControl = Convert.ToInt32(node.Attributes["flowControl"].Value.ToString());
                                manager.comm.CMC.HostAppClient.TCPClientPortNum = Convert.ToInt32(node.Attributes["TCPClientPortNum"].Value);
                                manager.comm.CMC.HostAppClient.TCPClientHostName = node.Attributes["TCPClientHostName"].Value.ToString();
                                manager.comm.CMC.HostAppServer.TCPServerPortNum = Convert.ToInt32(node.Attributes["TCPServerPortNum"].Value);
                                manager.comm.CMC.HostAppServer.TCPServerHostName = node.Attributes["TCPServerHostName"].Value.ToString();
                                manager.comm.TrackerPort = node.Attributes["TrackerPort"].Value.ToString();
                                manager.comm.ResetPort = node.Attributes["ResetPort"].Value.ToString();
                                manager.comm.HostPair1 = node.Attributes["HostPort1"].Value.ToString();
                                manager.comm.HostSWFilePath = node.Attributes["HostAppFilePath"].Value.ToString();
                                manager.comm.HostAppCfgFilePath = node.Attributes["HostAppCfgFilePath"].Value.ToString();
                                manager.comm.HostAppMEMSCfgPath = node.Attributes["HostAppMEMSCfgPath"].Value.ToString();
                                manager.comm.DefaultTCXOFreq = node.Attributes["DefaultTCXOFreq"].Value.ToString();
                                manager.comm.LNAType = Convert.ToInt32(node.Attributes["LNAType"].Value.ToString());
                                manager.comm.ReadBuffer = Convert.ToInt32(node.Attributes["ReadBuffer"].Value.ToString());
                                manager.comm.LDOMode = Convert.ToInt32(node.Attributes["LDOMode"].Value.ToString());
                                manager.comm.RxName = node.Attributes["RxName"].Value.ToString();
                                manager.comm.IsVersion4_1_A8AndAbove = node.Attributes["IsVersionGreater4_1_A8"].Value.ToString() == "1";
                                manager.comm.EESelect = node.Attributes["EESelect"].Value.ToString();
                                manager.comm.ServerName = node.Attributes["ServerName"].Value.ToString();
                                manager.comm.ServerPort = node.Attributes["ServerPort"].Value.ToString();
                                manager.comm.AuthenticationCode = node.Attributes["AuthenticationCode"].Value.ToString();
                                manager.comm.EEDayNum = node.Attributes["EEDayNum"].Value.ToString();
                                manager.comm.BankTime = node.Attributes["BankTime"].Value.ToString();
                                manager.comm.ProductFamily = (CommonClass.ProductType) Convert.ToInt32(node.Attributes["ProdFamily"].Value.ToString());
                                if (!(node.Attributes["RequiredHostRun"].Value.ToString() == "1"))
                                {
                                    goto Label_06FD;
                                }
                                manager.comm.RequireHostRun = true;
                                if (!(node.Attributes["RequireEE"].Value.ToString() == "True"))
                                {
                                    break;
                                }
                                manager.comm.RequireEE = true;
                                goto Label_070A;

                            case "frmCommOpen":
                                obj3 = CreateCommWindow();
                                open = (frmCommOpen) obj3;
                                clsGlobal.IsMarketingUser();
                                open.comm.MessageProtocol = node.Attributes["messageProtocol"].Value.ToString();
                                open.comm.PortName = node.Attributes["comport"].Value.ToString();
                                open.comm.BaudRate = node.Attributes["baud"].Value.ToString();
                                open.comm.CMC.HostAppClient.TCPClientPortNum = Convert.ToInt32(node.Attributes["TCPClientPortNum"].Value);
                                open.comm.CMC.HostAppClient.TCPClientHostName = node.Attributes["TCPClientHostName"].Value.ToString();
                                open.comm.CMC.HostAppServer.TCPServerPortNum = Convert.ToInt32(node.Attributes["TCPServerPortNum"].Value);
                                open.comm.CMC.HostAppServer.TCPServerHostName = node.Attributes["TCPServerHostName"].Value.ToString();
                                open.comm.TrackerPort = node.Attributes["TrackerPort"].Value.ToString();
                                open.comm.ResetPort = node.Attributes["ResetPort"].Value.ToString();
                                open.comm.HostPair1 = node.Attributes["HostPort1"].Value.ToString();
                                open.comm.HostSWFilePath = node.Attributes["HostAppFilePath"].Value.ToString();
                                open.comm.DefaultTCXOFreq = node.Attributes["DefaultTCXOFreq"].Value.ToString();
                                open.comm.LNAType = Convert.ToInt32(node.Attributes["LNAType"].Value.ToString());
                                open.comm.ReadBuffer = Convert.ToInt32(node.Attributes["ReadBuffer"].Value.ToString());
                                open.comm.LDOMode = Convert.ToInt32(node.Attributes["LDOMode"].Value.ToString());
                                open.comm.RxName = node.Attributes["RxName"].Value.ToString();
                                open.comm.EESelect = node.Attributes["EESelect"].Value.ToString();
                                open.comm.ServerName = node.Attributes["ServerName"].Value.ToString();
                                open.comm.ServerPort = node.Attributes["ServerPort"].Value.ToString();
                                open.comm.AuthenticationCode = node.Attributes["AuthenticationCode"].Value.ToString();
                                open.comm.EEDayNum = node.Attributes["EEDayNum"].Value.ToString();
                                open.comm.BankTime = node.Attributes["BankTime"].Value.ToString();
                                open.comm.ProductFamily = (CommonClass.ProductType) Convert.ToInt32(node.Attributes["ProdFamily"].Value.ToString());
                                if (!(node.Attributes["RequiredHostRun"].Value.ToString() == "1"))
                                {
                                    goto Label_2402;
                                }
                                open.comm.RequireHostRun = true;
                                if (!(node.Attributes["RequireEE"].Value.ToString() == "True"))
                                {
                                    goto Label_23F3;
                                }
                                open.comm.RequireEE = true;
                                goto Label_240F;

                            case "frmPerformanceMonitor":
                                obj3 = CreatefrmPerformanceMonitorWindow();
                                goto Label_291E;

                            case "frmPython":
                                obj3 = CreatePythonWindow();
                                goto Label_291E;

                            case "frmAutomationTests":
                                obj3 = CreateAutomationTestWindow();
                                goto Label_291E;

                            case "frmFileReplay":
                                obj3 = CreateFileReplayWindow();
                                goto Label_291E;
							/*
							 * //!
                            case "frmGPIBCtrl":
                                obj3 = CreateGPIBCtrlWindow();
                                goto Label_291E;
							*/
                            case "frmRackCtrl":
                                obj3 = CreateTestRackCtrl();
                                goto Label_291E;

                            case "frmSimplexCtrl":
                                obj3 = CreateSimplexCtrlWindow();
                                goto Label_291E;

                            case "frmSPAzCtrl":
                                obj3 = CreateSPAzCtrlWindow();
                                goto Label_291E;

                            case "frmRFPlaybackConfig":
                                obj3 = CreateRFReplayConfigWindow();
                                goto Label_291E;

                            case "frmRFPlaybackCtrl":
                                obj3 = CreateRFReplayPlaybackWindow();
                                goto Label_291E;

                            case "frmRFCaptureCtrl":
                                obj3 = CreateRFReplayCaptureWindow();
                                goto Label_291E;

                            case "frmE911Report":
                                obj3 = CreateE911CtrlWindow("E911");
                                goto Label_291E;

                            case "frmNavPerformanceReport":
                                obj3 = CreatePerformanceReportCtrlWindow();
                                goto Label_291E;

                            default:
                                goto Label_291E;
                        }
                        manager.comm.RequireEE = false;
                        goto Label_070A;
                    Label_06FD:
                        manager.comm.RequireHostRun = false;
                    Label_070A:
                        if ((str3 = node.Attributes["InputDeviceMode"].Value.ToString()) == null)
                        {
                            goto Label_0783;
                        }
                        if (!(str3 == "1"))
                        {
                            if (str3 == "2")
                            {
                                goto Label_0765;
                            }
                            if (str3 == "3")
                            {
                                goto Label_0774;
                            }
                            goto Label_0783;
                        }
                        manager.comm.InputDeviceMode = CommonClass.InputDeviceModes.RS232;
                        goto Label_0790;
                    Label_0765:
                        manager.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Client;
                        goto Label_0790;
                    Label_0774:
                        manager.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Server;
                        goto Label_0790;
                    Label_0783:
                        manager.comm.InputDeviceMode = CommonClass.InputDeviceModes.FilePlayBack;
                    Label_0790:
                        if ((str4 = node.Attributes["rxType"].Value.ToString()) != null)
                        {
                            if (!(str4 == "SLC"))
                            {
                                if (str4 == "GSW")
                                {
                                    goto Label_07F9;
                                }
                                if (str4 == "TTB")
                                {
                                    goto Label_0808;
                                }
                                if (str4 == "NMEA")
                                {
                                    goto Label_0817;
                                }
                            }
                            else
                            {
                                manager.comm.RxType = CommunicationManager.ReceiverType.SLC;
                            }
                        }
                        goto Label_0824;
                    Label_07F9:
                        manager.comm.RxType = CommunicationManager.ReceiverType.GSW;
                        goto Label_0824;
                    Label_0808:
                        manager.comm.RxType = CommunicationManager.ReceiverType.TTB;
                        goto Label_0824;
                    Label_0817:
                        manager.comm.RxType = CommunicationManager.ReceiverType.NMEA;
                    Label_0824:
                        if ((str5 = node.Attributes["viewType"].Value.ToString()) != null)
                        {
                            if (!(str5 == "GPS"))
                            {
                                if (str5 == "GP2")
                                {
                                    goto Label_089E;
                                }
                                if (str5 == "HEX")
                                {
                                    goto Label_08AD;
                                }
                                if (str5 == "SSB")
                                {
                                    goto Label_08BC;
                                }
                                if (str5 == "TEXT")
                                {
                                    goto Label_08CB;
                                }
                            }
                            else
                            {
                                manager.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GPS;
                            }
                        }
                        goto Label_08D8;
                    Label_089E:
                        manager.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GP2;
                        goto Label_08D8;
                    Label_08AD:
                        manager.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
                        goto Label_08D8;
                    Label_08BC:
                        manager.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.SSB;
                        goto Label_08D8;
                    Label_08CB:
                        manager.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
                    Label_08D8:
                        class2.DisplayBuffer = Convert.ToInt32(node.Attributes["bufferSize"].Value.ToString());
                        manager.comm.AutoReplyCtrl.ControlChannelVersion = node.Attributes["controlVersion"].Value.ToString();
                        manager.comm.AutoReplyCtrl.AidingProtocolVersion = node.Attributes["aidingVersion"].Value.ToString();
                        foreach (XmlNode node2 in node)
                        {
                            messageView = null;
                            switch (node2.Attributes["name"].Value.ToString())
                            {
                                case "frmCommInputMessage":
                                    manager._inputCommands = CreateInputCommandsWin(manager);
                                    if (manager._inputCommands != null)
                                    {
                                        manager._inputCommands.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._inputCommands.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._inputCommands.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._inputCommands.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.InputCommandLocation.Top = manager._inputCommands.WinTop;
                                        manager.InputCommandLocation.Left = manager._inputCommands.WinLeft;
                                        manager.InputCommandLocation.Width = manager._inputCommands.WinWidth;
                                        manager.InputCommandLocation.Height = manager._inputCommands.WinHeight;
                                        manager.InputCommandLocation.IsOpen = true;
                                        messageView = manager._inputCommands;
                                    }
                                    break;

                                case "frmCommRadarMap":
                                    manager._svsMapPanel = CreateRadarViewWin(manager);
                                    if (manager._svsMapPanel != null)
                                    {
                                        manager._svsMapPanel.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._svsMapPanel.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._svsMapPanel.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._svsMapPanel.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.SVsMapLocation.Top = manager._svsMapPanel.WinTop;
                                        manager.SVsMapLocation.Left = manager._svsMapPanel.WinLeft;
                                        manager.SVsMapLocation.Width = manager._svsMapPanel.WinWidth;
                                        manager.SVsMapLocation.Height = manager._svsMapPanel.WinHeight;
                                        manager.SVsMapLocation.IsOpen = true;
                                        messageView = manager._svsMapPanel;
                                    }
                                    break;

                                case "frmCommSVTrajectory":
                                    manager._svsTrajPanel = CreateSVTrajWin(manager);
                                    if (manager._svsTrajPanel != null)
                                    {
                                        manager._svsTrajPanel.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._svsTrajPanel.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._svsTrajPanel.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._svsTrajPanel.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.SVTrajViewLocation.Top = manager._svsTrajPanel.WinTop;
                                        manager.SVTrajViewLocation.Left = manager._svsTrajPanel.WinLeft;
                                        manager.SVTrajViewLocation.Width = manager._svsTrajPanel.WinWidth;
                                        manager.SVTrajViewLocation.Height = manager._svsTrajPanel.WinHeight;
                                        manager.SVTrajViewLocation.IsOpen = true;
                                        messageView = manager._svsTrajPanel;
                                    }
                                    break;

                                case "frmCommSVAvgCNo":
                                    manager._svsCNoPanel = CreateSVCNoWin(manager);
                                    if (manager._svsCNoPanel != null)
                                    {
                                        manager._svsCNoPanel.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._svsCNoPanel.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._svsCNoPanel.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._svsCNoPanel.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.SVCNoViewLocation.Top = manager._svsCNoPanel.WinTop;
                                        manager.SVCNoViewLocation.Left = manager._svsCNoPanel.WinLeft;
                                        manager.SVCNoViewLocation.Width = manager._svsCNoPanel.WinWidth;
                                        manager.SVCNoViewLocation.Height = manager._svsCNoPanel.WinHeight;
                                        manager.SVCNoViewLocation.IsOpen = true;
                                        messageView = manager._svsCNoPanel;
                                    }
                                    break;

                                case "frmCommSVTrackedVsTime":
                                    manager._svsTrackedVsTimePanel = CreateSVTrackedVsTimeWin(manager);
                                    if (manager._svsTrackedVsTimePanel != null)
                                    {
                                        manager._svsTrackedVsTimePanel.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._svsTrackedVsTimePanel.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._svsTrackedVsTimePanel.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._svsTrackedVsTimePanel.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.SVTrackedVsTimeViewLocation.Top = manager._svsTrackedVsTimePanel.WinTop;
                                        manager.SVTrackedVsTimeViewLocation.Left = manager._svsTrackedVsTimePanel.WinLeft;
                                        manager.SVTrackedVsTimeViewLocation.Width = manager._svsTrackedVsTimePanel.WinWidth;
                                        manager.SVTrackedVsTimeViewLocation.Height = manager._svsTrackedVsTimePanel.WinHeight;
                                        manager.SVTrackedVsTimeViewLocation.IsOpen = true;
                                        messageView = manager._svsTrackedVsTimePanel;
                                    }
                                    break;

                                case "frmSatelliteStats":
                                    manager._SatelliteStats = CreateSatelliteStatsWin(manager);
                                    if (manager._SatelliteStats != null)
                                    {
                                        manager._SatelliteStats.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._SatelliteStats.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._SatelliteStats.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._SatelliteStats.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.SatelliteStatsLocation.Top = manager._SatelliteStats.WinTop;
                                        manager.SatelliteStatsLocation.Left = manager._SatelliteStats.WinLeft;
                                        manager.SatelliteStatsLocation.Width = manager._SatelliteStats.WinWidth;
                                        manager.SatelliteStatsLocation.Height = manager._SatelliteStats.WinHeight;
                                        manager.SatelliteStatsLocation.IsOpen = true;
                                        messageView = manager._SatelliteStats;
                                    }
                                    break;

                                case "frmCommLocationMap":
                                    manager._locationViewPanel = CreateLocationMapWin(manager);
                                    if (manager._locationViewPanel != null)
                                    {
                                        manager._locationViewPanel.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._locationViewPanel.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._locationViewPanel.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._locationViewPanel.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.LocationMapLocation.Top = manager._locationViewPanel.WinTop;
                                        manager.LocationMapLocation.Left = manager._locationViewPanel.WinLeft;
                                        manager.LocationMapLocation.Width = manager._locationViewPanel.WinWidth;
                                        manager.LocationMapLocation.Height = manager._locationViewPanel.WinHeight;
                                        manager.LocationMapLocation.IsOpen = true;
                                        manager.comm.LocationMapRadius = Convert.ToDouble(node2.Attributes["locationMapRadius"].Value.ToString());
                                        messageView = manager._locationViewPanel;
                                    }
                                    break;

                                case "frmCommSignalView":
                                    manager._signalStrengthPanel = CreateSignalViewWin(manager);
                                    if (manager._signalStrengthPanel != null)
                                    {
                                        manager._signalStrengthPanel.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._signalStrengthPanel.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._signalStrengthPanel.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._signalStrengthPanel.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.SignalViewLocation.Top = manager._signalStrengthPanel.WinTop;
                                        manager.SignalViewLocation.Left = manager._signalStrengthPanel.WinLeft;
                                        manager.SignalViewLocation.Width = manager._signalStrengthPanel.WinWidth;
                                        manager.SignalViewLocation.Height = manager._signalStrengthPanel.WinHeight;
                                        manager.SignalViewLocation.IsOpen = true;
                                        messageView = manager._signalStrengthPanel;
                                    }
                                    break;

                                case "frmCommMessageFilter":
                                    manager.MessageView = CreateMessageViewWin(manager);
                                    if (manager.MessageView != null)
                                    {
                                        manager.MessageView.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager.MessageView.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager.MessageView.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager.MessageView.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.MessageViewLocation.Top = manager.MessageView.WinTop;
                                        manager.MessageViewLocation.Left = manager.MessageView.WinLeft;
                                        manager.MessageViewLocation.Width = manager.MessageView.WinWidth;
                                        manager.MessageViewLocation.Height = manager.MessageView.WinHeight;
                                        manager.MessageViewLocation.IsOpen = true;
                                        messageView = manager.MessageView;
                                    }
                                    break;

                                case "frmTTFFDisplay":
                                    manager._ttffDisplay = CreateTTFFWin(manager);
                                    if (manager._ttffDisplay != null)
                                    {
                                        manager._ttffDisplay.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._ttffDisplay.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._ttffDisplay.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._ttffDisplay.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.TTFFDisplayLocation.Top = manager._ttffDisplay.WinTop;
                                        manager.TTFFDisplayLocation.Left = manager._ttffDisplay.WinLeft;
                                        manager.TTFFDisplayLocation.Width = manager._ttffDisplay.WinWidth;
                                        manager.TTFFDisplayLocation.Height = manager._ttffDisplay.WinHeight;
                                        manager.TTFFDisplayLocation.IsOpen = true;
                                        messageView = manager._ttffDisplay;
                                    }
                                    break;

                                case "frmCommSiRFawareV2":
                                    manager._SiRFAware = CreateSiRFawareWin(manager);
                                    if (manager._SiRFAware != null)
                                    {
                                        manager._SiRFAware.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._SiRFAware.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._SiRFAware.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._SiRFAware.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.SiRFawareLocation.Top = manager._SiRFAware.WinTop;
                                        manager.SiRFawareLocation.Left = manager._SiRFAware.WinLeft;
                                        manager.SiRFawareLocation.Width = manager._SiRFAware.WinWidth;
                                        manager.SiRFawareLocation.Height = manager._SiRFAware.WinHeight;
                                        manager.SiRFawareLocation.IsOpen = true;
                                        messageView = manager._SiRFAware;
                                    }
                                    break;

                                case "frmCommDebugView":
                                    manager.DebugView = CreateDebugViewWin(manager);
                                    if (manager.DebugView != null)
                                    {
                                        manager.DebugView.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager.DebugView.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager.DebugView.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager.DebugView.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.DebugViewLocation.Top = manager.DebugView.WinTop;
                                        manager.DebugViewLocation.Left = manager.DebugView.WinLeft;
                                        manager.DebugViewLocation.Width = manager.DebugView.WinWidth;
                                        manager.DebugViewLocation.Height = manager.DebugView.WinHeight;
                                        manager.DebugViewLocation.IsOpen = true;
                                        messageView = manager.DebugView;
                                    }
                                    break;

                                case "frmCommResponseView":
                                    manager._responseView = CreateResponseViewWin(manager);
                                    if (manager._responseView != null)
                                    {
                                        manager._responseView.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._responseView.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._responseView.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._responseView.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.ResponseViewLocation.Top = manager._responseView.WinTop;
                                        manager.ResponseViewLocation.Left = manager._responseView.WinLeft;
                                        manager.ResponseViewLocation.Width = manager._responseView.WinWidth;
                                        manager.ResponseViewLocation.Height = manager._responseView.WinHeight;
                                        manager.ResponseViewLocation.IsOpen = true;
                                        messageView = manager._responseView;
                                    }
                                    break;

                                case "frmCommErrorView":
                                    manager._errorView = CreateErrorViewWin(manager);
                                    if (manager._errorView != null)
                                    {
                                        manager._errorView.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._errorView.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._errorView.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._errorView.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.ErrorViewLocation.Top = manager._errorView.WinTop;
                                        manager.ErrorViewLocation.Left = manager._errorView.WinLeft;
                                        manager.ErrorViewLocation.Width = manager._errorView.WinWidth;
                                        manager.ErrorViewLocation.Height = manager._errorView.WinHeight;
                                        manager.ErrorViewLocation.IsOpen = true;
                                        messageView = manager._errorView;
                                    }
                                    break;

                                case "frmInterenceReport":
                                    manager._interferenceReport = CreateInterferenceReportWindow(manager);
                                    if (manager._interferenceReport != null)
                                    {
                                        manager._interferenceReport.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._interferenceReport.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._interferenceReport.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._interferenceReport.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.InterferenceLocation.Top = manager._interferenceReport.WinTop;
                                        manager.InterferenceLocation.Left = manager._interferenceReport.WinLeft;
                                        manager.InterferenceLocation.Width = manager._interferenceReport.WinWidth;
                                        manager.InterferenceLocation.Height = manager._interferenceReport.WinHeight;
                                        manager.InterferenceLocation.IsOpen = true;
                                        messageView = manager._interferenceReport;
                                    }
                                    break;

                                case "frmCompassView":
                                    manager._compassView = CreateCompassViewWin(manager);
                                    if (manager._compassView != null)
                                    {
                                        manager._compassView.WinTop = Convert.ToInt32(node2.Attributes["top"].Value);
                                        manager._compassView.WinLeft = Convert.ToInt32(node2.Attributes["left"].Value);
                                        manager._compassView.WinWidth = Convert.ToInt32(node2.Attributes["width"].Value);
                                        manager._compassView.WinHeight = Convert.ToInt32(node2.Attributes["height"].Value);
                                        manager.CompassViewLocation.Top = manager._compassView.WinTop;
                                        manager.CompassViewLocation.Left = manager._compassView.WinLeft;
                                        manager.CompassViewLocation.Width = manager._compassView.WinWidth;
                                        manager.CompassViewLocation.Height = manager._compassView.WinHeight;
                                        manager.CompassViewLocation.IsOpen = true;
                                        messageView = manager._compassView;
                                    }
                                    break;
                            }
                            if (messageView != null)
                            {
                                loadLocation((Form) messageView, node2.Attributes["top"].Value.ToString(), node2.Attributes["left"].Value.ToString(), node2.Attributes["width"].Value.ToString(), node2.Attributes["height"].Value.ToString(), node2.Attributes["windowState"].Value.ToString());
                            }
                        }
                        if (!PortManagerHash.ContainsKey(manager.comm.PortName))
                        {
                            PortManagerHash.Add(manager.comm.PortName, manager);
                        }
                        else
                        {
                            PortManagerHash[manager.comm.PortName] = manager;
                        }
                        updateToolStripPortComboBox(manager.comm.PortName, true);
                        manager.UpdateMainWindow += new PortManager.updateParentEventHandler(updateMainWindowTitle);
                        toolStripNumPortTxtBox.Text = PortManagerHash.Count.ToString();
                        manager.PerPortToolStrip = AddPortToolbar((toolStripMain.Location.Y + (0x19 * PortManagerHash.Count)) + 0x23, manager.comm.PortName);
                        updateGUIOnConnectNDisconnect(manager);
                        manager.comm.SetupRxCtrl();
                        goto Label_291E;
                    Label_23F3:
                        open.comm.RequireEE = false;
                        goto Label_240F;
                    Label_2402:
                        open.comm.RequireHostRun = false;
                    Label_240F:
                        if ((str7 = node.Attributes["InputDeviceMode"].Value.ToString()) == null)
                        {
                            goto Label_2488;
                        }
                        if (!(str7 == "1"))
                        {
                            if (str7 == "2")
                            {
                                goto Label_246A;
                            }
                            if (str7 == "3")
                            {
                                goto Label_2479;
                            }
                            goto Label_2488;
                        }
                        open.comm.InputDeviceMode = CommonClass.InputDeviceModes.RS232;
                        goto Label_2495;
                    Label_246A:
                        open.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Client;
                        goto Label_2495;
                    Label_2479:
                        open.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Server;
                        goto Label_2495;
                    Label_2488:
                        open.comm.InputDeviceMode = CommonClass.InputDeviceModes.FilePlayBack;
                    Label_2495:
                        if ((str8 = node.Attributes["rxType"].Value.ToString()) != null)
                        {
                            if (!(str8 == "SLC"))
                            {
                                if (str8 == "GSW")
                                {
                                    goto Label_24FE;
                                }
                                if (str8 == "TTB")
                                {
                                    goto Label_250D;
                                }
                                if (str8 == "NMEA")
                                {
                                    goto Label_251C;
                                }
                            }
                            else
                            {
                                open.comm.RxType = CommunicationManager.ReceiverType.SLC;
                            }
                        }
                        goto Label_2529;
                    Label_24FE:
                        open.comm.RxType = CommunicationManager.ReceiverType.GSW;
                        goto Label_2529;
                    Label_250D:
                        open.comm.RxType = CommunicationManager.ReceiverType.TTB;
                        goto Label_2529;
                    Label_251C:
                        open.comm.RxType = CommunicationManager.ReceiverType.NMEA;
                    Label_2529:
                        if ((str9 = node.Attributes["viewType"].Value.ToString()) != null)
                        {
                            if (!(str9 == "GPS"))
                            {
                                if (str9 == "GP2")
                                {
                                    goto Label_25A3;
                                }
                                if (str9 == "HEX")
                                {
                                    goto Label_25B2;
                                }
                                if (str9 == "SSB")
                                {
                                    goto Label_25C1;
                                }
                                if (str9 == "TEXT")
                                {
                                    goto Label_25D0;
                                }
                            }
                            else
                            {
                                open.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GPS;
                            }
                        }
                        goto Label_25DD;
                    Label_25A3:
                        open.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GP2;
                        goto Label_25DD;
                    Label_25B2:
                        open.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
                        goto Label_25DD;
                    Label_25C1:
                        open.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.SSB;
                        goto Label_25DD;
                    Label_25D0:
                        open.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
                    Label_25DD:
                        class2.DisplayBuffer = Convert.ToInt32(node.Attributes["bufferSize"].Value.ToString());
                        open.comm.AutoReplyCtrl.ControlChannelVersion = node.Attributes["controlVersion"].Value.ToString();
                        open.comm.AutoReplyCtrl.AidingProtocolVersion = node.Attributes["aidingVersion"].Value.ToString();
                        foreach (XmlNode node3 in node)
                        {
                            messageView = null;
                            switch (node3.Attributes["name"].Value.ToString())
                            {
                                case "frmCommInputMessage":
                                    messageView = open.CreateInputCommandsWin();
                                    break;

                                case "frmCommRadarMap":
                                    messageView = open.CreateSVsMapWin();
                                    break;

                                case "frmSatelliteStats":
                                    messageView = open.CreateSatelliteStatsWin();
                                    break;

                                case "frmCommLocationMap":
                                    messageView = open.CreateLocationMapWin();
                                    open.comm.LocationMapRadius = Convert.ToDouble(node3.Attributes["locationMapRadius"].Value.ToString());
                                    break;

                                case "frmCommSignalView":
                                    messageView = open.CreateSignalViewWin();
                                    break;

                                case "frmEncryCtrl":
                                    messageView = open.CreateEncrypCtrlWin();
                                    break;

                                case "frmCommMessageFilter":
                                    open.frmCommOpenToolFilterCustom_Create();
                                    break;

                                case "frmTTFFDisplay":
                                    messageView = open.CreateTTFFWin();
                                    break;

                                case "frmSiRFAware":
                                    messageView = open.CreateSiRFAwareWin();
                                    break;
                            }
                            if (messageView != null)
                            {
                                loadLocation((Form) messageView, node3.Attributes["top"].Value.ToString(), node3.Attributes["left"].Value.ToString(), node3.Attributes["width"].Value.ToString(), node3.Attributes["height"].Value.ToString(), node3.Attributes["windowState"].Value.ToString());
                            }
                        }
                    Label_291E:
                        if (obj3 != null)
                        {
                            loadLocation((Form) obj3, node.Attributes["top"].Value.ToString(), node.Attributes["left"].Value.ToString(), node.Attributes["width"].Value.ToString(), node.Attributes["height"].Value.ToString(), node.Attributes["windowState"].Value.ToString());
                        }
                    }
                    if ((PortManagerHash.Count > 1) && !toolStripPortComboBox.Items.Contains("All"))
                    {
                        toolStripPortComboBox.Items.Add("All");
                    }
                    if (toolStripNumPortTxtBox.Text == clsGlobal.FilePlayBackPortName)
                    {
                        toolStripNumPortTxtBox.Text = "";
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("frmMDIMain() + frmSaveSettingsLoad() " + exception.ToString());
                }
            }
            isLoading = false;
            Refresh();
        }

        private void frmSaveSettingsOnClosing(string filePath)
        {
            StreamWriter writer;
            CommonUtilsClass class2 = new CommonUtilsClass();
            if (File.Exists(filePath))
            {
                if ((File.GetAttributes(filePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    MessageBox.Show(string.Format("File is read only - Window locations were not saved!\n{0}", filePath), "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Cursor = Cursors.Default;
                    return;
                }
                writer = new StreamWriter(filePath);
            }
            else
            {
                writer = File.CreateText(filePath);
            }
            if (writer != null)
            {
                object[] objArray;
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<windows>");
                string str2 = string.Empty;
                foreach (string str3 in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str3];
                    if ((manager != null) && (manager.comm.PortName != clsGlobal.FilePlayBackPortName))
                    {
                        int num = manager.comm.RequireHostRun ? 1 : 0;
                        int num2 = 0;
                        if (manager.comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
                        {
                            num2 = 1;
                        }
                        else if (manager.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client)
                        {
                            num2 = 2;
                        }
                        else if (manager.comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server)
                        {
                            num2 = 3;
                        }
                        else if (manager.comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
                        {
                            num2 = 4;
                        }
                        else
                        {
                            num2 = 0;
                        }
                        string format = "<mainWindow name=\"{0}\" comport=\"{1}\" baud=\"{2}\" rxType=\"{3}\" messageProtocol=\"{4}\" viewType=\"{5}\" bufferSize=\"{6}\" controlVersion=\"{7}\" aidingVersion=\"{8}\" TCPClientPortNum=\"{9}\" TCPClientHostName=\"{10}\" TCPServerPortNum=\"{11}\" TCPServerHostName=\"{12}\" TrackerPort=\"{13}\" ResetPort=\"{14}\" HostPort1=\"{15}\" RequiredHostRun=\"{16}\" InputDeviceMode=\"{17}\" HostAppFilePath=\"{18}\" DefaultTCXOFreq=\"{19}\" LNAType=\"{20}\" ReadBuffer=\"{21}\" LDOMode=\"{22}\" IsVersionGreater4_1_A8=\"{23}\" RequireEE=\"{24}\" EESelect=\"{25}\" ServerName=\"{26}\" ServerPort=\"{27}\" AuthenticationCode=\"{28}\" EEDayNum=\"{29}\" BankTime=\"{30}\" ProdFamily=\"{31}\" RxName=\"{32}\" flowControl=\"{33}\" HostAppCfgFilePath=\"{34}\" HostAppMEMSCfgPath=\"{35}\" >";
                        objArray = new object[0x24];
                        objArray[0] = "Port";
                        objArray[1] = manager.comm.PortName;
                        objArray[2] = manager.comm.BaudRate;
                        objArray[3] = manager.comm.RxType;
                        objArray[4] = manager.comm.MessageProtocol;
                        objArray[5] = manager.comm.RxCurrentTransmissionType.ToString();
                        int displayBuffer = class2.DisplayBuffer;
                        objArray[6] = displayBuffer.ToString();
                        objArray[7] = manager.comm.AutoReplyCtrl.ControlChannelVersion.ToString();
                        objArray[8] = manager.comm.AutoReplyCtrl.AidingProtocolVersion.ToString();
                        objArray[9] = manager.comm.CMC.HostAppClient.TCPClientPortNum;
                        objArray[10] = manager.comm.CMC.HostAppClient.TCPClientHostName;
                        objArray[11] = manager.comm.CMC.HostAppServer.TCPServerPortNum;
                        objArray[12] = manager.comm.CMC.HostAppServer.TCPServerHostName;
                        objArray[13] = manager.comm.TrackerPort;
                        objArray[14] = manager.comm.ResetPort;
                        objArray[15] = manager.comm.HostPair1;
                        objArray[0x10] = num;
                        objArray[0x11] = num2;
                        objArray[0x12] = manager.comm.HostSWFilePath;
                        objArray[0x13] = manager.comm.DefaultTCXOFreq;
                        objArray[20] = manager.comm.LNAType;
                        objArray[0x15] = manager.comm.ReadBuffer.ToString();
                        objArray[0x16] = manager.comm.LDOMode.ToString();
                        objArray[0x17] = manager.comm.IsVersion4_1_A8AndAbove ? "1" : "0";
                        objArray[0x18] = manager.comm.RequireEE;
                        objArray[0x19] = manager.comm.EESelect;
                        objArray[0x1a] = manager.comm.ServerName;
                        objArray[0x1b] = manager.comm.ServerPort;
                        objArray[0x1c] = manager.comm.AuthenticationCode;
                        objArray[0x1d] = manager.comm.EEDayNum;
                        objArray[30] = manager.comm.BankTime;
                        objArray[0x1f] = ((int) manager.comm.ProductFamily).ToString();
                        objArray[0x20] = manager.comm.RxName;
                        objArray[0x21] = manager.comm.FlowControl;
                        objArray[0x22] = manager.comm.HostAppCfgFilePath;
                        objArray[0x23] = manager.comm.HostAppMEMSCfgPath;
                        str2 = string.Format(format, objArray);
                        writer.WriteLine(str2);
                        if ((manager._signalStrengthPanel != null) && manager.SignalViewLocation.IsOpen)
                        {
                            frmCommSignalView view = manager._signalStrengthPanel;
                            manager.SignalViewLocation.Width = view.Width;
                            manager.SignalViewLocation.Height = view.Height;
                            manager.SignalViewLocation.Left = view.Left;
                            manager.SignalViewLocation.Top = view.Top;
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", new object[] { view.Name, view.Top.ToString(), view.Left.ToString(), view.Width.ToString(), view.Height.ToString(), view.WindowState.ToString() });
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._svsMapPanel != null) && manager.SVsMapLocation.IsOpen)
                        {
                            frmCommRadarMap map = manager._svsMapPanel;
                            manager.SVsMapLocation.Width = map.Width;
                            manager.SVsMapLocation.Height = map.Height;
                            manager.SVsMapLocation.Left = map.Left;
                            manager.SVsMapLocation.Top = map.Top;
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", new object[] { map.Name, map.Top.ToString(), map.Left.ToString(), map.Width.ToString(), map.Height.ToString(), map.WindowState.ToString() });
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._svsTrajPanel != null) && manager.SVTrajViewLocation.IsOpen)
                        {
                            frmCommSVTrajectory trajectory = manager._svsTrajPanel;
                            manager.SVTrajViewLocation.Width = trajectory.Width;
                            manager.SVTrajViewLocation.Height = trajectory.Height;
                            manager.SVTrajViewLocation.Left = trajectory.Left;
                            manager.SVTrajViewLocation.Top = trajectory.Top;
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", new object[] { trajectory.Name, trajectory.Top.ToString(), trajectory.Left.ToString(), trajectory.Width.ToString(), trajectory.Height.ToString(), trajectory.WindowState.ToString() });
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._svsCNoPanel != null) && manager.SVCNoViewLocation.IsOpen)
                        {
                            frmCommSVAvgCNo no = manager._svsCNoPanel;
                            manager.SVCNoViewLocation.Width = no.Width;
                            manager.SVCNoViewLocation.Height = no.Height;
                            manager.SVCNoViewLocation.Left = no.Left;
                            manager.SVCNoViewLocation.Top = no.Top;
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", new object[] { no.Name, no.Top.ToString(), no.Left.ToString(), no.Width.ToString(), no.Height.ToString(), no.WindowState.ToString() });
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._svsTrackedVsTimePanel != null) && manager.SVTrackedVsTimeViewLocation.IsOpen)
                        {
                            frmCommSVTrackedVsTime time = manager._svsTrackedVsTimePanel;
                            manager.SVTrackedVsTimeViewLocation.Width = time.Width;
                            manager.SVTrackedVsTimeViewLocation.Height = time.Height;
                            manager.SVTrackedVsTimeViewLocation.Left = time.Left;
                            manager.SVTrackedVsTimeViewLocation.Top = time.Top;
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", new object[] { time.Name, time.Top.ToString(), time.Left.ToString(), time.Width.ToString(), time.Height.ToString(), time.WindowState.ToString() });
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager.NavVsTimeView != null) && manager.NavVsTimeLocation.IsOpen)
                        {
                            frmCommNavAccVsTime navVsTimeView = manager.NavVsTimeView;
                            manager.NavVsTimeLocation.Width = navVsTimeView.Width;
                            manager.NavVsTimeLocation.Height = navVsTimeView.Height;
                            manager.NavVsTimeLocation.Left = navVsTimeView.Left;
                            manager.NavVsTimeLocation.Top = navVsTimeView.Top;
                            object[] args = new object[6];
                            args[0] = navVsTimeView.Name;
                            args[1] = navVsTimeView.Top.ToString();
                            args[2] = navVsTimeView.Left.ToString();
                            displayBuffer = navVsTimeView.Width;
                            args[3] = displayBuffer.ToString();
                            displayBuffer = navVsTimeView.Height;
                            args[4] = displayBuffer.ToString();
                            args[5] = navVsTimeView.WindowState.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", args);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._SatelliteStats != null) && manager.SatelliteStatsLocation.IsOpen)
                        {
                            frmSatelliteStats stats = manager._SatelliteStats;
                            manager.SatelliteStatsLocation.Width = stats.Width;
                            manager.SatelliteStatsLocation.Height = stats.Height;
                            manager.SatelliteStatsLocation.Left = stats.Left;
                            manager.SatelliteStatsLocation.Top = stats.Top;
                            objArray = new object[6];
                            objArray[0] = stats.Name;
                            displayBuffer = stats.Top;
                            objArray[1] = displayBuffer.ToString();
                            displayBuffer = stats.Left;
                            objArray[2] = displayBuffer.ToString();
                            displayBuffer = stats.Width;
                            objArray[3] = displayBuffer.ToString();
                            displayBuffer = stats.Height;
                            objArray[4] = displayBuffer.ToString();
                            objArray[5] = stats.WindowState.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._locationViewPanel != null) && manager.LocationMapLocation.IsOpen)
                        {
                            frmCommLocationMap map2 = manager._locationViewPanel;
                            manager.LocationMapLocation.Width = map2.Width;
                            manager.LocationMapLocation.Height = map2.Height;
                            manager.LocationMapLocation.Left = map2.Left;
                            manager.LocationMapLocation.Top = map2.Top;
                            objArray = new object[7];
                            objArray[0] = map2.Name;
                            displayBuffer = map2.Top;
                            objArray[1] = displayBuffer.ToString();
                            displayBuffer = map2.Left;
                            objArray[2] = displayBuffer.ToString();
                            displayBuffer = map2.Width;
                            objArray[3] = displayBuffer.ToString();
                            displayBuffer = map2.Height;
                            objArray[4] = displayBuffer.ToString();
                            objArray[5] = map2.WindowState.ToString();
                            objArray[6] = map2.CommWindow.LocationMapRadius.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\" locationMapRadius=\"{6}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._inputCommands != null) && manager.InputCommandLocation.IsOpen)
                        {
                            frmCommInputMessage message = manager._inputCommands;
                            manager.InputCommandLocation.Width = message.Width;
                            manager.InputCommandLocation.Height = message.Height;
                            manager.InputCommandLocation.Left = message.Left;
                            manager.InputCommandLocation.Top = message.Top;
                            objArray = new object[6];
                            objArray[0] = message.Name;
                            displayBuffer = message.Top;
                            objArray[1] = displayBuffer.ToString();
                            displayBuffer = message.Left;
                            objArray[2] = displayBuffer.ToString();
                            displayBuffer = message.Width;
                            objArray[3] = displayBuffer.ToString();
                            displayBuffer = message.Height;
                            objArray[4] = displayBuffer.ToString();
                            objArray[5] = message.WindowState.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._ttffDisplay != null) && manager.TTFFDisplayLocation.IsOpen)
                        {
                            frmTTFFDisplay display = manager._ttffDisplay;
                            manager.TTFFDisplayLocation.Width = display.Width;
                            manager.TTFFDisplayLocation.Height = display.Height;
                            manager.TTFFDisplayLocation.Left = display.Left;
                            manager.TTFFDisplayLocation.Top = display.Top;
                            objArray = new object[6];
                            objArray[0] = display.Name;
                            displayBuffer = display.Top;
                            objArray[1] = displayBuffer.ToString();
                            displayBuffer = display.Left;
                            objArray[2] = displayBuffer.ToString();
                            displayBuffer = display.Width;
                            objArray[3] = displayBuffer.ToString();
                            displayBuffer = display.Height;
                            objArray[4] = displayBuffer.ToString();
                            objArray[5] = display.WindowState.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager.DebugView != null) && manager.DebugViewLocation.IsOpen)
                        {
                            frmCommDebugView debugView = manager.DebugView;
                            manager.DebugViewLocation.Width = debugView.Width;
                            manager.DebugViewLocation.Height = debugView.Height;
                            manager.DebugViewLocation.Left = debugView.Left;
                            manager.DebugViewLocation.Top = debugView.Top;
                            objArray = new object[6];
                            objArray[0] = debugView.Name;
                            displayBuffer = debugView.Top;
                            objArray[1] = displayBuffer.ToString();
                            displayBuffer = debugView.Left;
                            objArray[2] = displayBuffer.ToString();
                            displayBuffer = debugView.Width;
                            objArray[3] = displayBuffer.ToString();
                            displayBuffer = debugView.Height;
                            objArray[4] = displayBuffer.ToString();
                            objArray[5] = debugView.WindowState.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._responseView != null) && manager.ResponseViewLocation.IsOpen)
                        {
                            frmCommResponseView view3 = manager._responseView;
                            manager.ResponseViewLocation.Width = view3.Width;
                            manager.ResponseViewLocation.Height = view3.Height;
                            manager.ResponseViewLocation.Left = view3.Left;
                            manager.ResponseViewLocation.Top = view3.Top;
                            objArray = new object[6];
                            objArray[0] = view3.Name;
                            displayBuffer = view3.Top;
                            objArray[1] = displayBuffer.ToString();
                            displayBuffer = view3.Left;
                            objArray[2] = displayBuffer.ToString();
                            displayBuffer = view3.Width;
                            objArray[3] = displayBuffer.ToString();
                            displayBuffer = view3.Height;
                            objArray[4] = displayBuffer.ToString();
                            objArray[5] = view3.WindowState.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._errorView != null) && manager.ErrorViewLocation.IsOpen)
                        {
                            frmCommErrorView view4 = manager._errorView;
                            manager.ErrorViewLocation.Width = view4.Width;
                            manager.ErrorViewLocation.Height = view4.Height;
                            manager.ErrorViewLocation.Left = view4.Left;
                            manager.ErrorViewLocation.Top = view4.Top;
                            objArray = new object[6];
                            objArray[0] = view4.Name;
                            displayBuffer = view4.Top;
                            objArray[1] = displayBuffer.ToString();
                            displayBuffer = view4.Left;
                            objArray[2] = displayBuffer.ToString();
                            displayBuffer = view4.Width;
                            objArray[3] = displayBuffer.ToString();
                            displayBuffer = view4.Height;
                            objArray[4] = displayBuffer.ToString();
                            objArray[5] = view4.WindowState.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._interferenceReport != null) && manager.InterferenceLocation.IsOpen)
                        {
                            frmInterferenceReport report = manager._interferenceReport;
                            manager.InterferenceLocation.Width = report.Width;
                            manager.InterferenceLocation.Height = report.Height;
                            manager.InterferenceLocation.Left = report.Left;
                            manager.InterferenceLocation.Top = report.Top;
                            objArray = new object[6];
                            objArray[0] = report.Name;
                            displayBuffer = report.Top;
                            objArray[1] = displayBuffer.ToString();
                            displayBuffer = report.Left;
                            objArray[2] = displayBuffer.ToString();
                            displayBuffer = report.Width;
                            objArray[3] = displayBuffer.ToString();
                            displayBuffer = report.Height;
                            objArray[4] = displayBuffer.ToString();
                            objArray[5] = report.WindowState.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._SiRFAware != null) && manager.SiRFawareLocation.IsOpen)
                        {
                            frmCommSiRFawareV2 ev = manager._SiRFAware;
                            manager.SiRFawareLocation.Width = ev.Width;
                            manager.SiRFawareLocation.Height = ev.Height;
                            manager.SiRFawareLocation.Left = ev.Left;
                            manager.SiRFawareLocation.Top = ev.Top;
                            objArray = new object[6];
                            objArray[0] = ev.Name;
                            displayBuffer = ev.Top;
                            objArray[1] = displayBuffer.ToString();
                            displayBuffer = ev.Left;
                            objArray[2] = displayBuffer.ToString();
                            displayBuffer = ev.Width;
                            objArray[3] = displayBuffer.ToString();
                            displayBuffer = ev.Height;
                            objArray[4] = displayBuffer.ToString();
                            objArray[5] = ev.WindowState.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager.MessageView != null) && manager.MessageViewLocation.IsOpen)
                        {
                            frmCommMessageFilter messageView = manager.MessageView;
                            manager.MessageViewLocation.Width = messageView.Width;
                            manager.MessageViewLocation.Height = messageView.Height;
                            manager.MessageViewLocation.Left = messageView.Left;
                            manager.MessageViewLocation.Top = messageView.Top;
                            objArray = new object[6];
                            objArray[0] = messageView.Name;
                            displayBuffer = messageView.Top;
                            objArray[1] = displayBuffer.ToString();
                            displayBuffer = messageView.Left;
                            objArray[2] = displayBuffer.ToString();
                            displayBuffer = messageView.Width;
                            objArray[3] = displayBuffer.ToString();
                            displayBuffer = messageView.Height;
                            objArray[4] = displayBuffer.ToString();
                            objArray[5] = messageView.WindowState.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        if ((manager._compassView != null) && manager.CompassViewLocation.IsOpen)
                        {
                            frmCompassView view5 = manager._compassView;
                            manager.CompassViewLocation.Width = view5.Width;
                            manager.CompassViewLocation.Height = view5.Height;
                            manager.CompassViewLocation.Left = view5.Left;
                            manager.CompassViewLocation.Top = view5.Top;
                            objArray = new object[6];
                            objArray[0] = view5.Name;
                            displayBuffer = view5.Top;
                            objArray[1] = displayBuffer.ToString();
                            displayBuffer = view5.Left;
                            objArray[2] = displayBuffer.ToString();
                            displayBuffer = view5.Width;
                            objArray[3] = displayBuffer.ToString();
                            displayBuffer = view5.Height;
                            objArray[4] = displayBuffer.ToString();
                            objArray[5] = view5.WindowState.ToString();
                            str2 = string.Format("<subWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</subWindow>");
                        }
                        writer.WriteLine("</mainWindow>");
                    }
                }
                foreach (Form form in base.MdiChildren)
                {
                    str2 = string.Empty;
                    switch (form.Name)
                    {
                        case "frmPerformanceMonitor":
                        case "frmPython":
                        case "frmAutomationTests":
                        case "frmFileReplay":
                        case "frmGPIBCtrl":
                        case "frmRackCtrl":
                        case "frmSimplexCtrl":
                        case "frmSPAzCtrl":
                        case "frmRFPlaybackConfig":
                        case "frmRFPlaybackCtrl":
                        case "frmE911Report":
                        case "frmNavPerformanceReport":
                            objArray = new object[] { form.Name, form.Top.ToString(), form.Left.ToString(), form.Width.ToString(), form.Height.ToString(), form.WindowState.ToString() };
                            str2 = string.Format("<mainWindow name=\"{0}\" top=\"{1}\" left=\"{2}\" width=\"{3}\" height=\"{4}\" windowState=\"{5}\">", objArray);
                            writer.WriteLine(str2);
                            writer.WriteLine("</mainWindow>");
                            break;
                    }
                }
                writer.WriteLine("</windows>");
                writer.Close();
            }
        }

        private bool frmSaveSettingsRead(string filePath)
        {
            CommonUtilsClass class2 = new CommonUtilsClass();
            bool flag = false;
            if (!File.Exists(filePath))
            {
                MessageBox.Show(string.Format("{0}\n not found use default", filePath), "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                filePath = _defaultWindowsRestoredFilePath;
            }
            if (File.Exists(filePath))
            {
                try
                {
                    _appWindowsSettings.Load(filePath);
                    XmlNodeList list = _appWindowsSettings.SelectNodes("/windows/mainWindow");
                    PortManager manager = null;
                    foreach (XmlNode node in list)
                    {
                        string str2;
                        string str4;
                        string str5;
                        if ((((str2 = node.Attributes["name"].Value.ToString()) == null) || !(str2 == "Port")) || (node.Attributes["comport"].Value.ToString() == clsGlobal.FilePlayBackPortName))
                        {
                            continue;
                        }
                        if (PortManagerHash.Count != 0)
                        {
                            goto Label_1411;
                        }
                        manager = new PortManager();
                        manager.comm.PortName = node.Attributes["comport"].Value.ToString();
                        manager.comm.sourceDeviceName = manager.comm.PortName;
                        manager.comm.MessageProtocol = node.Attributes["messageProtocol"].Value.ToString();
                        manager.comm.BaudRate = node.Attributes["baud"].Value.ToString();
                        manager.comm.FlowControl = Convert.ToInt32(node.Attributes["flowControl"].Value.ToString());
                        manager.comm.CMC.HostAppClient.TCPClientPortNum = Convert.ToInt32(node.Attributes["TCPClientPortNum"].Value);
                        manager.comm.CMC.HostAppClient.TCPClientHostName = node.Attributes["TCPClientHostName"].Value.ToString();
                        manager.comm.CMC.HostAppServer.TCPServerPortNum = Convert.ToInt32(node.Attributes["TCPServerPortNum"].Value);
                        manager.comm.CMC.HostAppServer.TCPServerHostName = node.Attributes["TCPServerHostName"].Value.ToString();
                        manager.comm.TrackerPort = node.Attributes["TrackerPort"].Value.ToString();
                        manager.comm.ResetPort = node.Attributes["ResetPort"].Value.ToString();
                        manager.comm.HostPair1 = node.Attributes["HostPort1"].Value.ToString();
                        manager.comm.HostSWFilePath = node.Attributes["HostAppFilePath"].Value.ToString();
                        manager.comm.HostAppCfgFilePath = node.Attributes["HostAppCfgFilePath"].Value.ToString();
                        manager.comm.HostAppMEMSCfgPath = node.Attributes["HostAppMEMSCfgPath"].Value.ToString();
                        manager.comm.DefaultTCXOFreq = node.Attributes["DefaultTCXOFreq"].Value.ToString();
                        manager.comm.LNAType = Convert.ToInt32(node.Attributes["LNAType"].Value.ToString());
                        manager.comm.ReadBuffer = Convert.ToInt32(node.Attributes["ReadBuffer"].Value.ToString());
                        manager.comm.LDOMode = Convert.ToInt32(node.Attributes["LDOMode"].Value.ToString());
                        manager.comm.RxName = node.Attributes["RxName"].Value.ToString();
                        manager.comm.IsVersion4_1_A8AndAbove = node.Attributes["IsVersionGreater4_1_A8"].Value.ToString() == "1";
                        manager.comm.EESelect = node.Attributes["EESelect"].Value.ToString();
                        manager.comm.ServerName = node.Attributes["ServerName"].Value.ToString();
                        manager.comm.ServerPort = node.Attributes["ServerPort"].Value.ToString();
                        manager.comm.AuthenticationCode = node.Attributes["AuthenticationCode"].Value.ToString();
                        manager.comm.EEDayNum = node.Attributes["EEDayNum"].Value.ToString();
                        manager.comm.BankTime = node.Attributes["BankTime"].Value.ToString();
                        manager.comm.ProductFamily = (CommonClass.ProductType) Convert.ToInt32(node.Attributes["ProdFamily"].Value.ToString());
                        if (node.Attributes["RequiredHostRun"].Value.ToString() == "1")
                        {
                            manager.comm.RequireHostRun = true;
                            if (node.Attributes["RequireEE"].Value.ToString() == "True")
                            {
                                manager.comm.RequireEE = true;
                            }
                            else
                            {
                                manager.comm.RequireEE = false;
                            }
                        }
                        else
                        {
                            manager.comm.RequireHostRun = false;
                        }
                        string str3 = node.Attributes["InputDeviceMode"].Value.ToString();
                        if (str3 == null)
                        {
                            goto Label_0661;
                        }
                        if (!(str3 == "1"))
                        {
                            if (str3 == "2")
                            {
                                goto Label_0637;
                            }
                            if (str3 == "3")
                            {
                                goto Label_0645;
                            }
                            if (str3 == "4")
                            {
                                goto Label_0653;
                            }
                            goto Label_0661;
                        }
                        manager.comm.InputDeviceMode = CommonClass.InputDeviceModes.RS232;
                        goto Label_066D;
                    Label_0637:
                        manager.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Client;
                        goto Label_066D;
                    Label_0645:
                        manager.comm.InputDeviceMode = CommonClass.InputDeviceModes.TCP_Server;
                        goto Label_066D;
                    Label_0653:
                        manager.comm.InputDeviceMode = CommonClass.InputDeviceModes.I2C;
                        goto Label_066D;
                    Label_0661:
                        manager.comm.InputDeviceMode = CommonClass.InputDeviceModes.FilePlayBack;
                    Label_066D:
                        if ((str4 = node.Attributes["rxType"].Value.ToString()) != null)
                        {
                            if (!(str4 == "SLC"))
                            {
                                if (str4 == "GSW")
                                {
                                    goto Label_06D5;
                                }
                                if (str4 == "TTB")
                                {
                                    goto Label_06E3;
                                }
                                if (str4 == "NMEA")
                                {
                                    goto Label_06F1;
                                }
                            }
                            else
                            {
                                manager.comm.RxType = CommunicationManager.ReceiverType.SLC;
                            }
                        }
                        goto Label_06FD;
                    Label_06D5:
                        manager.comm.RxType = CommunicationManager.ReceiverType.GSW;
                        goto Label_06FD;
                    Label_06E3:
                        manager.comm.RxType = CommunicationManager.ReceiverType.TTB;
                        goto Label_06FD;
                    Label_06F1:
                        manager.comm.RxType = CommunicationManager.ReceiverType.NMEA;
                    Label_06FD:
                        if ((str5 = node.Attributes["viewType"].Value.ToString()) != null)
                        {
                            if (!(str5 == "GPS"))
                            {
                                if (str5 == "GP2")
                                {
                                    goto Label_0776;
                                }
                                if (str5 == "HEX")
                                {
                                    goto Label_0784;
                                }
                                if (str5 == "SSB")
                                {
                                    goto Label_0792;
                                }
                                if (str5 == "TEXT")
                                {
                                    goto Label_07A0;
                                }
                            }
                            else
                            {
                                manager.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GPS;
                            }
                        }
                        goto Label_07AC;
                    Label_0776:
                        manager.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GP2;
                        goto Label_07AC;
                    Label_0784:
                        manager.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
                        goto Label_07AC;
                    Label_0792:
                        manager.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.SSB;
                        goto Label_07AC;
                    Label_07A0:
                        manager.comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
                    Label_07AC:
                        class2.DisplayBuffer = Convert.ToInt32(node.Attributes["bufferSize"].Value.ToString());
                        manager.comm.AutoReplyCtrl.ControlChannelVersion = node.Attributes["controlVersion"].Value.ToString();
                        manager.comm.AutoReplyCtrl.AidingProtocolVersion = node.Attributes["aidingVersion"].Value.ToString();
                        foreach (XmlNode node2 in node)
                        {
                            switch (node2.Attributes["name"].Value.ToString())
                            {
                                case "frmCommInputMessage":
                                    manager.InputCommandLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.InputCommandLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.InputCommandLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.InputCommandLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    manager.InputCommandLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.InputCommandLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.InputCommandLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.InputCommandLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommRadarMap":
                                    manager.SVsMapLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.SVsMapLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.SVsMapLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.SVsMapLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmSatelliteStats":
                                    manager.SatelliteStatsLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.SatelliteStatsLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.SatelliteStatsLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.SatelliteStatsLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommLocationMap":
                                    manager.LocationMapLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.LocationMapLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.LocationMapLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.LocationMapLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommSignalView":
                                    manager.SignalViewLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.SignalViewLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.SignalViewLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.SignalViewLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommMessageFilter":
                                    manager.MessageViewLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.MessageViewLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.MessageViewLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.MessageViewLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmTTFFDisplay":
                                    manager.TTFFDisplayLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.TTFFDisplayLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.TTFFDisplayLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.TTFFDisplayLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmSiRFAware":
                                    manager.SiRFawareLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.SiRFawareLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.SiRFawareLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.SiRFawareLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommDebugView":
                                    manager.DebugViewLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.DebugViewLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.DebugViewLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.DebugViewLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommResponseView":
                                    manager.ResponseViewLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.ResponseViewLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.ResponseViewLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.ResponseViewLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommErrorView":
                                    manager.ErrorViewLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.ErrorViewLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.ErrorViewLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.ErrorViewLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmInterenceReport":
                                    manager.InterferenceLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.InterferenceLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.InterferenceLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.InterferenceLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCompassView":
                                    manager.CompassViewLocation.Left = Convert.ToInt32(node2.Attributes["left"].Value.ToString());
                                    manager.CompassViewLocation.Top = Convert.ToInt32(node2.Attributes["top"].Value.ToString());
                                    manager.CompassViewLocation.Width = Convert.ToInt32(node2.Attributes["width"].Value.ToString());
                                    manager.CompassViewLocation.Height = Convert.ToInt32(node2.Attributes["height"].Value.ToString());
                                    break;
                            }
                        }
                        if (!PortManagerHash.ContainsKey(manager.comm.PortName))
                        {
                            PortManagerHash.Add(manager.comm.PortName, manager);
                        }
                        else
                        {
                            PortManagerHash[manager.comm.PortName] = manager;
                        }
                        updateToolStripPortComboBox(manager.comm.PortName, true);
                        manager.UpdateMainWindow += new PortManager.updateParentEventHandler(updateMainWindowTitle);
                        toolStripNumPortTxtBox.Text = PortManagerHash.Count.ToString();
                        manager.PerPortToolStrip = AddPortToolbar((toolStripMain.Location.Y + (0x19 * PortManagerHash.Count)) + 0x23, manager.comm.PortName);
                        updateGUIOnConnectNDisconnect(manager);
                        goto Label_1E7A;
                    Label_1411:
                        foreach (XmlNode node3 in node)
                        {
                            switch (node3.Attributes["name"].Value.ToString())
                            {
                                case "frmCommInputMessage":
                                    InputCommandLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    InputCommandLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    InputCommandLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    InputCommandLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommRadarMap":
                                    SVsMapLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    SVsMapLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    SVsMapLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    SVsMapLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmSatelliteStats":
                                    SatelliteStatsLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    SatelliteStatsLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    SatelliteStatsLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    SatelliteStatsLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommLocationMap":
                                    LocationMapLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    LocationMapLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    LocationMapLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    LocationMapLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommSignalView":
                                    SignalViewLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    SignalViewLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    SignalViewLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    SignalViewLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommMessageFilter":
                                    MessageViewLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    MessageViewLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    MessageViewLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    MessageViewLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmTTFFDisplay":
                                    TTFFDisplayLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    TTFFDisplayLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    TTFFDisplayLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    TTFFDisplayLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmSiRFAware":
                                    SiRFawareLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    SiRFawareLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    SiRFawareLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    SiRFawareLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommDebugView":
                                    DebugViewLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    DebugViewLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    DebugViewLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    DebugViewLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommResponseView":
                                    ResponseViewLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    ResponseViewLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    ResponseViewLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    ResponseViewLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCommErrorView":
                                    ErrorViewLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    ErrorViewLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    ErrorViewLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    ErrorViewLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmInterenceReport":
                                    InterferenceLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    InterferenceLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    InterferenceLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    InterferenceLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;

                                case "frmCompassView":
                                    CompassViewLocation.Left = Convert.ToInt32(node3.Attributes["left"].Value.ToString());
                                    CompassViewLocation.Top = Convert.ToInt32(node3.Attributes["top"].Value.ToString());
                                    CompassViewLocation.Width = Convert.ToInt32(node3.Attributes["width"].Value.ToString());
                                    CompassViewLocation.Height = Convert.ToInt32(node3.Attributes["height"].Value.ToString());
                                    break;
                            }
                        }
                    Label_1E7A:
                        flag = true;
                    }
                    if ((PortManagerHash.Count > 1) && !toolStripPortComboBox.Items.Contains("All"))
                    {
                        toolStripPortComboBox.Items.Add("All");
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("frmMDIMain() + frmSaveSettingsLoad() " + exception.ToString());
                }
            }
            return flag;
        }

        public bool GetAbort()
        {
            return clsGlobal.Abort;
        }

        private string getai3EphDateString(string yr_mon_day)
        {
            string[] strArray = new string[20];
            strArray = yr_mon_day.Split(new char[] { ' ' });
            string str = string.Empty;
            try
            {
                byte year = Convert.ToByte(strArray[0]);
                byte month = Convert.ToByte(strArray[0]);
                byte day = Convert.ToByte(strArray[0]);
                DateTime time = new DateTime(year, month, day);
                string format = "ddd MMM yyyy";
                str = time.ToString(format);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception in Rinex constructing date string: " + exception.ToString());
                return string.Empty;
            }
            return str;
        }

        public frmCommOpen GetCommWinRef(string portNum)
        {
            foreach (string str in clsGlobal.CommWinRef.Keys)
            {
                if (((str == portNum) || (str == ("COM" + portNum))) || (str == ("TCP" + portNum)))
                {
                    return (frmCommOpen) clsGlobal.CommWinRef[str];
                }
            }
            return null;
        }

        public frmCommOpen GetCommWinRefByIndex(int index)
        {
            int num = 0;
            foreach (string str in clsGlobal.CommWinRef.Keys)
            {
                if (num == index)
                {
                    return (frmCommOpen) clsGlobal.CommWinRef[str];
                }
                num++;
            }
            return null;
        }

        private object GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(frmAutomationTests).ToString())
            {
                _objFrmAutoTest = CreateAutomationTestWindow();
                return _objFrmAutoTest;
            }
            if (persistString == typeof(frmPerformanceMonitor).ToString())
            {
                _objFrmPerfMonitor = CreatefrmPerformanceMonitorWindow();
                return _objFrmPerfMonitor;
            }
            if (persistString == typeof(frmPython).ToString())
            {
                _objFrmPython = CreatePythonWindow();
                return _objFrmPython;
            }
            if (persistString == typeof(frmCommInputMessage).ToString())
            {
                if ((_objFrmCommOpen != null) && !_objFrmCommOpen.IsDisposed)
                {
                    _objFrmCommInputMessage = new frmCommInputMessage();
                    _objFrmCommInputMessage.CommWindow = _objFrmCommOpen.comm;
                    _objFrmCommOpen._inputCommands = _objFrmCommInputMessage;
                }
                return _objFrmCommInputMessage;
            }
            if (persistString == typeof(frmCommLocationMap).ToString())
            {
                if ((_objFrmCommOpen != null) && !_objFrmCommOpen.IsDisposed)
                {
                    _objFrmCommLocationMap = new frmCommLocationMap();
                    _objFrmCommLocationMap.CommWindow = _objFrmCommOpen.comm;
                    _objFrmCommOpen._locationViewPanel = _objFrmCommLocationMap;
                }
                return _objFrmCommLocationMap;
            }
            if (persistString == typeof(frmCommSignalView).ToString())
            {
                if ((_objFrmCommOpen != null) && !_objFrmCommOpen.IsDisposed)
                {
                    _objFrmCommSignalView = new frmCommSignalView();
                    _objFrmCommSignalView.CommWindow = _objFrmCommOpen.comm;
                    _objFrmCommOpen._signalStrengthPanel = _objFrmCommSignalView;
                }
                return _objFrmCommSignalView;
            }
            if (persistString == typeof(frmEncryCtrl).ToString())
            {
                if ((_objFrmCommOpen != null) && !_objFrmCommOpen.IsDisposed)
                {
                    _objFrmEncryCtrl = new frmEncryCtrl(_objFrmCommOpen.comm);
                    _objFrmEncryCtrl.CommWindow = _objFrmCommOpen.comm;
                }
                return _objFrmEncryCtrl;
            }
            if (persistString == typeof(frmRFCaptureCtrl).ToString())
            {
                _objFrmCaptureCtrl = CreateRFReplayCaptureWindow();
                return _objFrmCaptureCtrl;
            }
            if (persistString == typeof(frmRFPlaybackConfig).ToString())
            {
                _objFrmRFPlaybackConfig = CreateRFReplayConfigWindow();
                return _objFrmRFPlaybackConfig;
            }
            if (persistString == typeof(frmRFPlaybackCtrl).ToString())
            {
                _objFrmRFPlaybackCtrl = CreateRFReplayPlaybackWindow();
                return _objFrmRFPlaybackCtrl;
            }
            if (persistString == typeof(frmSimplexCtrl).ToString())
            {
                _objFrmSimplexCtrl = frmSimplexCtrl.GetChildInstance();
                return _objFrmSimplexCtrl;
            }
            if (persistString == typeof(frmSPAzCtrl).ToString())
            {
                _objFrmSPAzCtrl = frmSPAzCtrl.GetChildInstance();
                return _objFrmSPAzCtrl;
            }
            if (persistString == typeof(frmRackCtrl).ToString())
            {
                _objFrmRackCtrl = frmRackCtrl.GetChildInstance();
                return _objFrmRackCtrl;
            }
            if (persistString == typeof(frmE911Report).ToString())
            {
                _objFrmE911Report = frmE911Report.GetChildInstance("E911");
                return _objFrmE911Report;
            }
            if (persistString == typeof(frmNavPerformanceReport).ToString())
            {
                _objFrmNavPerformanceReport = frmNavPerformanceReport.GetChildInstance();
                return _objFrmNavPerformanceReport;
            }
			/*
			 * //!
            if (persistString == typeof(frmGPIBCtrl).ToString())
            {
                _objFrmGPIBCtrl = frmGPIBCtrl.GetChildInstance();
                return _objFrmNavPerformanceReport;
            }
			 */
            if (persistString.Contains("frmCommOpen"))
            {
                _objFrmCommOpen = new frmCommOpen();
                return _objFrmCommOpen;
            }
            return null;
        }

        public PortManager GetFirstAvailablePort()
        {
            foreach (string str in PortManagerHash.Keys)
            {
                if (!(str == clsGlobal.FilePlayBackPortName))
                {
                    return (PortManager) PortManagerHash[str];
                }
            }
            return null;
        }

        public ListenerManager GetListenerMgr()
        {
            return new ListenerManager();
        }

        public bool GetLoopitStatus()
        {
            bool flag = false;
            foreach (string str in PortManagerHash.Keys)
            {
                PortManager manager = (PortManager) PortManagerHash[str];
                if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                {
                    flag |= manager.comm.RxCtrl.ResetCtrl.LoopitInprogress;
                }
            }
            return flag;
        }

        public long GetMem()
        {
            return GC.GetTotalMemory(true);
        }

        public bool GetNavStatus(string comport)
        {
            bool flag = true;
            bool monitorNav = false;
            if (comport == "*")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    monitorNav = manager.comm.MonitorNav;
                    manager.comm.MonitorNav = true;
                    flag &= manager.comm.m_NavData.IsNav;
                    if (manager.comm.m_NavData.IsNav)
                    {
                        manager.comm.MonitorNav = monitorNav;
                    }
                }
                return flag;
            }
            PortManager manager2 = (PortManager) PortManagerHash[comport];
            monitorNav = manager2.comm.MonitorNav;
            manager2.comm.MonitorNav = true;
            flag &= manager2.comm.m_NavData.IsNav;
            if (manager2.comm.m_NavData.IsNav)
            {
                manager2.comm.MonitorNav = monitorNav;
            }
            return flag;
        }

        public PortManager GetPortRefByIndex(int index)
        {
            int num = 0;
            foreach (string str in PortManagerHash.Keys)
            {
                if (num == index)
                {
                    return (PortManager) PortManagerHash[str];
                }
                num++;
            }
            return null;
        }

        public PortManager GetPortRefByPortName(string portName)
        {
			foreach (string current in PortManagerHash.Keys)
			{
				if (PortManagerHash.ContainsKey(portName))
				{
					return (PortManager)PortManagerHash[portName];
				}
			}
            return null;
        }

        public PortManager GetPortRefByPortNum(string portNum)
        {
            string key = "COM" + portNum;
            if (PortManagerHash.ContainsKey(key))
            {
                return (PortManager) PortManagerHash[key];
            }
            key = "TCP" + portNum;
            if (PortManagerHash.ContainsKey(key))
            {
                return (PortManager) PortManagerHash[key];
            }
            key = "I2C" + portNum;
            if (PortManagerHash.ContainsKey(key))
            {
                return (PortManager) PortManagerHash[key];
            }
            key = "PBK" + portNum;
            if (PortManagerHash.ContainsKey(key))
            {
                return (PortManager) PortManagerHash[key];
            }
            return null;
        }

        public frmPython GetPythonWindowRef()
        {
            return frmPython.GetChildInstance();
        }

        private void gP2GPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFileConversionWindow(ConversionType.GP2ToGPS);
        }

        private void gPSNMEAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFileConversionWindow(ConversionType.GPSToNMEA);
        }

        private void gPSToKMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFileConversionWindow(ConversionType.GPSToKML);
        }

        private void gyroFactoryCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateGyroFacCalWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateGyroFacCalWin(ref target);
                    }
                }
            }
        }

        private void icConfigureClickHandler()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if ((flag && (manager != null)) && manager.comm.IsSourceDeviceOpen())
                {
                    string str2 = "IC Configuration: All";
                    frmTrackerICConfig_Ver2 ver = new frmTrackerICConfig_Ver2(manager.comm);
                    ver.Text = str2;
                    ver.ShowDialog();
                }
            }
        }

        private void icConfigureClickHandler(ref CommunicationManager comm)
        {
            if (comm != null)
            {
                if (!base.IsDisposed)
                {
                    string str = comm.sourceDeviceName + ": IC Configuration";
                    frmTrackerICConfig_Ver2 ver = new frmTrackerICConfig_Ver2(comm);
                    ver.Text = str;
                    ver.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void iCConfigureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    icConfigureClickHandler();
                    localUpdateStatusString("All");
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        icConfigureClickHandler(ref manager.comm);
                        localUpdateStatusString(manager.comm.PortName);
                    }
                }
            }
        }

        private void iCPeekPokeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    createPeekPokeWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        createPeekPokeWin(target);
                    }
                }
            }
        }

		#region InitializeComponent
		private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMDIMain));
			this.toolStripPortComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMain = new System.Windows.Forms.ToolStrip();
			this.toolStripHelpBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripNumPortTxtBox = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripUpDownArrowBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripPortConfigBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripPortOpenBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripPauseBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripUserTextBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSaveBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripResetBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSignalViewBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripRadarViewBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripMapViewBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripCompassViewBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripTTFFViewBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripResponseViewBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripDebugViewBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripErrorViewBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripMessageViewBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripOpenFileBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripBackBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripPlayBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripPause = new System.Windows.Forms.ToolStripButton();
			this.toolStripStopBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripNextBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripCloseFileBtn = new System.Windows.Forms.ToolStripButton();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.logFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stopLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.convertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gP2GPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.binGPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gPSNMEAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gPSToKMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NMEAtoGPStoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rinexToEphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExtracttoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.analysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem_Plot = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.fileSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fileSaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.filePrintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.filePrintPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
			this.fileOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fileCloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.fileExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.receiverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addReceiverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeReceiverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.receiverConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.receiverDisconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.signalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.radarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tTFFAndNavAccuracyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.responseViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.debugViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.errorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.messageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
			this.mEMSViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.altitudeMeterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.receiverViewCWDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.satellitesStatisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.receiverViewSiRFawareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
			this.siRFDRiveStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.siRFDRiveSensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.commandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
			this.pollSoftwareVesrionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pollNavParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pollAlmanacToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pollEphemerisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
			this.switchOperationModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.switchPowerModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.switchProtocolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
			this.setAlmanacToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setEphemerisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setEEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setDebugLevelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setDGPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setMEMSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.enableMEMSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.disableMEMSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setABPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.enableABPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.disableABPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.lowPowerCommandsBufferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this.iCConfigureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.iCPeekPokeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.inputCommandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.predefinedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.userDefinedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.navigationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.staticNavToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.set5HzNavToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.enable5HzNavToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.disable5HzNavToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
			this.dOPMaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.elevationMaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modeMaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.powerMaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
			this.sBASRangingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.enableSBASRangingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.disableSBASRangingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.plotsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.navAccuracyVsTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.averageCNoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sVTrackedVsTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sVTrajectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setReferenceLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.configureDebugErrorLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoTestLoopitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoTestStandardTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoTest3GPPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoTestTIA916ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoTestAdvancedTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoTestStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoTestAbortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.featuresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.featuresCWDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.powerModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MEMSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.featuresSiRFawareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tTFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aidingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aidingConfigureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aidingSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			this.aidingTTBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TTBConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TTBConfigureTimeAidingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TTBViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.aidingsDownloadServerAssistedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.instrumentControlMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rFReplayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rfReplayConfigurationMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.rfPlaybackCaptureMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.rfReplayPlaybackMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.rfReplaySynthesizerMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.simplexMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.sPAzMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.signalGeneratorMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.testRackMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.reportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reportE911Menu = new System.Windows.Forms.ToolStripMenuItem();
			this.report3GPPMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.reportTIA916Menu = new System.Windows.Forms.ToolStripMenuItem();
			this.reportPerformanceMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.reportResetMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.reportPseudoRangeMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.reportlSMResetMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.pointToPointAnalysisReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mPMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sDOGenerationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.siRFDRiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.navigationModeControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gyroFactoryCalibrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dRSensorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setPollToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.windowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cascadeMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.tileVerticalMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.tileHorizontalMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.restoreLayoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.defaultLayoutMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.previousSettingsLayoutMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.userSettingsLayoutMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.saveLayoutMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.closeAllMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.developerDocMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.userManualMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripLogStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.filePlayBackTrackBar = new System.Windows.Forms.TrackBar();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.logManagerStatusLabel = new System.Windows.Forms.Label();
			this.toolStripMain.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.filePlayBackTrackBar)).BeginInit();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripPortComboBox
			// 
			this.toolStripPortComboBox.Font = new System.Drawing.Font("Tahoma", 9.75F);
			this.toolStripPortComboBox.Name = "toolStripPortComboBox";
			this.toolStripPortComboBox.Size = new System.Drawing.Size(121, 29);
			this.toolStripPortComboBox.DropDown += new System.EventHandler(this.toolStripPortComboBox_DropDown);
			this.toolStripPortComboBox.SelectedIndexChanged += new System.EventHandler(this.toolStripPortComboBox_SelectedIndexChanged);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 29);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 29);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(6, 29);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(6, 29);
			// 
			// toolStripMain
			// 
			this.toolStripMain.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripHelpBtn,
            this.toolStripNumPortTxtBox,
            this.toolStripUpDownArrowBtn,
            this.toolStripPortComboBox,
            this.toolStripSeparator6,
            this.toolStripPortConfigBtn,
            this.toolStripPortOpenBtn,
            this.toolStripPauseBtn,
            this.toolStripUserTextBtn,
            this.toolStripSeparator7,
            this.toolStripSaveBtn,
            this.toolStripSeparator8,
            this.toolStripResetBtn,
            this.toolStripSeparator9,
            this.toolStripSignalViewBtn,
            this.toolStripRadarViewBtn,
            this.toolStripMapViewBtn,
            this.toolStripCompassViewBtn,
            this.toolStripTTFFViewBtn,
            this.toolStripResponseViewBtn,
            this.toolStripDebugViewBtn,
            this.toolStripErrorViewBtn,
            this.toolStripMessageViewBtn,
            this.toolStripSeparator10,
            this.toolStripSeparator5,
            this.toolStripOpenFileBtn,
            this.toolStripBackBtn,
            this.toolStripPlayBtn,
            this.toolStripPause,
            this.toolStripStopBtn,
            this.toolStripNextBtn,
            this.toolStripCloseFileBtn});
			this.toolStripMain.Location = new System.Drawing.Point(3, 0);
			this.toolStripMain.Name = "toolStripMain";
			this.toolStripMain.Size = new System.Drawing.Size(747, 29);
			this.toolStripMain.TabIndex = 5;
			this.toolStripMain.Text = "Main Tool Bar";
			// 
			// toolStripHelpBtn
			// 
			this.toolStripHelpBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripHelpBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripHelpBtn.Image")));
			this.toolStripHelpBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripHelpBtn.Name = "toolStripHelpBtn";
			this.toolStripHelpBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripHelpBtn.Text = "Help";
			this.toolStripHelpBtn.Click += new System.EventHandler(this.toolStripHelpBtn_Click);
			// 
			// toolStripNumPortTxtBox
			// 
			this.toolStripNumPortTxtBox.Enabled = false;
			this.toolStripNumPortTxtBox.Name = "toolStripNumPortTxtBox";
			this.toolStripNumPortTxtBox.ReadOnly = true;
			this.toolStripNumPortTxtBox.Size = new System.Drawing.Size(22, 29);
			// 
			// toolStripUpDownArrowBtn
			// 
			this.toolStripUpDownArrowBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripUpDownArrowBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripUpDownArrowBtn.Image")));
			this.toolStripUpDownArrowBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripUpDownArrowBtn.Name = "toolStripUpDownArrowBtn";
			this.toolStripUpDownArrowBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripUpDownArrowBtn.Text = "Display All";
			this.toolStripUpDownArrowBtn.Click += new System.EventHandler(this.toolStripUpDownArrowBtn_Click);
			// 
			// toolStripPortConfigBtn
			// 
			this.toolStripPortConfigBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripPortConfigBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripPortConfigBtn.Image")));
			this.toolStripPortConfigBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripPortConfigBtn.Name = "toolStripPortConfigBtn";
			this.toolStripPortConfigBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripPortConfigBtn.Text = "Receiver Settings";
			this.toolStripPortConfigBtn.Click += new System.EventHandler(this.toolStripPortConfigBtn_Click);
			// 
			// toolStripPortOpenBtn
			// 
			this.toolStripPortOpenBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripPortOpenBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripPortOpenBtn.Image")));
			this.toolStripPortOpenBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripPortOpenBtn.Name = "toolStripPortOpenBtn";
			this.toolStripPortOpenBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripPortOpenBtn.Text = "Connect";
			this.toolStripPortOpenBtn.Click += new System.EventHandler(this.toolStripPortOpenBtn_Click);
			// 
			// toolStripPauseBtn
			// 
			this.toolStripPauseBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripPauseBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripPauseBtn.Image")));
			this.toolStripPauseBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripPauseBtn.Name = "toolStripPauseBtn";
			this.toolStripPauseBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripPauseBtn.Text = "Pause";
			this.toolStripPauseBtn.Click += new System.EventHandler(this.toolStripPauseBtn_Click);
			// 
			// toolStripUserTextBtn
			// 
			this.toolStripUserTextBtn.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.toolStripUserTextBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripUserTextBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripUserTextBtn.Image")));
			this.toolStripUserTextBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripUserTextBtn.Name = "toolStripUserTextBtn";
			this.toolStripUserTextBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripUserTextBtn.Text = "User Text";
			this.toolStripUserTextBtn.Click += new System.EventHandler(this.toolStripUserText_Click);
			// 
			// toolStripSaveBtn
			// 
			this.toolStripSaveBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSaveBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSaveBtn.Image")));
			this.toolStripSaveBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSaveBtn.Name = "toolStripSaveBtn";
			this.toolStripSaveBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripSaveBtn.Text = "Log File";
			this.toolStripSaveBtn.Click += new System.EventHandler(this.toolStripSaveBtn_Click);
			// 
			// toolStripResetBtn
			// 
			this.toolStripResetBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripResetBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripResetBtn.Image")));
			this.toolStripResetBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripResetBtn.Name = "toolStripResetBtn";
			this.toolStripResetBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripResetBtn.Text = "Reset";
			this.toolStripResetBtn.Click += new System.EventHandler(this.toolStripResetBtn_Click);
			// 
			// toolStripSignalViewBtn
			// 
			this.toolStripSignalViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSignalViewBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSignalViewBtn.Image")));
			this.toolStripSignalViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSignalViewBtn.Name = "toolStripSignalViewBtn";
			this.toolStripSignalViewBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripSignalViewBtn.Text = "Signal View";
			this.toolStripSignalViewBtn.Click += new System.EventHandler(this.toolStripSignalViewBtn_Click);
			// 
			// toolStripRadarViewBtn
			// 
			this.toolStripRadarViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripRadarViewBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripRadarViewBtn.Image")));
			this.toolStripRadarViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripRadarViewBtn.Name = "toolStripRadarViewBtn";
			this.toolStripRadarViewBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripRadarViewBtn.Text = "Radar View";
			this.toolStripRadarViewBtn.Click += new System.EventHandler(this.toolStripRadarViewBtn_Click);
			// 
			// toolStripMapViewBtn
			// 
			this.toolStripMapViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripMapViewBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMapViewBtn.Image")));
			this.toolStripMapViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripMapViewBtn.Name = "toolStripMapViewBtn";
			this.toolStripMapViewBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripMapViewBtn.Text = "Location View";
			this.toolStripMapViewBtn.Click += new System.EventHandler(this.toolStripMapViewBtn_Click);
			// 
			// toolStripCompassViewBtn
			// 
			this.toolStripCompassViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripCompassViewBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripCompassViewBtn.Image")));
			this.toolStripCompassViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripCompassViewBtn.Name = "toolStripCompassViewBtn";
			this.toolStripCompassViewBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripCompassViewBtn.Text = "Compass View";
			this.toolStripCompassViewBtn.Click += new System.EventHandler(this.toolStripCompassViewBtn_Click);
			// 
			// toolStripTTFFViewBtn
			// 
			this.toolStripTTFFViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripTTFFViewBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripTTFFViewBtn.Image")));
			this.toolStripTTFFViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripTTFFViewBtn.Name = "toolStripTTFFViewBtn";
			this.toolStripTTFFViewBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripTTFFViewBtn.Text = "TTFF View";
			this.toolStripTTFFViewBtn.Click += new System.EventHandler(this.toolStripTTFFViewBtn_Click);
			// 
			// toolStripResponseViewBtn
			// 
			this.toolStripResponseViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripResponseViewBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripResponseViewBtn.Image")));
			this.toolStripResponseViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripResponseViewBtn.Name = "toolStripResponseViewBtn";
			this.toolStripResponseViewBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripResponseViewBtn.Text = "Response View";
			this.toolStripResponseViewBtn.Click += new System.EventHandler(this.toolStripResponseViewBtn_Click);
			// 
			// toolStripDebugViewBtn
			// 
			this.toolStripDebugViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDebugViewBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDebugViewBtn.Image")));
			this.toolStripDebugViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDebugViewBtn.Name = "toolStripDebugViewBtn";
			this.toolStripDebugViewBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripDebugViewBtn.Text = "Debug View";
			this.toolStripDebugViewBtn.Click += new System.EventHandler(this.toolStripDebugViewBtn_Click);
			// 
			// toolStripErrorViewBtn
			// 
			this.toolStripErrorViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripErrorViewBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripErrorViewBtn.Image")));
			this.toolStripErrorViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripErrorViewBtn.Name = "toolStripErrorViewBtn";
			this.toolStripErrorViewBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripErrorViewBtn.Text = "Error View";
			this.toolStripErrorViewBtn.Click += new System.EventHandler(this.toolStripErrorViewBtn_Click);
			// 
			// toolStripMessageViewBtn
			// 
			this.toolStripMessageViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripMessageViewBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMessageViewBtn.Image")));
			this.toolStripMessageViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripMessageViewBtn.Name = "toolStripMessageViewBtn";
			this.toolStripMessageViewBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripMessageViewBtn.Text = "Message View";
			this.toolStripMessageViewBtn.Click += new System.EventHandler(this.toolStripMessageViewBtn_Click);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(6, 29);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 29);
			// 
			// toolStripOpenFileBtn
			// 
			this.toolStripOpenFileBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripOpenFileBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOpenFileBtn.Image")));
			this.toolStripOpenFileBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripOpenFileBtn.Name = "toolStripOpenFileBtn";
			this.toolStripOpenFileBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripOpenFileBtn.Text = "Open File";
			this.toolStripOpenFileBtn.Click += new System.EventHandler(this.toolStripOpenFileBtn_Click);
			// 
			// toolStripBackBtn
			// 
			this.toolStripBackBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripBackBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBackBtn.Image")));
			this.toolStripBackBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripBackBtn.Name = "toolStripBackBtn";
			this.toolStripBackBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripBackBtn.Text = "Previous Epoch";
			this.toolStripBackBtn.Click += new System.EventHandler(this.toolStripBackBtn_Click);
			// 
			// toolStripPlayBtn
			// 
			this.toolStripPlayBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripPlayBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripPlayBtn.Image")));
			this.toolStripPlayBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripPlayBtn.Name = "toolStripPlayBtn";
			this.toolStripPlayBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripPlayBtn.Text = "Play File";
			this.toolStripPlayBtn.Click += new System.EventHandler(this.toolStripPlayBtn_Click);
			// 
			// toolStripPause
			// 
			this.toolStripPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripPause.Image = ((System.Drawing.Image)(resources.GetObject("toolStripPause.Image")));
			this.toolStripPause.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripPause.Name = "toolStripPause";
			this.toolStripPause.Size = new System.Drawing.Size(23, 26);
			this.toolStripPause.Text = "Pause";
			this.toolStripPause.Click += new System.EventHandler(this.toolStripPause_Click);
			// 
			// toolStripStopBtn
			// 
			this.toolStripStopBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripStopBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStopBtn.Image")));
			this.toolStripStopBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripStopBtn.Name = "toolStripStopBtn";
			this.toolStripStopBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripStopBtn.Text = "Stop";
			this.toolStripStopBtn.Click += new System.EventHandler(this.toolStripStopBtn_Click);
			// 
			// toolStripNextBtn
			// 
			this.toolStripNextBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripNextBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripNextBtn.Image")));
			this.toolStripNextBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripNextBtn.Name = "toolStripNextBtn";
			this.toolStripNextBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripNextBtn.Text = "Next Epoch";
			this.toolStripNextBtn.Click += new System.EventHandler(this.toolStripNextBtn_Click);
			// 
			// toolStripCloseFileBtn
			// 
			this.toolStripCloseFileBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripCloseFileBtn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripCloseFileBtn.Image")));
			this.toolStripCloseFileBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripCloseFileBtn.Name = "toolStripCloseFileBtn";
			this.toolStripCloseFileBtn.Size = new System.Drawing.Size(23, 26);
			this.toolStripCloseFileBtn.Text = "Close File";
			this.toolStripCloseFileBtn.Click += new System.EventHandler(this.toolStripCloseFileBtn_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackColor = System.Drawing.SystemColors.Menu;
			this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.receiverToolStripMenuItem,
            this.featuresToolStripMenuItem,
            this.aidingToolStripMenuItem,
            this.instrumentControlMenuItem,
            this.reportMenuItem,
            this.siRFDRiveToolStripMenuItem,
            this.windowMenuItem,
            this.helpMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.MdiWindowListItem = this.windowMenuItem;
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
			this.menuStrip1.Size = new System.Drawing.Size(1128, 28);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logFileToolStripMenuItem,
            this.convertToolStripMenuItem,
            this.ExtracttoolStripMenuItem,
            this.analysisToolStripMenuItem,
            this.toolStripMenuItem_Plot,
            this.toolStripSeparator,
            this.fileSaveToolStripMenuItem,
            this.fileSaveAsToolStripMenuItem,
            this.toolStripSeparator3,
            this.filePrintToolStripMenuItem,
            this.filePrintPreviewToolStripMenuItem,
            this.toolStripSeparator16,
            this.fileOpenToolStripMenuItem,
            this.fileCloseToolStripMenuItem,
            this.toolStripSeparator4,
            this.fileExitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// logFileToolStripMenuItem
			// 
			this.logFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startLogToolStripMenuItem,
            this.stopLogToolStripMenuItem});
			this.logFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("logFileToolStripMenuItem.Image")));
			this.logFileToolStripMenuItem.Name = "logFileToolStripMenuItem";
			this.logFileToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
			this.logFileToolStripMenuItem.Text = "&Log File";
			this.logFileToolStripMenuItem.DropDownOpened += new System.EventHandler(this.logFileToolStripMenuItem_DropDownOpened);
			// 
			// startLogToolStripMenuItem
			// 
			this.startLogToolStripMenuItem.Name = "startLogToolStripMenuItem";
			this.startLogToolStripMenuItem.Size = new System.Drawing.Size(118, 24);
			this.startLogToolStripMenuItem.Text = "&Start...";
			this.startLogToolStripMenuItem.Click += new System.EventHandler(this.logFileToolStripMenuItem_Click);
			// 
			// stopLogToolStripMenuItem
			// 
			this.stopLogToolStripMenuItem.Name = "stopLogToolStripMenuItem";
			this.stopLogToolStripMenuItem.Size = new System.Drawing.Size(118, 24);
			this.stopLogToolStripMenuItem.Text = "S&top";
			this.stopLogToolStripMenuItem.Click += new System.EventHandler(this.logFileToolStripMenuItem_Click);
			// 
			// convertToolStripMenuItem
			// 
			this.convertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gP2GPSToolStripMenuItem,
            this.binGPSToolStripMenuItem,
            this.gPSNMEAToolStripMenuItem,
            this.gPSToKMLToolStripMenuItem,
            this.NMEAtoGPStoolStripMenuItem,
            this.rinexToEphToolStripMenuItem});
			this.convertToolStripMenuItem.Name = "convertToolStripMenuItem";
			this.convertToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
			this.convertToolStripMenuItem.Text = "&Convert";
			// 
			// gP2GPSToolStripMenuItem
			// 
			this.gP2GPSToolStripMenuItem.Name = "gP2GPSToolStripMenuItem";
			this.gP2GPSToolStripMenuItem.Size = new System.Drawing.Size(195, 24);
			this.gP2GPSToolStripMenuItem.Text = "&GP2 To GPS...";
			this.gP2GPSToolStripMenuItem.Click += new System.EventHandler(this.gP2GPSToolStripMenuItem_Click);
			// 
			// binGPSToolStripMenuItem
			// 
			this.binGPSToolStripMenuItem.Name = "binGPSToolStripMenuItem";
			this.binGPSToolStripMenuItem.Size = new System.Drawing.Size(195, 24);
			this.binGPSToolStripMenuItem.Text = "&Bin To GP2/GPS...";
			this.binGPSToolStripMenuItem.Click += new System.EventHandler(this.binGPSToolStripMenuItem_Click);
			// 
			// gPSNMEAToolStripMenuItem
			// 
			this.gPSNMEAToolStripMenuItem.Name = "gPSNMEAToolStripMenuItem";
			this.gPSNMEAToolStripMenuItem.Size = new System.Drawing.Size(195, 24);
			this.gPSNMEAToolStripMenuItem.Text = "GPS To &NMEA...";
			this.gPSNMEAToolStripMenuItem.Click += new System.EventHandler(this.gPSNMEAToolStripMenuItem_Click);
			// 
			// gPSToKMLToolStripMenuItem
			// 
			this.gPSToKMLToolStripMenuItem.Name = "gPSToKMLToolStripMenuItem";
			this.gPSToKMLToolStripMenuItem.Size = new System.Drawing.Size(195, 24);
			this.gPSToKMLToolStripMenuItem.Text = "GP&S To KML...";
			this.gPSToKMLToolStripMenuItem.Click += new System.EventHandler(this.gPSToKMLToolStripMenuItem_Click);
			// 
			// NMEAtoGPStoolStripMenuItem
			// 
			this.NMEAtoGPStoolStripMenuItem.Name = "NMEAtoGPStoolStripMenuItem";
			this.NMEAtoGPStoolStripMenuItem.Size = new System.Drawing.Size(195, 24);
			this.NMEAtoGPStoolStripMenuItem.Text = "N&MEA To GPS...";
			this.NMEAtoGPStoolStripMenuItem.Click += new System.EventHandler(this.NMEAtoGPStoolStripMenuItem_Click_1);
			// 
			// rinexToEphToolStripMenuItem
			// 
			this.rinexToEphToolStripMenuItem.Name = "rinexToEphToolStripMenuItem";
			this.rinexToEphToolStripMenuItem.Size = new System.Drawing.Size(195, 24);
			this.rinexToEphToolStripMenuItem.Text = "&RINEX to ai3eph...";
			this.rinexToEphToolStripMenuItem.Click += new System.EventHandler(this.rinexToEphToolStripMenuItem_Click);
			// 
			// ExtracttoolStripMenuItem
			// 
			this.ExtracttoolStripMenuItem.Name = "ExtracttoolStripMenuItem";
			this.ExtracttoolStripMenuItem.Size = new System.Drawing.Size(216, 24);
			this.ExtracttoolStripMenuItem.Text = "&Extract/Find...";
			this.ExtracttoolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
			// 
			// analysisToolStripMenuItem
			// 
			this.analysisToolStripMenuItem.Name = "analysisToolStripMenuItem";
			this.analysisToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
			this.analysisToolStripMenuItem.Text = "Analysis...";
			this.analysisToolStripMenuItem.Click += new System.EventHandler(this.fileAnalysisMenu_Click);
			// 
			// toolStripMenuItem_Plot
			// 
			this.toolStripMenuItem_Plot.Name = "toolStripMenuItem_Plot";
			this.toolStripMenuItem_Plot.Size = new System.Drawing.Size(216, 24);
			this.toolStripMenuItem_Plot.Text = "Plot...";
			this.toolStripMenuItem_Plot.Click += new System.EventHandler(this.toolStripMenuItem_Plot_Click);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(213, 6);
			// 
			// fileSaveToolStripMenuItem
			// 
			this.fileSaveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fileSaveToolStripMenuItem.Image")));
			this.fileSaveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.fileSaveToolStripMenuItem.Name = "fileSaveToolStripMenuItem";
			this.fileSaveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.fileSaveToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
			this.fileSaveToolStripMenuItem.Text = "&Save";
			this.fileSaveToolStripMenuItem.Visible = false;
			// 
			// fileSaveAsToolStripMenuItem
			// 
			this.fileSaveAsToolStripMenuItem.Name = "fileSaveAsToolStripMenuItem";
			this.fileSaveAsToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
			this.fileSaveAsToolStripMenuItem.Text = "Save &As";
			this.fileSaveAsToolStripMenuItem.Visible = false;
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(213, 6);
			// 
			// filePrintToolStripMenuItem
			// 
			this.filePrintToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("filePrintToolStripMenuItem.Image")));
			this.filePrintToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.filePrintToolStripMenuItem.Name = "filePrintToolStripMenuItem";
			this.filePrintToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.filePrintToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
			this.filePrintToolStripMenuItem.Text = "&Print";
			this.filePrintToolStripMenuItem.Visible = false;
			// 
			// filePrintPreviewToolStripMenuItem
			// 
			this.filePrintPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("filePrintPreviewToolStripMenuItem.Image")));
			this.filePrintPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.filePrintPreviewToolStripMenuItem.Name = "filePrintPreviewToolStripMenuItem";
			this.filePrintPreviewToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
			this.filePrintPreviewToolStripMenuItem.Text = "Print Pre&view";
			this.filePrintPreviewToolStripMenuItem.Visible = false;
			// 
			// toolStripSeparator16
			// 
			this.toolStripSeparator16.Name = "toolStripSeparator16";
			this.toolStripSeparator16.Size = new System.Drawing.Size(213, 6);
			// 
			// fileOpenToolStripMenuItem
			// 
			this.fileOpenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fileOpenToolStripMenuItem.Image")));
			this.fileOpenToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.fileOpenToolStripMenuItem.Name = "fileOpenToolStripMenuItem";
			this.fileOpenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.fileOpenToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
			this.fileOpenToolStripMenuItem.Text = "Replay &Open";
			this.fileOpenToolStripMenuItem.Click += new System.EventHandler(this.fileOpenToolStripMenuItem_Click);
			// 
			// fileCloseToolStripMenuItem
			// 
			this.fileCloseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fileCloseToolStripMenuItem.Image")));
			this.fileCloseToolStripMenuItem.Name = "fileCloseToolStripMenuItem";
			this.fileCloseToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
			this.fileCloseToolStripMenuItem.Text = "Replay &Close";
			this.fileCloseToolStripMenuItem.Click += new System.EventHandler(this.fileCloseToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(213, 6);
			// 
			// fileExitToolStripMenuItem
			// 
			this.fileExitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fileExitToolStripMenuItem.Image")));
			this.fileExitToolStripMenuItem.Name = "fileExitToolStripMenuItem";
			this.fileExitToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
			this.fileExitToolStripMenuItem.Text = "E&xit";
			this.fileExitToolStripMenuItem.Click += new System.EventHandler(this.exitMenu_Click);
			// 
			// receiverToolStripMenuItem
			// 
			this.receiverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addReceiverToolStripMenuItem,
            this.removeReceiverToolStripMenuItem,
            this.receiverConnectToolStripMenuItem,
            this.receiverDisconnectToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.commandToolStripMenuItem,
            this.navigationToolStripMenuItem,
            this.plotsToolStripMenuItem,
            this.setReferenceLocationToolStripMenuItem,
            this.configureDebugErrorLogToolStripMenuItem,
            this.autoTestToolStripMenuItem});
			this.receiverToolStripMenuItem.Name = "receiverToolStripMenuItem";
			this.receiverToolStripMenuItem.Size = new System.Drawing.Size(77, 24);
			this.receiverToolStripMenuItem.Text = "&Receiver";
			this.receiverToolStripMenuItem.DropDownOpened += new System.EventHandler(this.receiverToolStripMenuItem_DropDownOpened);
			// 
			// addReceiverToolStripMenuItem
			// 
			this.addReceiverToolStripMenuItem.Name = "addReceiverToolStripMenuItem";
			this.addReceiverToolStripMenuItem.Size = new System.Drawing.Size(266, 24);
			this.addReceiverToolStripMenuItem.Text = "&Add Receiver...";
			this.addReceiverToolStripMenuItem.Click += new System.EventHandler(this.addReceiverMenu_Click);
			// 
			// removeReceiverToolStripMenuItem
			// 
			this.removeReceiverToolStripMenuItem.Name = "removeReceiverToolStripMenuItem";
			this.removeReceiverToolStripMenuItem.Size = new System.Drawing.Size(266, 24);
			this.removeReceiverToolStripMenuItem.Text = "Remo&ve Receiver...";
			this.removeReceiverToolStripMenuItem.Click += new System.EventHandler(this.removeReceiverToolStripMenuItem_Click);
			// 
			// receiverConnectToolStripMenuItem
			// 
			this.receiverConnectToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("receiverConnectToolStripMenuItem.Image")));
			this.receiverConnectToolStripMenuItem.Name = "receiverConnectToolStripMenuItem";
			this.receiverConnectToolStripMenuItem.Size = new System.Drawing.Size(266, 24);
			this.receiverConnectToolStripMenuItem.Text = "Connec&t...";
			this.receiverConnectToolStripMenuItem.Click += new System.EventHandler(this.receiverConnectToolStripMenuItem_Click);
			// 
			// receiverDisconnectToolStripMenuItem
			// 
			this.receiverDisconnectToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("receiverDisconnectToolStripMenuItem.Image")));
			this.receiverDisconnectToolStripMenuItem.Name = "receiverDisconnectToolStripMenuItem";
			this.receiverDisconnectToolStripMenuItem.Size = new System.Drawing.Size(266, 24);
			this.receiverDisconnectToolStripMenuItem.Text = "&Disconnect...";
			this.receiverDisconnectToolStripMenuItem.Click += new System.EventHandler(this.receiverDisconnectToolStripMenuItem_Click);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.signalToolStripMenuItem,
            this.radarToolStripMenuItem,
            this.mapToolStripMenuItem,
            this.tTFFAndNavAccuracyToolStripMenuItem,
            this.responseViewToolStripMenuItem,
            this.debugViewToolStripMenuItem,
            this.errorToolStripMenuItem,
            this.messageToolStripMenuItem,
            this.toolStripSeparator18,
            this.mEMSViewToolStripMenuItem,
            this.compassToolStripMenuItem,
            this.altitudeMeterToolStripMenuItem,
            this.receiverViewCWDetectionToolStripMenuItem,
            this.satellitesStatisticsToolStripMenuItem,
            this.receiverViewSiRFawareToolStripMenuItem,
            this.toolStripSeparator17,
            this.siRFDRiveStatusToolStripMenuItem,
            this.siRFDRiveSensorToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(266, 24);
			this.viewToolStripMenuItem.Text = "V&iew";
			this.viewToolStripMenuItem.DropDownOpened += new System.EventHandler(this.viewToolStripMenuItem_DropDownOpened);
			// 
			// signalToolStripMenuItem
			// 
			this.signalToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("signalToolStripMenuItem.Image")));
			this.signalToolStripMenuItem.Name = "signalToolStripMenuItem";
			this.signalToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.signalToolStripMenuItem.Text = "&Signal View";
			this.signalToolStripMenuItem.Click += new System.EventHandler(this.signalToolStripMenuItem_Click);
			// 
			// radarToolStripMenuItem
			// 
			this.radarToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("radarToolStripMenuItem.Image")));
			this.radarToolStripMenuItem.Name = "radarToolStripMenuItem";
			this.radarToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.radarToolStripMenuItem.Text = "&Radar View";
			this.radarToolStripMenuItem.Click += new System.EventHandler(this.radarToolStripMenuItem_Click);
			// 
			// mapToolStripMenuItem
			// 
			this.mapToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mapToolStripMenuItem.Image")));
			this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
			this.mapToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.mapToolStripMenuItem.Text = "Loc&ation View";
			this.mapToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
			// 
			// tTFFAndNavAccuracyToolStripMenuItem
			// 
			this.tTFFAndNavAccuracyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("tTFFAndNavAccuracyToolStripMenuItem.Image")));
			this.tTFFAndNavAccuracyToolStripMenuItem.Name = "tTFFAndNavAccuracyToolStripMenuItem";
			this.tTFFAndNavAccuracyToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.tTFFAndNavAccuracyToolStripMenuItem.Text = "&TTFF and Nav Accuracy View...";
			this.tTFFAndNavAccuracyToolStripMenuItem.Click += new System.EventHandler(this.tTFFAndNavAccuracyToolStripMenuItem_Click);
			// 
			// responseViewToolStripMenuItem
			// 
			this.responseViewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("responseViewToolStripMenuItem.Image")));
			this.responseViewToolStripMenuItem.Name = "responseViewToolStripMenuItem";
			this.responseViewToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.responseViewToolStripMenuItem.Text = "Respo&nse View";
			this.responseViewToolStripMenuItem.Click += new System.EventHandler(this.responseViewToolStripMenuItem_Click);
			// 
			// debugViewToolStripMenuItem
			// 
			this.debugViewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("debugViewToolStripMenuItem.Image")));
			this.debugViewToolStripMenuItem.Name = "debugViewToolStripMenuItem";
			this.debugViewToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.debugViewToolStripMenuItem.Text = "&Debug View";
			this.debugViewToolStripMenuItem.Click += new System.EventHandler(this.debugViewToolStripMenuItem_Click);
			// 
			// errorToolStripMenuItem
			// 
			this.errorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("errorToolStripMenuItem.Image")));
			this.errorToolStripMenuItem.Name = "errorToolStripMenuItem";
			this.errorToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.errorToolStripMenuItem.Text = "Err&or View";
			this.errorToolStripMenuItem.Click += new System.EventHandler(this.errorToolStripMenuItem_Click);
			// 
			// messageToolStripMenuItem
			// 
			this.messageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("messageToolStripMenuItem.Image")));
			this.messageToolStripMenuItem.Name = "messageToolStripMenuItem";
			this.messageToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.messageToolStripMenuItem.Text = "&Message View";
			this.messageToolStripMenuItem.Click += new System.EventHandler(this.messageToolStripMenuItem_Click);
			// 
			// toolStripSeparator18
			// 
			this.toolStripSeparator18.Name = "toolStripSeparator18";
			this.toolStripSeparator18.Size = new System.Drawing.Size(272, 6);
			// 
			// mEMSViewToolStripMenuItem
			// 
			this.mEMSViewToolStripMenuItem.Name = "mEMSViewToolStripMenuItem";
			this.mEMSViewToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.mEMSViewToolStripMenuItem.Text = "M&EMS View";
			this.mEMSViewToolStripMenuItem.Click += new System.EventHandler(this.mEMSViewToolStripMenuItem_Click);
			// 
			// compassToolStripMenuItem
			// 
			this.compassToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("compassToolStripMenuItem.Image")));
			this.compassToolStripMenuItem.Name = "compassToolStripMenuItem";
			this.compassToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.compassToolStripMenuItem.Text = "&Compass View";
			this.compassToolStripMenuItem.Click += new System.EventHandler(this.compassToolStripMenuItem_Click);
			// 
			// altitudeMeterToolStripMenuItem
			// 
			this.altitudeMeterToolStripMenuItem.Name = "altitudeMeterToolStripMenuItem";
			this.altitudeMeterToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.altitudeMeterToolStripMenuItem.Text = "A&ltimeter View";
			// 
			// receiverViewCWDetectionToolStripMenuItem
			// 
			this.receiverViewCWDetectionToolStripMenuItem.Name = "receiverViewCWDetectionToolStripMenuItem";
			this.receiverViewCWDetectionToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.receiverViewCWDetectionToolStripMenuItem.Text = "C&W Detection View...";
			this.receiverViewCWDetectionToolStripMenuItem.Click += new System.EventHandler(this.cWDetectionToolStripMenuItem_Click);
			// 
			// satellitesStatisticsToolStripMenuItem
			// 
			this.satellitesStatisticsToolStripMenuItem.Name = "satellitesStatisticsToolStripMenuItem";
			this.satellitesStatisticsToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.satellitesStatisticsToolStripMenuItem.Text = "Satellites Statistics &View...";
			this.satellitesStatisticsToolStripMenuItem.Click += new System.EventHandler(this.satellitesStatisticsToolStripMenuItem_Click);
			// 
			// receiverViewSiRFawareToolStripMenuItem
			// 
			this.receiverViewSiRFawareToolStripMenuItem.Name = "receiverViewSiRFawareToolStripMenuItem";
			this.receiverViewSiRFawareToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.receiverViewSiRFawareToolStripMenuItem.Text = "SiRF&aware Mode View...";
			this.receiverViewSiRFawareToolStripMenuItem.Click += new System.EventHandler(this.siRFawareToolStripMenuItem_Click);
			// 
			// toolStripSeparator17
			// 
			this.toolStripSeparator17.Name = "toolStripSeparator17";
			this.toolStripSeparator17.Size = new System.Drawing.Size(272, 6);
			// 
			// siRFDRiveStatusToolStripMenuItem
			// 
			this.siRFDRiveStatusToolStripMenuItem.Name = "siRFDRiveStatusToolStripMenuItem";
			this.siRFDRiveStatusToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.siRFDRiveStatusToolStripMenuItem.Text = "SiRFDRive Status";
			this.siRFDRiveStatusToolStripMenuItem.Click += new System.EventHandler(this.siRFDRiveStatusToolStripMenuItem_Click);
			// 
			// siRFDRiveSensorToolStripMenuItem
			// 
			this.siRFDRiveSensorToolStripMenuItem.Name = "siRFDRiveSensorToolStripMenuItem";
			this.siRFDRiveSensorToolStripMenuItem.Size = new System.Drawing.Size(275, 24);
			this.siRFDRiveSensorToolStripMenuItem.Text = "SiRFDRive Sensor";
			this.siRFDRiveSensorToolStripMenuItem.Click += new System.EventHandler(this.siRFDRiveSensorToolStripMenuItem_Click);
			// 
			// commandToolStripMenuItem
			// 
			this.commandToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem,
            this.toolStripSeparator20,
            this.pollSoftwareVesrionToolStripMenuItem,
            this.pollNavParametersToolStripMenuItem,
            this.pollAlmanacToolStripMenuItem,
            this.pollEphemerisToolStripMenuItem,
            this.toolStripSeparator21,
            this.switchOperationModeToolStripMenuItem,
            this.switchPowerModeToolStripMenuItem,
            this.switchProtocolsToolStripMenuItem,
            this.toolStripSeparator22,
            this.setAlmanacToolStripMenuItem,
            this.setEphemerisToolStripMenuItem,
            this.setEEToolStripMenuItem,
            this.setDebugLevelsToolStripMenuItem,
            this.setDGPSToolStripMenuItem,
            this.setMEMSToolStripMenuItem,
            this.setABPToolStripMenuItem,
            this.toolStripSeparator2,
            this.lowPowerCommandsBufferToolStripMenuItem,
            this.toolStripSeparator11,
            this.iCConfigureToolStripMenuItem,
            this.iCPeekPokeToolStripMenuItem,
            this.inputCommandsToolStripMenuItem});
			this.commandToolStripMenuItem.Name = "commandToolStripMenuItem";
			this.commandToolStripMenuItem.Size = new System.Drawing.Size(266, 24);
			this.commandToolStripMenuItem.Text = "&Command";
			this.commandToolStripMenuItem.DropDownOpened += new System.EventHandler(this.commandToolStripMenuItem_DropDownOpened);
			// 
			// resetToolStripMenuItem
			// 
			this.resetToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("resetToolStripMenuItem.Image")));
			this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
			this.resetToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.resetToolStripMenuItem.Text = "&Reset...";
			this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
			// 
			// toolStripSeparator20
			// 
			this.toolStripSeparator20.Name = "toolStripSeparator20";
			this.toolStripSeparator20.Size = new System.Drawing.Size(279, 6);
			// 
			// pollSoftwareVesrionToolStripMenuItem
			// 
			this.pollSoftwareVesrionToolStripMenuItem.Name = "pollSoftwareVesrionToolStripMenuItem";
			this.pollSoftwareVesrionToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.pollSoftwareVesrionToolStripMenuItem.Text = "Poll S/&W Version";
			this.pollSoftwareVesrionToolStripMenuItem.Click += new System.EventHandler(this.pollSoftwareVesrionToolStripMenuItem_Click);
			// 
			// pollNavParametersToolStripMenuItem
			// 
			this.pollNavParametersToolStripMenuItem.Name = "pollNavParametersToolStripMenuItem";
			this.pollNavParametersToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.pollNavParametersToolStripMenuItem.Text = "Poll &Nav Parameters";
			this.pollNavParametersToolStripMenuItem.Click += new System.EventHandler(this.pollNavParametersToolStripMenuItem_Click);
			// 
			// pollAlmanacToolStripMenuItem
			// 
			this.pollAlmanacToolStripMenuItem.Name = "pollAlmanacToolStripMenuItem";
			this.pollAlmanacToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.pollAlmanacToolStripMenuItem.Text = "Poll &Almanac...";
			this.pollAlmanacToolStripMenuItem.Click += new System.EventHandler(this.pollAlmanacToolStripMenuItem_Click);
			// 
			// pollEphemerisToolStripMenuItem
			// 
			this.pollEphemerisToolStripMenuItem.Name = "pollEphemerisToolStripMenuItem";
			this.pollEphemerisToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.pollEphemerisToolStripMenuItem.Text = "Poll &Ephemeris...";
			this.pollEphemerisToolStripMenuItem.Click += new System.EventHandler(this.pollEphemerisToolStripMenuItem_Click);
			// 
			// toolStripSeparator21
			// 
			this.toolStripSeparator21.Name = "toolStripSeparator21";
			this.toolStripSeparator21.Size = new System.Drawing.Size(279, 6);
			// 
			// switchOperationModeToolStripMenuItem
			// 
			this.switchOperationModeToolStripMenuItem.Name = "switchOperationModeToolStripMenuItem";
			this.switchOperationModeToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.switchOperationModeToolStripMenuItem.Text = "Switch &Operating Mode...";
			this.switchOperationModeToolStripMenuItem.Click += new System.EventHandler(this.switchOperationModeToolStripMenuItem_Click);
			// 
			// switchPowerModeToolStripMenuItem
			// 
			this.switchPowerModeToolStripMenuItem.Name = "switchPowerModeToolStripMenuItem";
			this.switchPowerModeToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.switchPowerModeToolStripMenuItem.Text = "Switch &Power Mode...";
			this.switchPowerModeToolStripMenuItem.Click += new System.EventHandler(this.switchPowerModeToolStripMenuItem_Click);
			// 
			// switchProtocolsToolStripMenuItem
			// 
			this.switchProtocolsToolStripMenuItem.Name = "switchProtocolsToolStripMenuItem";
			this.switchProtocolsToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.switchProtocolsToolStripMenuItem.Text = "Switch Pro&tocols...";
			this.switchProtocolsToolStripMenuItem.Click += new System.EventHandler(this.switchProtocolsToolStripMenuItem_Click);
			// 
			// toolStripSeparator22
			// 
			this.toolStripSeparator22.Name = "toolStripSeparator22";
			this.toolStripSeparator22.Size = new System.Drawing.Size(279, 6);
			// 
			// setAlmanacToolStripMenuItem
			// 
			this.setAlmanacToolStripMenuItem.Name = "setAlmanacToolStripMenuItem";
			this.setAlmanacToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.setAlmanacToolStripMenuItem.Text = "&Set Almanac...";
			this.setAlmanacToolStripMenuItem.Click += new System.EventHandler(this.setAlmanacToolStripMenuItem_Click);
			// 
			// setEphemerisToolStripMenuItem
			// 
			this.setEphemerisToolStripMenuItem.Name = "setEphemerisToolStripMenuItem";
			this.setEphemerisToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.setEphemerisToolStripMenuItem.Text = "Set Ep&hemeris...";
			this.setEphemerisToolStripMenuItem.Click += new System.EventHandler(this.setEphemerisToolStripMenuItem_Click);
			// 
			// setEEToolStripMenuItem
			// 
			this.setEEToolStripMenuItem.Name = "setEEToolStripMenuItem";
			this.setEEToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.setEEToolStripMenuItem.Text = "Set EE...";
			this.setEEToolStripMenuItem.Click += new System.EventHandler(this.setEEToolStripMenuItem_Click);
			// 
			// setDebugLevelsToolStripMenuItem
			// 
			this.setDebugLevelsToolStripMenuItem.Name = "setDebugLevelsToolStripMenuItem";
			this.setDebugLevelsToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.setDebugLevelsToolStripMenuItem.Text = "Set &Debug Levels...";
			this.setDebugLevelsToolStripMenuItem.Click += new System.EventHandler(this.setEncryptToolStripMenuItem_Click);
			// 
			// setDGPSToolStripMenuItem
			// 
			this.setDGPSToolStripMenuItem.Name = "setDGPSToolStripMenuItem";
			this.setDGPSToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.setDGPSToolStripMenuItem.Text = "S&et DGPS...";
			this.setDGPSToolStripMenuItem.Click += new System.EventHandler(this.setDGPSToolStripMenuItem_Click);
			// 
			// setMEMSToolStripMenuItem
			// 
			this.setMEMSToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableMEMSToolStripMenuItem,
            this.disableMEMSToolStripMenuItem});
			this.setMEMSToolStripMenuItem.Name = "setMEMSToolStripMenuItem";
			this.setMEMSToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.setMEMSToolStripMenuItem.Text = "Set &MEMS";
			this.setMEMSToolStripMenuItem.MouseHover += new System.EventHandler(this.setMEMSToolStripMenuItem_MouseHover);
			// 
			// enableMEMSToolStripMenuItem
			// 
			this.enableMEMSToolStripMenuItem.Name = "enableMEMSToolStripMenuItem";
			this.enableMEMSToolStripMenuItem.Size = new System.Drawing.Size(174, 24);
			this.enableMEMSToolStripMenuItem.Text = "&Enable MEMS";
			this.enableMEMSToolStripMenuItem.Click += new System.EventHandler(this.enableMEMSToolStripMenuItem_Click);
			// 
			// disableMEMSToolStripMenuItem
			// 
			this.disableMEMSToolStripMenuItem.Name = "disableMEMSToolStripMenuItem";
			this.disableMEMSToolStripMenuItem.Size = new System.Drawing.Size(174, 24);
			this.disableMEMSToolStripMenuItem.Text = "&Disable MEMS";
			this.disableMEMSToolStripMenuItem.Click += new System.EventHandler(this.disableMEMSToolStripMenuItem_Click);
			// 
			// setABPToolStripMenuItem
			// 
			this.setABPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableABPToolStripMenuItem,
            this.disableABPToolStripMenuItem});
			this.setABPToolStripMenuItem.Name = "setABPToolStripMenuItem";
			this.setABPToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.setABPToolStripMenuItem.Text = "Set A&BP";
			this.setABPToolStripMenuItem.DropDownOpened += new System.EventHandler(this.setABPToolStripMenuItem_DropDownOpened);
			// 
			// enableABPToolStripMenuItem
			// 
			this.enableABPToolStripMenuItem.Name = "enableABPToolStripMenuItem";
			this.enableABPToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
			this.enableABPToolStripMenuItem.Text = "&Enable ABP";
			this.enableABPToolStripMenuItem.Click += new System.EventHandler(this.enableABPToolStripMenuItem_Click);
			// 
			// disableABPToolStripMenuItem
			// 
			this.disableABPToolStripMenuItem.Name = "disableABPToolStripMenuItem";
			this.disableABPToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
			this.disableABPToolStripMenuItem.Text = "&Disable ABP";
			this.disableABPToolStripMenuItem.Click += new System.EventHandler(this.disableABPToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(279, 6);
			// 
			// lowPowerCommandsBufferToolStripMenuItem
			// 
			this.lowPowerCommandsBufferToolStripMenuItem.Name = "lowPowerCommandsBufferToolStripMenuItem";
			this.lowPowerCommandsBufferToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.lowPowerCommandsBufferToolStripMenuItem.Text = "&Low Power Commands Buffer...";
			this.lowPowerCommandsBufferToolStripMenuItem.Click += new System.EventHandler(this.lowPowerCommandsBufferToolStripMenuItem_Click);
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			this.toolStripSeparator11.Size = new System.Drawing.Size(279, 6);
			// 
			// iCConfigureToolStripMenuItem
			// 
			this.iCConfigureToolStripMenuItem.Name = "iCConfigureToolStripMenuItem";
			this.iCConfigureToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.iCConfigureToolStripMenuItem.Text = "&IC Configure...";
			this.iCConfigureToolStripMenuItem.Click += new System.EventHandler(this.iCConfigureToolStripMenuItem_Click);
			// 
			// iCPeekPokeToolStripMenuItem
			// 
			this.iCPeekPokeToolStripMenuItem.Name = "iCPeekPokeToolStripMenuItem";
			this.iCPeekPokeToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.iCPeekPokeToolStripMenuItem.Text = "IC Peek/Po&ke...";
			this.iCPeekPokeToolStripMenuItem.Click += new System.EventHandler(this.iCPeekPokeToolStripMenuItem_Click);
			// 
			// inputCommandsToolStripMenuItem
			// 
			this.inputCommandsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.predefinedToolStripMenuItem,
            this.userDefinedToolStripMenuItem});
			this.inputCommandsToolStripMenuItem.Name = "inputCommandsToolStripMenuItem";
			this.inputCommandsToolStripMenuItem.Size = new System.Drawing.Size(282, 24);
			this.inputCommandsToolStripMenuItem.Text = "Input &Commands";
			// 
			// predefinedToolStripMenuItem
			// 
			this.predefinedToolStripMenuItem.Name = "predefinedToolStripMenuItem";
			this.predefinedToolStripMenuItem.Size = new System.Drawing.Size(235, 24);
			this.predefinedToolStripMenuItem.Text = "&Predefined Message...";
			this.predefinedToolStripMenuItem.Click += new System.EventHandler(this.predefinedToolStripMenuItem_Click);
			// 
			// userDefinedToolStripMenuItem
			// 
			this.userDefinedToolStripMenuItem.Name = "userDefinedToolStripMenuItem";
			this.userDefinedToolStripMenuItem.Size = new System.Drawing.Size(235, 24);
			this.userDefinedToolStripMenuItem.Text = "&User Defined Message...";
			this.userDefinedToolStripMenuItem.Click += new System.EventHandler(this.userDefinedToolStripMenuItem_Click);
			// 
			// navigationToolStripMenuItem
			// 
			this.navigationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.staticNavToolStripMenuItem,
            this.set5HzNavToolStripMenuItem,
            this.toolStripSeparator14,
            this.dOPMaskToolStripMenuItem,
            this.elevationMaskToolStripMenuItem,
            this.modeMaskToolStripMenuItem,
            this.powerMaskToolStripMenuItem,
            this.toolStripSeparator19,
            this.sBASRangingToolStripMenuItem});
			this.navigationToolStripMenuItem.Name = "navigationToolStripMenuItem";
			this.navigationToolStripMenuItem.Size = new System.Drawing.Size(266, 24);
			this.navigationToolStripMenuItem.Text = "&Navigation";
			this.navigationToolStripMenuItem.DropDownOpened += new System.EventHandler(this.navigationToolStripMenuItem_DropDownOpened);
			// 
			// staticNavToolStripMenuItem
			// 
			this.staticNavToolStripMenuItem.Name = "staticNavToolStripMenuItem";
			this.staticNavToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
			this.staticNavToolStripMenuItem.Text = "Static &Nav...";
			this.staticNavToolStripMenuItem.Click += new System.EventHandler(this.staticNavToolStripMenuItem_Click);
			// 
			// set5HzNavToolStripMenuItem
			// 
			this.set5HzNavToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enable5HzNavToolStripMenuItem,
            this.disable5HzNavToolStripMenuItem});
			this.set5HzNavToolStripMenuItem.Name = "set5HzNavToolStripMenuItem";
			this.set5HzNavToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
			this.set5HzNavToolStripMenuItem.Text = "Set 5 &Hz Nav";
			this.set5HzNavToolStripMenuItem.DropDownOpened += new System.EventHandler(this.set5HzNavToolStripMenuItem_DropDownOpened);
			// 
			// enable5HzNavToolStripMenuItem
			// 
			this.enable5HzNavToolStripMenuItem.Name = "enable5HzNavToolStripMenuItem";
			this.enable5HzNavToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
			this.enable5HzNavToolStripMenuItem.Text = "&Enable 5Hz Nav";
			this.enable5HzNavToolStripMenuItem.Click += new System.EventHandler(this.enable5HzNavToolStripMenuItem_Click);
			// 
			// disable5HzNavToolStripMenuItem
			// 
			this.disable5HzNavToolStripMenuItem.Name = "disable5HzNavToolStripMenuItem";
			this.disable5HzNavToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
			this.disable5HzNavToolStripMenuItem.Text = "&Disable 5Hz Nav";
			this.disable5HzNavToolStripMenuItem.Click += new System.EventHandler(this.disable5HzNavToolStripMenuItem_Click);
			// 
			// toolStripSeparator14
			// 
			this.toolStripSeparator14.Name = "toolStripSeparator14";
			this.toolStripSeparator14.Size = new System.Drawing.Size(183, 6);
			// 
			// dOPMaskToolStripMenuItem
			// 
			this.dOPMaskToolStripMenuItem.Name = "dOPMaskToolStripMenuItem";
			this.dOPMaskToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
			this.dOPMaskToolStripMenuItem.Text = "&DOP Mask...";
			this.dOPMaskToolStripMenuItem.Click += new System.EventHandler(this.dOPMaskToolStripMenuItem_Click);
			// 
			// elevationMaskToolStripMenuItem
			// 
			this.elevationMaskToolStripMenuItem.Name = "elevationMaskToolStripMenuItem";
			this.elevationMaskToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
			this.elevationMaskToolStripMenuItem.Text = "&Elevation Mask...";
			this.elevationMaskToolStripMenuItem.Click += new System.EventHandler(this.elevationMaskToolStripMenuItem_Click);
			// 
			// modeMaskToolStripMenuItem
			// 
			this.modeMaskToolStripMenuItem.Name = "modeMaskToolStripMenuItem";
			this.modeMaskToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
			this.modeMaskToolStripMenuItem.Text = "&Mode Mask...";
			this.modeMaskToolStripMenuItem.Click += new System.EventHandler(this.modeMaskToolStripMenuItem_Click);
			// 
			// powerMaskToolStripMenuItem
			// 
			this.powerMaskToolStripMenuItem.Name = "powerMaskToolStripMenuItem";
			this.powerMaskToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
			this.powerMaskToolStripMenuItem.Text = "&Power Mask...";
			this.powerMaskToolStripMenuItem.Click += new System.EventHandler(this.powerMaskToolStripMenuItem_Click);
			// 
			// toolStripSeparator19
			// 
			this.toolStripSeparator19.Name = "toolStripSeparator19";
			this.toolStripSeparator19.Size = new System.Drawing.Size(183, 6);
			// 
			// sBASRangingToolStripMenuItem
			// 
			this.sBASRangingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableSBASRangingToolStripMenuItem,
            this.disableSBASRangingToolStripMenuItem});
			this.sBASRangingToolStripMenuItem.Name = "sBASRangingToolStripMenuItem";
			this.sBASRangingToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
			this.sBASRangingToolStripMenuItem.Text = "&SBAS Ranging";
			this.sBASRangingToolStripMenuItem.DropDownOpened += new System.EventHandler(this.sBASRangingToolStripMenuItem_DropDownOpened);
			// 
			// enableSBASRangingToolStripMenuItem
			// 
			this.enableSBASRangingToolStripMenuItem.Name = "enableSBASRangingToolStripMenuItem";
			this.enableSBASRangingToolStripMenuItem.Size = new System.Drawing.Size(226, 24);
			this.enableSBASRangingToolStripMenuItem.Text = "&Enable SBAS Ranging";
			this.enableSBASRangingToolStripMenuItem.Click += new System.EventHandler(this.enableSBASRangingToolStripMenuItem_Click);
			// 
			// disableSBASRangingToolStripMenuItem
			// 
			this.disableSBASRangingToolStripMenuItem.Name = "disableSBASRangingToolStripMenuItem";
			this.disableSBASRangingToolStripMenuItem.Size = new System.Drawing.Size(226, 24);
			this.disableSBASRangingToolStripMenuItem.Text = "&Disable SBAS Ranging";
			this.disableSBASRangingToolStripMenuItem.Click += new System.EventHandler(this.disableSBASRangingToolStripMenuItem_Click);
			// 
			// plotsToolStripMenuItem
			// 
			this.plotsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.navAccuracyVsTimeToolStripMenuItem,
            this.averageCNoToolStripMenuItem,
            this.sVTrackedVsTimeToolStripMenuItem,
            this.sVTrajectoryToolStripMenuItem});
			this.plotsToolStripMenuItem.Name = "plotsToolStripMenuItem";
			this.plotsToolStripMenuItem.Size = new System.Drawing.Size(266, 24);
			this.plotsToolStripMenuItem.Text = "&Plot Data";
			// 
			// navAccuracyVsTimeToolStripMenuItem
			// 
			this.navAccuracyVsTimeToolStripMenuItem.Name = "navAccuracyVsTimeToolStripMenuItem";
			this.navAccuracyVsTimeToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
			this.navAccuracyVsTimeToolStripMenuItem.Text = "Nav &Accuracy vs Time";
			this.navAccuracyVsTimeToolStripMenuItem.Click += new System.EventHandler(this.navAccuracyVsTimeToolStripMenuItem_Click);
			// 
			// averageCNoToolStripMenuItem
			// 
			this.averageCNoToolStripMenuItem.Name = "averageCNoToolStripMenuItem";
			this.averageCNoToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
			this.averageCNoToolStripMenuItem.Text = "SV Average &CNo";
			this.averageCNoToolStripMenuItem.Click += new System.EventHandler(this.averageCNoToolStripMenuItem_Click);
			// 
			// sVTrackedVsTimeToolStripMenuItem
			// 
			this.sVTrackedVsTimeToolStripMenuItem.Name = "sVTrackedVsTimeToolStripMenuItem";
			this.sVTrackedVsTimeToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
			this.sVTrackedVsTimeToolStripMenuItem.Text = "SV &Tracked vs Time";
			this.sVTrackedVsTimeToolStripMenuItem.Click += new System.EventHandler(this.sVTrackedVsTimeToolStripMenuItem_Click);
			// 
			// sVTrajectoryToolStripMenuItem
			// 
			this.sVTrajectoryToolStripMenuItem.Name = "sVTrajectoryToolStripMenuItem";
			this.sVTrajectoryToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
			this.sVTrajectoryToolStripMenuItem.Text = "SV Tra&jectory";
			this.sVTrajectoryToolStripMenuItem.Click += new System.EventHandler(this.sVTrajectoryToolStripMenuItem1_Click);
			// 
			// setReferenceLocationToolStripMenuItem
			// 
			this.setReferenceLocationToolStripMenuItem.Name = "setReferenceLocationToolStripMenuItem";
			this.setReferenceLocationToolStripMenuItem.Size = new System.Drawing.Size(266, 24);
			this.setReferenceLocationToolStripMenuItem.Text = "Set &Reference Location...";
			this.setReferenceLocationToolStripMenuItem.Click += new System.EventHandler(this.setReferenceLocationToolStripMenuItem_Click);
			// 
			// configureDebugErrorLogToolStripMenuItem
			// 
			this.configureDebugErrorLogToolStripMenuItem.Name = "configureDebugErrorLogToolStripMenuItem";
			this.configureDebugErrorLogToolStripMenuItem.Size = new System.Drawing.Size(266, 24);
			this.configureDebugErrorLogToolStripMenuItem.Text = "Configure Debug &Error Log...";
			this.configureDebugErrorLogToolStripMenuItem.Click += new System.EventHandler(this.configureDebugErrorLogToolStripMenuItem_Click);
			// 
			// autoTestToolStripMenuItem
			// 
			this.autoTestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoTestLoopitToolStripMenuItem,
            this.autoTestStandardTestsToolStripMenuItem,
            this.consoleToolStripMenuItem});
			this.autoTestToolStripMenuItem.Name = "autoTestToolStripMenuItem";
			this.autoTestToolStripMenuItem.Size = new System.Drawing.Size(266, 24);
			this.autoTestToolStripMenuItem.Text = "A&utomation Test";
			this.autoTestToolStripMenuItem.DropDownOpening += new System.EventHandler(this.standardTestToolStripMenuItem_DropDownOpening);
			this.autoTestToolStripMenuItem.MouseHover += new System.EventHandler(this.standardTestToolStripMenuItem_MouseHover);
			// 
			// autoTestLoopitToolStripMenuItem
			// 
			this.autoTestLoopitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("autoTestLoopitToolStripMenuItem.Image")));
			this.autoTestLoopitToolStripMenuItem.Name = "autoTestLoopitToolStripMenuItem";
			this.autoTestLoopitToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
			this.autoTestLoopitToolStripMenuItem.Text = "&Loopit...";
			this.autoTestLoopitToolStripMenuItem.Click += new System.EventHandler(this.autoTestLoopitToolStripMenuItem_Click);
			// 
			// autoTestStandardTestsToolStripMenuItem
			// 
			this.autoTestStandardTestsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoTest3GPPToolStripMenuItem,
            this.autoTestTIA916ToolStripMenuItem,
            this.autoTestAdvancedTestsToolStripMenuItem,
            this.autoTestStatusToolStripMenuItem,
            this.autoTestAbortToolStripMenuItem});
			this.autoTestStandardTestsToolStripMenuItem.Name = "autoTestStandardTestsToolStripMenuItem";
			this.autoTestStandardTestsToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
			this.autoTestStandardTestsToolStripMenuItem.Text = "&Test Cases";
			// 
			// autoTest3GPPToolStripMenuItem
			// 
			this.autoTest3GPPToolStripMenuItem.Name = "autoTest3GPPToolStripMenuItem";
			this.autoTest3GPPToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
			this.autoTest3GPPToolStripMenuItem.Text = "&3GPP...";
			this.autoTest3GPPToolStripMenuItem.Click += new System.EventHandler(this.autoTest3GPPToolStripMenuItem_Click);
			// 
			// autoTestTIA916ToolStripMenuItem
			// 
			this.autoTestTIA916ToolStripMenuItem.Name = "autoTestTIA916ToolStripMenuItem";
			this.autoTestTIA916ToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
			this.autoTestTIA916ToolStripMenuItem.Text = "TIA916...";
			this.autoTestTIA916ToolStripMenuItem.Click += new System.EventHandler(this.autoTestTIA916ToolStripMenuItem_Click);
			// 
			// autoTestAdvancedTestsToolStripMenuItem
			// 
			this.autoTestAdvancedTestsToolStripMenuItem.Name = "autoTestAdvancedTestsToolStripMenuItem";
			this.autoTestAdvancedTestsToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
			this.autoTestAdvancedTestsToolStripMenuItem.Text = "Ad&vanced Tests...";
			this.autoTestAdvancedTestsToolStripMenuItem.Click += new System.EventHandler(this.autoTestAdvancedTestToolStripMenuItem_Click);
			// 
			// autoTestStatusToolStripMenuItem
			// 
			this.autoTestStatusToolStripMenuItem.Name = "autoTestStatusToolStripMenuItem";
			this.autoTestStatusToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
			this.autoTestStatusToolStripMenuItem.Text = "&Status";
			this.autoTestStatusToolStripMenuItem.Click += new System.EventHandler(this.autoTestStatusToolStripMenuItem_Click);
			// 
			// autoTestAbortToolStripMenuItem
			// 
			this.autoTestAbortToolStripMenuItem.Name = "autoTestAbortToolStripMenuItem";
			this.autoTestAbortToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
			this.autoTestAbortToolStripMenuItem.Text = "&Abort";
			this.autoTestAbortToolStripMenuItem.Click += new System.EventHandler(this.autoTestAbortToolStripMenuItem_Click);
			// 
			// consoleToolStripMenuItem
			// 
			this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
			this.consoleToolStripMenuItem.Size = new System.Drawing.Size(146, 24);
			this.consoleToolStripMenuItem.Text = "&Console";
			this.consoleToolStripMenuItem.Click += new System.EventHandler(this.consoleToolStripMenuItem_Click);
			// 
			// featuresToolStripMenuItem
			// 
			this.featuresToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.featuresCWDetectionToolStripMenuItem,
            this.powerModeToolStripMenuItem,
            this.MEMSToolStripMenuItem,
            this.featuresSiRFawareToolStripMenuItem,
            this.tTFSToolStripMenuItem});
			this.featuresToolStripMenuItem.Name = "featuresToolStripMenuItem";
			this.featuresToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
			this.featuresToolStripMenuItem.Text = "Fea&tures";
			// 
			// featuresCWDetectionToolStripMenuItem
			// 
			this.featuresCWDetectionToolStripMenuItem.Name = "featuresCWDetectionToolStripMenuItem";
			this.featuresCWDetectionToolStripMenuItem.Size = new System.Drawing.Size(179, 24);
			this.featuresCWDetectionToolStripMenuItem.Text = "&CW Detection...";
			this.featuresCWDetectionToolStripMenuItem.Click += new System.EventHandler(this.featuresCWDetectionToolStripMenuItem_Click);
			// 
			// powerModeToolStripMenuItem
			// 
			this.powerModeToolStripMenuItem.Name = "powerModeToolStripMenuItem";
			this.powerModeToolStripMenuItem.Size = new System.Drawing.Size(179, 24);
			this.powerModeToolStripMenuItem.Text = "&Power Mode...";
			this.powerModeToolStripMenuItem.Click += new System.EventHandler(this.powerModeToolStripMenuItem_Click);
			// 
			// MEMSToolStripMenuItem
			// 
			this.MEMSToolStripMenuItem.Name = "MEMSToolStripMenuItem";
			this.MEMSToolStripMenuItem.Size = new System.Drawing.Size(179, 24);
			this.MEMSToolStripMenuItem.Text = "&MEMS...";
			this.MEMSToolStripMenuItem.Click += new System.EventHandler(this.MEMSToolStripMenuItem_Click);
			// 
			// featuresSiRFawareToolStripMenuItem
			// 
			this.featuresSiRFawareToolStripMenuItem.Name = "featuresSiRFawareToolStripMenuItem";
			this.featuresSiRFawareToolStripMenuItem.Size = new System.Drawing.Size(179, 24);
			this.featuresSiRFawareToolStripMenuItem.Text = "SiRF&aware...";
			this.featuresSiRFawareToolStripMenuItem.Click += new System.EventHandler(this.featuresSiRFawareToolStripMenuItem_Click);
			// 
			// tTFSToolStripMenuItem
			// 
			this.tTFSToolStripMenuItem.Name = "tTFSToolStripMenuItem";
			this.tTFSToolStripMenuItem.Size = new System.Drawing.Size(179, 24);
			this.tTFSToolStripMenuItem.Text = "TTFS...";
			this.tTFSToolStripMenuItem.Click += new System.EventHandler(this.tTFSToolStripMenuItem_Click);
			// 
			// aidingToolStripMenuItem
			// 
			this.aidingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aidingConfigureToolStripMenuItem,
            this.aidingSummaryToolStripMenuItem,
            this.toolStripSeparator12,
            this.aidingTTBToolStripMenuItem,
            this.toolStripSeparator13,
            this.aidingsDownloadServerAssistedDataToolStripMenuItem});
			this.aidingToolStripMenuItem.Name = "aidingToolStripMenuItem";
			this.aidingToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
			this.aidingToolStripMenuItem.Text = "&AGPS";
			this.aidingToolStripMenuItem.Click += new System.EventHandler(this.aidingToolStripMenuItem_Click);
			// 
			// aidingConfigureToolStripMenuItem
			// 
			this.aidingConfigureToolStripMenuItem.Name = "aidingConfigureToolStripMenuItem";
			this.aidingConfigureToolStripMenuItem.Size = new System.Drawing.Size(295, 24);
			this.aidingConfigureToolStripMenuItem.Text = "&Configure...";
			this.aidingConfigureToolStripMenuItem.Click += new System.EventHandler(this.aidingConfigureToolStripMenuItem_Click);
			// 
			// aidingSummaryToolStripMenuItem
			// 
			this.aidingSummaryToolStripMenuItem.Name = "aidingSummaryToolStripMenuItem";
			this.aidingSummaryToolStripMenuItem.Size = new System.Drawing.Size(295, 24);
			this.aidingSummaryToolStripMenuItem.Text = "&Summary";
			this.aidingSummaryToolStripMenuItem.Click += new System.EventHandler(this.aidingSummaryToolStripMenuItem_Click);
			// 
			// toolStripSeparator12
			// 
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			this.toolStripSeparator12.Size = new System.Drawing.Size(292, 6);
			// 
			// aidingTTBToolStripMenuItem
			// 
			this.aidingTTBToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TTBConnectToolStripMenuItem,
            this.TTBConfigureTimeAidingToolStripMenuItem,
            this.TTBViewToolStripMenuItem});
			this.aidingTTBToolStripMenuItem.Name = "aidingTTBToolStripMenuItem";
			this.aidingTTBToolStripMenuItem.Size = new System.Drawing.Size(295, 24);
			this.aidingTTBToolStripMenuItem.Text = "&TTB";
			this.aidingTTBToolStripMenuItem.DropDownOpened += new System.EventHandler(this.aidingTTBToolStripMenuItem_DropDownOpened);
			// 
			// TTBConnectToolStripMenuItem
			// 
			this.TTBConnectToolStripMenuItem.Name = "TTBConnectToolStripMenuItem";
			this.TTBConnectToolStripMenuItem.Size = new System.Drawing.Size(237, 24);
			this.TTBConnectToolStripMenuItem.Text = "&Connect TTB...";
			this.TTBConnectToolStripMenuItem.Click += new System.EventHandler(this.TTBConnectToolStripMenuItem_Click);
			// 
			// TTBConfigureTimeAidingToolStripMenuItem
			// 
			this.TTBConfigureTimeAidingToolStripMenuItem.Name = "TTBConfigureTimeAidingToolStripMenuItem";
			this.TTBConfigureTimeAidingToolStripMenuItem.Size = new System.Drawing.Size(237, 24);
			this.TTBConfigureTimeAidingToolStripMenuItem.Text = "Configure &Time Aiding...";
			this.TTBConfigureTimeAidingToolStripMenuItem.Click += new System.EventHandler(this.TTBConfigureTimeAidingToolStripMenuItem_Click);
			// 
			// TTBViewToolStripMenuItem
			// 
			this.TTBViewToolStripMenuItem.Name = "TTBViewToolStripMenuItem";
			this.TTBViewToolStripMenuItem.Size = new System.Drawing.Size(237, 24);
			this.TTBViewToolStripMenuItem.Text = "&View...";
			this.TTBViewToolStripMenuItem.Click += new System.EventHandler(this.TTBViewToolStripMenuItem_Click);
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			this.toolStripSeparator13.Size = new System.Drawing.Size(292, 6);
			// 
			// aidingsDownloadServerAssistedDataToolStripMenuItem
			// 
			this.aidingsDownloadServerAssistedDataToolStripMenuItem.Name = "aidingsDownloadServerAssistedDataToolStripMenuItem";
			this.aidingsDownloadServerAssistedDataToolStripMenuItem.Size = new System.Drawing.Size(295, 24);
			this.aidingsDownloadServerAssistedDataToolStripMenuItem.Text = "Download &Server Assisted Data...";
			// 
			// instrumentControlMenuItem
			// 
			this.instrumentControlMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rFReplayMenuItem,
            this.simplexMenu,
            this.sPAzMenu,
            this.signalGeneratorMenu,
            this.testRackMenu});
			this.instrumentControlMenuItem.Name = "instrumentControlMenuItem";
			this.instrumentControlMenuItem.Size = new System.Drawing.Size(144, 24);
			this.instrumentControlMenuItem.Text = "&Instrument Control";
			// 
			// rFReplayMenuItem
			// 
			this.rFReplayMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rfReplayConfigurationMenu,
            this.rfPlaybackCaptureMenu,
            this.rfReplayPlaybackMenu,
            this.rfReplaySynthesizerMenu});
			this.rFReplayMenuItem.Name = "rFReplayMenuItem";
			this.rFReplayMenuItem.Size = new System.Drawing.Size(259, 24);
			this.rFReplayMenuItem.Text = "&RF Replay";
			// 
			// rfReplayConfigurationMenu
			// 
			this.rfReplayConfigurationMenu.Name = "rfReplayConfigurationMenu";
			this.rfReplayConfigurationMenu.Size = new System.Drawing.Size(178, 24);
			this.rfReplayConfigurationMenu.Text = "Con&figuration...";
			this.rfReplayConfigurationMenu.Click += new System.EventHandler(this.rfReplayConfigurationMenu_Click);
			// 
			// rfPlaybackCaptureMenu
			// 
			this.rfPlaybackCaptureMenu.Name = "rfPlaybackCaptureMenu";
			this.rfPlaybackCaptureMenu.Size = new System.Drawing.Size(178, 24);
			this.rfPlaybackCaptureMenu.Text = "&Capture...";
			this.rfPlaybackCaptureMenu.Click += new System.EventHandler(this.rfPlaybackCaptureMenu_Click);
			// 
			// rfReplayPlaybackMenu
			// 
			this.rfReplayPlaybackMenu.Name = "rfReplayPlaybackMenu";
			this.rfReplayPlaybackMenu.Size = new System.Drawing.Size(178, 24);
			this.rfReplayPlaybackMenu.Text = "&Playback...";
			this.rfReplayPlaybackMenu.Click += new System.EventHandler(this.rfReplayPlaybackMenu_Click);
			// 
			// rfReplaySynthesizerMenu
			// 
			this.rfReplaySynthesizerMenu.Name = "rfReplaySynthesizerMenu";
			this.rfReplaySynthesizerMenu.Size = new System.Drawing.Size(178, 24);
			this.rfReplaySynthesizerMenu.Text = "&Synthesizer...";
			this.rfReplaySynthesizerMenu.Click += new System.EventHandler(this.rfReplaySynthesizerMenu_Click);
			// 
			// simplexMenu
			// 
			this.simplexMenu.Name = "simplexMenu";
			this.simplexMenu.Size = new System.Drawing.Size(259, 24);
			this.simplexMenu.Text = "&Spirent STR4500/GSS6700...";
			this.simplexMenu.Click += new System.EventHandler(this.simplexMenu_Click);
			// 
			// sPAzMenu
			// 
			this.sPAzMenu.Name = "sPAzMenu";
			this.sPAzMenu.Size = new System.Drawing.Size(259, 24);
			this.sPAzMenu.Text = "SPA&z...";
			this.sPAzMenu.Click += new System.EventHandler(this.sPAzMenu_Click);
			// 
			// signalGeneratorMenu
			// 
			this.signalGeneratorMenu.Name = "signalGeneratorMenu";
			this.signalGeneratorMenu.Size = new System.Drawing.Size(259, 24);
			this.signalGeneratorMenu.Text = "Signal &Generator...";
			this.signalGeneratorMenu.Click += new System.EventHandler(this.signalGeneratorMenu_Click);
			// 
			// testRackMenu
			// 
			this.testRackMenu.Name = "testRackMenu";
			this.testRackMenu.Size = new System.Drawing.Size(259, 24);
			this.testRackMenu.Text = "&Test Rack...";
			this.testRackMenu.Click += new System.EventHandler(this.testRackMenu_Click);
			// 
			// reportMenuItem
			// 
			this.reportMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportE911Menu,
            this.report3GPPMenu,
            this.reportTIA916Menu,
            this.reportPerformanceMenu,
            this.reportResetMenu,
            this.reportPseudoRangeMenu,
            this.reportlSMResetMenu,
            this.pointToPointAnalysisReportToolStripMenuItem,
            this.mPMToolStripMenuItem,
            this.sDOGenerationToolStripMenuItem});
			this.reportMenuItem.Name = "reportMenuItem";
			this.reportMenuItem.Size = new System.Drawing.Size(66, 24);
			this.reportMenuItem.Text = "Re&port";
			// 
			// reportE911Menu
			// 
			this.reportE911Menu.Name = "reportE911Menu";
			this.reportE911Menu.Size = new System.Drawing.Size(286, 24);
			this.reportE911Menu.Text = "&E911 Report...";
			this.reportE911Menu.Click += new System.EventHandler(this.reportE911Menu_Click);
			// 
			// report3GPPMenu
			// 
			this.report3GPPMenu.Name = "report3GPPMenu";
			this.report3GPPMenu.Size = new System.Drawing.Size(286, 24);
			this.report3GPPMenu.Text = "&3GPP Report...";
			this.report3GPPMenu.Click += new System.EventHandler(this.report3GPPMenu_Click);
			// 
			// reportTIA916Menu
			// 
			this.reportTIA916Menu.Name = "reportTIA916Menu";
			this.reportTIA916Menu.Size = new System.Drawing.Size(286, 24);
			this.reportTIA916Menu.Text = "TIA916 Report...";
			this.reportTIA916Menu.Click += new System.EventHandler(this.reportTIA916Menu_Click);
			// 
			// reportPerformanceMenu
			// 
			this.reportPerformanceMenu.Name = "reportPerformanceMenu";
			this.reportPerformanceMenu.Size = new System.Drawing.Size(286, 24);
			this.reportPerformanceMenu.Text = "&Performance Report...";
			this.reportPerformanceMenu.Click += new System.EventHandler(this.reportPerformanceMenu_Click);
			// 
			// reportResetMenu
			// 
			this.reportResetMenu.Name = "reportResetMenu";
			this.reportResetMenu.Size = new System.Drawing.Size(286, 24);
			this.reportResetMenu.Text = "&Reset Report...";
			this.reportResetMenu.Click += new System.EventHandler(this.reportResetMenu_Click);
			// 
			// reportPseudoRangeMenu
			// 
			this.reportPseudoRangeMenu.Name = "reportPseudoRangeMenu";
			this.reportPseudoRangeMenu.Size = new System.Drawing.Size(286, 24);
			this.reportPseudoRangeMenu.Text = "P&seudoRange Report...";
			this.reportPseudoRangeMenu.Click += new System.EventHandler(this.reportPseudoRangeMenu_Click);
			// 
			// reportlSMResetMenu
			// 
			this.reportlSMResetMenu.Name = "reportlSMResetMenu";
			this.reportlSMResetMenu.Size = new System.Drawing.Size(286, 24);
			this.reportlSMResetMenu.Text = "&LSM Reset Report...";
			this.reportlSMResetMenu.Click += new System.EventHandler(this.reportlSMResetMenu_Click);
			// 
			// pointToPointAnalysisReportToolStripMenuItem
			// 
			this.pointToPointAnalysisReportToolStripMenuItem.Name = "pointToPointAnalysisReportToolStripMenuItem";
			this.pointToPointAnalysisReportToolStripMenuItem.Size = new System.Drawing.Size(286, 24);
			this.pointToPointAnalysisReportToolStripMenuItem.Text = "Point To Point &Analysis Report...";
			this.pointToPointAnalysisReportToolStripMenuItem.Click += new System.EventHandler(this.pointToPointAnalysisReportToolStripMenuItem_Click);
			// 
			// mPMToolStripMenuItem
			// 
			this.mPMToolStripMenuItem.Name = "mPMToolStripMenuItem";
			this.mPMToolStripMenuItem.Size = new System.Drawing.Size(286, 24);
			this.mPMToolStripMenuItem.Text = "MPM...";
			this.mPMToolStripMenuItem.Click += new System.EventHandler(this.mPMToolStripMenuItem_Click);
			// 
			// sDOGenerationToolStripMenuItem
			// 
			this.sDOGenerationToolStripMenuItem.Name = "sDOGenerationToolStripMenuItem";
			this.sDOGenerationToolStripMenuItem.Size = new System.Drawing.Size(286, 24);
			this.sDOGenerationToolStripMenuItem.Text = "SDO Generation...";
			this.sDOGenerationToolStripMenuItem.Click += new System.EventHandler(this.sDOGenerationToolStripMenuItem_Click);
			// 
			// siRFDRiveToolStripMenuItem
			// 
			this.siRFDRiveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.navigationModeControlToolStripMenuItem,
            this.gyroFactoryCalibrationToolStripMenuItem,
            this.dRSensorsToolStripMenuItem,
            this.setPollToolStripMenuItem});
			this.siRFDRiveToolStripMenuItem.Name = "siRFDRiveToolStripMenuItem";
			this.siRFDRiveToolStripMenuItem.Size = new System.Drawing.Size(88, 24);
			this.siRFDRiveToolStripMenuItem.Text = "SiRFDRive";
			// 
			// navigationModeControlToolStripMenuItem
			// 
			this.navigationModeControlToolStripMenuItem.Name = "navigationModeControlToolStripMenuItem";
			this.navigationModeControlToolStripMenuItem.Size = new System.Drawing.Size(287, 24);
			this.navigationModeControlToolStripMenuItem.Text = "Navigation Mode Control...";
			this.navigationModeControlToolStripMenuItem.Click += new System.EventHandler(this.navigationModeControlToolStripMenuItem_Click);
			// 
			// gyroFactoryCalibrationToolStripMenuItem
			// 
			this.gyroFactoryCalibrationToolStripMenuItem.Name = "gyroFactoryCalibrationToolStripMenuItem";
			this.gyroFactoryCalibrationToolStripMenuItem.Size = new System.Drawing.Size(287, 24);
			this.gyroFactoryCalibrationToolStripMenuItem.Text = "Gyro Factory Calibration...";
			this.gyroFactoryCalibrationToolStripMenuItem.Click += new System.EventHandler(this.gyroFactoryCalibrationToolStripMenuItem_Click);
			// 
			// dRSensorsToolStripMenuItem
			// 
			this.dRSensorsToolStripMenuItem.Name = "dRSensorsToolStripMenuItem";
			this.dRSensorsToolStripMenuItem.Size = new System.Drawing.Size(287, 24);
			this.dRSensorsToolStripMenuItem.Text = "DR Sensor\'s Parameters...";
			this.dRSensorsToolStripMenuItem.Click += new System.EventHandler(this.dRSensorsToolStripMenuItem_Click);
			// 
			// setPollToolStripMenuItem
			// 
			this.setPollToolStripMenuItem.Name = "setPollToolStripMenuItem";
			this.setPollToolStripMenuItem.Size = new System.Drawing.Size(287, 24);
			this.setPollToolStripMenuItem.Text = "Set/Poll Generic Sensor Param...";
			this.setPollToolStripMenuItem.Click += new System.EventHandler(this.setPollToolStripMenuItem_Click);
			// 
			// windowMenuItem
			// 
			this.windowMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeMenu,
            this.tileVerticalMenu,
            this.tileHorizontalMenu,
            this.toolStripSeparator1,
            this.restoreLayoutMenuItem,
            this.saveLayoutMenu,
            this.closeAllMenu});
			this.windowMenuItem.Name = "windowMenuItem";
			this.windowMenuItem.Size = new System.Drawing.Size(76, 24);
			this.windowMenuItem.Text = "&Window";
			// 
			// cascadeMenu
			// 
			this.cascadeMenu.Name = "cascadeMenu";
			this.cascadeMenu.Size = new System.Drawing.Size(176, 24);
			this.cascadeMenu.Text = "&Cascade";
			this.cascadeMenu.Click += new System.EventHandler(this.cascadeMenu_Click);
			// 
			// tileVerticalMenu
			// 
			this.tileVerticalMenu.Name = "tileVerticalMenu";
			this.tileVerticalMenu.Size = new System.Drawing.Size(176, 24);
			this.tileVerticalMenu.Text = "Tile &Vertical";
			this.tileVerticalMenu.Click += new System.EventHandler(this.tileVerticalMenu_Click);
			// 
			// tileHorizontalMenu
			// 
			this.tileHorizontalMenu.Name = "tileHorizontalMenu";
			this.tileHorizontalMenu.Size = new System.Drawing.Size(176, 24);
			this.tileHorizontalMenu.Text = "Tile &Horizontal";
			this.tileHorizontalMenu.Click += new System.EventHandler(this.tileHorizontalMenu_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(173, 6);
			// 
			// restoreLayoutMenuItem
			// 
			this.restoreLayoutMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultLayoutMenu,
            this.previousSettingsLayoutMenu,
            this.userSettingsLayoutMenu});
			this.restoreLayoutMenuItem.Name = "restoreLayoutMenuItem";
			this.restoreLayoutMenuItem.Size = new System.Drawing.Size(176, 24);
			this.restoreLayoutMenuItem.Text = "&Restore Layout";
			this.restoreLayoutMenuItem.DropDownOpened += new System.EventHandler(this.restoreLayoutMenuItem_DropDownOpened);
			// 
			// defaultLayoutMenu
			// 
			this.defaultLayoutMenu.Name = "defaultLayoutMenu";
			this.defaultLayoutMenu.Size = new System.Drawing.Size(190, 24);
			this.defaultLayoutMenu.Text = "&Default";
			this.defaultLayoutMenu.Click += new System.EventHandler(this.defaultLayoutMenu_Click);
			// 
			// previousSettingsLayoutMenu
			// 
			this.previousSettingsLayoutMenu.Name = "previousSettingsLayoutMenu";
			this.previousSettingsLayoutMenu.Size = new System.Drawing.Size(190, 24);
			this.previousSettingsLayoutMenu.Text = "&Previous Settings";
			this.previousSettingsLayoutMenu.Click += new System.EventHandler(this.previousSettingsLayoutMenu_Click);
			// 
			// userSettingsLayoutMenu
			// 
			this.userSettingsLayoutMenu.Name = "userSettingsLayoutMenu";
			this.userSettingsLayoutMenu.Size = new System.Drawing.Size(190, 24);
			this.userSettingsLayoutMenu.Text = "&User Settings";
			this.userSettingsLayoutMenu.Click += new System.EventHandler(this.userSettingsLayoutMenu_Click);
			// 
			// saveLayoutMenu
			// 
			this.saveLayoutMenu.Name = "saveLayoutMenu";
			this.saveLayoutMenu.Size = new System.Drawing.Size(176, 24);
			this.saveLayoutMenu.Text = "&Save Layout";
			this.saveLayoutMenu.Click += new System.EventHandler(this.saveLayoutMenu_Click);
			// 
			// closeAllMenu
			// 
			this.closeAllMenu.Name = "closeAllMenu";
			this.closeAllMenu.Size = new System.Drawing.Size(176, 24);
			this.closeAllMenu.Text = "Close &All";
			this.closeAllMenu.Click += new System.EventHandler(this.closeAllMenu_Click);
			// 
			// helpMenuItem
			// 
			this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutMenu,
            this.developerDocMenu,
            this.userManualMenu});
			this.helpMenuItem.Name = "helpMenuItem";
			this.helpMenuItem.Size = new System.Drawing.Size(53, 24);
			this.helpMenuItem.Text = "&Help";
			// 
			// aboutMenu
			// 
			this.aboutMenu.Name = "aboutMenu";
			this.aboutMenu.Size = new System.Drawing.Size(254, 24);
			this.aboutMenu.Text = "&About";
			this.aboutMenu.Click += new System.EventHandler(this.aboutMenu_Click);
			// 
			// developerDocMenu
			// 
			this.developerDocMenu.Name = "developerDocMenu";
			this.developerDocMenu.Size = new System.Drawing.Size(254, 24);
			this.developerDocMenu.Text = "Developer &Documentation";
			this.developerDocMenu.Click += new System.EventHandler(this.developerDocMenu_Click);
			// 
			// userManualMenu
			// 
			this.userManualMenu.Image = ((System.Drawing.Image)(resources.GetObject("userManualMenu.Image")));
			this.userManualMenu.Name = "userManualMenu";
			this.userManualMenu.Size = new System.Drawing.Size(254, 24);
			this.userManualMenu.Text = "&User Manual";
			this.userManualMenu.Click += new System.EventHandler(this.userManualMenu_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.BackColor = System.Drawing.SystemColors.Menu;
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripLogStatusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 377);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
			this.statusStrip1.Size = new System.Drawing.Size(1128, 25);
			this.statusStrip1.TabIndex = 15;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel
			// 
			this.toolStripStatusLabel.BackColor = System.Drawing.SystemColors.Menu;
			this.toolStripStatusLabel.Name = "toolStripStatusLabel";
			this.toolStripStatusLabel.Size = new System.Drawing.Size(79, 20);
			this.toolStripStatusLabel.Text = "port status";
			// 
			// toolStripLogStatusLabel
			// 
			this.toolStripLogStatusLabel.BackColor = System.Drawing.SystemColors.Menu;
			this.toolStripLogStatusLabel.Name = "toolStripLogStatusLabel";
			this.toolStripLogStatusLabel.Size = new System.Drawing.Size(74, 20);
			this.toolStripLogStatusLabel.Text = "| Log: idle";
			// 
			// filePlayBackTrackBar
			// 
			this.filePlayBackTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.filePlayBackTrackBar.BackColor = System.Drawing.SystemColors.Menu;
			this.filePlayBackTrackBar.Location = new System.Drawing.Point(1000, 5);
			this.filePlayBackTrackBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.filePlayBackTrackBar.Name = "filePlayBackTrackBar";
			this.filePlayBackTrackBar.Size = new System.Drawing.Size(128, 56);
			this.filePlayBackTrackBar.TabIndex = 16;
			this.filePlayBackTrackBar.Scroll += new System.EventHandler(this.filePlayBackTrackBar_Scroll);
			// 
			// toolStripContainer1
			// 
			this.toolStripContainer1.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.BackColor = System.Drawing.SystemColors.Menu;
			this.toolStripContainer1.ContentPanel.Controls.Add(this.logManagerStatusLabel);
			this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1128, 36);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 28);
			this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(1128, 65);
			this.toolStripContainer1.TabIndex = 17;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.BackColor = System.Drawing.SystemColors.Menu;
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripMain);
			// 
			// logManagerStatusLabel
			// 
			this.logManagerStatusLabel.AutoSize = true;
			this.logManagerStatusLabel.Location = new System.Drawing.Point(4, 7);
			this.logManagerStatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.logManagerStatusLabel.Name = "logManagerStatusLabel";
			this.logManagerStatusLabel.Size = new System.Drawing.Size(106, 17);
			this.logManagerStatusLabel.TabIndex = 0;
			this.logManagerStatusLabel.Text = "Log File Status:";
			// 
			// frmMDIMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.ClientSize = new System.Drawing.Size(1128, 402);
			this.Controls.Add(this.filePlayBackTrackBar);
			this.Controls.Add(this.toolStripContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "frmMDIMain";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "SiRFLive";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMDIMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMDIMain_Load);
			this.toolStripMain.ResumeLayout(false);
			this.toolStripMain.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.filePlayBackTrackBar)).EndInit();
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.ContentPanel.PerformLayout();
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }
		#endregion

		public bool IsNullType(object x)
        {
            return (x == null);
        }

        private void loadLocation(Form formWindow, string top, string left, string width, string height, string state)
        {
            formWindow.Left = Convert.ToInt32(left);
            formWindow.Top = Convert.ToInt32(top);
            formWindow.Width = Convert.ToInt32(width);
            formWindow.Height = Convert.ToInt32(height);
            if (state == "Maximized")
            {
                formWindow.WindowState = FormWindowState.Maximized;
            }
            else if (state == "Minimized")
            {
                formWindow.WindowState = FormWindowState.Minimized;
            }
            else
            {
                formWindow.WindowState = FormWindowState.Normal;
            }
        }

        private void loadWindowLocation(Form targetForm, WinLocation loc)
        {
            targetForm.Left = loc.Left;
            targetForm.Top = loc.Top;
            if (loc.Width != 0)
            {
                targetForm.Width = loc.Width;
            }
            if (loc.Height != 0)
            {
                targetForm.Height = loc.Height;
            }
        }

        private frmCompassView localCreateCompassViewWin(PortManager target)
        {
            string str = target.comm.sourceDeviceName + ": MEMS Compass View";
            if ((target._compassView == null) || target._compassView.IsDisposed)
            {
                target._compassView = new frmCompassView(target.comm);
                if (target.CompassViewLocation.Width != 0)
                {
                    target._compassView.Width = target.CompassViewLocation.Width;
                    target._compassView.WinWidth = target.CompassViewLocation.Width;
                }
                if (target.CompassViewLocation.Height != 0)
                {
                    target._compassView.Height = target.CompassViewLocation.Height;
                    target._compassView.WinHeight = target.CompassViewLocation.Height;
                }
                if (target.CompassViewLocation.Left != 0)
                {
                    target._compassView.Left = target.CompassViewLocation.Left;
                    target._compassView.WinLeft = target.CompassViewLocation.Left;
                }
                if (target.CompassViewLocation.Top != 0)
                {
                    target._compassView.Top = target.CompassViewLocation.Top;
                    target._compassView.WinTop = target.CompassViewLocation.Top;
                }
                target._compassView.Show();
                target.CompassViewLocation.IsOpen = true;
            }
            else if (target.CompassViewLocation.IsOpen)
            {
                target._compassView.Close();
            }
            else
            {
                target._compassView.Show();
                target.CompassViewLocation.IsOpen = true;
            }
            target._compassView.Text = str;
            target._compassView.UpdatePortManager += new frmCompassView.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
            target._compassView.BringToFront();
            return target._compassView;
        }

        private frmCommDebugView localCreateDebugViewWin(PortManager target)
        {
            if ((target.DebugView != null) && !target.DebugView.IsDisposed)
            {
                target.DebugView.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": Debug View ";
                if ((target.comm.RxCtrl != null) && (target.comm.RxCtrl.ResetCtrl != null))
                {
                    str = str + target.comm.RxCtrl.ResetCtrl.ResetRxSwVersion;
                }
                if ((target.DebugView == null) || target.DebugView.IsDisposed)
                {
                    target.DebugView = new frmCommDebugView(target.comm);
                    if (target.DebugViewLocation.Width != 0)
                    {
                        target.DebugView.Width = target.DebugViewLocation.Width;
                        target.DebugView.WinWidth = target.DebugViewLocation.Width;
                    }
                    if (target.DebugViewLocation.Height != 0)
                    {
                        target.DebugView.Height = target.DebugViewLocation.Height;
                        target.DebugView.WinHeight = target.DebugViewLocation.Height;
                    }
                    if (target.DebugViewLocation.Left != 0)
                    {
                        target.DebugView.Left = target.DebugViewLocation.Left;
                        target.DebugView.WinLeft = target.DebugViewLocation.Left;
                    }
                    if (target.DebugViewLocation.Top != 0)
                    {
                        target.DebugView.Top = target.DebugViewLocation.Top;
                        target.DebugView.WinTop = target.DebugViewLocation.Top;
                    }
                    target.DebugView.Show();
                    target.DebugViewLocation.IsOpen = true;
                }
                target.DebugView.CommWindow = target.comm;
                target.DebugView.Text = str;
                target.DebugView.UpdatePortManager += new frmCommDebugView.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target.DebugView.BringToFront();
            }
            return target.DebugView;
        }

        private frmCommDRSensor localCreateDRSensorDataWin(PortManager target)
        {
            if ((target._DRSensorViewPanel != null) && !target._DRSensorViewPanel.IsDisposed)
            {
                target._DRSensorViewPanel.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ":  View";
                if ((target._DRSensorViewPanel == null) || target._DRSensorViewPanel.IsDisposed)
                {
                    target._DRSensorViewPanel = new frmCommDRSensor();
                    if (target.LocationDRSensorLocation.Width != 0)
                    {
                        target._DRSensorViewPanel.Width = target.LocationDRSensorLocation.Width;
                        target._DRSensorViewPanel.WinWidth = target.LocationDRSensorLocation.Width;
                    }
                    if (target.LocationDRSensorLocation.Height != 0)
                    {
                        target._DRSensorViewPanel.Height = target.LocationDRSensorLocation.Height;
                        target._DRSensorViewPanel.WinHeight = target.LocationDRSensorLocation.Height;
                    }
                    if (target.LocationDRSensorLocation.Left != 0)
                    {
                        target._DRSensorViewPanel.Left = target.LocationDRSensorLocation.Left;
                        target._DRSensorViewPanel.WinLeft = target.LocationDRSensorLocation.Left;
                    }
                    if (target.LocationDRSensorLocation.Top != 0)
                    {
                        target._DRSensorViewPanel.Top = target.LocationDRSensorLocation.Top;
                        target._DRSensorViewPanel.WinTop = target.LocationDRSensorLocation.Top;
                    }
                    target._DRSensorViewPanel.Show();
                    target.LocationDRSensorLocation.IsOpen = true;
                }
                target._DRSensorViewPanel.Text = str;
                target._DRSensorViewPanel.CommWindow = target.comm;
                target._DRSensorViewPanel.UpdatePortManager += new frmCommDRSensor.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target._DRSensorViewPanel.BringToFront();
            }
            return target._DRSensorViewPanel;
        }

        private frmCommDRStatus localCreateDRStatusWin(PortManager target)
        {
            if ((target._DRNavStatusViewPanel != null) && !target._DRNavStatusViewPanel.IsDisposed)
            {
                target._DRNavStatusViewPanel.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ":  View";
                if ((target._DRNavStatusViewPanel == null) || target._DRNavStatusViewPanel.IsDisposed)
                {
                    target._DRNavStatusViewPanel = new frmCommDRStatus();
                    if (target.LocationDRStatusLocation.Width != 0)
                    {
                        target._DRNavStatusViewPanel.Width = target.LocationDRStatusLocation.Width;
                        target._DRNavStatusViewPanel.WinWidth = target.LocationDRStatusLocation.Width;
                    }
                    if (target.LocationDRStatusLocation.Height != 0)
                    {
                        target._DRNavStatusViewPanel.Height = target.LocationDRStatusLocation.Height;
                        target._DRNavStatusViewPanel.WinHeight = target.LocationDRStatusLocation.Height;
                    }
                    if (target.LocationDRStatusLocation.Left != 0)
                    {
                        target._DRNavStatusViewPanel.Left = target.LocationDRStatusLocation.Left;
                        target._DRNavStatusViewPanel.WinLeft = target.LocationDRStatusLocation.Left;
                    }
                    if (target.LocationDRStatusLocation.Top != 0)
                    {
                        target._DRNavStatusViewPanel.Top = target.LocationDRStatusLocation.Top;
                        target._DRNavStatusViewPanel.WinTop = target.LocationDRStatusLocation.Top;
                    }
                    target._DRNavStatusViewPanel.Show();
                    target.LocationDRStatusLocation.IsOpen = true;
                }
                target._DRNavStatusViewPanel.Text = str;
                target._DRNavStatusViewPanel.CommWindow = target.comm;
                target._DRNavStatusViewPanel.UpdatePortManager += new frmCommDRStatus.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target._DRNavStatusViewPanel.BringToFront();
            }
            return target._DRNavStatusViewPanel;
        }

        private frmCommErrorView localCreateErrorViewWin(PortManager target)
        {
            if ((target._errorView != null) && !target._errorView.IsDisposed)
            {
                target._errorView.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": Error View";
                if ((target._errorView == null) || target._errorView.IsDisposed)
                {
                    target._errorView = new frmCommErrorView();
                    if (target.ErrorViewLocation.Width != 0)
                    {
                        target._errorView.Width = target.ErrorViewLocation.Width;
                        target._errorView.WinWidth = target.ErrorViewLocation.Width;
                    }
                    if (target.ErrorViewLocation.Height != 0)
                    {
                        target._errorView.Height = target.ErrorViewLocation.Height;
                        target._errorView.WinHeight = target.ErrorViewLocation.Height;
                    }
                    if (target.ErrorViewLocation.Left != 0)
                    {
                        target._errorView.Left = target.ErrorViewLocation.Left;
                        target._errorView.WinLeft = target.ErrorViewLocation.Left;
                    }
                    if (target.ErrorViewLocation.Top != 0)
                    {
                        target._errorView.Top = target.ErrorViewLocation.Top;
                        target._errorView.WinTop = target.ErrorViewLocation.Top;
                    }
                    target._errorView.Show();
                    target.ErrorViewLocation.IsOpen = true;
                }
                target._errorView.CommWindow = target.comm;
                target._errorView.Text = str;
                target._errorView.UpdatePortManager += new frmCommErrorView.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target._errorView.BringToFront();
            }
            return target._errorView;
        }

        private frmInterferenceReport localCreateInterferenceReportWindow(PortManager target)
        {
            string str = target.comm.sourceDeviceName + ": Interference Detection";
            if ((target._interferenceReport == null) || target._interferenceReport.IsDisposed)
            {
                target._interferenceReport = new frmInterferenceReport(target.comm);
                if (target.InterferenceLocation.Width != 0)
                {
                    target._interferenceReport.Width = target.InterferenceLocation.Width;
                    target._interferenceReport.WinWidth = target.InterferenceLocation.Width;
                }
                if (target.InterferenceLocation.Height != 0)
                {
                    target._interferenceReport.Height = target.InterferenceLocation.Height;
                    target._interferenceReport.WinHeight = target.InterferenceLocation.Height;
                }
                if (target.InterferenceLocation.Left != 0)
                {
                    target._interferenceReport.Left = target.InterferenceLocation.Left;
                    target._interferenceReport.WinLeft = target.InterferenceLocation.Left;
                }
                if (target.InterferenceLocation.Top != 0)
                {
                    target._interferenceReport.Top = target.InterferenceLocation.Top;
                    target._interferenceReport.WinTop = target.InterferenceLocation.Top;
                }
                target._interferenceReport.Show();
                target.InterferenceLocation.IsOpen = true;
            }
            else if (target.InterferenceLocation.IsOpen)
            {
                target._interferenceReport.Close();
            }
            else
            {
                target._interferenceReport.Show();
                target.InterferenceLocation.IsOpen = true;
                target._interferenceReport.StartListen();
            }
            target._interferenceReport.Text = str;
            target._interferenceReport.UpdatePortManager += new frmInterferenceReport.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
            target._interferenceReport.BringToFront();
            return target._interferenceReport;
        }

        private frmCommLocationMap localCreateLocationMapWin(PortManager target)
        {
            if ((target._locationViewPanel != null) && !target._locationViewPanel.IsDisposed)
            {
                target._locationViewPanel.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": Location View";
                if ((target._locationViewPanel == null) || target._locationViewPanel.IsDisposed)
                {
                    target._locationViewPanel = new frmCommLocationMap();
                    if (target.LocationMapLocation.Width != 0)
                    {
                        target._locationViewPanel.Width = target.LocationMapLocation.Width;
                        target._locationViewPanel.WinWidth = target.LocationMapLocation.Width;
                    }
                    if (target.LocationMapLocation.Height != 0)
                    {
                        target._locationViewPanel.Height = target.LocationMapLocation.Height;
                        target._locationViewPanel.WinHeight = target.LocationMapLocation.Height;
                    }
                    if (target.LocationMapLocation.Left != 0)
                    {
                        target._locationViewPanel.Left = target.LocationMapLocation.Left;
                        target._locationViewPanel.WinLeft = target.LocationMapLocation.Left;
                    }
                    if (target.LocationMapLocation.Top != 0)
                    {
                        target._locationViewPanel.Top = target.LocationMapLocation.Top;
                        target._locationViewPanel.WinTop = target.LocationMapLocation.Top;
                    }
                    target._locationViewPanel.Show();
                    target.LocationMapLocation.IsOpen = true;
                }
                target._locationViewPanel.Text = str;
                target._locationViewPanel.CommWindow = target.comm;
                target._locationViewPanel.UpdatePortManager += new frmCommLocationMap.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target._locationViewPanel.BringToFront();
            }
            return target._locationViewPanel;
        }

        private frmMEMSView localCreateMEMSViewWin(PortManager target)
        {
            string str = target.comm.sourceDeviceName + ": MEMS";
            if ((target._memsView == null) || target._memsView.IsDisposed)
            {
                target._memsView = new frmMEMSView(target.comm);
                if (target.MEMSLocation.Width != 0)
                {
                    target._memsView.Width = target.MEMSLocation.Width;
                    target._memsView.WinWidth = target.MEMSLocation.Width;
                }
                if (target.MEMSLocation.Height != 0)
                {
                    target._memsView.Height = target.MEMSLocation.Height;
                    target._memsView.WinHeight = target.MEMSLocation.Height;
                }
                if (target.MEMSLocation.Left != 0)
                {
                    target._memsView.Left = target.MEMSLocation.Left;
                    target._memsView.WinLeft = target.MEMSLocation.Left;
                }
                if (target.MEMSLocation.Top != 0)
                {
                    target._memsView.Top = target.MEMSLocation.Top;
                    target._memsView.WinTop = target.MEMSLocation.Top;
                }
                target._memsView.Show();
                target.MEMSLocation.IsOpen = true;
            }
            else if (target.MEMSLocation.IsOpen)
            {
                target._memsView.Close();
            }
            else
            {
                target._memsView.Show();
                target.MEMSLocation.IsOpen = true;
            }
            target._memsView.Text = str;
            target._memsView.UpdatePortManager += new frmMEMSView.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
            target._memsView.BringToFront();
            return target._memsView;
        }

        private frmCommMessageFilter localCreateMessageViewWin(PortManager target)
        {
            if ((target.MessageView != null) && target.MessageViewLocation.IsOpen)
            {
                target.MessageView.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": Message View";
                if ((target.MessageView == null) || target.MessageView.IsDisposed)
                {
                    target.MessageView = new frmCommMessageFilter();
                    if (target.MessageViewLocation.Width != 0)
                    {
                        target.MessageView.Width = target.MessageViewLocation.Width;
                        target.MessageView.WinWidth = target.MessageViewLocation.Width;
                    }
                    if (target.MessageViewLocation.Height != 0)
                    {
                        target.MessageView.Height = target.MessageViewLocation.Height;
                        target.MessageView.WinHeight = target.MessageViewLocation.Height;
                    }
                    if (target.MessageViewLocation.Left != 0)
                    {
                        target.MessageView.Left = target.MessageViewLocation.Left;
                        target.MessageView.WinLeft = target.MessageViewLocation.Left;
                    }
                    if (target.MessageViewLocation.Top != 0)
                    {
                        target.MessageView.Top = target.MessageViewLocation.Top;
                        target.MessageView.WinTop = target.MessageViewLocation.Top;
                    }
                    target.MessageView.Show();
                    target.MessageViewLocation.IsOpen = true;
                }
                target.MessageView.CommWindow = target.comm;
                target.MessageView.Text = str;
                target.MessageView.UpdatePortManager += new frmCommMessageFilter.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target.MessageView.BringToFront();
            }
            return target.MessageView;
        }

        private frmCommNavAccVsTime localCreateNavVsTimeWin(PortManager target)
        {
            if ((target.NavVsTimeView != null) && !target.NavVsTimeView.IsDisposed)
            {
                target.NavVsTimeView.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": Nav Accuracy vs Time ";
                if ((target.NavVsTimeView == null) || target.NavVsTimeView.IsDisposed)
                {
                    target.NavVsTimeView = new frmCommNavAccVsTime(target.comm);
                    if (target.NavVsTimeLocation.Width != 0)
                    {
                        target.NavVsTimeView.Width = target.NavVsTimeLocation.Width;
                        target.NavVsTimeView.WinWidth = target.NavVsTimeLocation.Width;
                    }
                    if (target.NavVsTimeLocation.Height != 0)
                    {
                        target.NavVsTimeView.Height = target.NavVsTimeLocation.Height;
                        target.NavVsTimeView.WinHeight = target.NavVsTimeLocation.Height;
                    }
                    if (target.NavVsTimeLocation.Left != 0)
                    {
                        target.NavVsTimeView.Left = target.NavVsTimeLocation.Left;
                        target.NavVsTimeView.WinLeft = target.NavVsTimeLocation.Left;
                    }
                    if (target.NavVsTimeLocation.Top != 0)
                    {
                        target.NavVsTimeView.Top = target.NavVsTimeLocation.Top;
                        target.NavVsTimeView.WinTop = target.NavVsTimeLocation.Top;
                    }
                    target.NavVsTimeView.Show();
                    target.NavVsTimeLocation.IsOpen = true;
                }
                target.NavVsTimeView.CommWindow = target.comm;
                target.NavVsTimeView.Text = str;
                target.NavVsTimeView.UpdatePortManager += new frmCommNavAccVsTime.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target.NavVsTimeView.BringToFront();
            }
            return target.NavVsTimeView;
        }

        private frmCommRadarMap localCreateRadarViewWin(PortManager target)
        {
            if ((target._svsMapPanel != null) && !target._svsMapPanel.IsDisposed)
            {
                target._svsMapPanel.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": Radar View";
                if ((target._svsMapPanel == null) || target._svsMapPanel.IsDisposed)
                {
                    target._svsMapPanel = new frmCommRadarMap();
                    if (target.SVsMapLocation.Width != 0)
                    {
                        target._svsMapPanel.Width = target.SVsMapLocation.Width;
                        target._svsMapPanel.WinWidth = target.SVsMapLocation.Width;
                    }
                    if (target.SVsMapLocation.Height != 0)
                    {
                        target._svsMapPanel.Height = target.SVsMapLocation.Height;
                        target._svsMapPanel.WinHeight = target.SVsMapLocation.Height;
                    }
                    if (target.SVsMapLocation.Left != 0)
                    {
                        target._svsMapPanel.Left = target.SVsMapLocation.Left;
                        target._svsMapPanel.WinLeft = target.SVsMapLocation.Left;
                    }
                    if (target.SVsMapLocation.Top != 0)
                    {
                        target._svsMapPanel.Top = target.SVsMapLocation.Top;
                        target._svsMapPanel.WinTop = target.SVsMapLocation.Top;
                    }
                    target._svsMapPanel.Show();
                    target.SVsMapLocation.IsOpen = true;
                }
                target._svsMapPanel.CommWindow = target.comm;
                target._svsMapPanel.Text = str;
                target._svsMapPanel.UpdatePortManager += new frmCommRadarMap.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target._svsMapPanel.BringToFront();
            }
            return target._svsMapPanel;
        }

        private frmCommResponseView localCreateResponseViewWin(PortManager target)
        {
            if ((target._responseView != null) && !target._responseView.IsDisposed)
            {
                target._responseView.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": Response View";
                if ((target._responseView == null) || target._responseView.IsDisposed)
                {
                    target._responseView = new frmCommResponseView();
                    if (target.ResponseViewLocation.Width != 0)
                    {
                        target._responseView.Width = target.ResponseViewLocation.Width;
                        target._responseView.WinWidth = target.ResponseViewLocation.Width;
                    }
                    if (target.ResponseViewLocation.Height != 0)
                    {
                        target._responseView.Height = target.ResponseViewLocation.Height;
                        target._responseView.WinHeight = target.ResponseViewLocation.Height;
                    }
                    if (target.ResponseViewLocation.Left != 0)
                    {
                        target._responseView.Left = target.ResponseViewLocation.Left;
                        target._responseView.WinLeft = target.ResponseViewLocation.Left;
                    }
                    if (target.ResponseViewLocation.Top != 0)
                    {
                        target._responseView.Top = target.ResponseViewLocation.Top;
                        target._responseView.WinTop = target.ResponseViewLocation.Top;
                    }
                    target._responseView.Show();
                    target.ResponseViewLocation.IsOpen = true;
                }
                target._responseView.CommWindow = target.comm;
                target._responseView.Text = str;
                target._responseView.UpdatePortManager += new frmCommResponseView.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target._responseView.BringToFront();
            }
            return target._responseView;
        }

        private frmSatelliteStats localCreateSatelliteStatsWin(PortManager target)
        {
            string str = target.comm.sourceDeviceName + ": Satellite Statistics";
            if ((target._SatelliteStats == null) || target._SatelliteStats.IsDisposed)
            {
                target._SatelliteStats = new frmSatelliteStats(target.comm);
                if (target.SatelliteStatsLocation.Width != 0)
                {
                    target._SatelliteStats.Width = target.SatelliteStatsLocation.Width;
                    target._SatelliteStats.WinWidth = target.SatelliteStatsLocation.Width;
                }
                if (target.SatelliteStatsLocation.Height != 0)
                {
                    target._SatelliteStats.Height = target.SatelliteStatsLocation.Height;
                    target._SatelliteStats.WinHeight = target.SatelliteStatsLocation.Height;
                }
                if (target.SatelliteStatsLocation.Left != 0)
                {
                    target._SatelliteStats.Left = target.SatelliteStatsLocation.Left;
                    target._SatelliteStats.WinLeft = target.SatelliteStatsLocation.Left;
                }
                if (target.SatelliteStatsLocation.Top != 0)
                {
                    target._SatelliteStats.Top = target.SatelliteStatsLocation.Top;
                    target._SatelliteStats.WinTop = target.SatelliteStatsLocation.Top;
                }
                target._SatelliteStats.Show();
                target.SatelliteStatsLocation.IsOpen = true;
            }
            else if (target.SatelliteStatsLocation.IsOpen)
            {
                target._SatelliteStats.Close();
            }
            else
            {
                target._SatelliteStats.Show();
                target.SatelliteStatsLocation.IsOpen = true;
            }
            target._SatelliteStats.Text = str;
            target._SatelliteStats.UpdatePortManager += new frmSatelliteStats.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
            target._SatelliteStats.BringToFront();
            return target._SatelliteStats;
        }

        private frmCommSignalView localCreateSignalViewWin(PortManager target)
        {
            if ((target._signalStrengthPanel != null) && !target._signalStrengthPanel.IsDisposed)
            {
                target._signalStrengthPanel.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": Signal View";
                if ((target._signalStrengthPanel == null) || target._signalStrengthPanel.IsDisposed)
                {
                    target._signalStrengthPanel = new frmCommSignalView();
                    if (target.SignalViewLocation.Width != 0)
                    {
                        target._signalStrengthPanel.Width = target.SignalViewLocation.Width;
                        target._signalStrengthPanel.WinWidth = target.SignalViewLocation.Width;
                    }
                    if (target.SignalViewLocation.Height != 0)
                    {
                        target._signalStrengthPanel.Height = target.SignalViewLocation.Height;
                        target._signalStrengthPanel.WinHeight = target.SignalViewLocation.Height;
                    }
                    if (target.SignalViewLocation.Left != 0)
                    {
                        target._signalStrengthPanel.Left = target.SignalViewLocation.Left;
                        target._signalStrengthPanel.WinLeft = target.SignalViewLocation.Left;
                    }
                    if (target.SignalViewLocation.Top != 0)
                    {
                        target._signalStrengthPanel.Top = target.SignalViewLocation.Top;
                        target._signalStrengthPanel.WinTop = target.SignalViewLocation.Top;
                    }
                    target._signalStrengthPanel.Show();
                    target.SignalViewLocation.IsOpen = true;
                }
                target._signalStrengthPanel.CommWindow = target.comm;
                target._signalStrengthPanel.Text = str;
                target._signalStrengthPanel.UpdatePortManager += new frmCommSignalView.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target._signalStrengthPanel.BringToFront();
            }
            return target._signalStrengthPanel;
        }

        private frmCommSiRFawareV2 localCreateSiRFawareWin(PortManager target)
        {
            string str = target.comm.sourceDeviceName + ": SiRFaware";
            if ((target._SiRFAware == null) || target._SiRFAware.IsDisposed)
            {
                target._SiRFAware = new frmCommSiRFawareV2(target.comm);
                if (target.SiRFawareLocation.Width != 0)
                {
                    target._SiRFAware.Width = target.SiRFawareLocation.Width;
                    target._SiRFAware.WinWidth = target.SiRFawareLocation.Width;
                }
                if (target.SiRFawareLocation.Height != 0)
                {
                    target._SiRFAware.Height = target.SiRFawareLocation.Height;
                    target._SiRFAware.WinHeight = target.SiRFawareLocation.Height;
                }
                if (target.SiRFawareLocation.Left != 0)
                {
                    target._SiRFAware.Left = target.SiRFawareLocation.Left;
                    target._SiRFAware.WinLeft = target.SiRFawareLocation.Left;
                }
                if (target.SiRFawareLocation.Top != 0)
                {
                    target._SiRFAware.Top = target.SiRFawareLocation.Top;
                    target._SiRFAware.WinTop = target.SiRFawareLocation.Top;
                }
                target.SiRFawareLocation.IsOpen = true;
                target._SiRFAware.StartListen();
            }
            else if (target.SiRFawareLocation.IsOpen)
            {
                target._SiRFAware.Close();
            }
            else
            {
                target.SiRFawareLocation.IsOpen = true;
                target._SiRFAware.StartListen();
            }
            target._SiRFAware.Text = str;
            target._SiRFAware.UpdatePortManager += new frmCommSiRFawareV2.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
            target._SiRFAware.Show();
            target._SiRFAware.BringToFront();
            return target._SiRFAware;
        }

        private frmCommSVAvgCNo localCreateSVCNoWin(PortManager target)
        {
            if ((target._svCNoView != null) && !target._svCNoView.IsDisposed)
            {
                target._svCNoView.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": SV Average CNo View";
                if ((target._svCNoView == null) || target._svCNoView.IsDisposed)
                {
                    target._svCNoView = new frmCommSVAvgCNo();
                    if (target.SVCNoViewLocation.Width != 0)
                    {
                        target._svCNoView.Width = target.SVCNoViewLocation.Width;
                        target._svCNoView.WinWidth = target.SVCNoViewLocation.Width;
                    }
                    if (target.SVTrajViewLocation.Height != 0)
                    {
                        target._svCNoView.Height = target.SVCNoViewLocation.Height;
                        target._svCNoView.WinHeight = target.SVCNoViewLocation.Height;
                    }
                    if (target.SVTrajViewLocation.Left != 0)
                    {
                        target._svCNoView.Left = target.SVCNoViewLocation.Left;
                        target._svCNoView.WinLeft = target.SVCNoViewLocation.Left;
                    }
                    if (target.SVCNoViewLocation.Top != 0)
                    {
                        target._svCNoView.Top = target.SVCNoViewLocation.Top;
                        target._svCNoView.WinTop = target.SVCNoViewLocation.Top;
                    }
                    target._svCNoView.Show();
                    target.SVCNoViewLocation.IsOpen = true;
                }
                target._svCNoView.CommWindow = target.comm;
                target._svCNoView.Text = str;
                target._svCNoView.UpdatePortManager += new frmCommSVAvgCNo.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target._svCNoView.BringToFront();
            }
            return target._svCNoView;
        }

        private frmCommSVTrackedVsTime localCreateSVTrackedVsTimeWin(PortManager target)
        {
            if ((target._svTrackedVsTimeView != null) && !target._svTrackedVsTimeView.IsDisposed)
            {
                target._svTrackedVsTimeView.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": SV Tracked vs Time View";
                if ((target._svTrackedVsTimeView == null) || target._svTrackedVsTimeView.IsDisposed)
                {
                    target._svTrackedVsTimeView = new frmCommSVTrackedVsTime();
                    target._svTrackedVsTimeView.CommWindow = target.comm;
                    if (target.SVTrackedVsTimeViewLocation.Width != 0)
                    {
                        target._svTrackedVsTimeView.Width = target.SVTrackedVsTimeViewLocation.Width;
                        target._svTrackedVsTimeView.WinWidth = target.SVTrackedVsTimeViewLocation.Width;
                    }
                    if (target.SVTrajViewLocation.Height != 0)
                    {
                        target._svTrackedVsTimeView.Height = target.SVTrackedVsTimeViewLocation.Height;
                        target._svTrackedVsTimeView.WinHeight = target.SVTrackedVsTimeViewLocation.Height;
                    }
                    if (target.SVTrajViewLocation.Left != 0)
                    {
                        target._svTrackedVsTimeView.Left = target.SVTrackedVsTimeViewLocation.Left;
                        target._svTrackedVsTimeView.WinLeft = target.SVTrackedVsTimeViewLocation.Left;
                    }
                    if (target.SVTrackedVsTimeViewLocation.Top != 0)
                    {
                        target._svTrackedVsTimeView.Top = target.SVTrackedVsTimeViewLocation.Top;
                        target._svTrackedVsTimeView.WinTop = target.SVTrackedVsTimeViewLocation.Top;
                    }
                    target._svTrackedVsTimeView.Show();
                    target.SVTrackedVsTimeViewLocation.IsOpen = true;
                }
                target._svTrackedVsTimeView.Text = str;
                target._svTrackedVsTimeView.UpdatePortManager += new frmCommSVTrackedVsTime.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target._svTrackedVsTimeView.BringToFront();
            }
            return target._svTrackedVsTimeView;
        }

        private frmCommSVTrajectory localCreateSVTrajWin(PortManager target)
        {
            if ((target._svTrajView != null) && !target._svTrajView.IsDisposed)
            {
                target._svTrajView.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": SV Trajectory View";
                if ((target._svTrajView == null) || target._svTrajView.IsDisposed)
                {
                    target._svTrajView = new frmCommSVTrajectory();
                    if (target.SVTrajViewLocation.Width != 0)
                    {
                        target._svTrajView.Width = target.SVTrajViewLocation.Width;
                        target._svTrajView.WinWidth = target.SVTrajViewLocation.Width;
                    }
                    if (target.SVTrajViewLocation.Height != 0)
                    {
                        target._svTrajView.Height = target.SVTrajViewLocation.Height;
                        target._svTrajView.WinHeight = target.SVTrajViewLocation.Height;
                    }
                    if (target.SVTrajViewLocation.Left != 0)
                    {
                        target._svTrajView.Left = target.SVTrajViewLocation.Left;
                        target._svTrajView.WinLeft = target.SVTrajViewLocation.Left;
                    }
                    if (target.SVTrajViewLocation.Top != 0)
                    {
                        target._svTrajView.Top = target.SVTrajViewLocation.Top;
                        target._svTrajView.WinTop = target.SVTrajViewLocation.Top;
                    }
                    target._svTrajView.Show();
                    target.SVTrajViewLocation.IsOpen = true;
                }
                target._svTrajView.CommWindow = target.comm;
                target._svTrajView.Text = str;
                target._svTrajView.UpdatePortManager += new frmCommSVTrajectory.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target._svTrajView.BringToFront();
            }
            return target._svTrajView;
        }

        private frmTTFFDisplay localCreateTTFFWin(PortManager target)
        {
            if ((target._ttffDisplay != null) && !target._ttffDisplay.IsDisposed)
            {
                target._ttffDisplay.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": TTFF/Nav Accuracy";
                if ((target._ttffDisplay == null) || target._ttffDisplay.IsDisposed)
                {
                    target._ttffDisplay = new frmTTFFDisplay(target.comm);
                    if (target.TTFFDisplayLocation.Width != 0)
                    {
                        target._ttffDisplay.Width = target.TTFFDisplayLocation.Width;
                        target._ttffDisplay.WinWidth = target.TTFFDisplayLocation.Width;
                    }
                    if (target.TTFFDisplayLocation.Height != 0)
                    {
                        target._ttffDisplay.Height = target.TTFFDisplayLocation.Height;
                        target._ttffDisplay.WinHeight = target.TTFFDisplayLocation.Height;
                    }
                    if (target.TTFFDisplayLocation.Left != 0)
                    {
                        target._ttffDisplay.Left = target.TTFFDisplayLocation.Left;
                        target._ttffDisplay.WinLeft = target.TTFFDisplayLocation.Left;
                    }
                    if (target.TTFFDisplayLocation.Top != 0)
                    {
                        target._ttffDisplay.Top = target.TTFFDisplayLocation.Top;
                        target._ttffDisplay.WinTop = target.TTFFDisplayLocation.Top;
                    }
                    target._ttffDisplay.Show();
                    target.TTFFDisplayLocation.IsOpen = true;
                }
                target._ttffDisplay.Text = str;
                target._ttffDisplay.UpdatePortManager += new frmTTFFDisplay.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target._ttffDisplay.BringToFront();
            }
            return target._ttffDisplay;
        }

        private frmTTFSView localCreateTTFSWin(PortManager target)
        {
            if ((target.TTFSView != null) && !target.TTFSView.IsDisposed)
            {
                target.TTFSView.Close();
            }
            else
            {
                string str = target.comm.sourceDeviceName + ": Time To First Sync";
                if ((target.TTFSView == null) || target.TTFSView.IsDisposed)
                {
                    target.TTFSView = new frmTTFSView();
                    if (target.TTFSViewLocation.Width != 0)
                    {
                        target.TTFSView.Width = target.TTFSViewLocation.Width;
                        target.TTFSView.WinWidth = target.TTFSViewLocation.Width;
                    }
                    if (target.TTFSViewLocation.Height != 0)
                    {
                        target.TTFSView.Height = target.TTFSViewLocation.Height;
                        target.TTFSView.WinHeight = target.TTFSViewLocation.Height;
                    }
                    if (target.TTFSViewLocation.Left != 0)
                    {
                        target.TTFSView.Left = target.TTFSViewLocation.Left;
                        target.TTFSView.WinLeft = target.TTFSViewLocation.Left;
                    }
                    if (target.TTFSViewLocation.Top != 0)
                    {
                        target.TTFSView.Top = target.TTFSViewLocation.Top;
                        target.TTFSView.WinTop = target.TTFSViewLocation.Top;
                    }
                    target.TTFSView.Show();
                    target.TTFSViewLocation.IsOpen = true;
                }
                target.TTFSView.Text = str;
                target.TTFSView.UpdatePortManager += new frmTTFSView.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                target.TTFSView.BringToFront();
            }
            return target.TTFSView;
        }

        private void localEnableDisableDRMenus(bool state)
        {
            if (!clsGlobal.IsMarketingUser())
            {
                siRFDRiveStatusToolStripMenuItem.Enabled = state;
                siRFDRiveSensorToolStripMenuItem.Enabled = state;
                siRFDRiveToolStripMenuItem.Enabled = state;
            }
        }

        private void localEnableDisableMenuAndButton(bool state)
        {
            viewToolStripMenuItem.Enabled = state;
            commandToolStripMenuItem.Enabled = state;
            autoTestToolStripMenuItem.Enabled = state;
            setReferenceLocationToolStripMenuItem.Enabled = state;
            toolStripSaveBtn.Enabled = state;
            toolStripResetBtn.Enabled = state;
            toolStripMessageViewBtn.Enabled = state;
            toolStripPauseBtn.Enabled = state;
            toolStripUserTextBtn.Enabled = state;
            logFileToolStripMenuItem.Enabled = state;
            navigationToolStripMenuItem.Enabled = state;
            plotsToolStripMenuItem.Enabled = state;
            featuresToolStripMenuItem.Enabled = state;
            aidingToolStripMenuItem.Enabled = state;
            tTFSToolStripMenuItem.Enabled = state;
            localEnableDisableDRMenus(state);
        }

        private void localEnableDisableMenuAndButtonForFilePlayback(bool state)
        {
            viewToolStripMenuItem.Enabled = state;
            commandToolStripMenuItem.Enabled = state;
            setReferenceLocationToolStripMenuItem.Enabled = state;
            toolStripSignalViewBtn.Enabled = state;
            toolStripRadarViewBtn.Enabled = state;
            toolStripMapViewBtn.Enabled = state;
            toolStripTTFFViewBtn.Enabled = state;
            toolStripResponseViewBtn.Enabled = state;
            toolStripDebugViewBtn.Enabled = state;
            toolStripErrorViewBtn.Enabled = state;
            toolStripMessageViewBtn.Enabled = state;
        }

        private void localEnableDisableMenuAndButtonPerProductType(bool state)
        {
            set5HzNavToolStripMenuItem.Enabled = state;
            enable5HzNavToolStripMenuItem.Enabled = state;
            disable5HzNavToolStripMenuItem.Enabled = state;
            setABPToolStripMenuItem.Enabled = state;
            enableABPToolStripMenuItem.Enabled = state;
            disableABPToolStripMenuItem.Enabled = state;
        }

        private void localEnableDisableMenuAndButtonPerProtocol(bool state)
        {
            toolStripTTFFViewBtn.Enabled = state;
            mEMSViewToolStripMenuItem.Enabled = state;
            compassToolStripMenuItem.Enabled = state;
            altitudeMeterToolStripMenuItem.Enabled = state;
            satellitesStatisticsToolStripMenuItem.Enabled = state;
            siRFDRiveStatusToolStripMenuItem.Enabled = state;
            siRFDRiveSensorToolStripMenuItem.Enabled = state;
            receiverViewCWDetectionToolStripMenuItem.Enabled = state;
            receiverViewSiRFawareToolStripMenuItem.Enabled = state;
            navigationToolStripMenuItem.Enabled = true;
            staticNavToolStripMenuItem.Enabled = state;
            set5HzNavToolStripMenuItem.Enabled = true;
            dOPMaskToolStripMenuItem.Enabled = state;
            elevationMaskToolStripMenuItem.Enabled = state;
            modeMaskToolStripMenuItem.Enabled = state;
            powerMaskToolStripMenuItem.Enabled = state;
            sBASRangingToolStripMenuItem.Enabled = state;
            enableSBASRangingToolStripMenuItem.Enabled = state;
            disableSBASRangingToolStripMenuItem.Enabled = state;
            featuresToolStripMenuItem.Enabled = state;
            aidingToolStripMenuItem.Enabled = state;
            autoTestToolStripMenuItem.Enabled = state;
            tTFFAndNavAccuracyToolStripMenuItem.Enabled = state;
            lowPowerCommandsBufferToolStripMenuItem.Enabled = state;
            messageToolStripMenuItem.Enabled = state;
            toolStripMessageViewBtn.Enabled = state;
            localEnableDisableDRMenus(state);
            commandToolStripMenuItem.Enabled = true;
            pollSoftwareVesrionToolStripMenuItem.Enabled = state;
            pollNavParametersToolStripMenuItem.Enabled = state;
            pollAlmanacToolStripMenuItem.Enabled = state;
            pollEphemerisToolStripMenuItem.Enabled = state;
            switchOperationModeToolStripMenuItem.Enabled = state;
            switchPowerModeToolStripMenuItem.Enabled = state;
            switchProtocolsToolStripMenuItem.Enabled = true;
            setAlmanacToolStripMenuItem.Enabled = state;
            setEphemerisToolStripMenuItem.Enabled = state;
            setEEToolStripMenuItem.Enabled = state;
            setDebugLevelsToolStripMenuItem.Enabled = state;
            setDGPSToolStripMenuItem.Enabled = state;
            setMEMSToolStripMenuItem.Enabled = state;
            enableMEMSToolStripMenuItem.Enabled = state;
            disableMEMSToolStripMenuItem.Enabled = state;
            setABPToolStripMenuItem.Enabled = state;
            enableABPToolStripMenuItem.Enabled = state;
            disableABPToolStripMenuItem.Enabled = state;
            iCConfigureToolStripMenuItem.Enabled = state;
            iCPeekPokeToolStripMenuItem.Enabled = state;
            plotsToolStripMenuItem.Enabled = state;
            averageCNoToolStripMenuItem.Enabled = state;
            navAccuracyVsTimeToolStripMenuItem.Enabled = state;
            sVTrajectoryToolStripMenuItem.Enabled = state;
            sVTrackedVsTimeToolStripMenuItem.Enabled = state;
            autoTestToolStripMenuItem.Enabled = state;
            autoTestLoopitToolStripMenuItem.Enabled = state;
            autoTestStandardTestsToolStripMenuItem.Enabled = state;
            autoTest3GPPToolStripMenuItem.Enabled = state;
            autoTestAdvancedTestsToolStripMenuItem.Enabled = state;
            autoTestStatusToolStripMenuItem.Enabled = state;
            autoTestAbortToolStripMenuItem.Enabled = state;
            consoleToolStripMenuItem.Enabled = state;
            inputCommandsToolStripMenuItem.Enabled = true;
            if (clsGlobal.IsMarketingUser())
            {
                predefinedToolStripMenuItem.Enabled = false;
                autoTestAdvancedTestsToolStripMenuItem.Visible = false;
            }
            else
            {
                predefinedToolStripMenuItem.Enabled = true;
            }
            userDefinedToolStripMenuItem.Enabled = true;
            tTFSToolStripMenuItem.Enabled = state;
        }

        private void localMenuBtnInit()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                enableDisableMenuAndButton(false);
            }
            else if (toolStripPortComboBox.Text != "All")
            {
                EventHandler method = null;
                EventHandler handler2 = null;
                EventHandler handler3 = null;
                EventHandler handler4 = null;
                PortManager tmpP = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if (tmpP != null)
                {
                    if (tmpP.comm != null)
                    {
                        if (!tmpP.comm.IsSourceDeviceOpen())
                        {
                            enableDisableMenuAndButton(false);
                        }
                        else
                        {
                            enableDisableMenuAndButton(true);
                            if (tmpP.comm.MessageProtocol != "NMEA")
                            {
                                if ((tmpP.comm.MessageProtocol == "OSP") || (tmpP.comm.MessageProtocol == "SSB"))
                                {
                                    enableDisableMenuAndButtonPerProtocol(true);
                                }
                            }
                            else
                            {
                                enableDisableMenuAndButtonPerProtocol(false);
                                if (tmpP.TTFFDisplayLocation.IsOpen && (tmpP._ttffDisplay != null))
                                {
                                    if (tmpP._ttffDisplay.InvokeRequired)
                                    {
                                        if (method == null)
                                        {
                                            method = delegate {
                                                tmpP._ttffDisplay.Close();
                                            };
                                        }
                                        tmpP._ttffDisplay.BeginInvoke(method);
                                    }
                                    else
                                    {
                                        tmpP._ttffDisplay.Close();
                                    }
                                }
                                if (tmpP.MessageViewLocation.IsOpen && (tmpP._ttffDisplay != null))
                                {
                                    if (tmpP.MessageView.InvokeRequired)
                                    {
                                        if (handler2 == null)
                                        {
                                            handler2 = delegate {
                                                tmpP.MessageView.Close();
                                            };
                                        }
                                        tmpP.MessageView.BeginInvoke(handler2);
                                    }
                                    else
                                    {
                                        tmpP.MessageView.Close();
                                    }
                                }
                                if (tmpP.SignalViewLocation.IsOpen && (tmpP._SiRFAware != null))
                                {
                                    if (tmpP._SiRFAware.InvokeRequired)
                                    {
                                        if (handler3 == null)
                                        {
                                            handler3 = delegate {
                                                tmpP._SiRFAware.Close();
                                            };
                                        }
                                        tmpP._SiRFAware.BeginInvoke(handler3);
                                    }
                                    else
                                    {
                                        tmpP._SiRFAware.Close();
                                    }
                                }
                                if (tmpP.TTFSViewLocation.IsOpen && (tmpP.TTFSView != null))
                                {
                                    if (tmpP.TTFSView.InvokeRequired)
                                    {
                                        if (handler4 == null)
                                        {
                                            handler4 = delegate {
                                                tmpP.TTFSView.Close();
                                            };
                                        }
                                        tmpP.TTFSView.BeginInvoke(handler4);
                                    }
                                    else
                                    {
                                        tmpP.TTFSView.Close();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    enableDisableMenuAndButton(false);
                }
            }
        }

        private void localSetMEMSModeCheck(CommunicationManager comm)
        {
            if (comm.MEMSModeToSet || (comm.dataGui.MEMS_State != -1))
            {
                disableMEMSToolStripMenuItem.Checked = false;
                enableMEMSToolStripMenuItem.Checked = true;
            }
            else
            {
                disableMEMSToolStripMenuItem.Checked = true;
                enableMEMSToolStripMenuItem.Checked = false;
            }
        }

        private void localSetToolStripPortText(string text)
        {
            toolStripPortComboBox.Text = text;
        }

        private void localUpdateAvgCNoViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateAvgCNoViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateAvgCNoViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.SVCNoViewLocation.IsOpen)
                    {
                        updateAvgCNoViewImage(true);
                        return;
                    }
                }
                updateAvgCNoViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateAvgCNoViewImage(manager2.SVCNoViewLocation.IsOpen);
                }
            }
        }

        private void localUpdateAvgCNoViewImage(bool state)
        {
            if (state)
            {
                averageCNoToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                averageCNoToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateCompassViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateCompassViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateCompassViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.CompassViewLocation.IsOpen)
                    {
                        updateCompassViewImage(true);
                        return;
                    }
                }
                updateCompassViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateCompassViewImage(manager2.CompassViewLocation.IsOpen);
                }
            }
        }

        private void localUpdateCompassViewImage(bool state)
        {
            if (state)
            {
                toolStripCompassViewBtn.CheckState = CheckState.Checked;
                compassToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripCompassViewBtn.CheckState = CheckState.Unchecked;
                compassToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateConnectBtnImage(CommunicationManager comm)
        {
            bool flag = false;
            if (comm != null)
            {
                if (toolStripPortComboBox.Text != comm.PortName)
                {
                    if (toolStripPortComboBox.Text == "All")
                    {
                        bool flag2 = false;
                        foreach (string str in PortManagerHash.Keys)
                        {
                            PortManager manager = (PortManager) PortManagerHash[str];
                            if (((manager != null) && (manager.comm != null)) && manager.comm.IsSourceDeviceOpen())
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        if (flag2)
                        {
                            toolStripPortOpenBtn.Image = Resources.connect;
                            toolStripPortOpenBtn.Text = "Disconnect";
                            ChangeRestoreLayoutState(true);
                            return;
                        }
                        toolStripPortOpenBtn.Image = Resources.disconnect;
                        toolStripPortOpenBtn.Text = "Connect";
                        ChangeRestoreLayoutState(false);
                    }
                    return;
                }
                flag = comm.IsSourceDeviceOpen();
            }
            if (flag)
            {
                flag = true;
                toolStripPortOpenBtn.Image = Resources.connect;
                toolStripPortOpenBtn.Text = "Disconnect";
                receiverConnectToolStripMenuItem.Enabled = false;
                receiverDisconnectToolStripMenuItem.Enabled = true;
            }
            else
            {
                toolStripPortOpenBtn.Image = Resources.disconnect;
                toolStripPortOpenBtn.Text = "Connect";
                receiverConnectToolStripMenuItem.Enabled = true;
                receiverDisconnectToolStripMenuItem.Enabled = false;
            }
        }

        private void localUpdateDebugViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateDebugViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateDebugViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.DebugViewLocation.IsOpen)
                    {
                        updateDebugViewImage(true);
                        return;
                    }
                }
                updateDebugViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateDebugViewImage(manager2.DebugViewLocation.IsOpen);
                }
            }
        }

        private void localUpdateDebugViewImage(bool state)
        {
            if (state)
            {
                toolStripDebugViewBtn.CheckState = CheckState.Checked;
                debugViewToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripDebugViewBtn.CheckState = CheckState.Unchecked;
                debugViewToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateDisableMEMSViewImage(bool state)
        {
            if (state)
            {
                disableMEMSToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                disableMEMSToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateDisableSBASRangingViewImage(bool state)
        {
            if (state)
            {
                disableSBASRangingToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                disableSBASRangingToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateDRSensorDataViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateDRSensorDataViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateDRSensorDataViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.LocationDRSensorLocation.IsOpen)
                    {
                        updateDRSensorDataViewImage(true);
                        return;
                    }
                }
                updateDRSensorDataViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateDRSensorDataViewImage(manager2.LocationDRSensorLocation.IsOpen);
                }
            }
        }

        private void localUpdateDRSensorDataViewImage(bool state)
        {
            if (state)
            {
                toolStripMapViewBtn.CheckState = CheckState.Checked;
                mapToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripMapViewBtn.CheckState = CheckState.Unchecked;
                mapToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateDRStatusViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateDRStatusViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateDRStatusViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.LocationDRStatusLocation.IsOpen)
                    {
                        updateDRStatusViewImage(true);
                        return;
                    }
                }
                updateDRStatusViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateDRStatusViewImage(manager2.LocationDRStatusLocation.IsOpen);
                }
            }
        }

        private void localUpdateDRStatusViewImage(bool state)
        {
            if (state)
            {
                toolStripMapViewBtn.CheckState = CheckState.Checked;
                mapToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripMapViewBtn.CheckState = CheckState.Unchecked;
                mapToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateEnableMEMSViewImage(bool state)
        {
            if (state)
            {
                enableMEMSToolStripMenuItem.CheckState = CheckState.Checked;
                disableMEMSToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
            else
            {
                enableMEMSToolStripMenuItem.CheckState = CheckState.Unchecked;
                disableMEMSToolStripMenuItem.CheckState = CheckState.Checked;
            }
        }

        private void localUpdateEnableSBASRangingViewImage(bool state)
        {
            if (state)
            {
                enableSBASRangingToolStripMenuItem.CheckState = CheckState.Checked;
                disableSBASRangingToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
            else
            {
                enableSBASRangingToolStripMenuItem.CheckState = CheckState.Unchecked;
                disableSBASRangingToolStripMenuItem.CheckState = CheckState.Checked;
            }
        }

        private void localUpdateErrorViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateErrorViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateErrorViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.ErrorViewLocation.IsOpen)
                    {
                        updateErrorViewImage(true);
                        return;
                    }
                }
                updateErrorViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateErrorViewImage(manager2.ErrorViewLocation.IsOpen);
                }
            }
        }

        private void localUpdateErrorViewImage(bool state)
        {
            if (state)
            {
                toolStripErrorViewBtn.CheckState = CheckState.Checked;
                errorToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripErrorViewBtn.CheckState = CheckState.Unchecked;
                errorToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateFilePlaybackPauseBtn(bool state)
        {
            if (state)
            {
                toolStripPause.Image = Resources.unpause;
                toolStripPause.Text = "Continue";
            }
            else
            {
                toolStripPause.Image = Resources.Pause;
                toolStripPause.Text = "Pause";
            }
        }

        private void localUpdateInterferenceViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateInterferenceViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateInterferenceViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.InterferenceLocation.IsOpen)
                    {
                        updateInterferenceViewImage(true);
                        return;
                    }
                }
                updateInterferenceViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateInterferenceViewImage(manager2.InterferenceLocation.IsOpen);
                }
            }
        }

        private void localUpdateInterferenceViewImage(bool state)
        {
            if (state)
            {
                receiverViewCWDetectionToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                receiverViewCWDetectionToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateLocationMapViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateLocationMapViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateLocationMapViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.LocationMapLocation.IsOpen)
                    {
                        updateLocationMapViewImage(true);
                        return;
                    }
                }
                updateLocationMapViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateLocationMapViewImage(manager2.LocationMapLocation.IsOpen);
                }
            }
        }

        private void localUpdateLocationMapViewImage(bool state)
        {
            if (state)
            {
                toolStripMapViewBtn.CheckState = CheckState.Checked;
                mapToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripMapViewBtn.CheckState = CheckState.Unchecked;
                mapToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateLogFileBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateLogFileImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateLogFileImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && (manager.comm != null))
                    {
                        logManagerStatusLabel.Text = manager.comm.Log.LogDirectory;
                        if (manager.comm.Log.IsFileOpen())
                        {
                            updateLogFileImage(true);
                            return;
                        }
                    }
                }
                updateLogFileImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    logManagerStatusLabel.Text = manager2.comm.Log.filename;
                    updateLogFileImage(manager2.comm.Log.IsFileOpen());
                }
            }
        }

        private void localUpdateLogFileImage(bool state)
        {
            if (state)
            {
                toolStripSaveBtn.Image = Resources.stopLog;
                toolStripSaveBtn.CheckState = CheckState.Checked;
                toolStripLogStatusLabel.Text = " | Log: Logging...";
                logFileToolStripMenuItem.Image = Resources.stopLog;
                logFileToolStripMenuItem.CheckState = CheckState.Checked;
                startLogToolStripMenuItem.Enabled = false;
                stopLogToolStripMenuItem.Enabled = true;
            }
            else
            {
                toolStripSaveBtn.Image = Resources.log;
                toolStripSaveBtn.CheckState = CheckState.Unchecked;
                toolStripLogStatusLabel.Text = " | Log: idle...";
                logFileToolStripMenuItem.Image = Resources.log;
                logFileToolStripMenuItem.CheckState = CheckState.Unchecked;
                startLogToolStripMenuItem.Enabled = true;
                stopLogToolStripMenuItem.Enabled = false;
            }
        }

        private void localUpdateMEMSViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateMEMSViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateMEMSViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.MEMSLocation.IsOpen)
                    {
                        updateMEMSViewImage(true);
                        return;
                    }
                }
                updateMEMSViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateMEMSViewImage(manager2.MEMSLocation.IsOpen);
                }
            }
        }

        private void localUpdateMEMSViewImage(bool state)
        {
            if (state)
            {
                mEMSViewToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                mEMSViewToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateMessageViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateMessageViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateMessageViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.MessageViewLocation.IsOpen)
                    {
                        updateMessageViewImage(true);
                        return;
                    }
                }
                updateMessageViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateMessageViewImage(manager2.MessageViewLocation.IsOpen);
                }
            }
        }

        private void localUpdateMessageViewImage(bool state)
        {
            if (state)
            {
                toolStripMessageViewBtn.CheckState = CheckState.Checked;
                messageToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripMessageViewBtn.CheckState = CheckState.Unchecked;
                messageToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateNavVsTimeBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateNavVsTimeImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateNavVsTimeImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.NavVsTimeLocation.IsOpen)
                    {
                        updateNavVsTimeImage(true);
                        return;
                    }
                }
                updateNavVsTimeImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateNavVsTimeImage(manager2.NavVsTimeLocation.IsOpen);
                }
            }
        }

        private void localUpdateNavVsTimeImage(bool state)
        {
            if (state)
            {
                navAccuracyVsTimeToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                navAccuracyVsTimeToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdatePerPortNameComboBox(PortManager target)
        {
            ((ToolStripComboBox) target.PerPortToolStrip.Items[_toolStripPortNameComboBoxIdx]).Items.Add(target.comm.PortName);
            target.PerPortToolStrip.Items[_toolStripPortNameComboBoxIdx].Text = target.comm.PortName;
        }

        private void localUpdateResponseViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateResponseViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateResponseViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.ResponseViewLocation.IsOpen)
                    {
                        updateResponseViewImage(true);
                        return;
                    }
                }
                updateResponseViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateResponseViewImage(manager2.ResponseViewLocation.IsOpen);
                }
            }
        }

        private void localUpdateResponseViewImage(bool state)
        {
            if (state)
            {
                toolStripResponseViewBtn.CheckState = CheckState.Checked;
                responseViewToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripResponseViewBtn.CheckState = CheckState.Unchecked;
                responseViewToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateSatellitesStatsViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateSatellitesStatsViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateSatellitesStatsViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.SatelliteStatsLocation.IsOpen)
                    {
                        updateSatellitesStatsViewImage(true);
                        return;
                    }
                }
                updateSatellitesStatsViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateSatellitesStatsViewImage(manager2.SatelliteStatsLocation.IsOpen);
                }
            }
        }

        private void localUpdateSatellitesStatsViewImage(bool state)
        {
            if (state)
            {
                satellitesStatisticsToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                satellitesStatisticsToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateSignalViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateSignalViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateSignalViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.SignalViewLocation.IsOpen)
                    {
                        updateSignalViewImage(true);
                        return;
                    }
                }
                updateSignalViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateSignalViewImage(manager2.SignalViewLocation.IsOpen);
                }
            }
        }

        private void localUpdateSignalViewImage(bool state)
        {
            if (state)
            {
                toolStripSignalViewBtn.CheckState = CheckState.Checked;
                signalToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripSignalViewBtn.CheckState = CheckState.Unchecked;
                signalToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateSiRFawareViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateSiRFawareViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateSiRFawareViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.SiRFawareLocation.IsOpen)
                    {
                        updateSiRFawareViewImage(true);
                        return;
                    }
                }
                updateSiRFawareViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateSiRFawareViewImage(manager2.SiRFawareLocation.IsOpen);
                }
            }
        }

        private void localUpdateSiRFawareViewImage(bool state)
        {
            if (state)
            {
                receiverViewSiRFawareToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                receiverViewSiRFawareToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateStatusString(string portName)
        {
            CommunicationManager manager4;
            string statusStr = string.Empty;
            string str = string.Empty;
            string[] strArray = new string[] { "FC:None", "FC:Xon/Xoff", "FC:Hardware", "FC:Both" };
            if ((portName != null) && (portName != string.Empty))
            {
                if (portName == "All")
                {
                    foreach (string str2 in PortManagerHash.Keys)
                    {
                        PortManager manager = (PortManager) PortManagerHash[str2];
                        if (manager == null)
                        {
                            return;
                        }
                        CommunicationManager comm = manager.comm;
                        if ((comm.FlowControl < 0) || (comm.FlowControl > 3))
                        {
                            comm.FlowControl = 0;
                        }
                        switch (comm.RxCurrentTransmissionType)
                        {
                            case CommunicationManager.TransmissionType.Text:
                                str = "NMEA/Text";
                                break;

                            case CommunicationManager.TransmissionType.Hex:
                                str = "Hex";
                                break;

                            case CommunicationManager.TransmissionType.SSB:
                                str = "SSB";
                                break;

                            case CommunicationManager.TransmissionType.GP2:
                                str = "GP2";
                                break;

                            case CommunicationManager.TransmissionType.GPS:
                                str = "GPS";
                                break;

                            default:
                                str = string.Empty;
                                break;
                        }
                        if (comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
                        {
                            statusStr = statusStr + string.Format("{0}[{1}:{2}:{3}:{4}:{5}] | Protocol: {6} | View: {7} | -- ", new object[] { comm.PortName, comm.BaudRate, comm.Parity, comm.StopBits, comm.DataBits, strArray[comm.FlowControl], comm.MessageProtocol, str });
                        }
                        else if (comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client)
                        {
                            statusStr = statusStr + string.Format("TCP Client[{0}:{1}] | Protocol: {2} | View: {3} |-- ", new object[] { comm.CMC.HostAppClient.TCPClientHostName, comm.CMC.HostAppClient.TCPClientPortNum, comm.MessageProtocol, str });
                        }
                        else if (comm.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server)
                        {
                            statusStr = statusStr + string.Format("TCP Client[{0}:{1}] | Protocol:{2} | View: {3} | -- ", new object[] { comm.CMC.HostAppServer.TCPServerHostName, comm.CMC.HostAppServer.TCPServerPortNum, comm.MessageProtocol, str });
                        }
                        else if (comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
                        {
                            if (comm.CMC.HostAppI2CSlave.I2CTalkMode == CommMgrClass.I2CSlave.I2CCommMode.COMM_MODE_I2C_SLAVE)
                            {
                                statusStr = string.Format("I2C[{0}:{1}]Slave | Protocol:{2} | View: {3} ", new object[] { comm.CMC.HostAppI2CSlave.I2CDevicePortNumMaster, comm.CMC.HostAppI2CSlave.I2CMasterAddress, comm.MessageProtocol, str });
                            }
                            else
                            {
                                statusStr = string.Format("I2C[{0}:{1}]Multi-Master | Protocol:{2} | View: {3} ", new object[] { comm.CMC.HostAppI2CSlave.I2CDevicePortNum, comm.CMC.HostAppI2CSlave.I2CSlaveAddress, comm.MessageProtocol, str });
                            }
                        }
                    }
                    goto Label_065F;
                }
                PortManager manager3 = (PortManager) PortManagerHash[portName];
                if (manager3 != null)
                {
                    manager4 = manager3.comm;
                    if ((manager4.FlowControl < 0) || (manager4.FlowControl > 3))
                    {
                        manager4.FlowControl = 0;
                    }
                    switch (manager4.RxCurrentTransmissionType)
                    {
                        case CommunicationManager.TransmissionType.Text:
                            str = "NMEA/Text";
                            goto Label_042F;

                        case CommunicationManager.TransmissionType.Hex:
                            str = "Hex";
                            goto Label_042F;

                        case CommunicationManager.TransmissionType.SSB:
                            str = "SSB";
                            goto Label_042F;

                        case CommunicationManager.TransmissionType.GP2:
                            str = "GP2";
                            goto Label_042F;

                        case CommunicationManager.TransmissionType.GPS:
                            str = "GPS";
                            goto Label_042F;
                    }
                    str = string.Empty;
                    goto Label_042F;
                }
            }
            return;
        Label_042F:
            if (manager4.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
            {
                statusStr = string.Format("{0}[{1}:{2}:{3}:{4}:{5}] | Protocol: {6} | View: {7}", new object[] { manager4.PortName, manager4.BaudRate, manager4.Parity, manager4.StopBits, manager4.DataBits, strArray[manager4.FlowControl], manager4.MessageProtocol, str });
            }
            else if (manager4.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Client)
            {
                statusStr = string.Format("TCP Client[{0}:{1}] | Protocol: {2} | View: {3}", new object[] { manager4.CMC.HostAppClient.TCPClientHostName, manager4.CMC.HostAppClient.TCPClientPortNum, manager4.MessageProtocol, str });
            }
            else if (manager4.InputDeviceMode == CommonClass.InputDeviceModes.TCP_Server)
            {
                statusStr = string.Format("TCP Client[{0}:{1}] | Protocol:{2} | View: {3}", new object[] { manager4.CMC.HostAppServer.TCPServerHostName, manager4.CMC.HostAppServer.TCPServerPortNum, manager4.MessageProtocol, str });
            }
            else if (manager4.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
            {
                if (manager4.CMC.HostAppI2CSlave.I2CTalkMode == CommMgrClass.I2CSlave.I2CCommMode.COMM_MODE_I2C_SLAVE)
                {
                    statusStr = string.Format("I2C[{0}:{1}]Slave | Protocol:{2} | View: {3} ", new object[] { manager4.CMC.HostAppI2CSlave.I2CDevicePortNumMaster, manager4.CMC.HostAppI2CSlave.I2CMasterAddress, manager4.MessageProtocol, str });
                }
                else
                {
                    statusStr = string.Format("I2C[{0}:{1}]Multi-Master | Protocol:{2} | View: {3} ", new object[] { manager4.CMC.HostAppI2CSlave.I2CDevicePortNum, manager4.CMC.HostAppI2CSlave.I2CSlaveAddress, manager4.MessageProtocol, str });
                }
            }
        Label_065F:
		base.BeginInvoke((MethodInvoker)delegate
		{
                toolStripStatusLabel.Text = statusStr;
            });
        }

        private void localUpdateSVsMapViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateSVsMapViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateSVsMapViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.SVsMapLocation.IsOpen)
                    {
                        updateSVsMapViewImage(true);
                        return;
                    }
                }
                updateSVsMapViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateSVsMapViewImage(manager2.SVsMapLocation.IsOpen);
                }
            }
        }

        private void localUpdateSVsMapViewImage(bool state)
        {
            if (state)
            {
                toolStripRadarViewBtn.CheckState = CheckState.Checked;
                radarToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripRadarViewBtn.CheckState = CheckState.Unchecked;
                radarToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateSVTrajViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateSVTrajViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateSVTrajViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.SVTrajViewLocation.IsOpen)
                    {
                        updateSVTrajViewImage(true);
                        return;
                    }
                }
                updateSVTrajViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateSVTrajViewImage(manager2.SVTrajViewLocation.IsOpen);
                }
            }
        }

        private void localUpdateSVTrajViewImage(bool state)
        {
            if (state)
            {
                sVTrajectoryToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                sVTrajectoryToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateToolStripPortComboBoxItems(bool setTextToAll)
        {
            if (PortManagerHash.Count > 1)
            {
                if (!toolStripPortComboBox.Items.Contains("All"))
                {
                    toolStripPortComboBox.Items.Add("All");
                }
                if (setTextToAll)
                {
                    toolStripPortComboBox.Text = "All";
                }
            }
        }

        private void localUpdateTrackedVsTimeViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateTrackedVsTimeViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateTrackedVsTimeViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.SVTrackedVsTimeViewLocation.IsOpen)
                    {
                        updateTrackedVsTimeViewImage(true);
                        return;
                    }
                }
                updateTrackedVsTimeViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateTrackedVsTimeViewImage(manager2.SVTrackedVsTimeViewLocation.IsOpen);
                }
            }
        }

        private void localUpdateTrackedVsTimeViewImage(bool state)
        {
            if (state)
            {
                sVTrackedVsTimeToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                sVTrackedVsTimeToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void localUpdateTTFFViewBtn()
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                updateTTFFViewImage(false);
            }
            else if (PortManagerHash.Count <= 0)
            {
                updateTTFFViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.TTFFDisplayLocation.IsOpen)
                    {
                        updateTTFFViewImage(true);
                        return;
                    }
                }
                updateTTFFViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateTTFFViewImage(manager2.TTFFDisplayLocation.IsOpen);
                }
            }
        }

        private void localUpdateTTFFViewImage(bool state)
        {
            if (state)
            {
                toolStripTTFFViewBtn.CheckState = CheckState.Checked;
                tTFFAndNavAccuracyToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                toolStripTTFFViewBtn.CheckState = CheckState.Unchecked;
                tTFFAndNavAccuracyToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void locationMapManualClick()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target._locationViewPanel = CreateLocationMapWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._locationViewPanel = CreateLocationMapWin(manager2);
                    }
                }
                updateLocationMapViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void locaUpdatePauseBtn(PortManager target)
        {
            if (!target.comm.DebugViewRTBDisplay.viewPause)
            {
                toolStripPauseBtn.Image = Resources.Pause;
                toolStripPauseBtn.Text = "Pause";
            }
            else
            {
                toolStripPauseBtn.Image = Resources.unpause;
                toolStripPauseBtn.Text = "Continue";
            }
        }

        private void logFileClickHandler()
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateLogFileWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateLogFileWin(ref target);
                    }
                }
                updateLogFileBtn();
            }
        }

        private void logFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logFileClickHandler();
        }

        private void logFileToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                updateLogFileBtn();
            }
        }

        private void lowPowerCommandsBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateLowPowerBufferWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateLowPowerBufferWin(target);
                    }
                }
            }
        }

        private void lowPowerModeClickHandler()
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    createLowPowerInputWindow();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        createLowPowerInputWindow(ref target);
                    }
                }
            }
        }

        private bool manualConnect(ref CommunicationManager comm)
        {
            if (comm == null)
            {
                return false;
            }
            try
            {
                bool flag2;
                if (clsGlobal.IsMarketingUser())
                {
                    comm.IsAutoDetectBaud = true;
                }
                if (comm.IsSourceDeviceOpen())
                {
                    goto Label_04E8;
                }
                if (!comm.RequireHostRun || (sysCmdExec.IsExistingWin(comm.HostSWFilePath) != 0))
                {
                    goto Label_02AC;
                }
                string argStr = string.Empty;
                switch (comm.InputDeviceMode)
                {
                    case CommonClass.InputDeviceModes.RS232:
                        argStr = @"-a\\.\\" + comm.HostPair1 + " -y\"" + comm.HostAppCfgFilePath + "\" -e\"" + comm.HostAppMEMSCfgPath + "\"";
                        break;

                    case CommonClass.InputDeviceModes.TCP_Client:
                        argStr = "-n" + comm.CMC.HostAppClient.TCPClientPortNum.ToString() + " -y\"" + comm.HostAppCfgFilePath + "\" -e\"" + comm.HostAppMEMSCfgPath + "\"";
                        break;

                    case CommonClass.InputDeviceModes.TCP_Server:
                        argStr = "-n" + comm.CMC.HostAppServer.TCPServerPortNum.ToString() + " -y\"" + comm.HostAppCfgFilePath + "\" -e\"" + comm.HostAppMEMSCfgPath + "\"";
                        break;
                }
                if (!comm.RequireEE)
                {
                    goto Label_02A4;
                }
                string eESelect = comm.EESelect;
                if (eESelect != null)
                {
                    if (!(eESelect == "CGEE"))
                    {
                        if (eESelect == "SGEE")
                        {
                            goto Label_01EB;
                        }
                        if (eESelect == "Mixed SGEE + CGEE")
                        {
                            goto Label_01F9;
                        }
                    }
                    else
                    {
                        argStr = argStr + " -mode \"ff4_cgee_only\"";
                    }
                }
                goto Label_0205;
            Label_01EB:
                argStr = argStr + " -mode \"ff4_sgee_only\"";
                goto Label_0205;
            Label_01F9:
                argStr = argStr + " -mode \"ff4_mixed_mode\"";
            Label_0205:;
                argStr = argStr + " -s\"" + comm.ServerName + "\" -d\"/diff/packedDifference.f2p" + comm.EEDayNum + "enc.ee\" -j\"" + comm.AuthenticationCode + "\" -k" + comm.ServerPort;
                if ((comm.EESelect == "CGEE") || (comm.EESelect == "Mixed SGEE + CGEE"))
                {
                    argStr = argStr + " -b" + comm.BankTime;
                }
            Label_02A4:
                RunHostAppFromMain(argStr, ref comm);
            Label_02AC:
                flag2 = false;
                if (comm.ProductFamily == CommonClass.ProductType.GSD4e)
                {
                    if (comm.InputDeviceMode == CommonClass.InputDeviceModes.RS232)
                    {
                        if ((comm.ResetPort != string.Empty) && (comm.ResetPort != "-1"))
                        {
                            string s = comm.ResetPort.Replace("COM", "");
                            int result = 0;
                            if (int.TryParse(s, out result))
                            {
                                comm.Init4eMPMWakeupPort(result);
                                comm.Toggle4eWakeupPort();
                            }
                        }
                        if (comm.IsAutoDetectBaud)
                        {
                            if (searchforProtocolAndBaud(ref comm) > 0)
                            {
                                flag2 = true;
                            }
                            else
                            {
                                flag2 = false;
                            }
                        }
                        else
                        {
                            comm.AutoDetectProtocolAndBaudDone = true;
                            flag2 = comm.OpenPort();
                        }
                    }
                    else if (comm.InputDeviceMode == CommonClass.InputDeviceModes.I2C)
                    {
                        if (searchforProtocol_I2C(ref comm) > 0)
                        {
                            flag2 = true;
                        }
                        else
                        {
                            flag2 = false;
                        }
                    }
                    else
                    {
                        flag2 = true;
                    }
                    if (!flag2)
                    {
                        comm.ConnectErrorString = "Autodetect baud failed!";
                        return false;
                    }
                    if (comm.MessageProtocol == "OSP")
                    {
                        comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.GPS;
                        comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.GPS;
                    }
                    else if (comm.MessageProtocol == "NMEA")
                    {
                        comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Text;
                        comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.Text;
                    }
                    else
                    {
                        comm.RxCurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
                        comm.CMC.RxCurrentTransmissionType = CommonClass.TransmissionType.Hex;
                    }
                }
                else
                {
                    comm.AutoDetectProtocolAndBaudDone = true;
                    if (!comm.OpenPort())
                    {
                        comm.ConnectErrorString = "Unable to open port!";
                        return false;
                    }
                }
                if (comm.RxType == CommunicationManager.ReceiverType.TTB)
                {
                    comm.WriteData("A0A2 0009 CCA6 0002 0100 0000 00 8175 B0B3");
                    comm.WriteData("A0A2 0009 CCA6 0004 0100 0000 00 8177 B0B3");
                    comm.WriteData("A0A2 0009 CCA6 0029 0100 0000 00 819C B0B3");
                }
                if (comm.RequireHostRun && (sysCmdExec.IsExistingWin(comm.HostSWFilePath) == 1))
                {
                    sysCmdExec.SetWinSize(comm.HostSWFilePath, 2);
                }
                if (comm.RequireHostRun)
                {
                    comm.RxCtrl.SetMEMSMode(0);
                }
                if (comm.MessageProtocol != "NMEA")
                {
                    comm.RxCtrl.PollClockStatus();
                    comm.RxCtrl.PollSWVersion();
                    comm.RxCtrl.PollNavigationParameters();
                }
                if (comm.RxName == string.Empty)
                {
                    comm.RxName = "SiRF_EVK";
                }
                goto Label_0515;
            Label_04E8:
                manualDisconnect(ref comm);
            }
            catch (Exception exception)
            {
                if (comm != null)
                {
                    comm.ConnectErrorString = "Error: " + exception.ToString();
                }
                return false;
            }
        Label_0515:
            return true;
        }

        private bool manualDisconnect(ref CommunicationManager comm)
        {
            try
            {
                if ((((comm != null) && (comm.RxCtrl != null)) && (comm.RxCtrl.ResetCtrl != null)) && (comm.RxCtrl.ResetCtrl.LoopitInprogress || !clsGlobal.ScriptDone))
                {
                    MessageBox.Show("Test in progress -- Abort test before proceeding", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return false;
                }
                DisconnectPort(ref comm);
            }
            catch (Exception exception)
            {
                if (comm != null)
                {
                    comm.ConnectErrorString = "Error:" + exception.Message;
                }
                return true;
            }
            return true;
        }

        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            locationMapManualClick();
        }

        private void memoryCleanUpMenu_Click(object sender, EventArgs e)
        {
            if (base.WindowState != FormWindowState.Minimized)
            {
                base.WindowState = FormWindowState.Minimized;
            }
            Thread.Sleep(0x3e8);
            base.WindowState = FormWindowState.Maximized;
        }

        private void MEMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memsViewClickHandler();
        }

        private void memsViewClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            if (!target.MEMSLocation.IsOpen)
                            {
                                target._memsView = CreateMEMSViewWin(target);
                            }
                            if (!target.CompassViewLocation.IsOpen)
                            {
                                target._compassView = CreateCompassViewWin(target);
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        if (!manager2.MEMSLocation.IsOpen)
                        {
                            manager2._memsView = CreateMEMSViewWin(manager2);
                        }
                        if (!manager2.CompassViewLocation.IsOpen)
                        {
                            manager2._compassView = CreateCompassViewWin(manager2);
                        }
                    }
                }
                updateMEMSViewBtn();
                updateCompassViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void mEMSViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memsViewClickHandler();
        }

        private void menuBtnInit()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localMenuBtnInit();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localMenuBtnInit();
            }
        }

        private void messageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageViewManualClickHandler();
        }

        private void MessageViewManualClickHandler()
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target.MessageView = CreateMessageViewWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2.MessageView = CreateMessageViewWin(manager2);
                    }
                }
                updateMessageViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        public void MinMaxForm(bool isMin)
        {
        }

        private void modeMaskClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateModeMaskWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateModeMaskWin(ref target);
                    }
                }
            }
        }

        private void modeMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modeMaskClickHandler();
        }

        private void mPMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateMPMGenWin();
        }

        private void navAccuracyVsTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            navVsTimeManualClickHandler();
        }

        private void navigationModeControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateNavModeControlWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateNavModeControlWin(ref target);
                    }
                }
            }
        }

        private void navigationToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (toolStripPortComboBox.Text != string.Empty)
            {
                PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if (manager != null)
                {
                    if (manager.comm.ProductFamily == CommonClass.ProductType.GSD4e)
                    {
                        enableDisableMenuAndButtonPerProductType(true);
                        if (manager.comm.MessageProtocol == "NMEA")
                        {
                            enableDisableMenuAndButtonPerProtocol(false);
                        }
                        else
                        {
                            enableDisableMenuAndButtonPerProtocol(true);
                        }
                    }
                    else
                    {
                        enableDisableMenuAndButtonPerProductType(false);
                    }
                }
            }
        }

        private void navVsTimeManualClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target.NavVsTimeView = CreateNavVsTimeWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2.NavVsTimeView = CreateNavVsTimeWin(manager2);
                    }
                }
                updateNavVsTimeBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void NMEAtoGPStoolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CreateFileConversionWindow(ConversionType.NMEAToGPS);
        }

        private void OnGCTimerEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                if (!clsGlobal.ScriptDone || clsGlobal.LoopitInProgress)
                {
                    MinMaxForm(true);
                }
            }
            catch
            {
            }
        }

        private void OnSleepThreadTimerEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                if (sleepThreads.Count > 1)
                {
                    sleepThreads.RemoveAt(sleepThreads.Count - 1);
                }
            }
            catch
            {
            }
        }

        private void openHelpManual()
        {
            string url = @"..\Doc\Help\SiRFLiveUserManual.chm";
            Help.ShowHelp(this, url);
        }

        private void parseFile()
        {
            EventHandler method = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            EventHandler handler4 = null;
            EventHandler handler5 = null;
            EventHandler handler6 = null;
            EventHandler handler7 = null;
            CommonUtilsClass class2 = new CommonUtilsClass();
            string timeStamp = string.Empty;
            int millisecondsTimeout = 500;
            bool flag = false;
            PortManager manager = (PortManager) PortManagerHash[clsGlobal.FilePlayBackPortName];
            Thread.CurrentThread.CurrentCulture = clsGlobal.MyCulture;
        Label_004F:
            try
            {
                switch (_playState)
                {
                    case _playStates.IDLE:
                        millisecondsTimeout = 500;
                        _fileIndex = 0L;
                        if (method == null)
                        {
                            method = delegate {
                                filePlayBackTrackBar.Enabled = true;
                            };
                        }
                        filePlayBackTrackBar.Invoke(method);
                        goto Label_0B9D;

                    case _playStates.PLAY:
                        millisecondsTimeout = _speedDelay;
                        class2.viewPause = false;
                        if (handler3 == null)
                        {
                            handler3 = delegate {
                                filePlayBackTrackBar.Enabled = false;
                            };
                        }
                        filePlayBackTrackBar.Invoke(handler3);
                        if (_fileHdlr != null)
                        {
                            string input = _fileHdlr[_fileIndex];
                            try
                            {
                                string str7;
                                if (input == "EOF")
                                {
                                    if (MessageBox.Show("End of file reached! -- Rewind?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                    {
                                        _fileIndex = 0L;
                                        if (((manager != null) && (manager._SiRFAware != null)) && manager.SiRFawareLocation.IsOpen)
                                        {
                                            manager._SiRFAware.SirfawareGUIInit();
                                        }
                                    }
                                    else
                                    {
                                        _playState = _playStates.IDLE;
                                        if (handler4 == null)
                                        {
                                            handler4 = delegate {
                                                Text = string.Format("{0}: File Playback Stop", clsGlobal.SiRFLiveVersion);
                                            };
                                        }
                                        base.Invoke(handler4);
                                    }
                                    if (handler5 == null)
                                    {
                                        handler5 = delegate {
                                            filePlayBackTrackBar.Value = filePlayBackTrackBar.Maximum;
                                        };
                                    }
                                    filePlayBackTrackBar.Invoke(handler5);
                                    goto Label_0B9D;
                                }
                                string inputString = string.Empty;
                                CommonClass.MessageType normal = CommonClass.MessageType.Normal;
                                input = input.TrimEnd(new char[] { '\n' });
                                input = input.TrimEnd(new char[] { '\r' });
                                string pattern = @"\(1\)\tA0 A2 \w+ \w+ 80";
                                string str5 = @"\(1\)\tA0 A2 \w+ \w+ D6";
                                string str6 = @"(?<rt>\w+) \w+ \w+ B0 B3";
                                Regex regex = new Regex(pattern, RegexOptions.Compiled);
                                if (regex.IsMatch(input))
                                {
                                    manager.comm.RxCtrl.ResetCtrl.ResetDataInit();
                                    regex = new Regex(str6, RegexOptions.Compiled);
                                    if (regex.IsMatch(input))
                                    {
                                        try
                                        {
                                            byte maskByte = byte.Parse(regex.Match(input).Result("${rt}"), NumberStyles.HexNumber);
                                            manager.comm.RxCtrl.ResetCtrl.ResetType = SSB_Format.GetResetTypeFromBitMap(maskByte);
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    else
                                    {
                                        manager.comm.RxCtrl.ResetCtrl.ResetType = "Unknown";
                                    }
                                }
                                regex = new Regex(str5, RegexOptions.Compiled);
                                if (regex.IsMatch(input))
                                {
                                    manager.comm.AutoReplyCtrl.AutoReplyParams.AutoReply = true;
                                }
                                switch (_fileType)
                                {
                                    case CommonClass.TransmissionType.Text:
                                        inputString = input;
                                        normal = CommonClass.MessageType.Incoming;
                                        timeStamp = string.Empty;
                                        goto Label_0863;

                                    case CommonClass.TransmissionType.GP2:
                                    {
                                        input = input.ToUpper();
                                        if (!input.Contains("A0 A2"))
                                        {
                                            goto Label_03A9;
                                        }
                                        int index = input.IndexOf("A0 A2");
                                        int num4 = input.IndexOf("(");
                                        inputString = input.Substring(index);
                                        str7 = input.Substring(num4 + 1, 1);
                                        if (!(str7 == "0"))
                                        {
                                            break;
                                        }
                                        normal = CommonClass.MessageType.Incoming;
                                        goto Label_0863;
                                    }
                                    case CommonClass.TransmissionType.GPS:
                                    {
                                        inputString = input;
                                        normal = CommonClass.MessageType.Normal;
                                        input = input.Replace(" ", "");
                                        string messageProtocol = manager.comm.MessageProtocol;
                                        if (manager.comm.MessageProtocol == "OSP")
                                        {
                                            messageProtocol = "SSB";
                                        }
                                        if (input.StartsWith("2,"))
                                        {
                                            manager.comm.getSatellitesDataForGUIFromCSV(2, 0, messageProtocol, input);
                                        }
                                        else if (input.StartsWith("4,"))
                                        {
                                            manager.comm.getSatellitesDataForGUIFromCSV(4, 0, messageProtocol, input);
                                        }
                                        else if (input.StartsWith("41,"))
                                        {
                                            manager.comm.getSatellitesDataForGUIFromCSV(0x29, 0, messageProtocol, input);
                                        }
                                        else if (input.StartsWith("30,"))
                                        {
                                            manager.comm.getSatellitesDataForGUIFromCSV(30, 0, messageProtocol, input);
                                        }
                                        else
                                        {
                                            if (!input.StartsWith("225,"))
                                            {
                                                goto Label_05FD;
                                            }
                                            if (!input.StartsWith("225,00"))
                                            {
                                                goto Label_053A;
                                            }
                                            if (!clsGlobal.IsMarketingUser() && (input.Length > 1))
                                            {
                                                string str9 = string.Empty;
                                                string[] strArray = input.Split(new char[] { ',' });
                                                for (int i = 2; i < strArray.Length; i += 2)
                                                {
                                                    byte num6 = (byte) int.Parse(strArray[i], NumberStyles.HexNumber);
                                                    num6 = (byte) (num6 ^ 0xff);
                                                    str9 = str9 + Convert.ToChar(num6);
                                                }
                                                inputString = str9;
                                            }
                                        }
                                        goto Label_0863;
                                    }
                                    default:
                                        inputString = input;
                                        normal = CommonClass.MessageType.Normal;
                                        timeStamp = string.Empty;
                                        goto Label_0863;
                                }
                                switch (str7)
                                {
                                    case "1":
                                    case "2":
                                        normal = CommonClass.MessageType.Outgoing;
                                        break;
                                }
                                goto Label_0863;
                            Label_03A9:
                                inputString = input;
                                goto Label_0863;
                            Label_053A:
                                if (input.StartsWith("225,6"))
                                {
                                    manager.GetTTFFFromCSVString(input);
                                    if ((!manager.comm.AutoReplyCtrl.AutoReplyParams.AutoReply && (manager._ttffDisplay != null)) && manager.TTFFDisplayLocation.IsOpen)
                                    {
                                        manager._ttffDisplay.updateTTFFNow();
                                    }
                                }
                                else if (input.StartsWith("225,7"))
                                {
                                    manager.GetTTFFFromCSVString(input);
                                    if ((manager.comm.AutoReplyCtrl.AutoReplyParams.AutoReply && (manager._ttffDisplay != null)) && manager.TTFFDisplayLocation.IsOpen)
                                    {
                                        manager._ttffDisplay.updateTTFFNow();
                                    }
                                }
                                goto Label_0863;
                            Label_05FD:
                                if (input.StartsWith("69,"))
                                {
                                    manager.comm.AutoReplyCtrl.AutoReplyParams.AutoReply = true;
                                    if (input.StartsWith("69,1"))
                                    {
                                        manager.GetOSPPositionFormCSVString(input);
                                    }
                                }
                                else if (input.StartsWith("77,"))
                                {
                                    if ((manager._SiRFAware != null) && manager.SiRFawareLocation.IsOpen)
                                    {
                                        Hashtable siRFAwareScanResHash = null;
                                        if (input.StartsWith("77,1"))
                                        {
                                            siRFAwareScanResHash = manager.comm.getMPMStatsDataForGUIFromCSV(0x4d, 1, "OSP", input);
                                        }
                                        else if (input.StartsWith("77,2"))
                                        {
                                            siRFAwareScanResHash = manager.comm.getMPMStatsDataForGUIFromCSV(0x4d, 2, "OSP", input);
                                        }
                                        else if (input.StartsWith("77,3"))
                                        {
                                            siRFAwareScanResHash = manager.comm.getMPMStatsDataForGUIFromCSV(0x4d, 3, "OSP", input);
                                        }
                                        else if (input.StartsWith("77,4"))
                                        {
                                            siRFAwareScanResHash = manager.comm.getMPMStatsDataForGUIFromCSV(0x4d, 4, "OSP", input);
                                        }
                                        else if (input.StartsWith("77,5"))
                                        {
                                            siRFAwareScanResHash = manager.comm.getMPMStatsDataForGUIFromCSV(0x4d, 5, "OSP", input);
                                        }
                                        else if (input.StartsWith("77,7"))
                                        {
                                            siRFAwareScanResHash = manager.comm.getMPMStatsDataForGUIFromCSV(0x4d, 7, "OSP", input);
                                        }
                                        manager._SiRFAware.UpdateSiRFawareGUI(siRFAwareScanResHash);
                                    }
                                }
                                else if (input.Contains("TTFF(s)"))
                                {
                                    try
                                    {
                                        if (manager._SiRFAware != null)
                                        {
                                            string[] strArray2 = input.Split(new char[] { '=' });
                                            manager._SiRFAware.TTFF = double.Parse(strArray2[1]);
                                            if (manager.SiRFawareLocation.IsOpen)
                                            {
                                                manager._SiRFAware.UpdateSiRFawareGUITime();
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                else if (input.Contains("TotalTimeinSiRFaware(s)"))
                                {
                                    try
                                    {
                                        if (manager._SiRFAware != null)
                                        {
                                            string[] strArray3 = input.Split(new char[] { '=' });
                                            manager._SiRFAware.TotalMPMTime = double.Parse(strArray3[1]);
                                            if (manager.SiRFawareLocation.IsOpen)
                                            {
                                                manager._SiRFAware.UpdateSiRFawareGUITime();
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            Label_0863:
                                if (manager.comm.File_DataReceived(normal, inputString, manager.comm.DebugViewIsMatchEnable, manager.comm.DebugViewMatchString, timeStamp))
                                {
                                    _playState = _playStates.PAUSE;
                                    updateFilePlaybackPauseBtn(true);
                                }
                                _fileIndex = _fileHdlr.Index + 1L;
                                if (_totalFileSize != 0L)
                                {
                                    int processPercentage = (int) ((((double) _fileHdlr.Index) / ((double) _totalFileSize)) * 100.0);
									filePlayBackTrackBar.Invoke((MethodInvoker)delegate
									{
                                        filePlayBackTrackBar.Value = processPercentage;
                                    });
                                }
                                if (manager.comm._isEpochMessage)
                                {
                                    manager.comm._isEpochMessage = false;
                                    if (_lastPlayState != _playStates.BACKWARD)
                                    {
                                        if (_epochList.Count >= 100)
                                        {
                                            _epochList.RemoveAt(0);
                                        }
                                        if (!_epochList.Contains(_fileIndex - 1L))
                                        {
                                            _epochList.Add(_fileIndex - 1L);
                                            _epochIndex = _epochList.Count - 1;
                                        }
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                if (_playState != _playStates.QUIT)
                                {
                                    string errorStr = string.Format("Error: {0}\n{1}", exception.Message, input);
                                    if ((manager != null) && (manager.comm != null))
                                    {
                                        manager.comm.ErrorPrint(errorStr);
                                    }
                                    _fileIndex = _fileHdlr.Index + 1L;
                                }
                            }
                        }
                        goto Label_0B9D;

                    case _playStates.PAUSE:
                        millisecondsTimeout = 500;
                        if (handler2 == null)
                        {
                            handler2 = delegate {
                                filePlayBackTrackBar.Enabled = true;
                            };
                        }
                        filePlayBackTrackBar.Invoke(handler2);
                        goto Label_0B9D;

                    case _playStates.FORWARD:
                        if (_playState != _lastPlayState)
                        {
                            manager.comm.WriteApp("User marker: Go forward to next epoch");
                        }
                        if (handler6 == null)
                        {
                            handler6 = delegate {
                                filePlayBackTrackBar.Enabled = false;
                            };
                        }
                        filePlayBackTrackBar.Invoke(handler6);
                        manager.comm._isCheckEpoch = true;
                        _lastPlayState = _playState;
                        _playState = _playStates.PLAY;
                        _epochIndex++;
                        if (_epochIndex > (_epochList.Count - 1))
                        {
                            _epochIndex = _epochList.Count - 1;
                            if (_epochIndex < 0)
                            {
                                _epochIndex = 0;
                            }
                        }
                        goto Label_0B9D;

                    case _playStates.BACKWARD:
                        if (_playState != _lastPlayState)
                        {
                            manager.comm.WriteApp("User marker: Go backward to last epoch");
                        }
                        if (handler7 == null)
                        {
                            handler7 = delegate {
                                filePlayBackTrackBar.Enabled = false;
                            };
                        }
                        filePlayBackTrackBar.Invoke(handler7);
                        manager.comm._isCheckEpoch = true;
                        _lastPlayState = _playState;
                        if (_epochIndex < 1)
                        {
                            MessageBox.Show("Reached end of backward list", "File Playback", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            _epochIndex = 0;
                            _playState = _playStates.PAUSE;
                        }
                        else
                        {
                            _fileIndex = _epochList[_epochIndex - 1];
                            _epochIndex--;
                            _playState = _playStates.PLAY;
                        }
                        goto Label_0B9D;

                    case _playStates.QUIT:
                        updateFilePlaybackPauseBtn(false);
                        flag = true;
                        goto Label_0B9D;
                }
                millisecondsTimeout = _speedDelay;
            }
            catch (Exception exception2)
            {
                if (_playState != _playStates.QUIT)
                {
                    MessageBox.Show(string.Format("Error: frmFileReplay: parseFile() {0}", exception2.ToString()), "ERROR", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
                    _fileIndex = _fileHdlr.Index + 1L;
                }
            }
        Label_0B9D:
            if (!flag)
            {
                Thread.Sleep(millisecondsTimeout);
                goto Label_004F;
            }
            manager.CloseAll();
			base.Invoke((MethodInvoker)delegate
			{
                toolStripNumPortTxtBox.Text = PortManagerHash.Count.ToString();
                toolStripPortComboBox.Items.Remove(clsGlobal.FilePlayBackPortName);
                if (toolStripPortComboBox.Items.Count >= 1)
                {
                    toolStripPortComboBox.Text = toolStripPortComboBox.Items[0].ToString();
                }
            });
            class2 = null;
            if (manager.comm.RxCtrl != null)
            {
                manager.comm.RxCtrl.Dispose();
                manager.comm.RxCtrl = null;
            }
            manager.comm.Dispose();
            manager.comm = null;
            manager = null;
            GC.Collect(2);
        }

        private void performanceMonitorMenu_Click(object sender, EventArgs e)
        {
            CreatefrmPerformanceMonitorWindow();
        }

        private void perPortToolStripDebugViewBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton) sender;
            string[] strArray = button.Name.Split(new char[] { '_' });
            if ((strArray.Length > 1) && PortManagerHash.ContainsKey(strArray[1]))
            {
                PortManager target = (PortManager) PortManagerHash[strArray[1]];
                CreateDebugViewWin(target);
            }
        }

        private void perPortToolStripErrorViewBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton) sender;
            string[] strArray = button.Name.Split(new char[] { '_' });
            if (strArray.Length > 1)
            {
                PortManager target = (PortManager) PortManagerHash[strArray[1]];
                CreateErrorViewWin(target);
            }
        }

        private void perPortToolStripLocationViewBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton) sender;
            string[] strArray = button.Name.Split(new char[] { '_' });
            if (strArray.Length > 1)
            {
                PortManager target = (PortManager) PortManagerHash[strArray[1]];
                CreateLocationMapWin(target);
            }
        }

        private void perPortToolStripPortOpenBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton) sender;
            string[] strArray = button.Name.Split(new char[] { '_' });
            if (strArray.Length > 1)
            {
                portOpenBtnClickHandler(strArray[1]);
            }
        }

        private void perPortToolStripRadarViewBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton) sender;
            string[] strArray = button.Name.Split(new char[] { '_' });
            if (strArray.Length > 1)
            {
                PortManager target = (PortManager) PortManagerHash[strArray[1]];
                CreateRadarViewWin(target);
            }
        }

        private void perPortToolStripResponseViewBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton) sender;
            string[] strArray = button.Name.Split(new char[] { '_' });
            if (strArray.Length > 1)
            {
                PortManager target = (PortManager) PortManagerHash[strArray[1]];
                CreateResponseViewWin(target);
            }
        }

        private void perPortToolStripSignalViewBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton) sender;
            string[] strArray = button.Name.Split(new char[] { '_' });
            if ((strArray.Length > 1) && PortManagerHash.ContainsKey(strArray[1]))
            {
                PortManager target = (PortManager) PortManagerHash[strArray[1]];
                CreateSignalViewWin(target);
            }
        }

        private void perPortToolStripTTFFViewBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton) sender;
            string[] strArray = button.Name.Split(new char[] { '_' });
            if (strArray.Length > 1)
            {
                PortManager target = (PortManager) PortManagerHash[strArray[1]];
                CreateTTFFWin(target);
            }
        }

        public void PerPortUpdateConnectBtnImage(PortManager target)
        {
            CommunicationManager comm;
            bool isConnect;
            if (target != null)
            {
                comm = target.comm;
                isConnect = false;
				base.BeginInvoke((MethodInvoker)delegate
				{
                    if (comm != null)
                    {
                        isConnect = comm.IsSourceDeviceOpen();
                    }
                    if (isConnect)
                    {
                        if (target.PerPortToolStrip != null)
                        {
                            target.PerPortToolStrip.Items[_toolStripConnectBtnIdx].Image = Resources.connect;
                            target.PerPortToolStrip.Items[_toolStripConnectBtnIdx].Text = "Disconnect";
                        }
                    }
                    else if (target.PerPortToolStrip != null)
                    {
                        target.PerPortToolStrip.Items[_toolStripConnectBtnIdx].Image = Resources.disconnect;
                        target.PerPortToolStrip.Items[_toolStripConnectBtnIdx].Text = "Connect";
                    }
                });
            }
        }

        private void pointToPointAnalysisReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_objFrmCompareWithRefReport == null)
            {
                _objFrmCompareWithRefReport = new frmCompareWithRef();
            }
            if (_objFrmCompareWithRefReport.IsDisposed)
            {
                _objFrmCompareWithRefReport = new frmCompareWithRef();
            }
            _objFrmCompareWithRefReport.MdiParent = this;
            _objFrmCompareWithRefReport.Show();
        }

        private void pollAlmanacToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    MessageBox.Show("Action can only be performed on 1 receiver at a time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        manager.PollAlmanacHandler();
                    }
                }
            }
        }

        private void pollEphemerisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    MessageBox.Show("Action can only be performed on 1 receiver at a time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        manager.PollEphemerisHandler();
                    }
                }
            }
        }

        private void pollNavParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (!(str == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager manager = (PortManager) PortManagerHash[str];
                            if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                            {
                                manager.comm.RxCtrl.PollNavigationParameters();
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                    {
                        manager2.comm.RxCtrl.PollNavigationParameters();
                    }
                }
            }
        }

        private void pollSoftwareVesrionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pollSWClickHandler();
        }

        private void pollSWClickHandler()
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (!(str == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager manager = (PortManager) PortManagerHash[str];
                            if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                            {
                                pollSWVersionHandler(manager.comm);
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                    {
                        pollSWVersionHandler(manager2.comm);
                    }
                }
            }
        }

        private void pollSWVersionHandler(CommunicationManager comm)
        {
            bool isSLCRx = false;
            int mid = 0x84;
            comm.CMC.TxCurrentTransmissionType = (CommonClass.TransmissionType) comm.TxCurrentTransmissionType;
            string messageProtocol = comm.MessageProtocol;
            if ((comm.RxType == CommunicationManager.ReceiverType.SLC) || (comm.RxType == CommunicationManager.ReceiverType.TTB))
            {
                isSLCRx = true;
                if (comm.RxType == CommunicationManager.ReceiverType.TTB)
                {
                    messageProtocol = "TTB";
                }
            }
            string str3 = messageProtocol;
            if (str3 != null)
            {
                if (!(str3 == "SSB") && !(str3 == "TTB"))
                {
                    if (str3 == "F")
                    {
                        mid = 0x17;
                    }
                    else if (str3 == "OSP")
                    {
                        messageProtocol = "SSB";
                        isSLCRx = false;
                        mid = 0x84;
                    }
                }
                else
                {
                    mid = 0x84;
                }
            }
            string msg = comm.m_Protocols.GetDefaultMsgtoSend(isSLCRx, mid, -1, "Software Version Request", messageProtocol);
            comm.WriteData(msg);
        }

        private void portOpenBtnClickHandler(string portName)
        {
            PortManager target = null;
            if (portName == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    if (str != clsGlobal.FilePlayBackPortName)
                    {
                        target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            if (target.comm.IsSourceDeviceOpen())
                            {
                                if (manualDisconnect(ref target.comm))
                                {
                                    target.comm.Log.UpdateMainWindow -= new LogManager.UpdateParentEventHandler(updateLogFileBtn);
                                    target.comm.UpdatePortMainWinTitle -= new CommunicationManager.UpdateParentPortEventHandler(UpdateWinTitle);
                                    if ((target._interferenceReport != null) && target.InterferenceLocation.IsOpen)
                                    {
                                        target._interferenceReport.StopListeners();
                                    }
                                    if ((target.MessageView != null) && target.MessageViewLocation.IsOpen)
                                    {
                                        target.MessageView.StopListeners();
                                    }
                                }
                            }
                            else if (manualConnect(ref target.comm))
                            {
                                target.comm.Log.DurationLoggingStatusLabel = logManagerStatusLabel;
                                target.comm.Log.UpdateMainWindow += new LogManager.UpdateParentEventHandler(updateLogFileBtn);
                                target.comm.UpdatePortMainWinTitle += new CommunicationManager.UpdateParentPortEventHandler(UpdateWinTitle);
                                target.RunAsyncProcess();
                                if (!target.SignalViewLocation.IsOpen)
                                {
                                    CreateSignalViewWin(target);
                                }
                                if (!target.DebugViewLocation.IsOpen)
                                {
                                    CreateDebugViewWin(target);
                                }
                                setSubWindowsVisibilty(target, true);
                            }
                            else
                            {
                                MessageBox.Show(target.comm.ConnectErrorString, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            }
                            updateGUIOnConnectNDisconnect(target);
                        }
                    }
                }
                localUpdateStatusString("All");
            }
            else
            {
                target = (PortManager) PortManagerHash[portName];
                if (target != null)
                {
                    if (target.comm.IsSourceDeviceOpen())
                    {
                        if (manualDisconnect(ref target.comm))
                        {
                            target.comm.Log.UpdateMainWindow -= new LogManager.UpdateParentEventHandler(updateLogFileBtn);
                            target.comm.UpdatePortMainWinTitle -= new CommunicationManager.UpdateParentPortEventHandler(UpdateWinTitle);
                            target.comm.Log.DurationLoggingStatusLabel = logManagerStatusLabel;
                            if ((target._interferenceReport != null) && target.InterferenceLocation.IsOpen)
                            {
                                target._interferenceReport.StopListeners();
                            }
                            if ((target.MessageView != null) && target.MessageViewLocation.IsOpen)
                            {
                                target.MessageView.StopListeners();
                            }
                            localUpdateStatusString(target.comm.PortName);
                        }
                    }
                    else if (manualConnect(ref target.comm))
                    {
                        target.comm.Log.DurationLoggingStatusLabel = logManagerStatusLabel;
                        target.comm.Log.UpdateMainWindow += new LogManager.UpdateParentEventHandler(updateLogFileBtn);
                        target.comm.UpdatePortMainWinTitle += new CommunicationManager.UpdateParentPortEventHandler(UpdateWinTitle);
                        if (toolStripPortComboBox.Text != target.comm.PortName)
                        {
                            if (!toolStripPortComboBox.Items.Contains(target.comm.PortName))
                            {
                                PortManagerHash.Remove(toolStripPortComboBox.Text);
                                PortManagerHash.Add(target.comm.PortName, target);
                                toolStripPortComboBox.Items.Remove(toolStripPortComboBox.Text);
                                toolStripPortComboBox.Items.Add(target.comm.PortName);
                            }
                            toolStripPortComboBox.Text = target.comm.PortName;
                        }
                        target.RunAsyncProcess();
                        if (!target.SignalViewLocation.IsOpen)
                        {
                            CreateSignalViewWin(target);
                        }
                        if (!target.DebugViewLocation.IsOpen)
                        {
                            CreateDebugViewWin(target);
                        }
                        setSubWindowsVisibilty(target, true);
                    }
                    else
                    {
                        MessageBox.Show(target.comm.ConnectErrorString, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    updateGUIOnConnectNDisconnect(target);
                }
            }
        }

        private bool portSettings()
        {
            if (PortManagerHash.Count <= 0)
            {
                callPortConfig();
            }
            else
            {
                PortManager manager;
                if (toolStripPortComboBox.Text == clsGlobal.FilePlayBackPortName)
                {
                    return true;
                }
                if (toolStripPortComboBox.Text == "")
                {
                    manager = new PortManager();
                }
                else
                {
                    manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                }
                if (manager != null)
                {
                    if (manager.comm.IsSourceDeviceOpen())
                    {
                        MessageBox.Show("Port is open - Disconnect port before proceeding!", "Port Configure Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return false;
                    }
                    if (manager != null)
                    {
                        manager = callCreateRxSettingsWindow(manager);
                        if (manager != null)
                        {
                            if (clsGlobal.IsMarketingUser())
                            {
                                restoreDefaultPortLayout(false);
                            }
                            else
                            {
                                if (!manager.SignalViewLocation.IsOpen)
                                {
                                    CreateSignalViewWin(manager);
                                }
                                if (!manager.DebugViewLocation.IsOpen)
                                {
                                    CreateDebugViewWin(manager);
                                }
                            }
                            updateGUIOnConnectNDisconnect(manager);
                        }
                    }
                }
            }
            updateAllMainBtn();
            menuBtnInit();
            frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            return true;
        }

        private void powerMaskClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreatePowerMaskWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreatePowerMaskWin(ref target);
                    }
                }
            }
        }

        private void powerMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            powerMaskClickHandler();
        }

        private void powerModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lowPowerModeClickHandler();
        }

        private void predefinedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateInputCommandsWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateInputCommandsWin(target);
                    }
                }
            }
        }

        private void previousSettingsLayoutMenu_Click(object sender, EventArgs e)
        {
            foreach (Form form in base.MdiChildren)
            {
                form.Close();
            }
            frmSaveSettingsLoad(_lastWindowsRestoredFilePath);
        }

        private void pythonConsoleMenu_Click(object sender, EventArgs e)
        {
            CreatePythonWindow();
        }

        private void radarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            radarViewManualClickHandler();
        }

        private void radarViewManualClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target._svsMapPanel = CreateRadarViewWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._svsMapPanel = CreateRadarViewWin(manager2);
                    }
                }
                updateSVsMapViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void receiverConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            portSettings();
        }

        private void receiverDisconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectHandler();
        }

        private void receiverToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != "All") && (toolStripPortComboBox.Text != string.Empty))
            {
                PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if (manager != null)
                {
                    if (manager.comm.IsSourceDeviceOpen())
                    {
                        if (manager.comm.ProductFamily == CommonClass.ProductType.GSD4t)
                        {
                            enableDisableMenuAndButtonPerProductType(false);
                            switchProtocolsToolStripMenuItem.Enabled = false;
                        }
                        else if (manager.comm.ProductFamily == CommonClass.ProductType.GSD4e)
                        {
                            enableDisableMenuAndButtonPerProductType(true);
                            switchProtocolsToolStripMenuItem.Enabled = true;
                        }
                        else
                        {
                            enableDisableMenuAndButtonPerProductType(false);
                            switchProtocolsToolStripMenuItem.Enabled = true;
                        }
                        if (manager.comm.MessageProtocol == "NMEA")
                        {
                            enableDisableMenuAndButtonPerProtocol(false);
                        }
                        else
                        {
                            enableDisableMenuAndButtonPerProtocol(true);
                        }
                    }
                    else
                    {
                        enableDisableMenuAndButton(false);
                        if (toolStripPortComboBox.Text == clsGlobal.FilePlayBackPortName)
                        {
                            featuresToolStripMenuItem.Enabled = true;
                        }
                    }
                }
            }
        }

        public void RemoveFromWindowTab(string name)
        {
            if (clsGlobal.WindowsTab.ContainsKey(name))
            {
                windowMenuItem.DropDownItems.Remove((ToolStripMenuItem) clsGlobal.WindowsTab[name]);
                clsGlobal.WindowsTab.Remove(name);
            }
        }

        private void removeReceiverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool flag = false;
            string text = toolStripPortComboBox.Text;
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (text == "All")
                {
                    flag = true;
                    cleanupPortManager();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager manager = (PortManager) PortManagerHash[text];
                    if (manager != null)
                    {
                        if (manager.comm.IsSourceDeviceOpen())
                        {
                            manager.comm.ClosePort();
                        }
                        manager.CloseAll();
                        if (manager.PerPortToolStrip != null)
                        {
                            manager.PerPortToolStrip.Dispose();
                        }
                        manager.PerPortToolStrip = null;
                        manager.comm.Dispose();
                        manager.comm = null;
                        manager = null;
                    }
                    PortManagerHash.Remove(text);
                }
                fileReplayCloseHandler();
                updateFilePlaybackBtn(false);
                updateAllMainBtn();
                menuBtnInit();
                if (flag)
                {
                    toolStripPortComboBox.Items.Clear();
                }
                else
                {
                    toolStripPortComboBox.Items.Remove(text);
                }
                toolStripPortComboBox.Text = string.Empty;
                toolStripNumPortTxtBox.Text = PortManagerHash.Count.ToString();
                if ((toolStripPortComboBox.Items.Count == 1) && (toolStripPortComboBox.Items[0].ToString() == "All"))
                {
                    toolStripPortComboBox.Items.Clear();
                }
                if (toolStripPortComboBox.Items.Count == 0)
                {
                    updateGUIOnConnectNDisconnect(null);
                }
            }
        }

        private void report3GPPMenu_Click(object sender, EventArgs e)
        {
            CreateE911CtrlWindow("3GPP");
        }

        private void reportE911Menu_Click(object sender, EventArgs e)
        {
            CreateE911CtrlWindow("E911");
        }

        private void reportlSMResetMenu_Click(object sender, EventArgs e)
        {
            CreateLSMReportWindow();
        }

        private void reportPerformanceMenu_Click(object sender, EventArgs e)
        {
            CreatePerformanceReportCtrlWindow();
        }

        private void reportPseudoRangeMenu_Click(object sender, EventArgs e)
        {
            CreatePRReportCtrlWindow();
        }

        private void reportResetMenu_Click(object sender, EventArgs e)
        {
            CreateE911CtrlWindow("Reset");
        }

        private void reportTIA916Menu_Click(object sender, EventArgs e)
        {
            CreateE911CtrlWindow("TIA916");
        }

        private void resetClickHandler()
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateResetWindow();
                    localUpdateStatusString("All");
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateResetWindow(target);
                        localUpdateStatusString(target.comm.PortName);
                    }
                }
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetClickHandler();
        }

        private void responseViewManualClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target._responseView = CreateResponseViewWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._responseView = CreateResponseViewWin(manager2);
                    }
                }
                updateResponseViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void responseViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            responseViewManualClickHandler();
        }

        private void restoreDefaultLayout()
        {
            if (base.MdiChildren.Length == 0)
            {
                frmSaveSettingsLoad(_defaultWindowsRestoredFilePath);
            }
        }

        private void restoreDefaultPortLayout(bool closeAllBeforeRestore)
        {
            if (PortManagerHash.Count <= 0)
            {
                restoreDefaultLayout();
            }
            else
            {
                PortManager tmpP = null;
                if (closeAllBeforeRestore && (base.MdiChildren.Length != 0))
                {
                    closeAllWindows();
                }
                if (base.MdiChildren.Length == 0)
                {
                    frmSaveSettingsRead(_defaultWindowsRestoredFilePath);
                    if ((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != "All"))
                    {
                        tmpP = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                        restoreDefaultWindows(tmpP);
                    }
                    else
                    {
                        foreach (string str in PortManagerHash.Keys)
                        {
                            tmpP = (PortManager) PortManagerHash[str];
                            restoreDefaultWindows(tmpP);
                        }
                    }
                }
                else if (frmSaveSettingsRead(_lastWindowsRestoredFilePath))
                {
                    restoreOpenWindowsLayout();
                }
                else
                {
                    frmSaveSettingsRead(_defaultWindowsRestoredFilePath);
                    restoreOpenWindowsLayout();
                }
            }
        }

        private void restoreDefaultWindows(PortManager tmpP)
        {
            bool flag = false;
            if (tmpP != null)
            {
                if (!tmpP.SignalViewLocation.IsOpen)
                {
                    tmpP._signalStrengthPanel = CreateSignalViewWin(tmpP);
                    flag = true;
                }
                else
                {
                    loadLocation(tmpP._signalStrengthPanel, tmpP.SignalViewLocation.Top.ToString(), tmpP.SignalViewLocation.Left.ToString(), tmpP.SignalViewLocation.Width.ToString(), tmpP.SignalViewLocation.Height.ToString(), "Normal");
                }
                if (!tmpP.SVsMapLocation.IsOpen)
                {
                    tmpP._svsMapPanel = CreateRadarViewWin(tmpP);
                    flag = true;
                }
                else
                {
                    loadLocation(tmpP._svsMapPanel, tmpP.SVsMapLocation.Top.ToString(), tmpP.SVsMapLocation.Left.ToString(), tmpP.SVsMapLocation.Width.ToString(), tmpP.SVsMapLocation.Height.ToString(), "Normal");
                }
                if (!tmpP.LocationMapLocation.IsOpen)
                {
                    tmpP._locationViewPanel = CreateLocationMapWin(tmpP);
                    flag = true;
                }
                else
                {
                    loadLocation(tmpP._locationViewPanel, tmpP.LocationMapLocation.Top.ToString(), tmpP.LocationMapLocation.Left.ToString(), tmpP.LocationMapLocation.Width.ToString(), tmpP.LocationMapLocation.Height.ToString(), "Normal");
                }
                if (!tmpP.TTFFDisplayLocation.IsOpen)
                {
                    tmpP._ttffDisplay = CreateTTFFWin(tmpP);
                    flag = true;
                }
                else
                {
                    loadLocation(tmpP._ttffDisplay, tmpP.TTFFDisplayLocation.Top.ToString(), tmpP.TTFFDisplayLocation.Left.ToString(), tmpP.TTFFDisplayLocation.Width.ToString(), tmpP.TTFFDisplayLocation.Height.ToString(), "Normal");
                }
                if (!tmpP.DebugViewLocation.IsOpen)
                {
                    tmpP.DebugView = CreateDebugViewWin(tmpP);
                    flag = true;
                }
                else
                {
                    loadLocation(tmpP.DebugView, tmpP.DebugViewLocation.Top.ToString(), tmpP.DebugViewLocation.Left.ToString(), tmpP.DebugViewLocation.Width.ToString(), tmpP.DebugViewLocation.Height.ToString(), "Normal");
                }
                if (!tmpP.ResponseViewLocation.IsOpen)
                {
                    tmpP._responseView = CreateResponseViewWin(tmpP);
                    flag = true;
                }
                else
                {
                    loadLocation(tmpP._responseView, tmpP.ResponseViewLocation.Top.ToString(), tmpP.ResponseViewLocation.Left.ToString(), tmpP.ResponseViewLocation.Width.ToString(), tmpP.ResponseViewLocation.Height.ToString(), "Normal");
                }
                if (!tmpP.ErrorViewLocation.IsOpen)
                {
                    tmpP._errorView = CreateErrorViewWin(tmpP);
                    flag = true;
                }
                else
                {
                    loadLocation(tmpP._errorView, tmpP.ErrorViewLocation.Top.ToString(), tmpP.ErrorViewLocation.Left.ToString(), tmpP.ErrorViewLocation.Width.ToString(), tmpP.ErrorViewLocation.Height.ToString(), "Normal");
                }
                setSubWindowsVisibilty(tmpP, true);
                if (flag)
                {
                    restoreOpenWindowsLayout();
                }
            }
        }

        private void restoreLayoutMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (toolStripPortComboBox.Text == string.Empty)
            {
                ChangeRestoreLayoutState(true);
            }
            else if (toolStripPortComboBox.Text == clsGlobal.FilePlayBackPortName)
            {
                ChangeRestoreLayoutState(false);
            }
            else
            {
                bool flag = false;
                if (!(toolStripPortComboBox.Text == "All"))
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        if (manager2.comm.IsSourceDeviceOpen())
                        {
                            ChangeRestoreLayoutState(false);
                        }
                        else
                        {
                            ChangeRestoreLayoutState(true);
                        }
                    }
                }
                else
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager manager = (PortManager) PortManagerHash[str];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        ChangeRestoreLayoutState(false);
                    }
                    else
                    {
                        ChangeRestoreLayoutState(true);
                    }
                }
            }
        }

        private void restoreOpenWindowsLayout()
        {
            foreach (Form form in base.MdiChildren)
            {
                if (form.Name == "frmCommSignalView")
                {
                    loadLocation(form, SignalViewLocation.Top.ToString(), SignalViewLocation.Left.ToString(), SignalViewLocation.Width.ToString(), SignalViewLocation.Height.ToString(), "Normal");
                }
                else if (form.Name == "frmCommRadarMap")
                {
                    loadLocation(form, SVsMapLocation.Top.ToString(), SVsMapLocation.Left.ToString(), SVsMapLocation.Width.ToString(), SVsMapLocation.Height.ToString(), "Normal");
                }
                else if (form.Name == "frmCommLocationMap")
                {
                    loadLocation(form, LocationMapLocation.Top.ToString(), LocationMapLocation.Left.ToString(), LocationMapLocation.Width.ToString(), LocationMapLocation.Height.ToString(), "Normal");
                }
                else if (form.Name == "frmTTFFDisplay")
                {
                    loadLocation(form, TTFFDisplayLocation.Top.ToString(), TTFFDisplayLocation.Left.ToString(), TTFFDisplayLocation.Width.ToString(), TTFFDisplayLocation.Height.ToString(), "Normal");
                }
                else if (form.Name == "frmCommDebugView")
                {
                    loadLocation(form, DebugViewLocation.Top.ToString(), DebugViewLocation.Left.ToString(), DebugViewLocation.Width.ToString(), DebugViewLocation.Height.ToString(), "Normal");
                }
                else if (form.Name == "frmCommResponseView")
                {
                    loadLocation(form, ResponseViewLocation.Top.ToString(), ResponseViewLocation.Left.ToString(), ResponseViewLocation.Width.ToString(), ResponseViewLocation.Height.ToString(), "Normal");
                }
                else if (form.Name == "frmCommErrorView")
                {
                    loadLocation(form, ErrorViewLocation.Top.ToString(), ErrorViewLocation.Left.ToString(), ErrorViewLocation.Width.ToString(), ErrorViewLocation.Height.ToString(), "Normal");
                }
            }
        }

        private void restoreUserLayout()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Config\UserData";
            dialog.Filter = "Layout (*.xml)|*.xml|All file (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (Form form in base.MdiChildren)
                {
                    form.Close();
                }
                _userWindowsRestoredFilePath = dialog.FileName;
                frmSaveSettingsLoad(_userWindowsRestoredFilePath);
            }
        }

        public void ResumeCreationOfDirectoryProtocolCoverageReport(string dirPath)
        {
            Report.ResumeCreationOfDirectoryProtocolCoverageReport(dirPath);
        }

        private void rfPlaybackCaptureMenu_Click(object sender, EventArgs e)
        {
            if (false)
            {
                MessageBox.Show("Feature not yet available", "Information");
            }
            else
            {
                CreateRFReplayCaptureWindow();
            }
        }

        private void rfReplayConfigurationMenu_Click(object sender, EventArgs e)
        {
            CreateRFReplayConfigWindow();
        }

        private void rfReplayPlaybackMenu_Click(object sender, EventArgs e)
        {
            CreateRFReplayPlaybackWindow();
        }

        private void rfReplaySynthesizerMenu_Click(object sender, EventArgs e)
        {
            CreateSynthesizerWindow();
        }

        private void rinexToEphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = dialog.FileName;
                int length = fileName.LastIndexOf('.');
                fileName.Substring(0, length);
                string path = fileName.Substring(0, length) + ".ai3eph";
                StreamWriter writer = new StreamWriter(path);
                try
                {
                    RinexFile file = new RinexFile();
                    file.Read(fileName);
                    int year = file.Year;
                    int month = file.Month;
                    int day = file.Day;
                    int hour = file.Hour;
                    int minute = file.Minute;
                    int second = file.Second;
                    for (int i = 0; i < 0x18; i += 2)
                    {
                        DateTime inTime = new DateTime(year, month, day, i, 0, 0);
                        GPSDateTime time2 = new GPSDateTime();
                        time2.SetUTCOffset(0);
                        time2.SetTime(inTime);
                        int gPSTOW = (int) time2.GetGPSTOW();
                        int num7 = gPSTOW + (time2.GetGPSWeek() * 0x93a80);
                        string str3 = string.Format("// Hour {0:D2}-{1:D2}\r\n", i, i + 2) + "// Ephemeris Collection Time(UTC): ";
                        string str4 = string.Format("{0:ddd MMM dd HH:mm:ss yyyy}", inTime);
                        str3 = str3 + str4 + "\r\n*********** Ephemeris Data at GPS time: ";
                        string str5 = num7.ToString();
                        str3 = str3 + str5 + "***********\r\n//\r\n//";
                        writer.WriteLine(str3);
                        for (byte j = 1; j <= 0x20; j = (byte) (j + 1))
                        {
                            RinexEph eph = file.SearchRinexArrayList(j, gPSTOW + file.UTCOffset);
                            if (eph != null)
                            {
                                string str6 = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23},", new object[] { 
                                    eph.svid, eph.ura_ind, (byte) eph.iode, (short) (eph.Crs * Math.Pow(2.0, 5.0)), (short) (eph.deltan * Math.Pow(2.0, 43.0)), (int) (eph.m0 * Math.Pow(2.0, 31.0)), (short) (eph.Cuc * Math.Pow(2.0, 29.0)), (uint) (eph.ecc * Math.Pow(2.0, 33.0)), (short) (eph.Cus * Math.Pow(2.0, 29.0)), (uint) (eph.sqrta * Math.Pow(2.0, 19.0)), (ushort) (eph.toe * Math.Pow(2.0, -4.0)), (short) (eph.Cic * Math.Pow(2.0, 29.0)), (int) (eph.omega0 * Math.Pow(2.0, 31.0)), (short) (eph.Cis * Math.Pow(2.0, 29.0)), (int) (eph.i0 * Math.Pow(2.0, 31.0)), (short) (eph.Crc * Math.Pow(2.0, 5.0)), 
                                    (int) (eph.omega * Math.Pow(2.0, 31.0)), (int) (eph.omegaDot * Math.Pow(2.0, 43.0)), (short) (eph.idot * Math.Pow(2.0, 43.0)), (ushort) (eph.toc * Math.Pow(2.0, -4.0)), (sbyte) (eph.tgd * Math.Pow(2.0, 31.0)), (sbyte) (eph.af2 * Math.Pow(2.0, 55.0)), (short) (eph.af1 * Math.Pow(2.0, 43.0)), (int) (eph.af0 * Math.Pow(2.0, 31.0))
                                 });
                                writer.WriteLine(str6);
                            }
                        }
                        writer.WriteLine("");
                    }
                    writer.Close();
                    MessageBox.Show("Conversion is complete and file saved as: " + path, "File Conversion Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Exception in Rinex to ai3eph file conversion: " + exception.ToString());
                }
            }
        }

        public void RunAutoTest()
        {
            try
            {
                frmAutomationTests childInstance = frmAutomationTests.GetChildInstance();
                if ((childInstance == null) || childInstance.IsDisposed)
                {
                    childInstance = new frmAutomationTests();
                }
                clsGlobal.ScriptDone = true;
                clsGlobal.Abort = false;
                childInstance.autoTestRunSingle();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void RunHostAppFromMain(string argStr, ref CommunicationManager comm)
        {
            if (comm != null)
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(Directory.GetParent(comm.HostSWFilePath).FullName);
                Process process = sysCmdExec.OpenWin("\"" + comm.HostSWFilePath + "\"", argStr);
                comm.HostAppCmdWinId = process.Id;
                Directory.SetCurrentDirectory(currentDirectory);
            }
        }

        private void runSingleTest()
        {
            EventHandler method = null;
            clsGlobal.DoneTests.Clear();
            try
            {
                if ((_objFrmPython != null) && !_objFrmPython.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            _objFrmPython.Close();
                        };
                    }
                    _objFrmPython.Invoke(method);
                }
                Cursor = Cursors.WaitCursor;
                _objFrmPython = CreatePythonWindow();
                _objFrmPython.MdiParent = this;
                _objFrmPython.Show();
                _objFrmPython.BringToFront();
                Cursor = Cursors.Default;
            }
            catch (Exception exception)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(string.Format("Test encounters error\n\n{0}", exception.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                _objFrmPython = null;
                clsGlobal.TestsToRun.Clear();
                clsGlobal.CurrentRunningTest = string.Empty;
                return;
            }
            Thread thread = new Thread(new ThreadStart(autoTestStart));
            thread.IsBackground = true;
            thread.Start();
        }

        private void rxTTBViewMenuClickHandler(PortManager target)
        {
            if (target != null)
            {
                if (!target.comm.TTBPort.IsOpen)
                {
                    MessageBox.Show("TTB port not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    if ((target._ttbWin == null) || target._ttbWin.IsDisposed)
                    {
                        target._ttbWin = new frmCommOpen();
                    }
                    target._ttbWin.UpdatePortManager += new frmCommOpen.UpdateWindowEventHandler(target.UpdateSubWindowOnClosed);
                    target._ttbWin.MdiParent = base.MdiParent;
                    target._ttbWin.comm.comPort = target.comm.TTBPort;
                    target._ttbWin.comm.comPort.Close();
                    Thread.Sleep(0x3e8);
                    target._ttbWin.Show();
                    target._ttbWin.MdiParent = this;
                    target.TTBWinLocation.IsOpen = true;
                    target._ttbWin.comm.RequireHostRun = false;
                    target._ttbWin.comm.MessageProtocol = "SSB";
                    target._ttbWin.comm.RxType = CommunicationManager.ReceiverType.TTB;
                    target._ttbWin.comm.InputDeviceMode = CommonClass.InputDeviceModes.RS232;
                    target._ttbWin.comm.BaudRate = target.comm.TTBPort.BaudRate.ToString();
                    target._ttbWin.comm.DataBits = target.comm.TTBPort.DataBits.ToString();
                    target._ttbWin.comm.StopBits = "One";
                    target._ttbWin.comm.Parity = "None";
                    target._ttbWin.comm.PortName = target.comm.TTBPort.PortName;
                    target._ttbWin.comm.ProductFamily = CommonClass.ProductType.GSW;
                    target._ttbWin.comm.AutoDetectProtocolAndBaudDone = true;
                    target._ttbWin.comm.dataPlot = null;
                    target._ttbWin.comm.OpenPort();
                    target._ttbWin.comm.WriteData("A0A2 0009 CCA6 0002 0100 0000 00 8175 B0B3");
                    target._ttbWin.comm.WriteData("A0A2 0009 CCA6 0004 0100 0000 00 8177 B0B3");
                    target._ttbWin.comm.WriteData("A0A2 0009 CCA6 0029 0100 0000 00 819C B0B3");
                    target._ttbWin.RunAsyncProcess();
                    target._ttbWin.Text = string.Format("TTB: {0}", target._ttbWin.comm.comPort.PortName);
                    target._ttbWin.UpdateMenuForTTB();
                    target._ttbSigWin = target._ttbWin.CreateSignalViewWin();
                    target._ttbSigWin.Text = string.Format("TTB: {0} Signal View", target._ttbWin.comm.comPort.PortName);
                    aidingTTBToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void satellitesStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target._SatelliteStats = CreateSatelliteStatsWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._SatelliteStats = CreateSatelliteStatsWin(manager2);
                    }
                }
                updateSatellitesStatsViewBtn();
            }
        }

        private void saveLayoutMenu_Click(object sender, EventArgs e)
        {
            frmCommonSimpleInput input = new frmCommonSimpleInput("Enter File Name:");
            input.updateParent += new frmCommonSimpleInput.updateParentEventHandler(updateFilePath);
            input.ShowDialog();
        }

        public void SaveLogDirectory(string dir)
        {
            clsGlobal.LogDirectory = dir;
        }

        public void SaveWindowsLocation()
        {
            frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
        }

        private void sbasConfigureClickHandler()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
                if ((flag && (manager != null)) && manager.comm.IsSourceDeviceOpen())
                {
                    string str2 = "DGPS Settings: All";
                    frmDGPS mdgps = new frmDGPS(manager.comm);
                    mdgps.Text = str2;
                    mdgps.ShowDialog();
                }
            }
        }

        private void sbasConfigureClickHandler(ref CommunicationManager comm)
        {
            if (comm != null)
            {
                if (!base.IsDisposed)
                {
                    string str = comm.sourceDeviceName + ": DGPS Settings";
                    frmDGPS mdgps = new frmDGPS(comm);
                    mdgps.Text = str;
                    mdgps.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void sBASRangingToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (toolStripPortComboBox.Text != string.Empty)
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    PortManager manager = null;
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (!(str == clsGlobal.FilePlayBackPortName))
                        {
                            manager = (PortManager) PortManagerHash[str];
                            if ((manager != null) && updateSBASRangingMenu(manager.comm))
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        updateSBASRangingMenu(manager2.comm);
                    }
                    else
                    {
                        setSBASRangingMenu(false);
                    }
                }
            }
        }

        private void sDOGenerationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateSDOGenWin();
        }

        private int searchforProtocol_I2C(ref CommunicationManager comm)
        {
            comm.AutoDetectProtocolAndBaudDone = false;
            comm.CurrentProtocol = comm.MessageProtocol;
            if (!comm.OpenPort())
            {
                return 0;
            }
            Thread.Sleep(5);
            frmSimpleDialog dialog = new frmSimpleDialog("Auto Detect Protocol");
            dialog.Show();
            dialog.MdiParent = clsGlobal.g_objfrmMDIMain;
            dialog.DisplayMessage = string.Format("\tDetecting {0}\n\n\t\t Please wait...", "NMEA");
            dialog.ShowMessage();
            if (comm.waitforNMEAMsg_I2C() != string.Empty)
            {
                comm.AutoDetectProtocolAndBaudDone = false;
                Thread.Sleep(50);
                if (!comm.ClosePort())
                {
                    dialog.Close();
                    return 0;
                }
                Thread.Sleep(5);
                comm.RxType = CommunicationManager.ReceiverType.NMEA;
                comm.RxType = CommunicationManager.ReceiverType.SLC;
                if (!comm.OpenPort())
                {
                    dialog.Close();
                    return 0;
                }
                comm.AutoDetectProtocolAndBaudDone = true;
                dialog.Close();
                return 1;
            }
            dialog.DisplayMessage = string.Format("\tDetecting {0}\n\n\t\t Please wait...", "OSP");
            dialog.ShowMessage();
            if (comm.waitforSSBMsg_I2C() != string.Empty)
            {
                comm.AutoDetectProtocolAndBaudDone = false;
                Thread.Sleep(50);
                if (!comm.ClosePort())
                {
                    dialog.Close();
                    return 0;
                }
                Thread.Sleep(5);
                comm.MessageProtocol = "OSP";
                comm.RxType = CommunicationManager.ReceiverType.SLC;
                if (!comm.OpenPort())
                {
                    dialog.Close();
                    return 0;
                }
                comm.AutoDetectProtocolAndBaudDone = true;
                dialog.Close();
                return 1;
            }
            dialog.Close();
            return 2;
        }

        private int searchforProtocolAndBaud(ref CommunicationManager comm)
        {
            comm.AutoDetectProtocolAndBaudDone = false;
            comm.CurrentProtocol = comm.MessageProtocol;
            comm.CurrentBaud = comm.BaudRate;
            Refresh();
            if (!comm.OpenPort())
            {
                return 0;
            }
            Thread.Sleep(10);
            int num = searchforProtocolAndBaud_sub(ref comm);
            if (num == 3)
            {
                comm.BaudRate = comm.CurrentBaud;
                comm.MessageProtocol = comm.CurrentProtocol;
                if (comm.CurrentProtocol == "OSP")
                {
                    comm.RxType = CommunicationManager.ReceiverType.SLC;
                    comm.RxTransType = CommunicationManager.TransmissionType.GPS;
                }
                else
                {
                    comm.RxType = CommunicationManager.ReceiverType.NMEA;
                    comm.RxTransType = CommunicationManager.TransmissionType.Text;
                }
                comm.SetupRxCtrl();
                comm.comPort.BaudRate = int.Parse(comm.BaudRate);
                comm.comPort.UpdateSetttings();
                comm.portDataInit();
            }
            comm.AutoDetectProtocolAndBaudDone = true;
            return num;
        }

        private bool searchforProtocolAndBaud_NMEA(ref CommunicationManager comm)
        {
            comm.MessageProtocol = "NMEA";
            comm.RxType = CommunicationManager.ReceiverType.NMEA;
            string[] strArray = new string[5];
            strArray = "4800,115200,38400,57600,9600,19200".Split(new char[] { ',' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if ((comm.CurrentProtocol != "NMEA") || (comm.CurrentBaud != strArray[i]))
                {
                    comm.BaudRate = strArray[i];
                    Refresh();
                    if (!comm.ClosePort())
                    {
                        return false;
                    }
                    if (!comm.OpenPort())
                    {
                        return false;
                    }
                    if (comm.waitforNMEAMsg())
                    {
                        return true;
                    }
                }
            }
            return (!comm.ClosePort() && false);
        }

        private bool searchforProtocolAndBaud_OSP(ref CommunicationManager comm)
        {
            comm.MessageProtocol = "OSP";
            comm.RxType = CommunicationManager.ReceiverType.SLC;
            string[] strArray = new string[5];
            strArray = "115200,4800,38400,9600,57600,19200".Split(new char[] { ',' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if ((comm.CurrentProtocol != "OSP") || (comm.CurrentBaud != strArray[i]))
                {
                    comm.BaudRate = strArray[i];
                    Refresh();
                    if (!comm.ClosePort())
                    {
                        return false;
                    }
                    if (!comm.OpenPort())
                    {
                        return false;
                    }
                    if (comm.waitforSSBMsg())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private int searchforProtocolAndBaud_sub(ref CommunicationManager comm)
        {
            string[] strArray = new string[10];
            string[] strArray2 = new string[10];
            string str = string.Empty;
            string str2 = string.Empty;
            comm.portDataInit();
            if (comm.MessageProtocol == "NMEA")
            {
                comm.RxType = CommunicationManager.ReceiverType.NMEA;
                if (comm.waitforNMEAMsg())
                {
                    comm.AutoDetectProtocolAndBaudDone = true;
                    return 1;
                }
                str = "OSP,NMEA,OSP,NMEA,OSP,OSP,OSP,OSP,OSP,OSP,OSP,OSP,NMEA,NMEA,NMEA,NMEA,NMEA";
                str2 = "115200,4800,4800,115200,38400,9600,57600,19200,230400,460800,912600,1228800,38400,9600,57600,19200,115200";
            }
            else if (comm.MessageProtocol == "OSP")
            {
                comm.RxType = CommunicationManager.ReceiverType.SLC;
                if (comm.waitforSSBMsg())
                {
                    comm.AutoDetectProtocolAndBaudDone = true;
                    return 1;
                }
                str = "NMEA,OSP,NMEA,OSP,NMEA,NMEA,NMEA,NMEA,NMEA,OSP,OSP,OSP,OSP,OSP,OSP,OSP,OSP";
                str2 = "4800,115200,115200,4800,38400,9600,57600,19200,115200,38400,9600,57600,19200,230400,460800,912600,1228800";
            }
            else
            {
                comm.AutoDetectProtocolAndBaudDone = true;
                return 2;
            }
            strArray = str2.Split(new char[] { ',' });
            strArray2 = str.Split(new char[] { ',' });
            frmSimpleDialog dialog = new frmSimpleDialog("Auto Detect Baud");
            dialog.Show();
            for (int i = 0; i < strArray2.Length; i++)
            {
                comm.AutoDetectProtocolAndBaudDone = false;
                dialog.DisplayMessage = string.Format("\tDetecting {0} - {1}\n\n\t\tPlease wait...", strArray2[i], strArray[i]);
                dialog.ShowMessage();
                if ((comm.CurrentProtocol != strArray2[i]) || (comm.CurrentBaud != strArray[i]))
                {
                    comm.BaudRate = strArray[i];
                    comm.MessageProtocol = strArray2[i];
                    if (strArray2[i] == "OSP")
                    {
                        comm.RxType = CommunicationManager.ReceiverType.SLC;
                        comm.RxTransType = CommunicationManager.TransmissionType.GPS;
                    }
                    else
                    {
                        comm.RxType = CommunicationManager.ReceiverType.NMEA;
                        comm.RxTransType = CommunicationManager.TransmissionType.Text;
                    }
                    Refresh();
                    comm.SetupRxCtrl();
                    uint baud = uint.Parse(comm.BaudRate);
                    bool flag = false;
                    int num3 = 0;
                    while (num3++ < 5)
                    {
                        flag = comm.comPort.UpdateBaudSettings(baud);
                        if (flag)
                        {
                            break;
                        }
                    }
                    if (!flag)
                    {
                        dialog.Close();
                        comm.ErrorPrint("Error updating port");
                        return 3;
                    }
                    comm.portDataInit();
                    Thread.Sleep(5);
                    if (comm.MessageProtocol == "OSP")
                    {
                        if (comm.waitforSSBMsg())
                        {
                            comm.AutoDetectProtocolAndBaudDone = true;
                            dialog.Close();
                            return 1;
                        }
                    }
                    else if (comm.MessageProtocol == "NMEA")
                    {
                        if (comm.waitforNMEAMsg())
                        {
                            comm.AutoDetectProtocolAndBaudDone = true;
                            dialog.Close();
                            return 1;
                        }
                    }
                    else
                    {
                        comm.AutoDetectProtocolAndBaudDone = true;
                        dialog.Close();
                        return 2;
                    }
                }
            }
            comm.AutoDetectProtocolAndBaudDone = true;
            dialog.Close();
            return 3;
        }

        private void SearchMenu(string username)
        {
            Hashtable userAccessInfo = UserAccess.GetUserAccessInfo(username);
            ToolStripMenuItem[] itemArray = new ToolStripMenuItem[] { 
                fileToolStripMenuItem, logFileToolStripMenuItem, startLogToolStripMenuItem, stopLogToolStripMenuItem, convertToolStripMenuItem, gP2GPSToolStripMenuItem, binGPSToolStripMenuItem, gPSNMEAToolStripMenuItem, gPSToKMLToolStripMenuItem, NMEAtoGPStoolStripMenuItem, ExtracttoolStripMenuItem, analysisToolStripMenuItem, toolStripMenuItem_Plot, fileOpenToolStripMenuItem, fileCloseToolStripMenuItem, fileExitToolStripMenuItem, 
                addReceiverToolStripMenuItem, removeReceiverToolStripMenuItem, receiverConnectToolStripMenuItem, receiverDisconnectToolStripMenuItem, signalToolStripMenuItem, radarToolStripMenuItem, mapToolStripMenuItem, tTFFAndNavAccuracyToolStripMenuItem, responseViewToolStripMenuItem, debugViewToolStripMenuItem, errorToolStripMenuItem, messageToolStripMenuItem, mEMSViewToolStripMenuItem, compassToolStripMenuItem, altitudeMeterToolStripMenuItem, receiverViewCWDetectionToolStripMenuItem, 
                satellitesStatisticsToolStripMenuItem, receiverViewSiRFawareToolStripMenuItem, siRFDRiveStatusToolStripMenuItem, siRFDRiveSensorToolStripMenuItem, siRFDRiveToolStripMenuItem, commandToolStripMenuItem, resetToolStripMenuItem, pollSoftwareVesrionToolStripMenuItem, pollAlmanacToolStripMenuItem, pollEphemerisToolStripMenuItem, pollNavParametersToolStripMenuItem, switchPowerModeToolStripMenuItem, switchOperationModeToolStripMenuItem, switchProtocolsToolStripMenuItem, setAlmanacToolStripMenuItem, setEphemerisToolStripMenuItem, 
                setEEToolStripMenuItem, setDebugLevelsToolStripMenuItem, setDGPSToolStripMenuItem, setMEMSToolStripMenuItem, enableMEMSToolStripMenuItem, disableMEMSToolStripMenuItem, setABPToolStripMenuItem, enableABPToolStripMenuItem, disableABPToolStripMenuItem, lowPowerCommandsBufferToolStripMenuItem, iCConfigureToolStripMenuItem, iCPeekPokeToolStripMenuItem, inputCommandsToolStripMenuItem, predefinedToolStripMenuItem, userDefinedToolStripMenuItem, navigationToolStripMenuItem, 
                set5HzNavToolStripMenuItem, enable5HzNavToolStripMenuItem, disable5HzNavToolStripMenuItem, dOPMaskToolStripMenuItem, elevationMaskToolStripMenuItem, modeMaskToolStripMenuItem, powerMaskToolStripMenuItem, sBASRangingToolStripMenuItem, plotsToolStripMenuItem, averageCNoToolStripMenuItem, navAccuracyVsTimeToolStripMenuItem, sVTrajectoryToolStripMenuItem, sVTrackedVsTimeToolStripMenuItem, setReferenceLocationToolStripMenuItem, configureDebugErrorLogToolStripMenuItem, autoTestToolStripMenuItem, 
                autoTestLoopitToolStripMenuItem, autoTestStandardTestsToolStripMenuItem, autoTest3GPPToolStripMenuItem, autoTestTIA916ToolStripMenuItem, autoTestStatusToolStripMenuItem, autoTestAbortToolStripMenuItem, consoleToolStripMenuItem, featuresToolStripMenuItem, powerModeToolStripMenuItem, MEMSToolStripMenuItem, featuresSiRFawareToolStripMenuItem, tTFSToolStripMenuItem, aidingConfigureToolStripMenuItem, aidingSummaryToolStripMenuItem, aidingTTBToolStripMenuItem, TTBConnectToolStripMenuItem, 
                TTBConfigureTimeAidingToolStripMenuItem, TTBViewToolStripMenuItem, aidingsDownloadServerAssistedDataToolStripMenuItem, instrumentControlMenuItem, rFReplayMenuItem, rfReplayConfigurationMenu, rfPlaybackCaptureMenu, rfReplayPlaybackMenu, rfReplaySynthesizerMenu, simplexMenu, sPAzMenu, signalGeneratorMenu, testRackMenu, reportMenuItem, reportE911Menu, report3GPPMenu, 
                reportTIA916Menu, reportPerformanceMenu, reportResetMenu, pointToPointAnalysisReportToolStripMenuItem, mPMToolStripMenuItem, sDOGenerationToolStripMenuItem, windowMenuItem, cascadeMenu, tileVerticalMenu, tileHorizontalMenu, restoreLayoutMenuItem, defaultLayoutMenu, previousSettingsLayoutMenu, userSettingsLayoutMenu, saveLayoutMenu, helpMenuItem, 
                aboutMenu, developerDocMenu, userManualMenu
             };
            for (int i = 0; i < itemArray.GetLength(0); i++)
            {
                ToolStripMenuItem item = itemArray[i];
                string key = item.Text.Replace("&", "");
                if (userAccessInfo.ContainsKey(key))
                {
                    string str3 = (string) userAccessInfo[key];
                    if (str3 == null)
                    {
                        goto Label_064D;
                    }
                    if (!(str3 == "Hidden"))
                    {
                        if (str3 == "Disabled")
                        {
                            goto Label_0634;
                        }
                        if (str3 == "Enabled")
                        {
                            goto Label_063D;
                        }
                        goto Label_064D;
                    }
                    item.Visible = false;
                }
                continue;
            Label_0634:
                item.Enabled = false;
                continue;
            Label_063D:
                item.Visible = true;
                item.Enabled = true;
                continue;
            Label_064D:
                item.Visible = false;
            }
        }

        private void SearchToolStrip(string username)
        {
            Hashtable userAccessInfo = UserAccess.GetUserAccessInfo(username);
            foreach (ToolStripItem item in new ToolStripItem[] { toolStripPortComboBox, toolStripUpDownArrowBtn, toolStripNumPortTxtBox, toolStripPortOpenBtn, toolStripSaveBtn, toolStripResetBtn, toolStripSignalViewBtn, toolStripRadarViewBtn, toolStripMapViewBtn, toolStripTTFFViewBtn, toolStripResponseViewBtn, toolStripDebugViewBtn, toolStripErrorViewBtn })
            {
                string name = item.Name;
                if (userAccessInfo.ContainsKey(name))
                {
                    string str3 = (string) userAccessInfo[name];
                    if (str3 == null)
                    {
                        goto Label_0120;
                    }
                    if (!(str3 == "Hidden"))
                    {
                        if (str3 == "Disabled")
                        {
                            goto Label_0107;
                        }
                        if (str3 == "Enabled")
                        {
                            goto Label_0110;
                        }
                        goto Label_0120;
                    }
                    item.Visible = false;
                }
                continue;
            Label_0107:
                item.Enabled = false;
                continue;
            Label_0110:
                item.Visible = true;
                item.Enabled = true;
                continue;
            Label_0120:
                item.Visible = false;
            }
        }

        private void set5HzNavMenu(bool state)
        {
            if (state)
            {
                disable5HzNavToolStripMenuItem.Checked = false;
                enable5HzNavToolStripMenuItem.Checked = true;
            }
            else
            {
                disable5HzNavToolStripMenuItem.Checked = true;
                enable5HzNavToolStripMenuItem.Checked = false;
            }
        }

        private void set5HzNavModeHandler(ref CommunicationManager comm)
        {
            try
            {
                comm.FiveHzNavModeToSet = true;
                comm.FiveHzNavModePendingSet = true;
                comm.RxCtrl.PollNavigationParameters();
            }
            catch
            {
            }
        }

        private void set5HzNavToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (toolStripPortComboBox.Text != string.Empty)
            {
                PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if (manager == null)
                {
                    goto Label_007D;
                }
                if (manager.comm.ProductFamily == CommonClass.ProductType.GSD4e)
                {
                    enableDisableMenuAndButtonPerProductType(true);
                    if (manager.comm.MessageProtocol == "NMEA")
                    {
                        enableDisableMenuAndButtonPerProtocol(false);
                    }
                    else
                    {
                        enableDisableMenuAndButtonPerProtocol(true);
                    }
                    goto Label_007D;
                }
                enableDisableMenuAndButtonPerProductType(false);
            }
            return;
        Label_007D:
            if (toolStripPortComboBox.Text == "All")
            {
                PortManager manager2 = null;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager2 = (PortManager) PortManagerHash[str];
                        if ((manager2 != null) && update5HzNavMenu(manager2.comm))
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                PortManager manager3 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if (manager3 != null)
                {
                    update5HzNavMenu(manager3.comm);
                    return;
                }
                set5HzNavMenu(false);
            }
        }

        public void SetAbort(bool value)
        {
            clsGlobal.Abort = value;
        }

        private void setABPMenu(bool state)
        {
            if (state)
            {
                disableABPToolStripMenuItem.Checked = false;
                enableABPToolStripMenuItem.Checked = true;
            }
            else
            {
                disableABPToolStripMenuItem.Checked = true;
                enableABPToolStripMenuItem.Checked = false;
            }
        }

        private void setABPModeHandler(ref CommunicationManager comm)
        {
            try
            {
                comm.ABPModeToSet = true;
                comm.ABPModePendingSet = true;
                comm.RxCtrl.PollNavigationParameters();
            }
            catch
            {
            }
        }

        private void setABPToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (toolStripPortComboBox.Text != string.Empty)
            {
                PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager == null) || !manager.comm.IsSourceDeviceOpen())
                {
                    goto Label_008A;
                }
                if (manager.comm.ProductFamily == CommonClass.ProductType.GSD4e)
                {
                    enableDisableMenuAndButtonPerProductType(true);
                    if (manager.comm.MessageProtocol == "NMEA")
                    {
                        enableDisableMenuAndButtonPerProtocol(false);
                    }
                    else
                    {
                        enableDisableMenuAndButtonPerProtocol(true);
                    }
                    goto Label_008A;
                }
                enableDisableMenuAndButtonPerProductType(false);
            }
            return;
        Label_008A:
            if (toolStripPortComboBox.Text == "All")
            {
                PortManager manager2 = null;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        manager2 = (PortManager) PortManagerHash[str];
                        if ((manager2 != null) && updateABPMenu(manager2.comm))
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                PortManager manager3 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if (manager3 != null)
                {
                    updateABPMenu(manager3.comm);
                    return;
                }
                setABPMenu(false);
            }
        }

        private void setAddReceiverMenuState()
        {
            if (clsGlobal.IsMarketingUser())
            {
                bool flag = false;
                foreach (Form form in base.MdiChildren)
                {
                    if (form.Name == "frmCommOpen")
                    {
                        flag = true;
                        _objFrmCommOpen = (frmCommOpen) form;
                        break;
                    }
                }
                if (!flag)
                {
                }
            }
        }

        public void SetAgilentHP8648CInterface()
        {
            //! SigGenCtrl = new GPIB_Mgr_Agilent_HP8648C();
        }

        private void setAlmanacToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = "alm";
                dialog.Filter = "Almanac files (*.alm)|*.alm";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string msg = utils_AutoReply.getAlmFromFileForSet(dialog.FileName);
                    if (toolStripPortComboBox.Text == "All")
                    {
                        foreach (string str3 in PortManagerHash.Keys)
                        {
                            if (!(str3 == clsGlobal.FilePlayBackPortName))
                            {
                                PortManager manager = (PortManager) PortManagerHash[str3];
                                if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                                {
                                    manager.comm.WriteData(msg);
                                    manager.comm.WriteApp(msg);
                                }
                            }
                        }
                    }
                    else
                    {
                        clsGlobal.PerformOnAll = false;
                        PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                        if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                        {
                            manager2.comm.WriteData(msg);
                            manager2.comm.WriteApp(msg);
                        }
                    }
                }
            }
        }

        private void setCGEEClickHandler(ref CommunicationManager comm)
        {
            if (comm != null)
            {
                if (!base.IsDisposed)
                {
                    string str = comm.sourceDeviceName + ": Set EE";
                    frmEE mee = new frmEE(comm);
                    mee.Text = str;
                    mee.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Port not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public void SetCommWinRef(string m_key, frmCommOpen m_val)
        {
            if (clsGlobal.CommWinRef.ContainsKey(m_key))
            {
                clsGlobal.CommWinRef.Remove(m_key);
            }
            clsGlobal.CommWinRef.Add(m_key, m_val);
        }

        private void setDefaultWindowsLocation()
        {
            SignalViewLocation.Top = 0;
            SignalViewLocation.Left = 0;
            SignalViewLocation.Width = 0x179;
            SignalViewLocation.Height = 0x11f;
            SVsMapLocation.Top = 0;
            SVsMapLocation.Left = 0x178;
            SVsMapLocation.Width = 0x11a;
            SVsMapLocation.Height = 0x11f;
            LocationMapLocation.Top = 0;
            LocationMapLocation.Left = 0x292;
            LocationMapLocation.Width = 0x288;
            LocationMapLocation.Height = 510;
            TTFFDisplayLocation.Top = 0x233;
            TTFFDisplayLocation.Left = 0;
            TTFFDisplayLocation.Width = 0x292;
            TTFFDisplayLocation.Height = 0x13b;
            DebugViewLocation.Top = 0x11f;
            DebugViewLocation.Left = 0;
            DebugViewLocation.Width = 0x292;
            DebugViewLocation.Height = 0x114;
            ResponseViewLocation.Top = 510;
            ResponseViewLocation.Left = 0x292;
            ResponseViewLocation.Width = 340;
            ResponseViewLocation.Height = 0x170;
            ErrorViewLocation.Top = 510;
            ErrorViewLocation.Left = 0x3e6;
            ErrorViewLocation.Width = 0x134;
            ErrorViewLocation.Height = 0x170;
        }

        private void setDGPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    sbasConfigureClickHandler();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        sbasConfigureClickHandler(ref manager.comm);
                    }
                }
            }
        }

        private void setEEClickHandler()
        {
            if (PortManagerHash.Count > 0)
            {
                PortManager manager = null;
                bool flag = false;
                foreach (string str in PortManagerHash.Keys)
                {
                    manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        flag = true;
                        break;
                    }
                }
                if ((flag && (manager != null)) && manager.comm.IsSourceDeviceOpen())
                {
                    string str2 = "Set EE: All";
                    frmEE mee = new frmEE(manager.comm);
                    mee.Text = str2;
                    mee.ShowDialog();
                }
            }
        }

        private void setEEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    setEEClickHandler();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        setCGEEClickHandler(ref manager.comm);
                    }
                }
            }
        }

        private void setEncryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateEncrypCtrlWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateEncrypCtrlWin(ref target);
                    }
                }
            }
        }

        private void setEphemerisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = "eph";
                dialog.Filter = "Ephemeris files (*.eph)|*.eph";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string str2 = utils_AutoReply.getEphFromFileForSet(dialog.FileName);
                    if (toolStripPortComboBox.Text == "All")
                    {
                        foreach (string str3 in PortManagerHash.Keys)
                        {
                            if (str3 != clsGlobal.FilePlayBackPortName)
                            {
                                PortManager manager = (PortManager) PortManagerHash[str3];
                                if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                                {
                                    foreach (string str4 in str2.Split(new char[] { ',' }))
                                    {
                                        if (str4 != string.Empty)
                                        {
                                            manager.comm.WriteData(str4);
                                            manager.comm.WriteApp(str4);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        clsGlobal.PerformOnAll = false;
                        PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                        if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                        {
                            foreach (string str5 in str2.Split(new char[] { ',' }))
                            {
                                if (str5 != string.Empty)
                                {
                                    manager2.comm.WriteData(str5);
                                    manager2.comm.WriteApp(str5);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void setMEMSModeCheck(CommunicationManager comm)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localSetMEMSModeCheck(comm);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localSetMEMSModeCheck(comm);
            }
        }

        private void setMEMSModeHandler(ref CommunicationManager comm)
        {
            comm.MEMSModeToSet = true;
            if (comm.RxCtrl != null)
            {
                comm.RxCtrl.SetMEMSMode(1);
            }
        }

        private void setMEMSToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager manager = (PortManager) PortManagerHash[str];
                    if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                    {
                        setMEMSModeCheck(manager.comm);
                    }
                }
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && manager2.comm.IsSourceDeviceOpen())
                {
                    setMEMSModeCheck(manager2.comm);
                }
            }
        }

        private void setPollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateSetPollGenericSensorParamWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateSetPollGenericSensorParamWin(ref target);
                    }
                }
            }
        }

        private void setPortSubWindowsLayout(string portName)
        {
            if (!isLoading)
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    PortManager target = (PortManager) PortManagerHash[str];
                    if (target != null)
                    {
                        if ((str != portName) && (portName != "All"))
                        {
                            setSubWindowsVisibilty(target, false);
                        }
                        else
                        {
                            setSubWindowsVisibilty(target, true);
                        }
                    }
                }
            }
        }

        private void setReferenceLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                try
                {
                    PortManager manager = null;
                    bool flag = false;
                    if (toolStripPortComboBox.Text == "All")
                    {
                        clsGlobal.PerformOnAll = true;
                        foreach (string str in PortManagerHash.Keys)
                        {
                            manager = (PortManager) PortManagerHash[str];
                            if (manager != null)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        clsGlobal.PerformOnAll = false;
                        manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                        flag = true;
                    }
                    if (flag && (manager != null))
                    {
                        frmSetReferenceLocation location = new frmSetReferenceLocation();
                        location.CommWindow = manager.comm;
                        location.ShowDialog();
                    }
                    if (toolStripPortComboBox.Text == "All")
                    {
                        foreach (string str2 in PortManagerHash.Keys)
                        {
                            manager = (PortManager) PortManagerHash[str2];
                            if (((manager != null) && (manager._ttffDisplay != null)) && manager.TTFFDisplayLocation.IsOpen)
                            {
                                manager._ttffDisplay.SetReferenceLocation();
                            }
                        }
                    }
                    else if (((manager != null) && (manager._ttffDisplay != null)) && manager.TTFFDisplayLocation.IsOpen)
                    {
                        manager._ttffDisplay.SetReferenceLocation();
                    }
                }
                catch
                {
                }
            }
        }

        private void setSBASRangingMenu(bool state)
        {
            if (state)
            {
                disableSBASRangingToolStripMenuItem.Checked = false;
                enableSBASRangingToolStripMenuItem.Checked = true;
            }
            else
            {
                disableSBASRangingToolStripMenuItem.Checked = true;
                enableSBASRangingToolStripMenuItem.Checked = false;
            }
        }

        private void setSBASRangingModeHandler(ref CommunicationManager comm)
        {
            comm.SBASRangingToSet = true;
            comm.SBASRangingPendingSet = true;
            comm.RxCtrl.PollNavigationParameters();
        }

        public void SetScriptDone(bool value)
        {
            clsGlobal.ScriptDone = value;
        }

        private void setSubWindowsVisibilty(PortManager target, bool state)
        {
            if (target != null)
            {
                if ((target._inputCommands != null) && InputCommandLocation.IsOpen)
                {
                    if (state)
                    {
                        target._inputCommands.Top = target.InputCommandLocation.Top;
                        target._inputCommands.WinTop = target.InputCommandLocation.Top;
                        target._inputCommands.Left = target.InputCommandLocation.Left;
                        target._inputCommands.WinLeft = target.InputCommandLocation.Left;
                        if (target.InputCommandLocation.Width != 0)
                        {
                            target._inputCommands.Width = target.InputCommandLocation.Width;
                            target._inputCommands.WinWidth = target.InputCommandLocation.Width;
                        }
                        if (target.InputCommandLocation.Height != 0)
                        {
                            target._inputCommands.Height = target.InputCommandLocation.Height;
                            target._inputCommands.WinHeight = target.InputCommandLocation.Height;
                        }
                        target._inputCommands.Visible = true;
                        target._inputCommands.Show();
                        target._inputCommands.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._inputCommands.Name, target._inputCommands.Left, target._inputCommands.Top, target._inputCommands.Width, target._inputCommands.Height, true);
                        target._inputCommands.Visible = false;
                    }
                }
                if ((target._svsMapPanel != null) && target.SVsMapLocation.IsOpen)
                {
                    if (state)
                    {
                        target._svsMapPanel.Top = target.SVsMapLocation.Top;
                        target._svsMapPanel.WinTop = target.SVsMapLocation.Top;
                        target._svsMapPanel.Left = target.SVsMapLocation.Left;
                        target._svsMapPanel.WinLeft = target.SVsMapLocation.Left;
                        if (target.SVsMapLocation.Width != 0)
                        {
                            target._svsMapPanel.Width = target.SVsMapLocation.Width;
                            target._svsMapPanel.WinWidth = target.SVsMapLocation.Width;
                        }
                        if (target.SVsMapLocation.Height != 0)
                        {
                            target._svsMapPanel.Height = target.SVsMapLocation.Height;
                            target._svsMapPanel.WinHeight = target.SVsMapLocation.Height;
                        }
                        target._svsMapPanel.Visible = true;
                        target._svsMapPanel.Show();
                        target._svsMapPanel.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._svsMapPanel.Name, target._svsMapPanel.Left, target._svsMapPanel.Top, target._svsMapPanel.Width, target._svsMapPanel.Height, true);
                        target._svsMapPanel.Visible = false;
                    }
                }
                if ((target._svsTrajPanel != null) && target.SVTrajViewLocation.IsOpen)
                {
                    if (state)
                    {
                        target._svsTrajPanel.Top = target.SVTrajViewLocation.Top;
                        target._svsTrajPanel.WinTop = target.SVTrajViewLocation.Top;
                        target._svsTrajPanel.Left = target.SVTrajViewLocation.Left;
                        target._svsTrajPanel.WinLeft = target.SVTrajViewLocation.Left;
                        if (target.SVTrajViewLocation.Width != 0)
                        {
                            target._svsTrajPanel.Width = target.SVTrajViewLocation.Width;
                            target._svsTrajPanel.WinWidth = target.SVTrajViewLocation.Width;
                        }
                        if (target.SVTrajViewLocation.Height != 0)
                        {
                            target._svsTrajPanel.Height = target.SVTrajViewLocation.Height;
                            target._svsTrajPanel.WinHeight = target.SVTrajViewLocation.Height;
                        }
                        target._svsTrajPanel.Visible = true;
                        target._svsTrajPanel.Show();
                        target._svsTrajPanel.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._svsTrajPanel.Name, target._svsTrajPanel.Left, target._svsTrajPanel.Top, target._svsTrajPanel.Width, target._svsTrajPanel.Height, true);
                        target._svsTrajPanel.Visible = false;
                    }
                }
                if ((target._svsCNoPanel != null) && target.SVCNoViewLocation.IsOpen)
                {
                    if (state)
                    {
                        target._svsCNoPanel.Top = target.SVCNoViewLocation.Top;
                        target._svsCNoPanel.WinTop = target.SVCNoViewLocation.Top;
                        target._svsCNoPanel.Left = target.SVCNoViewLocation.Left;
                        target._svsCNoPanel.WinLeft = target.SVCNoViewLocation.Left;
                        if (target.SVCNoViewLocation.Width != 0)
                        {
                            target._svsCNoPanel.Width = target.SVCNoViewLocation.Width;
                            target._svsCNoPanel.WinWidth = target.SVCNoViewLocation.Width;
                        }
                        if (target.SVCNoViewLocation.Height != 0)
                        {
                            target._svsCNoPanel.Height = target.SVCNoViewLocation.Height;
                            target._svsCNoPanel.WinHeight = target.SVCNoViewLocation.Height;
                        }
                        target._svsCNoPanel.Visible = true;
                        target._svsCNoPanel.Show();
                        target._svsCNoPanel.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._svsCNoPanel.Name, target._svsCNoPanel.Left, target._svsCNoPanel.Top, target._svsCNoPanel.Width, target._svsCNoPanel.Height, true);
                        target._svsCNoPanel.Visible = false;
                    }
                }
                if ((target._svsTrackedVsTimePanel != null) && target.SVTrackedVsTimeViewLocation.IsOpen)
                {
                    if (state)
                    {
                        target._svsTrackedVsTimePanel.Top = target.SVTrackedVsTimeViewLocation.Top;
                        target._svsTrackedVsTimePanel.WinTop = target.SVTrackedVsTimeViewLocation.Top;
                        target._svsTrackedVsTimePanel.Left = target.SVTrackedVsTimeViewLocation.Left;
                        target._svsTrackedVsTimePanel.WinLeft = target.SVTrackedVsTimeViewLocation.Left;
                        if (target.SVTrackedVsTimeViewLocation.Width != 0)
                        {
                            target._svsTrackedVsTimePanel.Width = target.SVTrackedVsTimeViewLocation.Width;
                            target._svsTrackedVsTimePanel.WinWidth = target.SVTrackedVsTimeViewLocation.Width;
                        }
                        if (target.SVTrackedVsTimeViewLocation.Height != 0)
                        {
                            target._svsTrackedVsTimePanel.Height = target.SVTrackedVsTimeViewLocation.Height;
                            target._svsTrackedVsTimePanel.WinHeight = target.SVTrackedVsTimeViewLocation.Height;
                        }
                        target._svsTrackedVsTimePanel.Visible = true;
                        target._svsTrackedVsTimePanel.Show();
                        target._svsTrackedVsTimePanel.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._svsTrackedVsTimePanel.Name, target._svsTrackedVsTimePanel.Left, target._svsTrackedVsTimePanel.Top, target._svsTrackedVsTimePanel.Width, target._svsTrackedVsTimePanel.Height, true);
                        target._svsTrackedVsTimePanel.Visible = false;
                    }
                }
                if ((target.NavVsTimeView != null) && target.NavVsTimeLocation.IsOpen)
                {
                    if (state)
                    {
                        target.NavVsTimeView.Top = target.NavVsTimeLocation.Top;
                        target.NavVsTimeView.WinTop = target.NavVsTimeLocation.Top;
                        target.NavVsTimeView.Left = target.NavVsTimeLocation.Left;
                        target.NavVsTimeView.WinLeft = target.NavVsTimeLocation.Left;
                        if (target.NavVsTimeLocation.Width != 0)
                        {
                            target.NavVsTimeView.Width = target.NavVsTimeLocation.Width;
                            target.NavVsTimeView.WinWidth = target.NavVsTimeLocation.Width;
                        }
                        if (target.NavVsTimeLocation.Height != 0)
                        {
                            target.NavVsTimeView.Height = target.NavVsTimeLocation.Height;
                            target.NavVsTimeView.WinHeight = target.NavVsTimeLocation.Height;
                        }
                        target.NavVsTimeView.Visible = true;
                        target.NavVsTimeView.Show();
                        target.NavVsTimeView.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target.NavVsTimeView.Name, target.NavVsTimeView.Left, target.NavVsTimeView.Top, target.NavVsTimeView.Width, target.NavVsTimeView.Height, true);
                        target.NavVsTimeView.Visible = false;
                    }
                }
                if ((target._SatelliteStats != null) && target.SatelliteStatsLocation.IsOpen)
                {
                    if (state)
                    {
                        target._SatelliteStats.Top = target.SVsMapLocation.Top;
                        target._SatelliteStats.WinTop = target.SVsMapLocation.Top;
                        target._SatelliteStats.Left = target.SVsMapLocation.Left;
                        target._SatelliteStats.WinLeft = target.SVsMapLocation.Left;
                        if (target.SVsMapLocation.Width != 0)
                        {
                            target._SatelliteStats.Width = target.SVsMapLocation.Width;
                            target._SatelliteStats.WinWidth = target.SVsMapLocation.Width;
                        }
                        if (target.SVsMapLocation.Height != 0)
                        {
                            target._SatelliteStats.Height = target.SVsMapLocation.Height;
                            target._SatelliteStats.WinHeight = target.SVsMapLocation.Height;
                        }
                        target._SatelliteStats.Visible = true;
                        target._SatelliteStats.Show();
                        target._SatelliteStats.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._SatelliteStats.Name, target._SatelliteStats.Left, target._SatelliteStats.Top, target._SatelliteStats.Width, target._SatelliteStats.Height, true);
                        target._SatelliteStats.Visible = false;
                    }
                }
                if ((target.MessageView != null) && target.MessageViewLocation.IsOpen)
                {
                    if (state)
                    {
                        target.MessageView.Top = target.MessageViewLocation.Top;
                        target.MessageView.WinTop = target.MessageViewLocation.Top;
                        target.MessageView.Left = target.MessageViewLocation.Left;
                        target.MessageView.WinLeft = target.MessageViewLocation.Left;
                        if (target.MessageViewLocation.Width != 0)
                        {
                            target.MessageView.Width = target.MessageViewLocation.Width;
                            target.MessageView.WinWidth = target.MessageViewLocation.Width;
                        }
                        if (target.MessageViewLocation.Height != 0)
                        {
                            target.MessageView.Height = target.MessageViewLocation.Height;
                            target.MessageView.WinHeight = target.MessageViewLocation.Height;
                        }
                        target.MessageView.Visible = true;
                        target.MessageView.Show();
                        target.MessageView.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target.MessageView.Name, target.MessageView.Left, target.MessageView.Top, target.MessageView.Width, target.MessageView.Height, true);
                        target.MessageView.Visible = false;
                    }
                }
                if ((target._interferenceReport != null) && target.InterferenceLocation.IsOpen)
                {
                    if (state)
                    {
                        target._interferenceReport.Top = target.InterferenceLocation.Top;
                        target._interferenceReport.WinTop = target.InterferenceLocation.Top;
                        target._interferenceReport.Left = target.InterferenceLocation.Left;
                        target._interferenceReport.WinLeft = target.InterferenceLocation.Left;
                        if (target.InterferenceLocation.Width != 0)
                        {
                            target._interferenceReport.Width = target.InterferenceLocation.Width;
                            target._interferenceReport.WinWidth = target.InterferenceLocation.Width;
                        }
                        if (target.InterferenceLocation.Height != 0)
                        {
                            target._interferenceReport.Height = target.InterferenceLocation.Height;
                            target._interferenceReport.WinHeight = target.InterferenceLocation.Height;
                        }
                        target._interferenceReport.Visible = true;
                        target._interferenceReport.Show();
                        target._interferenceReport.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._interferenceReport.Name, target._interferenceReport.Left, target._interferenceReport.Top, target._interferenceReport.Width, target._interferenceReport.Height, true);
                        target._interferenceReport.Visible = false;
                    }
                }
                if ((target._SiRFAware != null) && target.SiRFawareLocation.IsOpen)
                {
                    if (state)
                    {
                        target._SiRFAware.Top = target.SiRFawareLocation.Top;
                        target._SiRFAware.WinTop = target.SiRFawareLocation.Top;
                        target._SiRFAware.Left = target.SiRFawareLocation.Left;
                        target._SiRFAware.WinLeft = target.SiRFawareLocation.Left;
                        if (target.SiRFawareLocation.Width != 0)
                        {
                            target._SiRFAware.Width = target.SiRFawareLocation.Width;
                            target._SiRFAware.WinWidth = target.SiRFawareLocation.Width;
                        }
                        if (target.SiRFawareLocation.Height != 0)
                        {
                            target._SiRFAware.Height = target.SiRFawareLocation.Height;
                            target._SiRFAware.WinHeight = target.SiRFawareLocation.Height;
                        }
                        target._SiRFAware.Visible = true;
                        target._SiRFAware.Show();
                        target._SiRFAware.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._SiRFAware.Name, target._SiRFAware.Left, target._SiRFAware.Top, target._SiRFAware.Width, target._SiRFAware.Height, true);
                        target._SiRFAware.Visible = false;
                    }
                }
                if ((target._memsView != null) && target.MEMSLocation.IsOpen)
                {
                    if (state)
                    {
                        target._memsView.Top = target.MEMSLocation.Top;
                        target._memsView.WinTop = target.MEMSLocation.Top;
                        target._memsView.Left = target.MEMSLocation.Left;
                        target._memsView.WinLeft = target.MEMSLocation.Left;
                        if (target.MEMSLocation.Width != 0)
                        {
                            target._memsView.Width = target.MEMSLocation.Width;
                            target._memsView.WinWidth = target.MEMSLocation.Width;
                        }
                        if (target.MEMSLocation.Height != 0)
                        {
                            target._memsView.Height = target.MEMSLocation.Height;
                            target._memsView.WinHeight = target.MEMSLocation.Height;
                        }
                        target._memsView.Visible = true;
                        target._memsView.Show();
                        target._memsView.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._memsView.Name, target._memsView.Left, target._memsView.Top, target._memsView.Width, target._memsView.Height, true);
                        target._memsView.Visible = false;
                    }
                }
                if ((target._responseView != null) && target.ResponseViewLocation.IsOpen)
                {
                    if (state)
                    {
                        target._responseView.Top = target.ResponseViewLocation.Top;
                        target._responseView.WinTop = target.ResponseViewLocation.Top;
                        target._responseView.Left = target.ResponseViewLocation.Left;
                        target._responseView.WinLeft = target.ResponseViewLocation.Left;
                        if (target.ResponseViewLocation.Width != 0)
                        {
                            target._responseView.Width = target.ResponseViewLocation.Width;
                            target._responseView.WinWidth = target.ResponseViewLocation.Width;
                        }
                        if (target.ResponseViewLocation.Height != 0)
                        {
                            target._responseView.Height = target.ResponseViewLocation.Height;
                            target._responseView.WinHeight = target.ResponseViewLocation.Height;
                        }
                        target._responseView.Visible = true;
                        target._responseView.Show();
                        target._responseView.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._responseView.Name, target._responseView.Left, target._responseView.Top, target._responseView.Width, target._responseView.Height, true);
                        target._responseView.Visible = false;
                    }
                }
                if ((target._errorView != null) && target.ErrorViewLocation.IsOpen)
                {
                    if (state)
                    {
                        target._errorView.Top = target.ErrorViewLocation.Top;
                        target._errorView.WinTop = target.ErrorViewLocation.Top;
                        target._errorView.Left = target.ErrorViewLocation.Left;
                        target._errorView.WinLeft = target.ErrorViewLocation.Left;
                        if (target.ErrorViewLocation.Width != 0)
                        {
                            target._errorView.Width = target.ErrorViewLocation.Width;
                            target._errorView.WinWidth = target.ErrorViewLocation.Width;
                        }
                        if (target.ErrorViewLocation.Height != 0)
                        {
                            target._errorView.Height = target.ErrorViewLocation.Height;
                            target._errorView.WinHeight = target.ErrorViewLocation.Height;
                        }
                        target._errorView.Visible = true;
                        target._errorView.Show();
                        target._errorView.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._errorView.Name, target._errorView.Left, target._errorView.Top, target._errorView.Width, target._errorView.Height, true);
                        target._errorView.Visible = false;
                    }
                }
                if ((target._peekPokeWin != null) && target.PeekPokeLocation.IsOpen)
                {
                    if (state)
                    {
                        target._peekPokeWin.Top = target.PeekPokeLocation.Top;
                        target._peekPokeWin.WinTop = target.PeekPokeLocation.Top;
                        target._peekPokeWin.Left = target.PeekPokeLocation.Left;
                        target._peekPokeWin.WinLeft = target.PeekPokeLocation.Left;
                        if (target.PeekPokeLocation.Width != 0)
                        {
                            target._peekPokeWin.Width = target.PeekPokeLocation.Width;
                            target._peekPokeWin.WinWidth = target.PeekPokeLocation.Width;
                        }
                        if (target.PeekPokeLocation.Height != 0)
                        {
                            target._peekPokeWin.Height = target.PeekPokeLocation.Height;
                            target._peekPokeWin.WinHeight = target.PeekPokeLocation.Height;
                        }
                        target._peekPokeWin.Visible = true;
                        target._peekPokeWin.Show();
                        target._peekPokeWin.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._peekPokeWin.Name, target._peekPokeWin.Left, target._peekPokeWin.Top, target._peekPokeWin.Width, target._peekPokeWin.Height, true);
                        target._peekPokeWin.Visible = false;
                    }
                }
                if ((target._ttbWin != null) && target.TTBWinLocation.IsOpen)
                {
                    if (state)
                    {
                        target._ttbWin.Top = target.TTBWinLocation.Top;
                        target._ttbWin.Left = target.TTBWinLocation.Left;
                        if (target.TTBWinLocation.Width != 0)
                        {
                            target._ttbWin.Width = target.TTBWinLocation.Width;
                        }
                        if (target.PeekPokeLocation.Height != 0)
                        {
                            target._ttbWin.Height = target.TTBWinLocation.Height;
                        }
                        target._ttbWin.Visible = true;
                        target._ttbWin.Show();
                        target._ttbWin.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._ttbWin.Name, target._ttbWin.Left, target._ttbWin.Top, target._ttbWin.Width, target._ttbWin.Height, true);
                        target._ttbWin.Visible = false;
                    }
                }
                if ((target._locationViewPanel != null) && target.LocationMapLocation.IsOpen)
                {
                    if (state)
                    {
                        target._locationViewPanel.Top = target.LocationMapLocation.Top;
                        target._locationViewPanel.WinTop = target.LocationMapLocation.Top;
                        target._locationViewPanel.Left = target.LocationMapLocation.Left;
                        target._locationViewPanel.WinLeft = target.LocationMapLocation.Left;
                        if (target.LocationMapLocation.Width != 0)
                        {
                            target._locationViewPanel.Width = target.LocationMapLocation.Width;
                            target._locationViewPanel.WinWidth = target.LocationMapLocation.Width;
                        }
                        if (target.LocationMapLocation.Height != 0)
                        {
                            target._locationViewPanel.Height = target.LocationMapLocation.Height;
                            target._locationViewPanel.WinHeight = target.LocationMapLocation.Height;
                        }
                        target._locationViewPanel.Visible = true;
                        target._locationViewPanel.Show();
                        target._locationViewPanel.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._locationViewPanel.Name, target._locationViewPanel.Left, target._locationViewPanel.Top, target._locationViewPanel.Width, target._locationViewPanel.Height, true);
                        target._locationViewPanel.Visible = false;
                    }
                }
                if ((target._ttffDisplay != null) && target.TTFFDisplayLocation.IsOpen)
                {
                    if (state)
                    {
                        target._ttffDisplay.Top = target.TTFFDisplayLocation.Top;
                        target._ttffDisplay.WinTop = target.TTFFDisplayLocation.Top;
                        target._ttffDisplay.Left = target.TTFFDisplayLocation.Left;
                        target._ttffDisplay.WinLeft = target.TTFFDisplayLocation.Left;
                        if (target.TTFFDisplayLocation.Width != 0)
                        {
                            target._ttffDisplay.Width = target.TTFFDisplayLocation.Width;
                            target._ttffDisplay.WinWidth = target.TTFFDisplayLocation.Width;
                        }
                        if (target.TTFFDisplayLocation.Height != 0)
                        {
                            target._ttffDisplay.Height = target.TTFFDisplayLocation.Height;
                            target._ttffDisplay.WinHeight = target.TTFFDisplayLocation.Height;
                        }
                        target._ttffDisplay.Visible = true;
                        target._ttffDisplay.Show();
                        target._ttffDisplay.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._ttffDisplay.Name, target._ttffDisplay.Left, target._ttffDisplay.Top, target._ttffDisplay.Width, target._ttffDisplay.Height, true);
                        target._ttffDisplay.Visible = false;
                    }
                }
                if ((target.DebugView != null) && target.DebugViewLocation.IsOpen)
                {
                    if (state)
                    {
                        target.DebugView.Top = target.DebugViewLocation.Top;
                        target.DebugView.WinTop = target.DebugViewLocation.Top;
                        target.DebugView.Left = target.DebugViewLocation.Left;
                        target.DebugView.WinLeft = target.DebugViewLocation.Left;
                        if (target.DebugViewLocation.Width != 0)
                        {
                            target.DebugView.Width = target.DebugViewLocation.Width;
                            target.DebugView.WinWidth = target.DebugViewLocation.Width;
                        }
                        if (target.DebugViewLocation.Height != 0)
                        {
                            target.DebugView.Height = target.DebugViewLocation.Height;
                            target.DebugView.WinHeight = target.DebugViewLocation.Height;
                        }
                        target.DebugView.Visible = true;
                        target.DebugView.Show();
                        target.DebugView.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target.DebugView.Name, target.DebugView.Left, target.DebugView.Top, target.DebugView.Width, target.DebugView.Height, true);
                        target.DebugView.Visible = false;
                    }
                }
                if ((target._signalStrengthPanel != null) && target.SignalViewLocation.IsOpen)
                {
                    if (state)
                    {
                        target._signalStrengthPanel.Top = target.SignalViewLocation.Top;
                        target._signalStrengthPanel.WinTop = target.SignalViewLocation.Top;
                        target._signalStrengthPanel.Left = target.SignalViewLocation.Left;
                        target._signalStrengthPanel.WinLeft = target.SignalViewLocation.Left;
                        if (target.SignalViewLocation.Width != 0)
                        {
                            target._signalStrengthPanel.Width = target.SignalViewLocation.Width;
                            target._signalStrengthPanel.WinWidth = target.SignalViewLocation.Width;
                        }
                        if (target.SignalViewLocation.Height != 0)
                        {
                            target._signalStrengthPanel.Height = target.SignalViewLocation.Height;
                            target._signalStrengthPanel.WinHeight = target.SignalViewLocation.Height;
                        }
                        target._signalStrengthPanel.Visible = true;
                        target._signalStrengthPanel.Show();
                        target._signalStrengthPanel.BringToFront();
                    }
                    else
                    {
                        target.UpdateSubWindowOnClosed(target._signalStrengthPanel.Name, target._signalStrengthPanel.Left, target._signalStrengthPanel.Top, target._signalStrengthPanel.Width, target._signalStrengthPanel.Height, true);
                        target._signalStrengthPanel.Visible = false;
                    }
                }
            }
        }

        public void SetTestRackInterface()
        {
            TestRackCtrl = new TestRackMgr();
        }

        public void SetToolStripPortText(string text)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localSetToolStripPortText(text);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localSetToolStripPortText(text);
            }
        }

        private void setup3GPPTest(string testType)
        {
            frm3GPPConfig config;
            if (toolStripPortComboBox.Text == clsGlobal.FilePlayBackPortName)
            {
                return;
            }
            _testName = testType;
            DialogResult none = DialogResult.None;
            if (!clsGlobal.ScriptDone)
            {
                MessageBox.Show("Test is running. Please abort current test before proceeding!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (clsGlobal.IsMarketingUser())
            {
                if (ConfigurationManager.AppSettings["Show3GPPWarning"] == "1")
                {
                    none = new frmAutomationWarning(testType).ShowDialog();
                }
                else
                {
                    none = DialogResult.OK;
                }
                if (none == DialogResult.OK)
                {
                    none = CreateTestStationWindow(testType);
                }
            }
            else if (base.MdiChildren.Length <= 0)
            {
                none = CreateTestStationWindow(testType);
            }
            else if (MessageBox.Show("All windows will be closed. Proceed?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                EventHandler method = null;
                foreach (Form childForm in base.MdiChildren)
                {
                    if (childForm.Name != "frmStationSetup")
                    {
                        if (method == null)
                        {
                            method = delegate {
                                childForm.Close();
                            };
                        }
                        base.Invoke(method);
                    }
                }
                none = CreateTestStationWindow(testType);
            }
            if (none != DialogResult.OK)
            {
                return;
            }
            clearAutomationStatus();
            string str = testType;
            if (str != null)
            {
                if (!(str == "3GPP"))
                {
                    if (str == "TIA916")
                    {
                        clsGlobal.TestScriptPath = clsGlobal.InstalledDirectory + @"\scripts\TIA916\TIA916.py";
                        goto Label_0181;
                    }
                }
                else
                {
                    clsGlobal.TestScriptPath = clsGlobal.InstalledDirectory + @"\scripts\3GPP\3GPP.py";
                    goto Label_0181;
                }
            }
            clsGlobal.TestScriptPath = string.Empty;
        Label_0181:
            config = new frm3GPPConfig(clsGlobal.TestScriptPath);
            if (config.ShowDialog() == DialogResult.OK)
            {
                runSingleTest();
            }
        }

        private PortManager setupFileReplayPort()
        {
            PortManager manager = null;
            if (PortManagerHash.ContainsKey(clsGlobal.FilePlayBackPortName))
            {
                manager = (PortManager) PortManagerHash[clsGlobal.FilePlayBackPortName];
            }
            else
            {
                manager = new PortManager();
                manager.comm.PortName = clsGlobal.FilePlayBackPortName;
                manager.comm.PortNum = clsGlobal.FilePlayBackPortNum;
                manager.comm.InputDeviceMode = CommonClass.InputDeviceModes.FilePlayBack;
                manager.comm.sourceDeviceName = clsGlobal.FilePlayBackPortName;
                PortManagerHash.Add(clsGlobal.FilePlayBackPortName, manager);
                switch (_fileType)
                {
                    case CommonClass.TransmissionType.Text:
                        manager.comm.MessageProtocol = "NMEA";
                        goto Label_00F9;

                    case CommonClass.TransmissionType.Hex:
                    case CommonClass.TransmissionType.GP2:
                    case CommonClass.TransmissionType.GPS:
                        manager.comm.MessageProtocol = "OSP";
                        manager.comm.AidingProtocol = "OSP";
                        goto Label_00F9;
                }
                manager.comm.MessageProtocol = "OSP";
                manager.comm.AidingProtocol = "OSP";
            }
        Label_00F9:
            if (manager != null)
            {
                manager.comm.FilePlaybackCommSetup();
                manager.RunAsyncProcess();
            }
            return manager;
        }

        private void signalGeneratorMenu_Click(object sender, EventArgs e)
        {
            try
            {
                //! CreateGPIBCtrlWindow();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void signalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            signalViewManualClickHandler();
        }

        private void signalViewManualClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target._signalStrengthPanel = CreateSignalViewWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._signalStrengthPanel = CreateSignalViewWin(manager2);
                    }
                }
                updateSignalViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void simplexMenu_Click(object sender, EventArgs e)
        {
            CreateSimplexCtrlWindow();
        }

        private void siRFawareClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (!(str == clsGlobal.FilePlayBackPortName))
                        {
                            PortManager target = (PortManager) PortManagerHash[str];
                            if (target != null)
                            {
                                target._SiRFAware = CreateSiRFawareWin(target);
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._SiRFAware = CreateSiRFawareWin(manager2);
                    }
                }
                updateSiRFawareViewBtn();
            }
        }

        private void siRFawareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            siRFawareClickHandler();
        }

        private void siRFDRiveSensorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DRSensorDataClick();
        }

        private void siRFDRiveStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DRNavStatusClick();
        }

        private void sPAzMenu_Click(object sender, EventArgs e)
        {
            CreateSPAzCtrlWindow();
        }

        private void standardTestToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (GetLoopitStatus() && clsGlobal.ScriptDone)
            {
                autoTestLoopitToolStripMenuItem.Text = "Stop Loopit...";
                autoTestLoopitToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                autoTestLoopitToolStripMenuItem.Text = "Loopit...";
                autoTestLoopitToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void standardTestToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
        }

        public void StartLoopit(string resetType, int iteration, uint timeout, bool earlyCompletion)
        {
            if (!clsGlobal.LoopitInProgress)
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    if (MessageBox.Show(string.Format("{0}\n{1}\n\t{3}", "For position accuracy evaluation, reference position must be set correctly.", "Setting reference position: Receiver --> Set Reference Position.", "Proceed?"), "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        return;
                    }
                    PortManager manager = (PortManager) PortManagerHash[str];
                    clsGlobal.LoopitInProgress |= manager.comm.RxCtrl.ResetCtrl.LoopitInprogress;
                }
                if (timeout < 10)
                {
                    timeout = 10;
                }
                clsGlobal.LoopitIteration = iteration;
                clsGlobal.LoopitResetType = resetType;
                clsGlobal.LoopitTimeout = timeout;
                clsGlobal.LoopitEarlyTermination = earlyCompletion;
                new frmLoopit().StartLoopit();
                clsGlobal.LoopitInProgress = true;
            }
        }

        private void staticNavClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateStaticNavWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateStaticNavWin(ref target);
                    }
                }
            }
        }

        private void staticNavToolStripMenuItem_Click(object sender, EventArgs e)
        {
            staticNavClickHandler();
        }

        public void StopLoopit()
        {
            foreach (string str in PortManagerHash.Keys)
            {
                EventHandler method = null;
                PortManager thisComWin;
                if (str != clsGlobal.FilePlayBackPortName)
                {
                    thisComWin = (PortManager) PortManagerHash[str];
                    if ((thisComWin != null) && thisComWin.comm.IsSourceDeviceOpen())
                    {
                        thisComWin.comm.RxCtrl.ResetCtrl.LoopitInprogress = false;
                        thisComWin.comm.RxCtrl.ResetCtrl.ResetTimerStop(false);
                        thisComWin.comm.RxCtrl.ResetCtrl.ResetTimerClose();
                        thisComWin.comm.RxCtrl.ResetCtrl.ResetCount = thisComWin.comm.RxCtrl.ResetCtrl.TotalNumberOfResets + 1;
                        thisComWin.comm.RxCtrl.ResetCtrl.CloseTTFFLog();
                        thisComWin.comm.Log.CloseFile();
                        thisComWin.comm.ListenersCtrl.Stop();
                        if ((thisComWin.DebugView != null) && thisComWin.DebugViewLocation.IsOpen)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    thisComWin.DebugView.Text = "Loopit Aborted -- " + thisComWin.comm.WindowTitle;
                                };
                            }
                            thisComWin.DebugView.Invoke(method);
                        }
                    }
                }
            }
            Text = clsGlobal.SiRFLiveVersion;
            clsGlobal.LoopitInProgress = false;
        }

        private string StripDollarSigns(string inStr)
        {
            return inStr.Split(new char[] { '$' })[1];
        }

        private void SVCNoViewClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target._svsCNoPanel = CreateSVCNoWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._svsCNoPanel = CreateSVCNoWin(manager2);
                    }
                }
                updateAvgCNoViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void sVTrackedVsTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVTrackedVsTimeViewClickHandler();
        }

        private void SVTrackedVsTimeViewClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target._svsTrackedVsTimePanel = CreateSVTrackedVsTimeWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._svsTrackedVsTimePanel = CreateSVTrackedVsTimeWin(manager2);
                    }
                }
                updateTrackedVsTimeViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void sVTrajectoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SVTrajViewClickHandler();
        }

        private void SVTrajViewClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target._svsTrajPanel = CreateSVTrajWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._svsTrajPanel = CreateSVTrajWin(manager2);
                    }
                }
                updateSVTrajViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void switchOperationModeClickHandler()
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateSwitchOperationModeWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateSwitchOperationModeWin(ref target);
                    }
                }
            }
        }

        private void switchOperationModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchOperationModeClickHandler();
        }

        private void switchPowerModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lowPowerModeClickHandler();
        }

        private void switchProtocolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    if (createSwitchProtocolWindow() != DialogResult.Cancel)
                    {
                        Thread.Sleep(500);
                        PortManager manager = null;
                        foreach (string str in PortManagerHash.Keys)
                        {
                            manager = (PortManager) PortManagerHash[str];
                            if (((manager != null) && manager.comm.IsSourceDeviceOpen()) && (manager.comm.ProductFamily == CommonClass.ProductType.GSD4e))
                            {
                                manager.comm.SwitchProtocol();
                                localUpdateStatusString(manager.comm.PortName);
                            }
                        }
                        Thread.Sleep(200);
                        menuBtnInit();
                    }
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (((manager2 != null) && manager2.comm.IsSourceDeviceOpen()) && ((manager2.comm.ProductFamily == CommonClass.ProductType.GSD4e) && (createSwitchProtocolWindow(ref manager2.comm) != DialogResult.Cancel)))
                    {
                        Thread.Sleep(500);
                        manager2.comm.SwitchProtocol();
                        localUpdateStatusString(manager2.comm.PortName);
                        Thread.Sleep(200);
                        menuBtnInit();
                    }
                }
            }
        }

        private void testRackMenu_Click(object sender, EventArgs e)
        {
            try
            {
                CreateTestRackCtrl();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR!");
            }
        }

        private void tileHorizontalMenu_Click(object sender, EventArgs e)
        {
            base.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tileVerticalMenu_Click(object sender, EventArgs e)
        {
            base.LayoutMdi(MdiLayout.TileVertical);
        }

        private void toolStripBackBtn_Click(object sender, EventArgs e)
        {
            if (_playState != _playStates.BACKWARD)
            {
                _lastPlayState = _playState;
                _playState = _playStates.BACKWARD;
                Text = string.Format("{0}: -- File Playback Backward ", clsGlobal.SiRFLiveVersion);
                filePlayBackTrackBar.Enabled = false;
                updateFilePlaybackPauseBtn(false);
            }
        }

        private void toolStripCloseFileBtn_Click(object sender, EventArgs e)
        {
            fileReplayCloseActionHandler();
        }

        private void toolStripCompassViewBtn_Click(object sender, EventArgs e)
        {
            compassViewClickHandler();
        }

        private void toolStripDebugViewBtn_Click(object sender, EventArgs e)
        {
            debugViewManualClickHandler();
        }

        private void toolStripErrorViewBtn_Click(object sender, EventArgs e)
        {
            errorViewManualClickHandler();
        }

        private void toolStripHelpBtn_Click(object sender, EventArgs e)
        {
            openHelpManual();
        }

        private void toolStripMapViewBtn_Click(object sender, EventArgs e)
        {
            locationMapManualClick();
        }

        private void toolStripMenuItem_Plot_Click(object sender, EventArgs e)
        {
            CreateFilePlotWindow();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CreateFileExtractWindow();
        }

        private void toolStripMessageViewBtn_Click(object sender, EventArgs e)
        {
            MessageViewManualClickHandler();
        }

        private void toolStripNextBtn_Click(object sender, EventArgs e)
        {
            if (_playState != _playStates.FORWARD)
            {
                _lastPlayState = _playState;
                _playState = _playStates.FORWARD;
                Text = string.Format("{0}: -- File Playback Forward ", clsGlobal.SiRFLiveVersion);
                filePlayBackTrackBar.Enabled = false;
                updateFilePlaybackPauseBtn(false);
            }
        }

        private void toolStripOpenFileBtn_Click(object sender, EventArgs e)
        {
            filePlaybackOpenActionHandler();
        }

        private void toolStripPause_Click(object sender, EventArgs e)
        {
            _lastPlayState = _playState;
            if ((_playState != _playStates.IDLE) && (_playState != _playStates.QUIT))
            {
                if (_playState != _playStates.PAUSE)
                {
                    _playState = _playStates.PAUSE;
                    updateFilePlaybackPauseBtn(true);
                    filePlayBackTrackBar.Enabled = true;
                    Text = string.Format("{0}: -- File Playback Pause ", clsGlobal.SiRFLiveVersion);
                }
                else
                {
                    _playState = _playStates.PLAY;
                    updateFilePlaybackPauseBtn(false);
                    filePlayBackTrackBar.Enabled = false;
                    Text = string.Format("{0}: -- File Playback Play ", clsGlobal.SiRFLiveVersion);
                }
            }
        }

        private void toolStripPauseBtn_Click(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (!(toolStripPortComboBox.Text == "All"))
                {
                    if (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)
                    {
                        PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                        if ((target.comm != null) && target.comm.IsSourceDeviceOpen())
                        {
                            target.comm.DebugViewRTBDisplay.viewPause = !target.comm.DebugViewRTBDisplay.viewPause;
                            updatePauseBtn(target);
                            if ((target.DebugView != null) && target.DebugViewLocation.IsOpen)
                            {
                                target.DebugView.frmCommDebugViewPauseBtnImage();
                            }
                        }
                    }
                }
                else
                {
                    PortManager manager = null;
                    bool flag = false;
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (str != clsGlobal.FilePlayBackPortName)
                        {
                            manager = (PortManager) PortManagerHash[str];
                            if (((manager.comm != null) && manager.comm.IsSourceDeviceOpen()) && manager.comm.DebugViewRTBDisplay.viewPause)
                            {
                                manager.comm.DebugViewRTBDisplay.viewPause = !manager.comm.DebugViewRTBDisplay.viewPause;
                                updatePauseBtn(manager);
                                if ((manager.DebugView != null) && manager.DebugViewLocation.IsOpen)
                                {
                                    manager.DebugView.frmCommDebugViewPauseBtnImage();
                                }
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (!flag)
                    {
                        foreach (string str2 in PortManagerHash.Keys)
                        {
                            manager = (PortManager) PortManagerHash[str2];
                            if ((manager.comm != null) && manager.comm.IsSourceDeviceOpen())
                            {
                                updatePauseBtn(manager);
                                if ((manager.DebugView != null) && manager.DebugViewLocation.IsOpen)
                                {
                                    manager.DebugView.frmCommDebugViewPauseBtnImage();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void toolStripPlayBtn_Click(object sender, EventArgs e)
        {
            if ((_isFileOpen || (fileReplayOpenHandler() != DialogResult.Cancel)) && (_playState != _playStates.PLAY))
            {
                if (_playState != _playStates.PAUSE)
                {
                    PortManager manager = (PortManager) PortManagerHash[clsGlobal.FilePlayBackPortName];
                    if (((manager != null) && (manager._SiRFAware != null)) && manager.SiRFawareLocation.IsOpen)
                    {
                        manager._SiRFAware.SirfawareGUIInit();
                    }
                }
                _lastPlayState = _playState;
                _playState = _playStates.PLAY;
                Text = string.Format("{0}: File Playback Play", clsGlobal.SiRFLiveVersion);
                filePlayBackTrackBar.Enabled = false;
                updateFilePlaybackPauseBtn(false);
            }
        }

        private void toolStripPortComboBox_DropDown(object sender, EventArgs e)
        {
            foreach (string str in PortManagerHash.Keys)
            {
                updateToolStripPortComboBox(str, true);
            }
            toolStripNumPortTxtBox.Text = PortManagerHash.Count.ToString();
        }

        private void toolStripPortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                string str = toolStripPortComboBox.SelectedItem.ToString();
                if (!(str == "All"))
                {
                    PortManager tmpP = (PortManager) PortManagerHash[str];
                    if (tmpP != null)
                    {
                        updateGUIOnConnectNDisconnect(tmpP);
                        setPortSubWindowsLayout(tmpP.comm.PortName);
                        if (tmpP.comm.MessageProtocol == "NMEA")
                        {
                            enableDisableMenuAndButtonPerProtocol(false);
                        }
                        else
                        {
                            enableDisableMenuAndButtonPerProtocol(true);
                        }
                    }
                }
                else
                {
                    foreach (string str2 in PortManagerHash.Keys)
                    {
                        PortManager manager = (PortManager) PortManagerHash[str2];
                        if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            UpdateConnectBtnImage(manager.comm);
                            break;
                        }
                    }
                    localUpdateStatusString("All");
                    setPortSubWindowsLayout("All");
                }
            }
        }

        private void toolStripPortConfigBtn_Click(object sender, EventArgs e)
        {
            portSettings();
        }

        private void toolStripPortOpenBtn_Click(object sender, EventArgs e)
        {
            frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            connectHandler();
        }

        private void toolStripRadarViewBtn_Click(object sender, EventArgs e)
        {
            radarViewManualClickHandler();
        }

        private void toolStripResetBtn_Click(object sender, EventArgs e)
        {
            resetClickHandler();
        }

        private void toolStripResponseViewBtn_Click(object sender, EventArgs e)
        {
            responseViewManualClickHandler();
        }

        private void toolStripSaveBtn_Click(object sender, EventArgs e)
        {
            logFileClickHandler();
        }

        private void toolStripSignalViewBtn_Click(object sender, EventArgs e)
        {
            signalViewManualClickHandler();
        }

        private void toolStripStopBtn_Click(object sender, EventArgs e)
        {
            if (_isFileOpen && (_playState != _playStates.IDLE))
            {
                _playState = _playStates.IDLE;
                Text = string.Format("{0}: File Playback Stop", clsGlobal.SiRFLiveVersion);
                filePlayBackTrackBar.Enabled = true;
                updateFilePlaybackPauseBtn(false);
            }
        }

        private void toolStripTTFFViewBtn_Click(object sender, EventArgs e)
        {
            ttffViewManualClickHandler();
        }

        private void toolStripUpDownArrowBtn_Click(object sender, EventArgs e)
        {
            if (PortManagerHash.Count > 1)
            {
                PortManager target = null;
                if (!_isExpand)
                {
                    _isExpand = true;
                    foreach (string str in PortManagerHash.Keys)
                    {
                        if (str != clsGlobal.FilePlayBackPortName)
                        {
                            target = (PortManager) PortManagerHash[str];
                            target.PerPortToolStrip.Visible = true;
                            PerPortUpdateConnectBtnImage(target);
                            updatePerPortNameComboBox(target);
                            UpdateAllPortSubWinTitle(target);
                        }
                    }
                    toolStripUpDownArrowBtn.Image = Resources.FillUpHS;
                    toolStripUpDownArrowBtn.Text = "Compress";
                }
                else
                {
                    _isExpand = false;
                    foreach (string str2 in PortManagerHash.Keys)
                    {
                        target = (PortManager) PortManagerHash[str2];
                        target.PerPortToolStrip.Visible = false;
                        PerPortUpdateConnectBtnImage(target);
                        UpdateAllPortSubWinTitle(target);
                    }
                    toolStripUpDownArrowBtn.Image = Resources.FillDownHS;
                    toolStripUpDownArrowBtn.Text = "Display All";
                }
            }
        }

        private void toolStripUserText_Click(object sender, EventArgs e)
        {
            if (toolStripPortComboBox.Text != string.Empty &&
				toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName &&
				PortManagerHash.Count > 0
				)
            {
                frmCommonSimpleInput input = new frmCommonSimpleInput("Enter User Text");
                input.updateParent += new frmCommonSimpleInput.updateParentEventHandler(writeUserText);
                input.ShowDialog();
            }
        }

        private void TTBConfigureTimeAidingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolStripPortComboBox.Text != string.Empty && PortManagerHash.Count > 0)
            {
                if (toolStripPortComboBox.Text == "All")
                    MessageBox.Show("Configuring multiple TTBs is not supported in this version.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                        CreateTTBTimeAidCfgWindow(target);
                }
            }
        }

        private void TTBConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                    MessageBox.Show("Connecting to multiple TTBs is not supported in this version.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                        CreateTTBConnectWindow(target);
                }
            }
        }

        private void TTBResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void TTBViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                    MessageBox.Show("Configuring multiple TTBs is not supported in this version.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (target != null && target.comm.IsSourceDeviceOpen())
                        rxTTBViewMenuClickHandler(target);
                }
            }
        }

        private void tTFFAndNavAccuracyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ttffViewManualClickHandler();
        }

        private void ttffViewManualClickHandler()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target._ttffDisplay = CreateTTFFWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2._ttffDisplay = CreateTTFFWin(manager2);
                    }
                }
                updateTTFFViewBtn();
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private void tTFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ttfsViewManualClickHanlder();
        }

        private void ttfsViewManualClickHanlder()
        {
            if ((toolStripPortComboBox.Text != string.Empty) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    foreach (string str in PortManagerHash.Keys)
                    {
                        PortManager target = (PortManager) PortManagerHash[str];
                        if (target != null)
                        {
                            target.TTFSView = CreateTTFSWin(target);
                        }
                    }
                }
                else
                {
                    PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if (manager2 != null)
                    {
                        manager2.TTFSView = CreateTTFSWin(manager2);
                    }
                }
                frmSaveSettingsOnClosing(_lastWindowsRestoredFilePath);
            }
        }

        private bool update5HzNavMenu(CommunicationManager targetComm)
        {
            bool flag = false;
            try
            {
                if (targetComm.IsSourceDeviceOpen())
                {
                    targetComm.RxCtrl.PollNavigationParameters();
                    if ((targetComm.NavigationParamrters.FiveHzNavMode & 1) == 1)
                    {
                        set5HzNavMenu(true);
                        return true;
                    }
                    set5HzNavMenu(false);
                    return false;
                }
                flag = false;
                set5HzNavMenu(false);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        private bool updateABPMenu(CommunicationManager targetComm)
        {
            bool flag = false;
            try
            {
                if (targetComm.IsSourceDeviceOpen())
                {
                    targetComm.RxCtrl.PollNavigationParameters();
                    if ((targetComm.NavigationParamrters.ABMMode & 1) == 1)
                    {
                        setABPMenu(true);
                        return true;
                    }
                    setABPMenu(false);
                    return false;
                }
                flag = false;
                setABPMenu(false);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        private void updateAllMainBtn()
        {
            updateLogFileBtn();
            updateSignalViewBtn();
            updateSVsMapViewBtn();
            updateLocationMapViewBtn();
            updateTTFFViewBtn();
            updateResponseViewBtn();
            updateDebugViewBtn();
            updateErrorViewBtn();
            updateMessageViewBtn();
            updateInterferenceViewBtn();
            updateSatellitesStatsViewBtn();
            updateSiRFawareViewBtn();
            updateMEMSViewBtn();
            updateCompassViewBtn();
            updateAvgCNoViewBtn();
            updateSVTrajViewBtn();
            updateTrackedVsTimeViewBtn();
            updateNavVsTimeBtn();
        }

        public void UpdateAllPortSubWinTitle(PortManager target)
        {
            if (target != null)
            {
                updateSignalViewTitle(target);
                updateSVsMapViewTitle(target);
                updateLocationMapViewTitle(target);
                updateTTFFViewTitle(target);
                updateResponseViewTitle(target);
                updateDebugViewTitle(target);
                updateErrorViewTitle(target);
                updateInterferenceViewTitle(target);
                updateSatellitesStatsViewTitle(target);
                updateSiRFawareViewTitle(target);
                updateInputCommandTitle(target);
                updateTransmitMessageTitle(target);
                updateSVsTrajViewTitle(target);
                updateSVsCNoViewTitle(target);
                updateSVsTrackedVsTimeViewTitle(target);
            }
        }

        private void updateAvgCNoViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateAvgCNoViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateAvgCNoViewBtn();
            }
        }

        private void updateAvgCNoViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateAvgCNoViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateAvgCNoViewImage(state);
            }
        }

        private void updateCompassViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateCompassViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateCompassViewBtn();
            }
        }

        private void updateCompassViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateCompassViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateCompassViewImage(state);
            }
        }

        public void UpdateConnectBtnImage(CommunicationManager targetComm)
        {
            EventHandler method = null;
            if (targetComm != null)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            localUpdateConnectBtnImage(targetComm);
                        };
                    }
                    base.BeginInvoke(method);
                }
                else
                {
                    localUpdateConnectBtnImage(targetComm);
                }
            }
        }

        public void UpdateDataFromScript()
        {
            base.BeginInvoke((MethodInvoker)delegate {
                int num = 0;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (str != clsGlobal.FilePlayBackPortName)
                    {
                        num++;
                        PortManager manager = (PortManager) PortManagerHash[str];
                        if (manager != null)
                        {
                            if (manager.PerPortToolStrip == null)
                            {
                                manager.PerPortToolStrip = AddPortToolbar((toolStripMain.Location.Y + (0x19 * num)) + 0x23, manager.comm.PortName);
                                localUpdateStatusString(manager.comm.PortName);
                            }
                            if (!toolStripPortComboBox.Items.Contains(str))
                            {
                                toolStripPortComboBox.Items.Add(str);
                            }
                            if (manager.comm != null)
                            {
                                manager.comm.Log.DurationLoggingStatusLabel = logManagerStatusLabel;
                                manager.comm.Log.UpdateMainWindow += new LogManager.UpdateParentEventHandler(updateLogFileBtn);
                                manager.comm.UpdatePortMainWinTitle += new CommunicationManager.UpdateParentPortEventHandler(UpdateWinTitle);
                            }
                        }
                    }
                }
                toolStripNumPortTxtBox.Text = PortManagerHash.Count.ToString();
            });
        }

        private void updateDebugViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateDebugViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateDebugViewBtn();
            }
        }

        private void updateDebugViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateDebugViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateDebugViewImage(state);
            }
        }

        private void updateDebugViewTitle(PortManager target)
        {
            if ((target != null) && (target.DebugViewLocation.IsOpen && (target.DebugView != null)))
            {
                EventHandler method = null;
                string titleStr = target.comm.sourceDeviceName + ": Debug View ";
                if ((target.comm.RxCtrl != null) && (target.comm.RxCtrl.ResetCtrl != null))
                {
                    titleStr = titleStr + target.comm.RxCtrl.ResetCtrl.ResetRxSwVersion;
                }
                if (target.DebugView.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target.DebugView.Text = titleStr;
                        };
                    }
                    target.DebugView.BeginInvoke(method);
                }
                else
                {
                    target.DebugView.Text = titleStr;
                }
            }
        }

        private void updateDisableMEMSViewBtn()
        {
            updateDisableMEMSViewImage(true);
            updateEnableMEMSViewImage(false);
        }

        private void updateDisableMEMSViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateDisableMEMSViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateDisableMEMSViewImage(state);
            }
        }

        private void updateDisableSBASRangingViewBtn()
        {
            updateDisableSBASRangingViewImage(true);
            updateEnableSBASRangingViewImage(false);
        }

        private void updateDisableSBASRangingViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateDisableSBASRangingViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateDisableSBASRangingViewImage(state);
            }
        }

        private void updateDRSensorDataViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateDRSensorDataViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateDRSensorDataViewBtn();
            }
        }

        private void updateDRSensorDataViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateDRSensorDataViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateDRSensorDataViewImage(state);
            }
        }

        private void updateDRStatusViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateDRStatusViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateDRStatusViewBtn();
            }
        }

        private void updateDRStatusViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateDRStatusViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateDRStatusViewImage(state);
            }
        }

        private void updateEnableMEMSViewBtn()
        {
            if ((toolStripPortComboBox.Text == string.Empty) || (PortManagerHash.Count <= 0))
            {
                updateEnableMEMSViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    if (((PortManager) PortManagerHash[str]) != null)
                    {
                        updateEnableMEMSViewImage(true);
                        updateDisableMEMSViewImage(false);
                        return;
                    }
                }
                updateEnableMEMSViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateEnableMEMSViewImage(true);
                    updateDisableMEMSViewImage(false);
                }
            }
        }

        private void updateEnableMEMSViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateEnableMEMSViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateEnableMEMSViewImage(state);
            }
        }

        private void updateEnableSBASRangingBtn()
        {
            if ((toolStripPortComboBox.Text == string.Empty) || (PortManagerHash.Count <= 0))
            {
                updateEnableSBASRangingViewImage(false);
            }
            else if (toolStripPortComboBox.Text == "All")
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    if (((PortManager) PortManagerHash[str]) != null)
                    {
                        updateEnableSBASRangingViewImage(true);
                        updateDisableSBASRangingViewImage(false);
                        return;
                    }
                }
                updateEnableSBASRangingViewImage(false);
            }
            else
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2 != null) && (manager2.comm != null))
                {
                    updateEnableSBASRangingViewImage(true);
                    updateDisableSBASRangingViewImage(false);
                }
            }
        }

        private void updateEnableSBASRangingViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateEnableSBASRangingViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateEnableSBASRangingViewImage(state);
            }
        }

        private void updateErrorViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateErrorViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateErrorViewBtn();
            }
        }

        private void updateErrorViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateErrorViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateErrorViewImage(state);
            }
        }

        private void updateErrorViewTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.ErrorViewLocation.IsOpen && (target._errorView != null)))
            {
                if (target._errorView.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._errorView.Text = target.comm.sourceDeviceName + ": Error View";
                        };
                    }
                    target._errorView.BeginInvoke(method);
                }
                else
                {
                    target._errorView.Text = target.comm.sourceDeviceName + ": Error View";
                }
            }
        }

        internal void updateFileCovAvail()
        {
            gP2GPSToolStripMenuItem.Enabled = true;
            binGPSToolStripMenuItem.Enabled = true;
            gPSNMEAToolStripMenuItem.Enabled = true;
            NMEAtoGPStoolStripMenuItem.Enabled = true;
            gPSToKMLToolStripMenuItem.Enabled = true;
        }

        private void updateFilePath(string filename)
        {
            if ((filename != null) && (filename != string.Empty))
            {
                _userWindowsRestoredFilePath = ConfigurationManager.AppSettings["InstalledDirectory"] + @"\Config\UserData\" + filename.Replace(" ", "") + ".xml";
                frmSaveSettingsOnClosing(_userWindowsRestoredFilePath);
            }
        }

        private void updateFilePlaybackBtn(bool state)
        {
            toolStripBackBtn.Enabled = state;
            toolStripPause.Enabled = state;
            toolStripStopBtn.Enabled = state;
            toolStripNextBtn.Enabled = state;
            toolStripCloseFileBtn.Enabled = state;
            fileCloseToolStripMenuItem.Enabled = state;
            receiverConnectToolStripMenuItem.Enabled = !state;
            toolStripPortConfigBtn.Enabled = !state;
            toolStripPortOpenBtn.Enabled = !state;
            if (!state)
            {
                bool flag = false;
                try
                {
                    toolStripPortComboBox.Items.Remove(clsGlobal.FilePlayBackPortName);
                    if (PortManagerHash.ContainsKey(clsGlobal.FilePlayBackPortName))
                    {
                        PortManagerHash.Remove(clsGlobal.FilePlayBackPortName);
                    }
                }
                catch
                {
                }
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(str == clsGlobal.FilePlayBackPortName))
                    {
                        flag = true;
                        toolStripPortComboBox.Text = str;
                        break;
                    }
                }
                if (!flag)
                {
                    toolStripPortComboBox.Text = string.Empty;
                }
            }
        }

        private void updateFilePlaybackPauseBtn(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateFilePlaybackPauseBtn(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateFilePlaybackPauseBtn(state);
            }
        }

        public void UpdateGUIFromScript()
        {
            foreach (string str in PortManagerHash.Keys)
            {
                PortManager tmpP = (PortManager) PortManagerHash[str];
                if (tmpP != null)
                {
                    updateGUIOnConnectNDisconnect(tmpP);
                    updateAllMainBtn();
                    menuBtnInit();
                    if (tmpP.comm.IsSourceDeviceOpen())
                    {
                        break;
                    }
                }
            }
        }

        private void updateGUIOnConnectNDisconnect(PortManager tmpP)
        {
            if (tmpP != null)
            {
                UpdateConnectBtnImage(tmpP.comm);
                PerPortUpdateConnectBtnImage(tmpP);
                UpdateAllPortSubWinTitle(tmpP);
                UpdateStatusString(tmpP.comm.PortName);
            }
            else
            {
                UpdateConnectBtnImage(null);
            }
        }

        private void updateInputCommandTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.InputCommandLocation.IsOpen && (target._inputCommands != null)))
            {
                if (target._inputCommands.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._inputCommands.Text = target.comm.sourceDeviceName + ": Predefined Message";
                        };
                    }
                    target._inputCommands.BeginInvoke(method);
                }
                else
                {
                    target._inputCommands.Text = target.comm.sourceDeviceName + ": Predefined Message";
                }
            }
        }

        private void updateInterferenceViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateInterferenceViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateInterferenceViewBtn();
            }
        }

        private void updateInterferenceViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateInterferenceViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateInterferenceViewImage(state);
            }
        }

        private void updateInterferenceViewTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.InterferenceLocation.IsOpen && (target._interferenceReport != null)))
            {
                if (target._interferenceReport.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._interferenceReport.Text = target.comm.sourceDeviceName + ": Interference Detection";
                        };
                    }
                    target._interferenceReport.BeginInvoke(method);
                }
                else
                {
                    target._interferenceReport.Text = target.comm.sourceDeviceName + ": Interference Detection";
                }
            }
        }

        private void updateLocationMapViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateLocationMapViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateLocationMapViewBtn();
            }
        }

        private void updateLocationMapViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateLocationMapViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateLocationMapViewImage(state);
            }
        }

        private void updateLocationMapViewTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.LocationMapLocation.IsOpen && (target._locationViewPanel != null)))
            {
                if (target._locationViewPanel.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._locationViewPanel.Text = target.comm.sourceDeviceName + ": Location View";
                        };
                    }
                    target._locationViewPanel.BeginInvoke(method);
                }
                else
                {
                    target._locationViewPanel.Text = target.comm.sourceDeviceName + ": Location View";
                }
            }
        }

        private void updateLogFileBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateLogFileBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateLogFileBtn();
            }
        }

        private void updateLogFileImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateLogFileImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateLogFileImage(state);
            }
        }

        private void updateMainWindowTitle(string title)
        {
            EventHandler method = null;
            if (title != string.Empty)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            Text = string.Format("{0}: {1}", clsGlobal.SiRFLiveVersion, title);
                        };
                    }
                    base.BeginInvoke(method);
                }
                else
                {
                    Text = string.Format("{0}: {1}", clsGlobal.SiRFLiveVersion, title);
                }
            }
            updateAllMainBtn();
        }

        private void updateMEMSViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateMEMSViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateMEMSViewBtn();
            }
        }

        private void updateMEMSViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateMEMSViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateMEMSViewImage(state);
            }
        }

        private void updateMessageViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateMessageViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateMessageViewBtn();
            }
        }

        private void updateMessageViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateMessageViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateMessageViewImage(state);
            }
        }

        private void updateMessageViewTitle(PortManager target)
        {
            EventHandler method = null;
            if (target.MessageViewLocation.IsOpen && (target.MessageView != null))
            {
                if (target.MessageView.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target.MessageView.Text = target.comm.sourceDeviceName + ": Mesage View";
                        };
                    }
                    target.MessageView.BeginInvoke(method);
                }
                else
                {
                    target._signalStrengthPanel.Text = target.comm.sourceDeviceName + ": Message View";
                }
            }
        }

        private void updateNavVsTimeBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateNavVsTimeBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateNavVsTimeBtn();
            }
        }

        private void updateNavVsTimeImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateNavVsTimeImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateNavVsTimeImage(state);
            }
        }

        private void updateNavVsTimeTitle(PortManager target)
        {
            if ((target != null) && (target.NavVsTimeLocation.IsOpen && (target.DebugView != null)))
            {
                EventHandler method = null;
                string titleStr = target.comm.sourceDeviceName + ": Nav Accuracy vs Time View ";
                if (target.NavVsTimeView.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target.NavVsTimeView.Text = titleStr;
                        };
                    }
                    target.NavVsTimeView.BeginInvoke(method);
                }
                else
                {
                    target.NavVsTimeView.Text = titleStr;
                }
            }
        }

        private void updatePauseBtn(PortManager target)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        locaUpdatePauseBtn(target);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                locaUpdatePauseBtn(target);
            }
        }

        private void updatePerPortNameComboBox(PortManager target)
        {
            EventHandler method = null;
            if (target != null)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            localUpdatePerPortNameComboBox(target);
                        };
                    }
                    base.BeginInvoke(method);
                }
                else
                {
                    localUpdatePerPortNameComboBox(target);
                }
            }
        }

        private void updateResponseViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateResponseViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateResponseViewBtn();
            }
        }

        private void updateResponseViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateResponseViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateResponseViewImage(state);
            }
        }

        private void updateResponseViewTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.ResponseViewLocation.IsOpen && (target._responseView != null)))
            {
                if (target._responseView.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._responseView.Text = target.comm.sourceDeviceName + ": Response View";
                        };
                    }
                    target._responseView.BeginInvoke(method);
                }
                else
                {
                    target._responseView.Text = target.comm.sourceDeviceName + ": Response View";
                }
            }
        }

        private void updateSatellitesStatsViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateSatellitesStatsViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateSatellitesStatsViewBtn();
            }
        }

        private void updateSatellitesStatsViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateSatellitesStatsViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateSatellitesStatsViewImage(state);
            }
        }

        private void updateSatellitesStatsViewTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.SatelliteStatsLocation.IsOpen && (target._SatelliteStats != null)))
            {
                if (target._SatelliteStats.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._SatelliteStats.Text = target.comm.sourceDeviceName + ": Satellite Statistics";
                        };
                    }
                    target._SatelliteStats.BeginInvoke(method);
                }
                else
                {
                    target._SatelliteStats.Text = target.comm.sourceDeviceName + ": Satellite Statistics";
                }
            }
        }

        private bool updateSBASRangingMenu(CommunicationManager targetComm)
        {
            bool flag = false;
            try
            {
                if (targetComm.IsSourceDeviceOpen())
                {
                    targetComm.RxCtrl.PollNavigationParameters();
                    if (targetComm.NavigationParamrters.SBASRangingMode == 1)
                    {
                        setSBASRangingMenu(true);
                        return true;
                    }
                    setSBASRangingMenu(false);
                    return false;
                }
                flag = false;
                setSBASRangingMenu(false);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        private void updateSignalViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateSignalViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateSignalViewBtn();
            }
        }

        private void updateSignalViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateSignalViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateSignalViewImage(state);
            }
        }

        private void updateSignalViewTitle(PortManager target)
        {
            if (target != null &&
				target.SignalViewLocation.IsOpen &&
				target._signalStrengthPanel != null
				)
            {
                if (target._signalStrengthPanel.InvokeRequired)
                {
                    target._signalStrengthPanel.BeginInvoke((EventHandler)delegate {
                            target._signalStrengthPanel.Text = target.comm.sourceDeviceName + ": Signal View";
                        });
                }
                else
                    target._signalStrengthPanel.Text = target.comm.sourceDeviceName + ": Signal View";
            }
        }

        private void updateSiRFawareViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateSiRFawareViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateSiRFawareViewBtn();
            }
        }

        private void updateSiRFawareViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateSiRFawareViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateSiRFawareViewImage(state);
            }
        }

        private void updateSiRFawareViewTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.SiRFawareLocation.IsOpen && (target._SiRFAware != null)))
            {
                if (target._SiRFAware.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._SiRFAware.Text = target.comm.sourceDeviceName + ": SiRFaware";
                        };
                    }
                    target._SiRFAware.BeginInvoke(method);
                }
                else
                {
                    target._SiRFAware.Text = target.comm.sourceDeviceName + ": SiRFaware";
                }
            }
        }

        public void UpdateStatusString(string portName)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateStatusString(portName);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateStatusString(portName);
            }
        }

        private void updateSVsCNoViewTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.SVCNoViewLocation.IsOpen && (target._svsCNoPanel != null)))
            {
                if (target._svsCNoPanel.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._svsCNoPanel.Text = target.comm.sourceDeviceName + ": SV Average CNo View";
                        };
                    }
                    target._svsCNoPanel.BeginInvoke(method);
                }
                else
                {
                    target._svsCNoPanel.Text = target.comm.sourceDeviceName + ": SV Average CNo View";
                }
            }
        }

        private void updateSVsMapViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateSVsMapViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateSVsMapViewBtn();
            }
        }

        private void updateSVsMapViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateSVsMapViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateSVsMapViewImage(state);
            }
        }

        private void updateSVsMapViewTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.SVsMapLocation.IsOpen && (target._svsMapPanel != null)))
            {
                if (target._svsMapPanel.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._svsMapPanel.Text = target.comm.sourceDeviceName + ": Radar View";
                        };
                    }
                    target._svsMapPanel.BeginInvoke(method);
                }
                else
                {
                    target._svsMapPanel.Text = target.comm.sourceDeviceName + ": Radar View";
                }
            }
        }

        private void updateSVsTrackedVsTimeViewTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.SVTrackedVsTimeViewLocation.IsOpen && (target._svsTrackedVsTimePanel != null)))
            {
                if (target._svsTrackedVsTimePanel.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._svsTrackedVsTimePanel.Text = target.comm.sourceDeviceName + ": SV Tracked vs Time View";
                        };
                    }
                    target._svsTrackedVsTimePanel.BeginInvoke(method);
                }
                else
                {
                    target._svsTrackedVsTimePanel.Text = target.comm.sourceDeviceName + ": SV Tracked vs Time View";
                }
            }
        }

        private void updateSVsTrajViewTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.SVTrajViewLocation.IsOpen && (target._svsTrajPanel != null)))
            {
                if (target._svsTrajPanel.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._svsTrajPanel.Text = target.comm.sourceDeviceName + ": SV Trajectory View";
                        };
                    }
                    target._svsTrajPanel.BeginInvoke(method);
                }
                else
                {
                    target._svsTrajPanel.Text = target.comm.sourceDeviceName + ": SV Trajectory View";
                }
            }
        }

        private void updateSVTrajViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateSVTrajViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateSVTrajViewBtn();
            }
        }

        private void updateSVTrajViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateSVTrajViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateSVTrajViewImage(state);
            }
        }

        private void updateToolStripPortComboBox(string key, bool add)
        {
            if (!toolStripPortComboBox.Items.Contains(key))
            {
                if (add)
                {
                    toolStripPortComboBox.Items.Add(key);
                    toolStripPortComboBox.Text = key;
                }
            }
            else if (!add)
            {
                toolStripPortComboBox.Items.Remove(key);
                if (toolStripPortComboBox.Items.Count >= 1)
                {
                    toolStripPortComboBox.Text = toolStripPortComboBox.Items[0].ToString();
                }
            }
            if ((PortManagerHash.Count > 1) && !toolStripPortComboBox.Items.Contains("All"))
            {
                toolStripPortComboBox.Items.Add("All");
            }
        }

        public void UpdateToolStripPortComboBoxItems(bool setTextToAll)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateToolStripPortComboBoxItems(setTextToAll);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateToolStripPortComboBoxItems(setTextToAll);
            }
        }

        private void updateTrackedVsTimeViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateTrackedVsTimeViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateTrackedVsTimeViewBtn();
            }
        }

        private void updateTrackedVsTimeViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateTrackedVsTimeViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateTrackedVsTimeViewImage(state);
            }
        }

        private void updateTransmitMessageTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.TransmitSerialMessageLocation.IsOpen && (target._transmitSerialMessageWin != null)))
            {
                if (target._transmitSerialMessageWin.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._transmitSerialMessageWin.Text = target.comm.sourceDeviceName + ": User Defined Message";
                        };
                    }
                    target._transmitSerialMessageWin.BeginInvoke(method);
                }
                else
                {
                    target._transmitSerialMessageWin.Text = target.comm.sourceDeviceName + ": User Defined Message";
                }
            }
        }

        private void updateTTFFViewBtn()
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateTTFFViewBtn();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateTTFFViewBtn();
            }
        }

        private void updateTTFFViewImage(bool state)
        {
            EventHandler method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        localUpdateTTFFViewImage(state);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                localUpdateTTFFViewImage(state);
            }
        }

        private void updateTTFFViewTitle(PortManager target)
        {
            EventHandler method = null;
            if ((target != null) && (target.TTFFDisplayLocation.IsOpen && (target._ttffDisplay != null)))
            {
                if (target._ttffDisplay.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            target._ttffDisplay.Text = target.comm.sourceDeviceName + ": TTFF/Nav Accuracy";
                        };
                    }
                    target._ttffDisplay.BeginInvoke(method);
                }
                else
                {
                    target._ttffDisplay.Text = target.comm.sourceDeviceName + ": TTFF/Nav Accuracy";
                }
            }
        }

        internal void UpdateWinTitle(string sourcDevName, string title)
        {
            try
            {
                foreach (string str in PortManagerHash.Keys)
                {
                    EventHandler method = null;
                    PortManager tmpP = (PortManager) PortManagerHash[str];
                    if ((tmpP != null) && (str == sourcDevName))
                    {
                        if ((tmpP.DebugView != null) && tmpP.DebugViewLocation.IsOpen)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    tmpP.DebugView.Text = title;
                                };
                            }
                            tmpP.DebugView.BeginInvoke(method);
                        }
                        if (clsGlobal.IsMarketingUser())
                        {
                            updateMainWindowTitle(title);
                        }
                        return;
                    }
                }
            }
            catch
            {
            }
        }

        private void userDefinedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((toolStripPortComboBox.Text != string.Empty) && (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)) && (PortManagerHash.Count > 0))
            {
                if (toolStripPortComboBox.Text == "All")
                {
                    clsGlobal.PerformOnAll = true;
                    CreateTransmitSerialMessageWin();
                }
                else
                {
                    clsGlobal.PerformOnAll = false;
                    PortManager target = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                    if ((target != null) && target.comm.IsSourceDeviceOpen())
                    {
                        CreateTransmitSerialMessageWin(target);
                    }
                }
            }
        }

        private void userManualMenu_Click(object sender, EventArgs e)
        {
            openHelpManual();
        }

        private void userSettingsLayoutMenu_Click(object sender, EventArgs e)
        {
            restoreUserLayout();
        }

        private void viewToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if ((toolStripPortComboBox.Text != "All") && (toolStripPortComboBox.Text != string.Empty))
            {
                PortManager manager = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager != null) && manager.comm.IsSourceDeviceOpen())
                {
                    if (manager.comm.MessageProtocol == "NMEA")
                    {
                        enableDisableMenuAndButtonPerProtocol(false);
                    }
                    else
                    {
                        enableDisableMenuAndButtonPerProtocol(true);
                    }
                }
            }
        }

        private void windowMenuOpenComm_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in windowMenuItem.DropDownItems)
            {
                if (clsGlobal.CommWinRef.ContainsKey(item.Text))
                {
                    ((frmCommOpen) clsGlobal.CommWinRef[item.Text]).BringToFront();
                    break;
                }
            }
        }

        private void writeUserText(string inputText)
        {
            if (toolStripPortComboBox.Text == "All")
            {
                PortManager manager = null;
                foreach (string str in PortManagerHash.Keys)
                {
                    if (!(toolStripPortComboBox.Text == clsGlobal.FilePlayBackPortName))
                    {
                        manager = (PortManager) PortManagerHash[str];
                        if ((manager.comm != null) && manager.comm.IsSourceDeviceOpen())
                        {
                            manager.comm.WriteApp(inputText);
                        }
                    }
                }
            }
            else if (toolStripPortComboBox.Text != clsGlobal.FilePlayBackPortName)
            {
                PortManager manager2 = (PortManager) PortManagerHash[toolStripPortComboBox.Text];
                if ((manager2.comm != null) && manager2.comm.IsSourceDeviceOpen())
                {
                    manager2.comm.WriteApp(inputText);
                }
            }
        }

        public string SetupScriptsPath
        {
            get
            {
                return clsGlobal.InstalledDirectory;
            }
            set
            {
                clsGlobal.InstalledDirectory = value;
            }
        }

        public string TestScriptPath
        {
            get
            {
                return clsGlobal.TestScriptPath;
            }
            set
            {
                clsGlobal.TestScriptPath = value;
            }
        }

        private enum _playStates
        {
            IDLE,
            PLAY,
            PAUSE,
            FORWARD,
            BACKWARD,
            QUIT,
            UNKNOWN
        }

        internal delegate void updateParentEventHandler();
    }
}

