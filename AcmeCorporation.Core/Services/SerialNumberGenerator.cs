namespace AcmeCorporation.Core.Services;

public static class SerialNumberGenerator
{
    public static List<string> Generate(int count = 100)
    {
        // Randomly generate serial numbers
        // based on chars A-Z, 0-9
        // 4 segments of 4 chars each,
        // the first must be == ACME
        
        var random = new Random();
        return Enumerable.Range(0, count)
            .Select(_ => $"ACME-{Segment(random)}-{Segment(random)}-{Segment(random)}")
            .ToList();
    }

    private static string Segment(Random rng)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Range(0, 4)
            .Select(_ => chars[rng.Next(chars.Length)]).ToArray());
    }
}