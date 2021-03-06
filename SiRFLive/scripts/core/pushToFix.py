import scriptGlobals
import scriptUtilities
import scriptSim

try:  
    # Read configuration parameters
    # Data in pair key and value
    sCfg = ConfigParser.ConfigParser()

    sCfg.read(scriptGlobals.ScriptConfigFilePath )

    logTime = sCfg.getint('TEST_PARAM', 'LOG_TIME')
    
    PTFPeriodArray = sCfg.get('POWER_PARAM', 'PTF_PERIOD')
    PTFMaxOffTimeArray = sCfg.get('POWER_PARAM', 'MAX_OFF_TIME')
    PTFMaxSearchTimeArray = sCfg.get('POWER_PARAM', 'MAX_SEARCH_TIME')
    
    # Convert string to array
    PTFPeriodArray = eval('['+PTFPeriodArray+']')
    PTFMaxOffTimeArray = eval('['+PTFMaxOffTimeArray+']')
    PTFMaxSearchTimeArray = eval('['+PTFMaxSearchTimeArray+']')
    
    testContinue = True;
    paramsLenArray = [len(PTFPeriodArray), len(PTFMaxOffTimeArray), len(PTFMaxSearchTimeArray)]
    if (scriptUtilities.areAllEqual(paramsLenArray) == False):
	print ("Error: Length of PTF parameters NOT equal ")
	scriptGlobals.TestAborted = True
    else:	
	if (scriptGlobals.SignalSource == General.clsGlobal.SIM):
	    if (scriptSim.isSimRunning() == True):
		result = MessageBoxEx.Show("SIM is running -- Proceed?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information,20000)
		if (result == DialogResult.Yes):
		    testContinue = True
		    scriptSim.simStop()
		    scriptSim.simLoad(scriptGlobals.SimFile)		
		    scriptSim.simRun()
		else:
		    testContinue = False
	    else:
		scriptSim.simLoad(scriptGlobals.SimFile)	    
		scriptSim.simRun()
	if (testContinue == True):
	    scriptGlobals.MainFrame.Delay(5)
	    # set to high level for factory reset
	    if (scriptGlobals.SignalType.ToLower() == "dbhz"):
		defaultAtten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,40,scriptGlobals.SignalType)
	    elif (scriptGlobals.SignalType.ToLower() == "dbm"):
		defaultAtten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,-130,scriptGlobals.SignalType)
	    else:
		print "Signal Type is not correct"
		defaultAtten = 0
	    # set atten
	    scriptUtilities.setAtten(defaultAtten)   
	    
	    # setup each active com ports
	    comIdx = 0	
	    for usePort in scriptGlobals.DutMainPortsList:	
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue
		myPort = scriptGlobals.DutPortManagersList[comIdx]
		
		# reset logfile name
		myPort.comm.Log.SetDurationLoggingStatusLabel("")
		if (myPort.comm.IsSourceDeviceOpen() == False):
		    if (myPort.comm.OpenPort() == False):
			portList[comIdx] = -1
			continue
		    myPort.RunAsyncProcess()
		if (myPort.SignalViewLocation.IsOpen == False):
		    mainFrame.CreateSignalViewWin(myPort)
		if (myPort.DebugViewLocation.IsOpen == False):
		    mainFrame.CreateDebugViewWin(myPort)
		myPort.comm.RxCtrl.OpenChannel("SSB")
		myPort.comm.RxCtrl.PollSWVersion()
		myPort.comm.ReadAutoReplyData(scriptGlobals.ScriptConfigFilePath)
		myPort.comm.RxCtrl.ResetCtrl.Reset(General.clsGlobal.HOT)
	
		comIdx = comIdx + 1
	    scriptUtilities.init()
	    mainFrame.UpdateGUIFromScript()
	    mainFrame.Delay(10)
	    # setup each active com ports
	    comIdx = 0	
	    for usePort in scriptGlobals.DutMainPortsList:	
		if (usePort < 0):
		    comIdx = comIdx + 1
		    continue
		myPort = scriptGlobals.DutPortManagersList[comIdx]
		myPort.comm.RxCtrl.OpenChannel("SSB")
		myPort.comm.RxCtrl.OpenChannel("STAT")
	
		comId = comIdx + 1
	    # Loop for PTF configuration
	    for PTFIndex in range(len(PTFPeriodArray)):
		PTFMaxOffTime = PTFMaxOffTimeArray[PTFIndex]
		PTFMaxSearchTime = PTFMaxSearchTimeArray[PTFIndex]
		PTFTBF = PTFPeriodArray[PTFIndex]
				
		# Loop for each test levels    
		for levelIndex in range(0, len(scriptGlobals.TestSignalLevelsList)):
		    level = scriptGlobals.TestSignalLevelsList[levelIndex]
		    if (scriptGlobals.TestAborted == True):
			print "Test aborted"
			break
		    atten = Utilities.HelperFunctions.GetCalibrationAtten(scriptGlobals.CableLoss,level,scriptGlobals.SignalType)
		    diffAtten = atten - defaultAtten
		    if ((levelIndex == 0) and (diffAtten > 5) and (atten > 5)):
			dropAtten = divmod(atten,5)
			drop5dBLoop = dropAtten[0]
			restAtten = dropAtten[1]
	    
			for dropIndex in range(0, int(drop5dBLoop)+1):
			    atten1 = 5*dropIndex + defaultAtten
			    scriptUtilities.setAtten(atten1)
			    mainFrame.Delay(20)
	    
			atten1 = restAtten +  atten1
			scriptUtilities.setAtten(atten1)
	    
		    else:
			scriptUtilities.setAtten(atten)
	    
		    # setup each active com ports
		    comIdx = 0	
		    for usePort in scriptGlobals.DutMainPortsList:	
			if (usePort < 0):
			    comIdx = comIdx + 1
			    continue
			myPort = scriptGlobals.DutPortManagersList[comIdx]
			myPort.comm.RxCtrl.OpenChannel("SSB")
			myPort.comm.RxCtrl.OpenChannel("STAT")
			myPort.comm.RxCtrl.PollSWVersion()
			# Create directory for log files
			Now = time.localtime(time.time())
			timeNowStr = time.strftime("%m%d%Y_%H%M%S", Now)
			testName = scriptGlobals.ScriptName
			portLogFile = "%s%s_%d_%s_%s_%s%s" %(scriptGlobals.TestResultsDirectory,testName,PTFTBF,scriptGlobals.DutNamesList[comIdx],timeNowStr,myPort.comm.PortName,scriptGlobals.LogFileExtsList[comIdx])
			
			# update Test Info	    
			scriptGlobals.TestID = "%s-%d" % (scriptGlobals.TestName,PTFTBF)	 
			Now = time.localtime(time.time())
			scriptGlobals.StartTime = time.strftime("%m/%d/%Y %H:%M:%S", Now)
			
			scriptUtilities.updateDUTInfo(comIdx)
			myPort.comm.m_TestSetup.Atten = atten
			
			myPort.comm.Log.OpenFile(portLogFile)
	    
			myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Navlib_Data = True
			myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.Enable_Development_Data = True
			myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.EnableFullSystemReset = True
			myPort.comm.RxCtrl.ResetCtrl.ResetInitParams.EnableEncryptedData = False
	    
			myPort.comm.RxCtrl.ResetCtrl.Reset(General.clsGlobal.HOT)
	    
			# Send full power mode
			myPort.comm.LowPowerParams.Mode = 0;
			myPort.comm.RxCtrl.SetPowerMode(False);
	    
			comIdx = comIdx + 1
	    
		    navStatus = False
		    count = 0
		    while((navStatus == False) and (count < 12)):
			navStatus = mainFrame.GetNavStatus("*")
			count = count + 1
			print "Wait for nav loop %d" %(count)
			if ((navStatus == True) or (count >= 12)):
			    break
			# if ((General.clsGlobal.Abort == True) or (General.clsGlobal.AbortSingle == True)):
			    # scriptGlobals.TestAborted = True
			    # break
			mainFrame.Delay(5)
	    
		    # Set PTF mode
		    logStr = "PTF(%d) begins..." %(PTFTBF)
		    scriptUtilities.logApp("*", logStr)
		    comIdx = 0	
		    for usePort in scriptGlobals.DutMainPortsList:	
			if (usePort < 0):
			    comIdx = comIdx + 1
			    continue
			myPort = scriptGlobals.DutPortManagersList[comIdx]
			myPort.comm.LowPowerParams.Mode = 4;
			myPort.comm.LowPowerParams.PTFPeriod = PTFTBF			
			myPort.comm.LowPowerParams.PTFMaxOffTime = PTFMaxOffTime
			myPort.comm.LowPowerParams.PTFMaxSearchTime = PTFMaxSearchTime
	    
			myPort.comm.RxCtrl.SetPowerMode(False);
			comIdx = comIdx + 1
	    
		    print "%s: Start logging for %d seconds ... " % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),logTime)
		    mainFrame.Delay(logTime)
	    
		    #cleanup
	
		    comIdx = 0	
		    for usePort in scriptGlobals.DutMainPortsList:	
			if (usePort < 0):
			    comIdx = comIdx + 1
			    continue
			myPort = scriptGlobals.DutPortManagersList[comIdx]
			# Send full power mode
			myPort.comm.LowPowerParams.Mode = 0;
			myPort.comm.RxCtrl.SetPowerMode(False);
			myPort.comm.Log.CloseFile()		    
			comIdx = comIdx + 1  
		
		# wait for cycle to end
		print "%s: Wait for cycle end (%d seconds) ... " % (time.strftime("%Y/%m/%d %H:%M:%S", time.localtime()),PTFTBF)
		mainFrame.Delay(PTFTBF)
		# End Loop for PTF Configuration
		
	    # set default atten
	    scriptUtilities.setAtten(defaultAtten)	    
	    print "Done: %s" % (scriptGlobals.ScriptName)
    
    # wait for cycle to end
    mainFrame.Delay(PTFTBF)
    mainFrame.SetScriptDone(True)
    
    #cleanup
    scriptGlobals.Exiting  = True
    comIdx = 0	
    for usePort in scriptGlobals.DutMainPortsList:	
	if (usePort < 0):
	    comIdx = comIdx + 1
	    continue
	myPort = scriptGlobals.DutPortManagersList[comIdx]
	# Send full power mode
	myPort.comm.ListenersCtrl.Stop()
	myPort.comm.ListenersCtrl.Cleanup()
	myPort.comm.ClosePort()
	# myPort.StopAsyncProcess()
	comIdx = comIdx + 1

except:
    scriptUtilities.ExceptionHandler()

