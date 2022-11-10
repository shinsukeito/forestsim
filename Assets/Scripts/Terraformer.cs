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
	public Tilemap forestTilemap;
	public Acre[,] map;

	[Range(0, 100)]
	public int fieldSpawnPercent;
	public int oceanSize = 0;
	public int riverSpawnSize = 3;
	public int riverCount = 3;

	public TileBase barrenTile;
	public TileBase fieldTile;
	public TileBase oceanTile;
	public TileBase riverTile;

	public TileBase borealTile;
	public TileBase bushlandTile;
	public TileBase mangroveTile;
	public TileBase rainforestTile;

	// Start is called before the first frame update
	void Start()
	{
		BigBang();
		PaintWorld();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			BigBang();
			PaintWorld();
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Vector3Int tilePosition = forestTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			print(tilePosition);
			PlantForest(tilePosition.x, tilePosition.y, ForestType.Boreal);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Vector3Int tilePosition = forestTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			print(tilePosition);
			PlantForest(tilePosition.x, tilePosition.y, ForestType.Bushland);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Vector3Int tilePosition = forestTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			print(tilePosition);
			PlantForest(tilePosition.x, tilePosition.y, ForestType.Mangrove);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			Vector3Int tilePosition = forestTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			print(tilePosition);
			PlantForest(tilePosition.x, tilePosition.y, ForestType.Rainforest);
		}
	}

	void BigBang()
	{
		map = new Acre[mapWidth, mapHeight];

		terrainTilemap.ClearAllTiles();
		terrainTilemap.transform.position = new Vector2(-mapWidth / 2, -mapHeight / 2);

		forestTilemap.ClearAllTiles();
		forestTilemap.transform.position = new Vector2(-mapWidth / 2, -mapHeight / 2);

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
					newAcre.fieldType = FieldType.Ocean;
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

		for (int i = 0; i < riverCount; i++)
			Erode();

		PlantWorldTree(mapWidth / 2, mapHeight / 2);
		PlantWorldTree(mapWidth / 2 - 1, mapHeight / 2);
		PlantWorldTree(mapWidth / 2, mapHeight / 2 - 1);
		PlantWorldTree(mapWidth / 2 - 1, mapHeight / 2 - 1);
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

	void Erode()
	{
		Vector2Int river = new Vector2Int(
			Random.Range(mapWidth / 2 - riverSpawnSize, mapWidth / 2 + riverSpawnSize),
			Random.Range(mapHeight / 2 - riverSpawnSize, mapHeight / 2 + riverSpawnSize)
		);

		map[river.x, river.y].fieldType = FieldType.River;
		int closestX = river.x - mapWidth / 2;
		int closestY = river.y - mapHeight / 2;
		Vector2Int closestBorder = new Vector2Int(0, 1);

		if (Mathf.Abs(closestX) < Mathf.Abs(closestY))
			closestBorder = new Vector2Int(closestX < 0 ? -1 : 1, 0);
		else
			closestBorder = new Vector2Int(0, closestY < 0 ? -1 : 1);


		Vector2Int previousRiver = river;
		while (true)
		{
			if (Random.Range(0, 1f) <= 0.5f)
			{
				river += closestBorder;
			}
			else
			{
				switch (Random.Range(0, 4))
				{
					case 0:
						river.x++;
						break;
					case 1:
						river.x--;
						break;
					case 2:
						river.y++;
						break;
					case 3:
						river.y--;
						break;
				}
			}

			if (previousRiver == river) continue;
			if (map[river.x, river.y].fieldType == FieldType.Ocean) break;
			if (river.x < 0 || river.x > mapWidth - 1 || river.y < 0 || river.y > mapHeight - 1) break;

			map[river.x, river.y].fieldType = FieldType.River;
			previousRiver = river;
		}
	}

	void PlantWorldTree(int x, int y)
	{
		map[x, y].fieldType = FieldType.Field;
		map[x, y].forestType = ForestType.WorldTree;
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
					case FieldType.Barren:
						barrenCount++;
						break;
					case FieldType.Field:
						fieldCount++;
						break;
					case FieldType.Ocean:
						waterCount++;
						break;
				}
			}
		}

		FieldType thisFieldType = map[x, y].fieldType;
		if (thisFieldType == FieldType.Ocean) return FieldType.Ocean;
		if (waterCount > 3) return FieldType.Ocean;
		else if (waterCount > 2 && thisFieldType == FieldType.Field) return FieldType.Ocean;
		else if (fieldCount > 6) return FieldType.Field;
		else return thisFieldType;
	}

	void PaintWorld()
	{
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				PaintField(x, y);
				PaintForest(x, y);
			}
		}
	}

	void PaintField(int x, int y)
	{
		switch (map[x, y].fieldType)
		{
			case FieldType.Barren:
				terrainTilemap.SetTile(new Vector3Int(x, y, 0), barrenTile);
				break;
			case FieldType.Field:
				terrainTilemap.SetTile(new Vector3Int(x, y, 0), fieldTile);
				break;
			case FieldType.Ocean:
				terrainTilemap.SetTile(new Vector3Int(x, y, 0), oceanTile);
				break;
			case FieldType.River:
				terrainTilemap.SetTile(new Vector3Int(x, y, 0), riverTile);
				break;
		}
	}

	void PaintForest(int x, int y)
	{
		switch (map[x, y].forestType)
		{
			case ForestType.Boreal:
				forestTilemap.SetTile(new Vector3Int(x, y, 0), borealTile);
				break;
			case ForestType.Bushland:
				forestTilemap.SetTile(new Vector3Int(x, y, 0), bushlandTile);
				break;
			case ForestType.Mangrove:
				forestTilemap.SetTile(new Vector3Int(x, y, 0), mangroveTile);
				break;
			case ForestType.Rainforest:
				forestTilemap.SetTile(new Vector3Int(x, y, 0), rainforestTile);
				break;
		}
	}

	void PlantForest(int x, int y, ForestType forestType)
	{
		if (map[x, y].forestType == ForestType.WorldTree) return;

		map[x, y].forestType = forestType;
		switch (map[x, y].forestType)
		{
			case ForestType.Boreal:
				if (map[x, y].fieldType == FieldType.Field)
					forestTilemap.SetTile(new Vector3Int(x, y, 0), borealTile);
				break;
			case ForestType.Bushland:
				if (map[x, y].fieldType == FieldType.Field || map[x, y].fieldType == FieldType.Barren)
					forestTilemap.SetTile(new Vector3Int(x, y, 0), bushlandTile);
				break;
			case ForestType.Mangrove:
				if (map[x, y].fieldType == FieldType.Ocean)
					forestTilemap.SetTile(new Vector3Int(x, y, 0), mangroveTile);
				break;
			case ForestType.Rainforest:
				if (map[x, y].fieldType == FieldType.Field)
					forestTilemap.SetTile(new Vector3Int(x, y, 0), rainforestTile);
				break;
		}
	}

	public Vector2 GetDimensions()
	{
		return new Vector2(mapWidth, mapHeight);
	}
}
