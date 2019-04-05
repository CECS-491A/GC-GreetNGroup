﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccessLayer.Models
{
    public class GNGLog
    {
        public string logID { get; set; }
        public string userID { get; set; }
        public string ipAddress { get; set; }
        public string dateTime { get; set; }
        public string description { get; set; }
    }
}