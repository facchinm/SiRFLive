﻿namespace LogManagerClassLibrary
{
    using System;
    using System.IO;

    internal class writeBin : writeModule
    {
        internal writeBin(BinaryWriter bt)
        {
            base.m_writeBinObj = bt;
        }

        internal override void Write(byte[] msgB)
        {
            base.m_writeBinObj.Write(msgB);
        }

        internal override void WriteLine(byte[] msgB)
        {
            base.m_writeBinObj.Write(msgB);
            base.m_writeBinObj.Write("\r\n");
        }
    }
}

