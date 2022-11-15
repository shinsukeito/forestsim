#nullable enable

public enum FieldType
{
	Barren,
	Field,
	Ocean,
	River
}

public class Acre
{
	public FieldType fieldType = FieldType.Ocean;
	public Forest? forest = null;

	public void OnEachDay(int day)
	{
		if (forest != null)
		{
			forest.Grow();
		}

	}

	public void OnEachSeason(Season season)
	{
		if (forest != null)
		{
			forest.Photosynthesise();
		}
	}

	public void OnEachCycle(int cycle)
	{

	}
}
