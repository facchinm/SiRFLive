﻿namespace SiRFLive.Analysis
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct APR
    {
        public double lat;
        public double lon;
        public int height;
        public int horzErr;
    }
}

