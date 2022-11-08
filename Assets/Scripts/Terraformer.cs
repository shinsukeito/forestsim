using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Terraformer : MonoBehaviour
{
	[Range(0, 50)]
	public int mapWidth = 25;
	[Range(0, 50)]
	public int mapHeight = 15;
	public Tilemap terrainTilemap;
	public Acre[,] map;

	[Range(0, 100)]
	public int fieldSpawnPercent;
	public int oceanSize = 0;

	public TileBase fieldTile;
	public TileBase barrenTile;
	public TileBase waterTile;

	// Start is called before the first frame update
	void Start()
	{
		BigBang();
		Paint();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			BigBang();
			Paint();
		}
	}

	void BigBang()
	{
		map = new Acre[mapWidth, mapHeight];
		terrainTilemap.ClearAllTiles();
		terrainTilemap.transform.position = new Vector2(-mapWidth / 2, -mapHeight / 2);

		string seed = Time.time.ToString();
		System.Random randomValue = new System.Random(seed.GetHashCode());

		// Initialise all Acres and build intial map:
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				Acre newAcre = new Acre();
				if (x <= oceanSize - 1 || x >= mapWidth - oceanSize || y <= oceanSize - 1 || y >= mapHeight - oceanSize)
				{
					newAcre.fieldType = FieldType.Water;
				}
				else
				{
					if (randomValue.Next(0, 100) < fieldSpawnPercent)
					{
						newAcre.fieldType = FieldType.Field;
					}
					else
					{
						newAcre.fieldType = FieldType.Barren;
					}
				}

				map[x, y] = newAcre;
			}
		}

		for (int i = 0; i < 2; i++)
			Smooth();
	}

	void Smooth()
	{
		// Determine new field types:
		FieldType[,] newFieldTypes = new FieldType[mapWidth, mapHeight];
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				newFieldTypes[x, y] = DetermineFieldType(x, y);
			}
		}

		// Apply new field types to map:
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				map[x, y].fieldType = newFieldTypes[x, y];
			}
		}
	}

	FieldType DetermineFieldType(int x, int y)
	{
		int fieldCount = 0;
		int barrenCount = 0;
		int waterCount = 0;

		for (int nx = x - 1; nx <= x + 1; nx++)
		{
			for (int ny = y - 1; ny <= y + 1; ny++)
			{
				if (nx < 0 || nx > mapWidth - 1 || ny < 0 || ny > mapHeight - 1) continue;

				switch (map[nx, ny].fieldType)
				{
					case FieldType.Field:
						fieldCount++;
						break;
					case FieldType.Barren:
						barrenCount++;
						break;
					case FieldType.Water:
						waterCount++;
						break;
				}
			}
		}

		FieldType thisFieldType = map[x, y].fieldType;
		if (thisFieldType == FieldType.Water) return FieldType.Water;
		if (waterCount > 3) return FieldType.Water;
		else if (waterCount > 2 && thisFieldType == FieldType.Field) return FieldType.Water;
		else if (fieldCount > 6) return FieldType.Field;
		else return thisFieldType;
	}

	void Paint()
	{
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				switch (map[x, y].fieldType)
				{
					case FieldType.Field:
						terrainTilemap.SetTile(new Vector3Int(x, y, 0), fieldTile);
						break;
					case FieldType.Barren:
						terrainTilemap.SetTile(new Vector3Int(x, y, 0), barrenTile);
						break;
					case FieldType.Water:
						terrainTilemap.SetTile(new Vector3Int(x, y, 0), waterTile);
						break;
				}
			}
		}
	}

	public Vector2 GetDimensions()
	{
		return new Vector2(mapWidth, mapHeight);
	}
}
