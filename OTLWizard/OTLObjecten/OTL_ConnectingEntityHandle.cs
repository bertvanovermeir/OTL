﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.OTLObjecten
{
    public class OTL_ConnectingEntityHandle
    {

        public string DisplayName { get; set; }
        public bool isDirectional { get; set; }
        public string bronId { get; set; }
        public string doelId { get; set; }
        public string typeuri { get; set; }
        public string relationName { get; set; }

        public OTL_ConnectingEntityHandle()
        {

        }

    }
}
