﻿namespace CommonClassLibrary
{
    using System;

    public class MessageQData
    {
        public int MessageChanId = -1;
        public string MessageCommMgr = string.Empty;
        public int MessageId = -1;
        public CommonClassLibrary.CommonClass.MessageSource MessageSource = CommonClassLibrary.CommonClass.MessageSource.UNDEFINED;
        public int MessageSubId = -1;
        public string MessageText = string.Empty;
        public string MessageTime = string.Empty;
        public CommonClassLibrary.CommonClass.MessageType MessageType;
    }
}

