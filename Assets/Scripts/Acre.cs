using UnityEngine.Tilemaps;

public enum FieldType
{
	Barren,
	Field,
	Ocean,
	River
}


public enum ForestType
{
	None,
	Boreal,
	Bushland,
	Mangrove,
	Rainforest
}

public class Acre
{
	public FieldType fieldType = FieldType.Ocean;
	public ForestType forestType = ForestType.None;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
