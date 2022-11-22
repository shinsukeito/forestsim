using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Terraformer : MonoBehaviour
{
	[Header("References")]
	public Yggdrasil yggdrasil;
	public Tilemap terrainTilemap;
	public Tilemap forestTilemap;
	public Tilemap forestHoverTilemap;
	public Tilemap disasterTilemap;
	public GameObject healthbarPrefab;

	[Header("Map Configuration")]
	[Range(0, 50)]
	public int mapWidth = 25;
	[Range(0, 50)]
	public int mapHeight = 15;
	public Acre[,] map;

	[Range(0, 100)]
	public int fieldSpawnPercent;
	public int oceanSize = 0;
	public int riverSpawnSize = 3;
	public int riverCount = 3;

	[Header("Disaster Configuration")]
	public int bushfireExtinguishChance = 25;

	public int blizzardRadius = 2;
	public int[] blizzardHinderChance = new int[] { 70, 30 };
	public int[] blizzardDestroyChance = new int[] { 30, 10 };

	public int droughtRadius = 2;
	public float droughtHinderModifier = 0.65f;

	public int floodRadius = 1;
	public int floodChance = 60;
	public int floodDamage = 2;

	[Header("Field Tiles")]
	public TileBase barrenTile;
	public TileBase fieldTile;
	public TileBase oceanTile;
	public TileBase riverTile;

	[Header("Forest Tiles")]
	public TileBase borealTile;
	public TileBase bushlandTile;
	public TileBase mangroveTile;
	public TileBase rainforestTile;

	[Header("Disaster Tiles")]
	public TileBase blizzardTile;
	public TileBase bushfireTile;
	public TileBase droughtTile;
	public TileBase floodTile;

	// Start is called before the first frame update
	void Start()
	{
		BigBang();
		PaintWorld();
	}

	// Update is called once per frame
	void Update()
	{
		// if (Input.GetKeyDown(KeyCode.R))
		// {
		// 	BigBang();
		// 	PaintWorld();
		// }
	}

	void BigBang()
	{
		map = new Acre[mapWidth, mapHeight];

		terrainTilemap.ClearAllTiles();
		terrainTilemap.transform.position = new Vector2(-mapWidth / 2, -mapHeight / 2);

		forestTilemap.ClearAllTiles();
		forestTilemap.transform.position = new Vector2(-mapWidth / 2, -mapHeight / 2);

		forestHoverTilemap.ClearAllTiles();
		forestHoverTilemap.transform.position = new Vector2(-mapWidth / 2, -mapHeight / 2);

		disasterTilemap.ClearAllTiles();
		disasterTilemap.transform.position = new Vector2(-mapWidth / 2, -mapHeight / 2);

		string seed = Time.time.ToString();
		System.Random randomValue = new System.Random(seed.GetHashCode());

		// https://www.youtube.com/watch?v=v7yyZZjF1z4
		// Initialise all Acres and build intial map:
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				Acre newAcre = new Acre(this, x, y);
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

		// Create World Tree and add it as reference to four tiles: 
		Healthbar hb = Instantiate(healthbarPrefab, new Vector3(0, -0.4f), Quaternion.identity).GetComponent<Healthbar>();
		yggdrasil.hb = hb;
		Forest worldTree = new Forest(yggdrasil, map[mapWidth / 2, mapHeight / 2], ForestType.WorldTree, hb);

		PlantWorldTree(mapWidth / 2, mapHeight / 2, worldTree);
		PlantWorldTree(mapWidth / 2 - 1, mapHeight / 2, worldTree);
		PlantWorldTree(mapWidth / 2, mapHeight / 2 - 1, worldTree);
		PlantWorldTree(mapWidth / 2 - 1, mapHeight / 2 - 1, worldTree);
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

	void PlantWorldTree(int x, int y, Forest worldTree)
	{
		map[x, y].fieldType = FieldType.Field;
		map[x, y].forest = worldTree;
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
				terrainTilemap.SetTile(new Vector3Int(x, y), barrenTile);
				break;
			case FieldType.Field:
				terrainTilemap.SetTile(new Vector3Int(x, y), fieldTile);
				break;
			case FieldType.Ocean:
				terrainTilemap.SetTile(new Vector3Int(x, y), oceanTile);
				break;
			case FieldType.River:
				terrainTilemap.SetTile(new Vector3Int(x, y), riverTile);
				break;
		}
	}

	public void PaintForest(int x, int y)
	{
		if (map[x, y].forest == null) return;

		switch (map[x, y].forest.GetForestType())
		{
			case ForestType.Boreal:
				forestTilemap.SetTile(new Vector3Int(x, y), borealTile);
				break;
			case ForestType.Bushland:
				forestTilemap.SetTile(new Vector3Int(x, y), bushlandTile);
				break;
			case ForestType.Mangrove:
				forestTilemap.SetTile(new Vector3Int(x, y), mangroveTile);
				break;
			case ForestType.Rainforest:
				forestTilemap.SetTile(new Vector3Int(x, y), rainforestTile);
				break;
		}
	}

	public void PaintDisaster(int x, int y)
	{
		if (map[x, y].disaster == null) return;

		switch (map[x, y].disaster.GetDisasterType())
		{
			case DisasterType.Blizzard:
				disasterTilemap.SetTile(new Vector3Int(x, y), blizzardTile);
				break;
			case DisasterType.Bushfire:
				disasterTilemap.SetTile(new Vector3Int(x, y), bushfireTile);
				break;
			case DisasterType.Drought:
				disasterTilemap.SetTile(new Vector3Int(x, y), droughtTile);
				break;
			case DisasterType.Flood:
				disasterTilemap.SetTile(new Vector3Int(x, y), floodTile);
				break;
		}
	}

	public void EraseForest(int x, int y)
	{
		forestTilemap.SetTile(new Vector3Int(x, y), null);
	}

	public void EraseDisaster(int x, int y)
	{
		disasterTilemap.SetTile(new Vector3Int(x, y), null);
	}

	public bool PlantForest(int x, int y, ForestType forestType)
	{
		if (!Plantable(x, y, forestType)) return false;

		Vector3 healthbarPosition = forestTilemap.GetCellCenterWorld(new Vector3Int(x, y)) + new Vector3(0, -0.4f);
		Healthbar hb = Instantiate(healthbarPrefab, healthbarPosition, Quaternion.identity).GetComponent<Healthbar>();
		map[x, y].forest = new Forest(yggdrasil, map[x, y], forestType, hb);
		PaintForest(x, y);

		return true;
	}

	public bool Plantable(int x, int y, ForestType forestType)
	{
		if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) return false;
		if (map[x, y].forest != null) return false;

		switch (forestType)
		{
			case ForestType.Boreal:
				if (map[x, y].fieldType == FieldType.Field)
					return true;
				break;
			case ForestType.Bushland:
				if (map[x, y].fieldType == FieldType.Field || map[x, y].fieldType == FieldType.Barren)
					return true;
				break;
			case ForestType.Mangrove:
				if (map[x, y].fieldType == FieldType.Field)
					return true;
				break;
			case ForestType.Rainforest:
				if (map[x, y].fieldType == FieldType.Field)
					return true;
				break;
		}

		return false;
	}

	public Vector2 GetDimensions()
	{
		return new Vector2(mapWidth, mapHeight);
	}

	public List<Forest> GetForests()
	{
		List<Forest> forests = new List<Forest>();
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				if (map[x, y].forest == null) continue;
				if (map[x, y].forest.GetForestType() == ForestType.WorldTree) continue;

				forests.Add(map[x, y].forest);
			}
		}

		return forests;
	}

	public int GetForestCount()
	{
		int count = 0;
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				if (map[x, y].forest == null) continue;
				if (map[x, y].forest.GetForestType() == ForestType.WorldTree) continue;

				count++;
			}
		}

		return count;
	}

	public Acre GetAcre(int x, int y)
	{
		if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) return null;
		return map[x, y];
	}

	public void OnEachDay(int day)
	{
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				map[x, y].OnEachDay(day);
			}
		}
	}

	public void OnEachSeason(Season season)
	{
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				map[x, y].OnEachSeason(season);
			}
		}

		// Spawn disasters:
		List<Acre> targets;
		Acre target;

		switch (season)
		{
			case Season.Cold:
				targets = GetAcresOfType(new List<FieldType>() { FieldType.Field, FieldType.Barren });
				target = targets[Random.Range(0, targets.Count)];

				targets = GetAcresInCircle(target.x, target.y, blizzardRadius);
				targets.ForEach((a) =>
				{
					if (a.fieldType == FieldType.River || a.fieldType == FieldType.Ocean) return;
					Wreak(DisasterType.Blizzard, a.x, a.y);
				});
				break;

			case Season.Dry:
				targets = GetAcresOfType(new List<FieldType>() { FieldType.Field, FieldType.Barren });
				target = targets[Random.Range(0, targets.Count)];

				targets = GetAcresInCircle(target.x, target.y, droughtRadius);
				targets.ForEach((a) =>
				{
					if (a.fieldType == FieldType.River || a.fieldType == FieldType.Ocean) return;
					Wreak(DisasterType.Drought, a.x, a.y);
				});
				break;

			case Season.Hot:
				targets = GetAcresOfType(new List<FieldType>() { FieldType.Field, FieldType.Barren });
				target = targets[Random.Range(0, targets.Count)];

				Wreak(DisasterType.Bushfire, target.x, target.y);
				break;

			case Season.Wet:
				targets = new List<Acre>();

				List<Acre> rivers = GetAcresOfType(new List<FieldType>() { FieldType.River });
				rivers.ForEach((r) =>
				{
					List<Acre> neighbours = r.GetNeighbours(floodRadius, true);

					neighbours.ForEach((n) =>
					{
						if (n.fieldType == FieldType.Field || n.fieldType == FieldType.Barren)
							if (!targets.Contains(n)) targets.Add(n);
					});
				});

				targets.ForEach((t) =>
				{
					if (Random.Range(0, 100) <= floodChance)
						Wreak(DisasterType.Flood, t.x, t.y);
				});
				break;
		}
	}

	public void OnEachCycle(int cycle)
	{
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				map[x, y].OnEachCycle(cycle);
			}
		}

		// Blizzard and drought radius increases every 2 cycles:
		if (cycle % 2 == 0)
		{
			droughtRadius++;
			blizzardRadius++;
		}

		// Flood radius increases every 3 cycles:
		if (cycle % 3 == 0)
		{
			floodRadius++;
		}
	}

	public void Wreak(DisasterType disasterType, int x, int y)
	{
		Acre acre = map[x, y];
		switch (disasterType)
		{
			case DisasterType.Bushfire:
				if (acre.fieldType == FieldType.Ocean || acre.fieldType == FieldType.River) return;
				if (acre.disaster != null && acre.disaster.GetDisasterType() == DisasterType.Bushfire) return;
				break;
		}

		Disaster newDisaster = new Disaster(acre, disasterType);
		map[x, y].disaster = newDisaster;
		PaintDisaster(x, y);
	}

	public List<Acre> GetNeighbours(int x, int y, int size, bool includeSelf, bool circular)
	{
		List<Acre> neighbours = new List<Acre>();

		for (int nx = x - size; nx <= x + size; nx++)
		{
			for (int ny = y - size; ny <= y + size; ny++)
			{
				if (circular && Mathf.Abs(x - nx) + Mathf.Abs(y - ny) > size) continue;
				if (nx < 0 || nx > mapWidth - 1 || ny < 0 || ny > mapHeight - 1) continue;
				if (!includeSelf && nx == x && ny == y) continue;

				neighbours.Add(map[nx, ny]);
			}
		}

		return neighbours;
	}

	public List<Acre> GetAcresOfType(List<FieldType> types)
	{
		List<Acre> acres = new List<Acre>();

		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				if (types.Contains(map[x, y].fieldType)) acres.Add(map[x, y]);
			}
		}

		return acres;
	}

	private List<Acre> GetAcresInCircle(int cx, int cy, int radius)
	{
		// https://stackoverflow.com/questions/10878209/midpoint-circle-algorithm-for-filled-circles
		List<Acre> acres = new List<Acre>();

		int error = -radius;
		int x = radius;
		int y = 0;

		while (x >= y)
		{
			int lastY = y;

			error += y;
			++y;
			error += y;

			Plot4points(acres, cx, cy, x, lastY);

			if (error >= 0)
			{
				if (x != lastY)
					Plot4points(acres, cx, cy, lastY, x);

				error -= x;
				--x;
				error -= x;
			}
		}

		return acres;
	}

	private void Plot4points(List<Acre> acres, int cx, int cy, int x, int y)
	{
		HorizontalLine(acres, cx - x, cy + y, cx + x);
		if (y != 0)
			HorizontalLine(acres, cx - x, cy - y, cx + x);
	}

	private void HorizontalLine(List<Acre> acres, int x0, int y0, int x1)
	{
		if (y0 < 0 || y0 >= mapHeight) return;
		for (int x = x0; x <= x1; ++x)
		{
			if (x < 0 || x >= mapWidth) continue;
			acres.Add(map[x, y0]);
		}
	}

	public List<Acre> GetSpellTiles(int x, int y, ForestType forestType, int level)
	{
		int size = level == 1 ? 1 : 2;
		return GetNeighbours(x, y, size, true, true);
	}
}
