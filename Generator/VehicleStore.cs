using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace HugeDataService.Generator
{
    public class VehicleStore
    {
        private readonly VehicleListGenerator _listGenerator;
        private readonly VehicleGroupGenerator _groupGenerator;
        private readonly GenerateOptions _options;
        private readonly ILogger<VehicleStore> _logger;

        public VehicleStore(
            VehicleListGenerator listGenerator, 
            VehicleGroupGenerator groupGenerator,
            GenerateOptions options,
            ILogger<VehicleStore> logger)
        {
            _listGenerator = listGenerator;
            _groupGenerator = groupGenerator;
            _options = options;
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.LogInformation("Start generation sample dataset...");
            var list = _listGenerator.Generate(_options.RowCount, _options.ColumnCount);
            Data = _groupGenerator.GroupBy(list, _options.GroupBy);
            _logger.LogInformation("Finished generation sample dataset");
        }

        public IDictionary<string, Bag> Data { get; private set; }
    }
}