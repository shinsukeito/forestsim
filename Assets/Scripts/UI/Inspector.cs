using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class Inspector : MonoBehaviour
{
	[Header("References")]
	public Terraformer terraformer;
	public Yggdrasil yggdrasil;
	public Tilemap tilemap;
	public TMP_Text textInspector;

	private Vector3Int inspectedTile;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Vector3Int tilePosition = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		if (tilePosition != inspectedTile)
		{
			Acre acre = terraformer.GetAcre(tilePosition.x, tilePosition.y);
			if (acre == null) return;

			string terrainText = $"Terrain: {acre.fieldType}";

			string forestText;
			if (acre.forest == null)
			{
				forestText = "Forest: None";

			}
			else
			{
				ForestType forestType = acre.forest.GetForestType();

				if (forestType == ForestType.WorldTree)
				{
					forestText = "Yggdrasil";
					forestText += $"\n - Health: {yggdrasil.health} / {yggdrasil.maxHealth}";
				}
				else
				{
					forestText = $"Forest: {forestType}";
					forestText += $"\n - Level: {acre.forest.GetLevel() + 1}";
					forestText += $"\n - Experience: {acre.forest.GetExperience()}";
					forestText += $"\n - Health: {acre.forest.GetHealth()}";
					forestText += $"\n - Sunlight Generation: {acre.forest.GetSunlightGeneration()}";
				}
			};

			string disasterText;
			if (acre.disaster == null)
			{
				disasterText = "Disaster: None";
			}
			else
			{
				disasterText = "";

				switch (acre.disaster.GetDisasterType())
				{
					case DisasterType.Blizzard:
						disasterText = $"Disaster: <color=#74F5FD>The Frosts</color>";
						disasterText += "\n - Chance to highly damage forests";
						disasterText += "\n - Forests have a chance not to grow";
						break;
					case DisasterType.Bushfire:
						disasterText = $"Disaster: <color=#E37840>the Burns</color>";
						disasterText += "\n - Damages forests";
						disasterText += "\n - Has a chance to spread each day";
						break;
					case DisasterType.Drought:
						disasterText = $"Disaster: <color=#FBEAA3>The Famine</color>";
						disasterText += "\n - Hinders forests from growing";
						disasterText += "\n - Low but steady damage to forests";
						break;
					case DisasterType.Flood:
						disasterText = $"Disaster: <color=#1475C0>The Rains</color>";
						disasterText += "\n - Damages forests";
						disasterText += "\n - Deals more damage the longer it remains";
						break;
				}
			};

			textInspector.text = $"{terrainText}\n\n{forestText}\n\n{disasterText}";
		}
	}
}
