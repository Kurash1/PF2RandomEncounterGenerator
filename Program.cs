using Csv;
// You need to include an exported table from AoN with the following fields
// - level
// - creature_family
// - rarity
// - url
// https://2e.aonprd.com/Monsters.aspx?Letter=&exclude-rarities=unique&legacy=no&sort=name-asc&display=table&columns=level+creature_family+rarity+url
var csv = File.ReadAllText("table-data.csv");
var creatures = CsvReader.ReadFromText(csv);
Random rnd = new();

int PartyLevel = GetNullableIntArgument(0, 1);
int EncounterXP = GetNullableIntArgument(1, 80);

ConsoleKeyInfo? input = null;
do
{
	GenerateEncounter();
	Console.WriteLine("Press Enter to generate another one, Press Any Other Key to Exit.");
	input = Console.ReadKey();
	Console.WriteLine();
} while (input.HasValue && input.Value.Key == ConsoleKey.Enter);

void GenerateEncounter()
{
	// In -4 to +4 format
	List<int> CreatureLevels = [];
	do
	{
		int randomLevel = rnd.Next(-4, 5);
	
		if (randomLevel + PartyLevel < -1)
			continue;

		if (GetEncounterXPTotal(CreatureLevels) + LevelToXP(randomLevel) <= EncounterXP)
			CreatureLevels.Add(randomLevel);
	
		if (GetEncounterXPTotal(CreatureLevels) >= EncounterXP - 9)
			break;
		if (PartyLevel < 3 && GetEncounterXPTotal(CreatureLevels) >= EncounterXP - 14)
			break;
		if (PartyLevel < 2 && GetEncounterXPTotal(CreatureLevels) >= EncounterXP - 19)
			break;

	} while(GetEncounterXPTotal(CreatureLevels) < EncounterXP);
	Console.WriteLine($"Final XP of Encounter: {GetEncounterXPTotal(CreatureLevels)}");

	foreach (int level in CreatureLevels)
	{
		var creature = GetRandomCreature(PartyLevel + level);
		PrintCreature(creature);
	}
}
int GetEncounterXPTotal(List<int> encounter)
{
	return (from level in encounter select LevelToXP(level)).Sum(level => level);
}
int LevelToXP(int level) => level switch
{
	-4 => 10,
	-3 => 15,
	-2 => 20,
	-1 => 30,
	0 => 40,
	1 => 60,
	2 => 80,
	3 => 120,
	4 => 160,
	_ => throw new NotImplementedException($"Creature level outside party level range")
};
ICsvLine GetRandomCreature(int level)
{
	var selectedList = from creature in creatures where creature["level"].Trim() == level.ToString() select creature;
	return selectedList.RandomElement();
}
void PrintCreature(ICsvLine line)
{
	Console.WriteLine($@"# {line["name"]} - {line["level"]}
	; {line["creature_family"]} - {line["rarity"]}
	https://2e.aonprd.com{line["url"]}");
}
int GetNullableIntArgument(int index, int defaultValue)
{
	if (args.Length > index && int.TryParse(args[index], out int value))
		return value;
	return defaultValue;
}
public static class Helpers
{
	public static T RandomElement<T>(this IEnumerable<T> enumerable)
	{
		return enumerable.RandomElementUsing<T>(new Random());
	}

	public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand)
	{
		int index = rand.Next(0, enumerable.Count());
		return enumerable.ElementAt(index);
	}
}