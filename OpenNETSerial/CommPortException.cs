﻿namespace OpenNETCF.IO.Serial
{
    using System;

    public class CommPortException : Exception
    {
        public CommPortException(string desc) : base(desc)
        {
        }
    }
}

