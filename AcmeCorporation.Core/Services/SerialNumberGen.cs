namespace AcmeCorporation.Core.Services;

public class SerialNumberGen
{
    public static List<string> Generate(int count = 100)
    {
        var serialNumbers = new List<string>();
        var random = new Random();

        for (var i = 0; i < count; i++) 
        {
            var serial = $"ACME-{GenerateSegment(random)}-{GenerateSegment(random)}-{GenerateSegment(random)}";
            serialNumbers.Add(serial);
        }
        return serialNumbers;
    }

    private static string GenerateSegment(Random random)
    {
        const string chars = "ABCDEFGHIJKLMOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 4)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}