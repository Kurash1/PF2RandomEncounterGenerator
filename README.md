A simple random encounter generator for Pathfinder 2e.

# USAGE

Run the program in the command line, the 1st argument should be the party level, the 2nd argument should be the desired encounter xp.

The program will generate an encounter for that party level at or below the desired encounter xp.

# BUILD

Standard building process for C# applications, but you will need to manually export the Creature list you'd like to use from Archives of Nethys. It should be in CSV format, named table-data.csv, and should reside next to the executable.

the following fields are used by the generator

- level
- creature_family
- rarity
- url

Here is what the release version uses: https://2e.aonprd.com/Monsters.aspx?Letter=&exclude-rarities=unique&legacy=no&sort=name-asc&display=table&columns=level+creature_family+rarity+url
