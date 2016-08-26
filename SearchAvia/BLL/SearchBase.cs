using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchAvia
{
    public abstract class SearchBase
    {
        public abstract bool IsSearching { get; protected set; }
        public abstract void Start();
        public abstract string GetResult();
    }
}