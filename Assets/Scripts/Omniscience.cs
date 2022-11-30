using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameDifficulty
{
	Easy,
	Normal,
	Hard
}

public class Omniscience
{
	public static GameDifficulty Difficulty { get; set; } = GameDifficulty.Normal;
}
