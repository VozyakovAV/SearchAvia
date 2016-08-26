using System;

namespace SearchAvia
{
    public class StatusSearchVM
    {
        public bool IsSearching { get; set; }
        public string Message { get; set; }
        public SearchResultVM Result { get; set; }
    }
}