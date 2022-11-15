using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Inspector : MonoBehaviour
{
	[Header("References")]
	public Terraformer terraformer;
	public Tilemap tilemap;
	public Text textInspector;

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
					forestText = "Forest: World Tree";
				}
				else
				{
					forestText = $"Forest: {forestType}";
					forestText += $"\n - Level: {acre.forest.GetLevel()}";
					forestText += $"\n - Experience: {acre.forest.GetExperience()}";
					forestText += $"\n - Health: {acre.forest.GetHealth()}";
					forestText += $"\n - Sunlight Generation: {acre.forest.GetSunlightGeneration()}";
				}
			};


			textInspector.text = $"{terrainText}\n\n---\n\n{forestText}";
		}
	}
}
