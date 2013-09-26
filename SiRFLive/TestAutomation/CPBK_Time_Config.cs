﻿namespace SiRFLive.TestAutomation
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct CPBK_Time_Config
    {
        public byte u8Hours;
        public byte u8Minutes;
        public byte u8Seconds;
    }
}

