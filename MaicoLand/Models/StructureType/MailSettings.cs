﻿using System;
namespace MaicoLand.Models.StructureType
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string SenderKey { set; get; }
        public string UserName { set; get; }

    }
}
