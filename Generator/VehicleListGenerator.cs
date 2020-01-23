using System.Collections.Generic;
using Bogus;

namespace HugeDataService.Generator
{
    public class VehicleListGenerator
    {
        private readonly Faker _faker = new Faker();

        public VehicleListGenerator()
        {
            _faker.Random = new Randomizer(0xFEED);
        }

        public ICollection<Bag> Generate(int rowCount, int columnCount)
        {
            var result = new List<Bag>(rowCount);

            for (var index = 0; index < rowCount; index++)
            {
                result.Add(BuildVehicle(index, columnCount));
            }

            return result;
        }

        private Bag BuildVehicle(long index, int columnCount)
        {
            var vehicle = new Bag();
            vehicle["Index"] = index;
            vehicle[nameof(_faker.Vehicle.Manufacturer)] = _faker.Vehicle.Manufacturer();
            vehicle[nameof(_faker.Vehicle.Model)] = _faker.Vehicle.Model();
            vehicle[nameof(_faker.Vehicle.Type)] = _faker.Vehicle.Type();
            vehicle[nameof(_faker.Vehicle.Fuel)] = _faker.Vehicle.Fuel();
            vehicle[nameof(_faker.Vehicle.Vin)] = _faker.Vehicle.Vin();
            vehicle[nameof(_faker.Finance.TransactionType)] = _faker.Finance.TransactionType();
            vehicle[nameof(_faker.Finance.AccountName)] = _faker.Finance.AccountName();
            vehicle[nameof(_faker.Finance.Account)] = _faker.Finance.Account();
            vehicle[nameof(_faker.Finance.Currency)] = _faker.Finance.Currency().Code;
            vehicle[nameof(_faker.Finance.Amount)] = _faker.Finance.Amount();

            var extendedColumnCount = columnCount - vehicle.Count;

            for (var columnIndex = 0; columnIndex < extendedColumnCount; columnIndex++)
            {
                var alphaIndex = ToAlphabet(columnIndex);
                vehicle[$"Value{alphaIndex}"] = _faker.Random.Int(0, 1_0000_000);
            }

            return vehicle;
        }

        private static string ToAlphabet(int number)
        {
            const char firstChar = 'A';
            string result = "";
            while (number >= 0)
            {
                result = (char) (firstChar + number % 26) + result;
                number /= 26;
                number--;
            }

            return result;
        }
    }
}