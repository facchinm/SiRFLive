﻿namespace SiRFLive.General
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct User
    {
        public int userId;
        public string userName;
        public string ItemName;
        public string AccessRight;
    }
}

