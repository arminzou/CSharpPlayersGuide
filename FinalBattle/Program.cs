using FinalBattle;

string playerName = ColoredConsole.Prompt("What is player's name?");

IPlayer player1 = new ConsolePlayer();
IPlayer player2 = new ComputerPlayer();

Party heroes = new Party(player1);
heroes.Characters.Add(new TheTrueProgrammer(playerName));
heroes.Characters.Add(new VinFletcher() { EquippedGear = new VinsBow() });
heroes.Gears.Add(new Sword());
heroes.Gears.Add(new Hammer());
heroes.Items.Add(new HealthPotion());
heroes.Items.Add(new HealthPotion());
heroes.Items.Add(new HealthPotion());

List<Party> monsterParties = new List<Party>() { CreateMonsterParty1(player2), CreateMonsterParty2(player2), CreateMonsterParty3(player2), CreateMonsterParty4(player2) };

foreach (var monsterParty in monsterParties)
{
    Battle battle = new Battle(heroes, monsterParty);
    battle.Run();
    if (heroes.Characters.Count == 0) break;
}

if (heroes.Characters.Count > 0) ColoredConsole.WriteLine("You have defeated the Uncoded One's forces! You have won the battle!", ConsoleColor.Green);
else ColoredConsole.WriteLine("You have been defeated. The Uncoded One has won.", ConsoleColor.Red);

Party CreateMonsterParty1(IPlayer controllingPlayer)
{
    Party monsters = new Party(controllingPlayer);
    monsters.Characters.Add(new Skeleton { EquippedGear = new Dagger() });
    monsters.Items.Add(new HealthPotion());
    return monsters;
}

Party CreateMonsterParty2(IPlayer controllingPlayer)
{
    Party monsters = new Party(controllingPlayer);
    monsters.Characters.Add(new Skeleton());
    monsters.Characters.Add(new Skeleton());
    monsters.Items.Add(new HealthPotion());
    monsters.Gears.Add(new Dagger());
    monsters.Gears.Add(new Dagger());
    return monsters;
}

// Create the monsters for Battle #3.
Party CreateMonsterParty3(IPlayer controllingPlayer)
{
    Party monsters = new Party(controllingPlayer);
    monsters.Characters.Add(new StoneAmarok());
    monsters.Characters.Add(new StoneAmarok());
    monsters.Items.Add(new HealthPotion());
    monsters.Items.Add(new HealthPotion());
    return monsters;
}

Party CreateMonsterParty4(IPlayer controllingPlayer)
{
    Party monsters = new Party(controllingPlayer);
    monsters.Characters.Add(new TheUncodedOne());
    monsters.Items.Add(new HealthPotion());
    return monsters;
}
