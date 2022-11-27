using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class Yggdrasil : MonoBehaviour
{
	[Header("References")]
	public Terraformer terraformer;
	public Tilemap forestHoverTilemap;
	public Tilemap spellTilemap;
	public TMP_Text textSunlight;
	public Healthbar hb;

	[Header("Configurations")]
	public int sunlight = 0;
	public int startingSunlight = 400;
	public int forestCost = 100;
	public int spellCost = 25;
	public int expPerGrowth = 3;
	public int maxHealth = 100;
	public int health = 100;
	public TileBase borealTile;
	public TileBase bushlandTile;
	public TileBase mangroveTile;
	public TileBase rainforestTile;
	public TileBase spellTile;

	private Vector3Int hoveredTile;
	private ForestType selectedForestType = ForestType.None;

	// Start is called before the first frame update
	void Start()
	{
		SetSunlight(startingSunlight);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			ToggleSelectedForest(ForestType.Boreal);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			ToggleSelectedForest(ForestType.Bushland);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			ToggleSelectedForest(ForestType.Mangrove);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			ToggleSelectedForest(ForestType.Rainforest);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Escape))
		{
			ToggleSelectedForest(ForestType.None);
		}

		// Forest Placement:
		Vector3Int tilePosition = forestHoverTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		if (selectedForestType != ForestType.None)
		{
			if (Input.GetMouseButtonDown(1))
			{
				ToggleSelectedForest(ForestType.None);
			}

			if (tilePosition != hoveredTile)
			{
				forestHoverTilemap.SetTile(hoveredTile, null);
				hoveredTile = tilePosition;

				if (terraformer.Plantable(hoveredTile.x, hoveredTile.y, selectedForestType))
				{
					switch (selectedForestType)
					{
						case ForestType.Boreal:
							forestHoverTilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), borealTile);
							break;
						case ForestType.Bushland:
							forestHoverTilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), bushlandTile);
							break;
						case ForestType.Mangrove:
							forestHoverTilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), mangroveTile);
							break;
						case ForestType.Rainforest:
							forestHoverTilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), rainforestTile);
							break;
					}
				}
			}

			if (Input.GetMouseButtonDown(0) && sunlight >= forestCost)
			{
				forestHoverTilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), null);
				bool planted = terraformer.PlantForest(tilePosition.x, tilePosition.y, selectedForestType);

				if (planted)
					SetSunlight(sunlight - forestCost);

				if (sunlight < forestCost)
					selectedForestType = ForestType.None;
			}
		}
		// Forest Spell:
		else
		{
			spellTilemap.ClearAllTiles();

			Acre acre = terraformer.GetAcre(tilePosition.x, tilePosition.y);
			if (acre == null || acre.forest == null || acre.forest.GetForestType() == ForestType.WorldTree) return;

			List<Acre> acres = terraformer.GetSpellTiles(tilePosition.x, tilePosition.y, acre.forest.GetForestType(), acre.forest.GetLevel());

			acres.ForEach((Acre a) =>
			{
				spellTilemap.SetTile(new Vector3Int(a.x, a.y, 0), spellTile);
			});

			if (Input.GetMouseButtonDown(0) && sunlight >= spellCost)
			{
				switch (acre.forest.GetForestType())
				{
					case ForestType.Boreal:
						acres.ForEach((Acre a) =>
						{
							a.RemoveDisaster(DisasterType.Blizzard);
						});
						break;
					case ForestType.Bushland:
						acres.ForEach((Acre a) =>
						{
							a.RemoveDisaster(DisasterType.Drought);
						});
						break;
					case ForestType.Mangrove:
						acres.ForEach((Acre a) =>
						{
							a.RemoveDisaster(DisasterType.Flood);
						});
						break;
					case ForestType.Rainforest:
						acres.ForEach((Acre a) =>
						{
							a.RemoveDisaster(DisasterType.Bushfire);
						});
						break;
				}

				SetSunlight(sunlight - spellCost);
			}
		}
	}

	void ToggleSelectedForest(ForestType forestType)
	{
		if (sunlight < forestCost || selectedForestType == forestType) selectedForestType = ForestType.None;
		else selectedForestType = forestType;

		if (selectedForestType == ForestType.None)
		{
			forestHoverTilemap.SetTile(new Vector3Int(hoveredTile.x, hoveredTile.y, 0), null);
			return;
		}

		if (terraformer.Plantable(hoveredTile.x, hoveredTile.y, selectedForestType))
			switch (selectedForestType)
			{
				case ForestType.Boreal:
					forestHoverTilemap.SetTile(new Vector3Int(hoveredTile.x, hoveredTile.y, 0), borealTile);
					break;
				case ForestType.Bushland:
					forestHoverTilemap.SetTile(new Vector3Int(hoveredTile.x, hoveredTile.y, 0), bushlandTile);
					break;
				case ForestType.Mangrove:
					forestHoverTilemap.SetTile(new Vector3Int(hoveredTile.x, hoveredTile.y, 0), mangroveTile);
					break;
				case ForestType.Rainforest:
					forestHoverTilemap.SetTile(new Vector3Int(hoveredTile.x, hoveredTile.y, 0), rainforestTile);
					break;
			}
		else
			forestHoverTilemap.SetTile(hoveredTile, null);
	}

	public void SelectBoreal()
	{
		ToggleSelectedForest(ForestType.Boreal);
	}
	public void SelectBushland()
	{
		ToggleSelectedForest(ForestType.Bushland);
	}
	public void SelectMangrove()
	{
		ToggleSelectedForest(ForestType.Mangrove);
	}
	public void SelectRainforest()
	{
		ToggleSelectedForest(ForestType.Rainforest);
	}

	public void SetSunlight(int amount)
	{
		sunlight = amount;
		textSunlight.text = $"Sunlight: {sunlight}";
	}

	public void ChangeHealth(int amount)
	{
		health += amount;
		if (health + amount <= 0)
			health = 0;

		hb.GetComponent<Healthbar>().SetFill(health * 1f / maxHealth);
	}
}
