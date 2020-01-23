using System;
using Microsoft.AspNetCore.Mvc;

namespace HugeDataService.Controllers
{
    public class PagedRequest
    {
        public string[] GroupKey { get; set; } = Array.Empty<string>();
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 100;
    }
}