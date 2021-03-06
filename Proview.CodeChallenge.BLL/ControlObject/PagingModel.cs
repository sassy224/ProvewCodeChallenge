﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proview.CodeChallenge.BLL.ControlObject
{
    /// <summary>
    /// This class is used to store data that can be used to filter, sort and paginate a table
    /// </summary>
    public class PagingModel
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SearchString { get; set; }
        public string SortOn { get; set; }
        public string SortReversed { get; set; }
    }
}