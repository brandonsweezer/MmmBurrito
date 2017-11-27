using System;

public class RandomGenerator
{
	private static readonly Random random = new Random();

	public static double RandomNumberBetween(double minValue, double maxValue)
	{
		var next = random.NextDouble();

		return minValue + (next * (maxValue - minValue));
	}
}

