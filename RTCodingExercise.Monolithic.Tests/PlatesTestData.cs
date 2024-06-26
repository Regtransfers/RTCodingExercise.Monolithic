using System;
using System.Collections.Generic;
using NanoidDotNet;
using RTCodingExercise.Monolithic.Common.Models;

namespace RTCodingExercise.Monolithic.Tests;

public static class PlatesTestData
{
    public static List<Plate> PlatesList = new List<Plate>();
    public const string PLATE_CONSTANT = "x";
        
    static PlatesTestData()
    {
        var random = new Random();

        for (int i = 0; i < 100; i++)
        {
            var salePrice = random.Next(1, 1000);
            var reg = GenerateAndDeconstructRegistration(out var letters1, out var numbers, out var letters2) + PLATE_CONSTANT;
            PlatesList.Add(new Plate
            {
                Id = Guid.NewGuid(),
                Registration = reg,
                Numbers = numbers,
                Letters = letters2 + "x",
                PurchasePrice = random.Next(1, 1000),
                SalePrice = salePrice,
                MarkUp = (decimal)(salePrice * 1.2),
                Reserved = random.NextDouble() > 0.5
            });    
        }
    }
        
    private static string GenerateAndDeconstructRegistration(out string letters1, out int numbers, out string letters2)
    {
        letters1 = Nanoid.Generate(Nanoid.Alphabets.Letters, 1);
        numbers = int.Parse(Nanoid.Generate("123456789", 3));
        letters2 = Nanoid.Generate(Nanoid.Alphabets.Letters, 3);
        var reg = $"{letters1}{numbers}{letters2}";
        return reg;
    }

    public static IEnumerable<object[]> Data
    {
        get
        {
            var result = new List<object[]>();
            foreach (var plate in PlatesList)
            {
                result.Add(new object[] { plate } );
            }
            return result;
        }
    }
}