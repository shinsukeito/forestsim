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
}
