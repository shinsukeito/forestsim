using System.Collections;
using System.Collections.Generic;

public struct ForestStats
{
	public int maxLevel;
	public int[] bark; // Maximum Health
	public int[] leaves; // Sunlight Generation
	public int[] roots; // Damage Resistance

	public ForestStats(int maxLevel, int[] bark, int[] leaves, int[] roots)
	{
		this.maxLevel = maxLevel;
		this.bark = bark;
		this.leaves = leaves;
		this.roots = roots;
	}
}
