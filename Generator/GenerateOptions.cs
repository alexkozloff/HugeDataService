using System;

namespace HugeDataService.Generator
{
    public class GenerateOptions
    {
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public string[] GroupBy { get; set; } = Array.Empty<string>();
    }
}