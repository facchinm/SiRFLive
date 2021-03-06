﻿namespace OpenNETCF.IO.Serial
{
    using System;

    public class HandshakeDsrDtr : DetailedPortSettings
    {
        protected override void Init()
        {
            base.Init();
            base.OutCTS = false;
            base.OutDSR = true;
            base.OutX = false;
            base.InX = false;
            base.RTSControl = RTSControlFlows.Enable;
            base.DTRControl = DTRControlFlows.Handshake;
            base.TxContinueOnXOff = true;
            base.DSRSensitive = false;
        }
    }
}

