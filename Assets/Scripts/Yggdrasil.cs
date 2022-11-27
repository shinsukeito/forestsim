using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.EventSystems;

public class Yggdrasil : MonoBehaviour
{
	[Header("References")]
	public Terraformer terraformer;
	public Tilemap forestHoverTilemap;
	public Tilemap spellTilemap;
	public TMP_Text textSunlight;
	public Healthbar hb;

	public Image borealButton;
	public Image bushlandButton;
	public Image mangroveButton;
	public Image rainforestButton;
	public Color buttonSelectedColor = new Color(0.7f, 0.7f, 0.7f);

	[Header("Configurations")]
	public int sunlight = 0;
	public int startingSunlight = 400;
	public int forestCost = 100;
	public int spellCost = 25;
	public int expPerGrowth = 3;
	public int maxHealth = 100;
	public int health = 100;
	public int healAmount = 15;
	public int healCost = 100;
	public TileBase borealTile;
	public TileBase bushlandTile;
	public TileBase mangroveTile;
	public TileBase rainforestTile;
	public TileBase spellTile;

	private Vector3Int hoveredTile;
	private ForestType selectedForestType = ForestType.None;

	private bool hoveringUI = false;

	// Start is called before the first frame update
	void Start()
	{
		SetSunlight(startingSunlight);
	}

	// Update is called once per frame
	void Update()
	{
		hoveringUI = EventSystem.current.IsPointerOverGameObject();

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

		if (!hoveringUI)
		{
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
					ShowHoveredForest();
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

				if (tilePosition != hoveredTile)
				{
					TooltipSystem.Hide();
					hoveredTile = tilePosition;
				}

				Acre acre = terraformer.GetAcre(tilePosition.x, tilePosition.y);
				if (acre == null || acre.forest == null) return;

				ForestType thisType = acre.forest.GetForestType();

				List<Acre> acres = terraformer.GetSpellTiles(tilePosition.x, tilePosition.y, thisType, acre.forest.GetLevel());

				acres.ForEach((Acre a) =>
				{
					spellTilemap.SetTile(new Vector3Int(a.x, a.y, 0), spellTile);
				});

				switch (thisType)
				{
					// Heal:
					case ForestType.WorldTree:
						TooltipSystem.ShowText("Heal Yggdrassil", $"{healCost}", $"Heal Yggdrassil for {healAmount} health");
						if (Input.GetMouseButtonDown(0) && sunlight >= healCost)
						{
							ChangeHealth(healAmount);
							SetSunlight(sunlight - healCost);
						}
						break;

					// Activate Forests:
					case ForestType.Boreal:
						TooltipSystem.ShowText("Activate Boreal Forest", $"{spellCost}", $"Repel Blizzard in surrounding tiles");
						if (Input.GetMouseButtonDown(0) && sunlight >= spellCost)
						{
							acres.ForEach((Acre a) =>
							{
								a.RemoveDisaster(DisasterType.Blizzard);
							});
							SetSunlight(sunlight - spellCost);
						}
						break;
					case ForestType.Bushland:
						TooltipSystem.ShowText("Activate Bushland Forest", $"{spellCost}", $"Repel Drought in surrounding tiles");
						if (Input.GetMouseButtonDown(0) && sunlight >= spellCost)
						{
							acres.ForEach((Acre a) =>
							{
								a.RemoveDisaster(DisasterType.Drought);
							});
							SetSunlight(sunlight - spellCost);
						}
						break;
					case ForestType.Mangrove:
						TooltipSystem.ShowText("Activate Mangrove Forest", $"{spellCost}", $"Repel Flood in surrounding tiles");
						if (Input.GetMouseButtonDown(0) && sunlight >= spellCost)
						{
							acres.ForEach((Acre a) =>
							{
								a.RemoveDisaster(DisasterType.Flood);
							});
							SetSunlight(sunlight - spellCost);
						}
						break;
					case ForestType.Rainforest:
						TooltipSystem.ShowText("Activate Rainforest Forest", $"{spellCost}", $"Repel Bushfire in surrounding tiles");
						if (Input.GetMouseButtonDown(0) && sunlight >= spellCost)
						{
							acres.ForEach((Acre a) =>
							{
								a.RemoveDisaster(DisasterType.Bushfire);
							});
							SetSunlight(sunlight - spellCost);
						}
						break;
				}
			}
		}
	}

	void ToggleSelectedForest(ForestType forestType)
	{
		spellTilemap.ClearAllTiles();

		if (sunlight < forestCost || selectedForestType == forestType) selectedForestType = ForestType.None;
		else selectedForestType = forestType;

		if (selectedForestType == ForestType.None)
		{
			forestHoverTilemap.SetTile(new Vector3Int(hoveredTile.x, hoveredTile.y, 0), null);
			borealButton.color = bushlandButton.color = mangroveButton.color = rainforestButton.color = buttonSelectedColor;
			return;
		}

		switch (selectedForestType)
		{
			case ForestType.Boreal:
				bushlandButton.color = mangroveButton.color = rainforestButton.color = buttonSelectedColor;
				borealButton.color = new Color(1, 1, 1);
				break;
			case ForestType.Bushland:
				borealButton.color = mangroveButton.color = rainforestButton.color = buttonSelectedColor;
				bushlandButton.color = new Color(1, 1, 1);
				break;
			case ForestType.Mangrove:
				borealButton.color = bushlandButton.color = rainforestButton.color = buttonSelectedColor;
				mangroveButton.color = new Color(1, 1, 1);
				break;
			case ForestType.Rainforest:
				borealButton.color = bushlandButton.color = mangroveButton.color = buttonSelectedColor;
				rainforestButton.color = new Color(1, 1, 1);
				break;
		}

		ShowHoveredForest();
	}

	void ShowHoveredForest()
	{
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
		textSunlight.text = $"{sunlight}";
	}

	public void ChangeHealth(int amount)
	{
		health += amount;
		if (health + amount <= 0)
			health = 0;

		hb.GetComponent<Healthbar>().SetFill(health * 1f / maxHealth);
	}
}
