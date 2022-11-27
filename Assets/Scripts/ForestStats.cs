using System.Collections;
using System.Collections.Generic;

public struct ForestStats
{
	public int maxLevel;
	public int[] bark; // Maximum Health
	public int[] leaves; // Sunlight Generation
	public int[] roots; // Damage Resistance
	public int[] costs; // Activation Cost

	public ForestStats(int maxLevel, int[] bark, int[] leaves, int[] roots, int[] costs)
	{
		this.maxLevel = maxLevel;
		this.bark = bark;
		this.leaves = leaves;
		this.roots = roots;
		this.costs = costs;
	}
}
