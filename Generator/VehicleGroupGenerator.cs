using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace HugeDataService.Generator
{
    public class VehicleGroupGenerator
    {
        public IDictionary<string, Bag> GroupBy(IEnumerable<Bag> list, string[] groupBy)
        {
            if (groupBy.Any())
            {
                var groupColumn = groupBy.First();
                return list.GroupBy(item => item[groupColumn]).ToImmutableSortedDictionary(
                    groups => (string) groups.Key,
                    groups => BuildGroup(groupColumn, groups.Key, groups, groupBy.Skip(1).ToArray()));
            }

            return list
                .Select((item, index) => (item, index))
                .ToImmutableSortedDictionary(x => x.index.ToString(), x => x.item);
        }

        private Bag BuildGroup(string groupColumn, object groupValue, IEnumerable<Bag> list, string[] groupBy)
        {
            var bag = new Bag();
            CalculateSummary(bag, list);
            bag.Remove("Index"); 
            bag[groupColumn] = groupValue;
            bag.Nested = GroupBy(list, groupBy);
            return bag;
        }

        private void CalculateSummary(Bag bag, IEnumerable<Bag> list)
        {
            var template = list.FirstOrDefault() ?? new Bag();
            foreach (var (columnName, value) in template)
            {
                var values = list.Select(item => item[columnName]);
                bag[columnName] = IsNumeric(value)
                    ? values.Select(Convert.ToDecimal).Sum()
                    : values.Count();
            }
        }

        private bool IsNumeric(object value) =>
            value is int || value is long || value is decimal || value is float || value is double;
    }
}