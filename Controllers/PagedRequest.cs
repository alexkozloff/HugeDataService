using System;

namespace HugeDataService.Controllers
{
    public class PagedRequest
    {
        public string[] GroupKey { get; set; } = Array.Empty<string>();
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = int.MaxValue;
    }
}