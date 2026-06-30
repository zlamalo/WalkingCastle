using System;

public class RNGService
{
    private Random random = new();

    /// <summary>
    /// Pass in values from 0 to 1 that represent chance
    /// </summary>
    /// <param name="chance"></param>
    /// <returns></returns>
    public bool IsSuccess(float chance)
    {
        chance = Math.Clamp(chance, 0, 1);
        return random.NextDouble() < chance;
    }

    public int RandomAmount(ValuesRange range)
    {
        return random.Next(range.Min, range.Max + 1);
    }
}