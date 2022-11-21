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
	public TMP_Text textSunlight;
	public Healthbar hb;

	[Header("Configurations")]
	public int sunlight = 0;
	public int forestCost = 50;
	public int spellCost = 10;
	public int expPerGrowth = 3;
	public int maxHealth = 50;
	public int health = 50;
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
		SetSunlight(200);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SetSelectedForest(ForestType.Boreal);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			SetSelectedForest(ForestType.Bushland);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			SetSelectedForest(ForestType.Mangrove);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			SetSelectedForest(ForestType.Rainforest);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SetSelectedForest(ForestType.None);
		}

		// Forest Placement:
		Vector3Int tilePosition = forestHoverTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		if (selectedForestType != ForestType.None)
		{
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

				selectedForestType = ForestType.None;
			}
		}
		// Forest Spell:
		else
		{
			forestHoverTilemap.ClearAllTiles();

			Acre acre = terraformer.GetAcre(tilePosition.x, tilePosition.y);
			if (acre == null || acre.forest == null || acre.forest.GetForestType() == ForestType.WorldTree) return;

			List<Acre> acres = terraformer.GetSpellTiles(tilePosition.x, tilePosition.y, acre.forest.GetForestType(), acre.forest.GetLevel());

			acres.ForEach((Acre a) =>
			{
				forestHoverTilemap.SetTile(new Vector3Int(a.x, a.y, 0), spellTile);
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

	void SetSelectedForest(ForestType forestType)
	{
		if (sunlight < forestCost) selectedForestType = ForestType.None;
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
		SetSelectedForest(ForestType.Boreal);
	}
	public void SelectBushland()
	{
		SetSelectedForest(ForestType.Bushland);
	}
	public void SelectMangrove()
	{
		SetSelectedForest(ForestType.Mangrove);
	}
	public void SelectRainforest()
	{
		SetSelectedForest(ForestType.Rainforest);
	}

	public void SetSunlight(int amount)
	{
		sunlight = amount;
		textSunlight.text = $"Sunlight: {sunlight}";
	}

	public void ChangeHealth(int amount)
	{
		health += amount;
		hb.GetComponent<Healthbar>().SetFill(health * 1f / maxHealth);
	}

}
