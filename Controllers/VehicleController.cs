using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HugeDataService.Generator;
using Microsoft.AspNetCore.Mvc;

namespace HugeDataService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly VehicleStore _vehicleStore;
        private readonly VehicleListGenerator _generator;
        private readonly GenerateOptions _options;

        public VehicleController(VehicleStore vehicleStore, VehicleListGenerator generator, GenerateOptions options)
        {
            _vehicleStore = vehicleStore;
            _generator = generator;
            _options = options;
        }

        [HttpGet]
        [Route("")]
        public PagedResult<ICollection<Bag>> Query([FromQuery]PagedRequest request)
        {
            var items = QueryInternal(_vehicleStore.Data, request.GroupKey);
            var viewport = items.Skip(request.Skip).Take(request.Take).ToList();
            return new PagedResult<ICollection<Bag>>(viewport, items.Count);
        }

        private ICollection<Bag> QueryInternal(IDictionary<string, Bag> groups, string[] drillDownPath)
        {
            if (drillDownPath.Any())
            {
                var key = drillDownPath.First();
                if (groups.TryGetValue(key, out var bag))
                {
                    return QueryInternal(bag.Nested, drillDownPath.Skip(1).ToArray());
                }
                
                throw new InvalidOperationException($"Grouping key '{key}' not found.");
            }

            return groups.Values;
        }

        [HttpGet]
        [Route("schema")]
        public ActionResult Schema()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Generation options:");
            sb.AppendLine($"\tMax row count: {_options.RowCount}");
            sb.AppendLine($"\tMax column count: {_options.ColumnCount}");
            sb.AppendLine();
            sb.AppendLine($"Group by: {string.Join(", ", _options.GroupBy)}");
            sb.AppendLine();
            sb.AppendLine("Schema:");

            var template = _generator.Generate(1, _options.ColumnCount).First();
            foreach (var (columnName, value) in template)
            {
                sb.AppendLine($"\t{columnName.PadRight(30)}{value.GetType().Name}");
            }

            sb.AppendLine();
            sb.AppendLine("Example:");
            
            var groupings = _options.GroupBy.Take(2)
                .Select(columnName => $"groupKey={Uri.EscapeDataString(template[columnName].ToString())}&");

            var host = ControllerContext.HttpContext.Request.Host;

            sb.AppendLine($"http://{host}/vehicle?{string.Join("", groupings)}skip=100&take=100");
            
            return Ok(sb.ToString());
        }
    }
}