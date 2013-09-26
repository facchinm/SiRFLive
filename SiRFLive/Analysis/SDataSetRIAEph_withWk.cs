﻿namespace SiRFLive.Analysis
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit, Pack=1)]
    public struct SDataSetRIAEph_withWk
    {
        [FieldOffset(0)]
        public SDataSetRIAEph Eph;
        [FieldOffset(60)]
        public ushort Eph_Wk;
    }
}

