﻿namespace OpenNETCF.IO.Serial
{
    using System;

    [Flags]
    internal enum DB
    {
        DATABITS_16 = 0x10,
        DATABITS_16X = 0x20,
        DATABITS_5 = 1,
        DATABITS_6 = 2,
        DATABITS_7 = 4,
        DATABITS_8 = 8
    }
}

