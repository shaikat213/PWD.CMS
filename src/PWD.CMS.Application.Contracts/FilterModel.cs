﻿using PWD.CMS.CMSEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWD.CMS
{
    public class FilterModel
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public bool IsDesc { get; set; }
    }

}
