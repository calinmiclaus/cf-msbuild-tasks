﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudFoundry.Build
{
    public class AppDetails
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Cpu
    {
        public int max { get; set; }
        public int min { get; set; }
    }

    public class Instances
    {
        public int max { get; set; }
        public int min { get; set; }
    }

    public class Autoscale
    {
        public Cpu cpu { get; set; }
        public string enabled { get; set; }
        public Instances instances { get; set; }
    }

    public class ServiceDetails
    {
        public string plan { get; set; }
        public string type { get; set; }
    }

    public class PushProperties
    {
        public string app_dir { get; set; }
        public Dictionary<string, AppDetails> applications { get; set; }
        public Autoscale autoscale { get; set; }
        public int disk { get; set; }
        public int memory { get; set; }
        public string name { get; set; }
        public string placement_zone { get; set; }

        public Dictionary<string, ServiceDetails> services { get; set; }
        public bool sso_enabled { get; set; }
        public string stack { get; set; }
    }

}