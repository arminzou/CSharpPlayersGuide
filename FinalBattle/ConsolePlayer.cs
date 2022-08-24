namespace FinalBattle;

public class ConsolePlayer : IPlayer
{
    public IAction ChooseAction(Battle battle, Character actor)
    {
        List<MenuChoice> menuChoices = CreateMenuOptions(battle, actor);

        for (int i = 0; i < menuChoices.Count; i++)
        {
            ColoredConsole.WriteLine($"{i + 1} - {menuChoices[i].Description}", menuChoices[i].Enabled ? ConsoleColor.Gray : ConsoleColor.DarkGray);
        }

        string choice = ColoredConsole.Prompt("What do you want to do?");
        int menuIndex = Convert.ToInt32(choice) - 1;

        if (menuChoices[menuIndex].Enabled) return menuChoices[menuIndex]!.Action!;

        return new DoNothingAction();
    }

    private List<MenuChoice> CreateMenuOptions(Battle battle, Character character)
    {
        Party currentParty = battle.GetPartyFor(character);
        Party enemyParty = battle.GetEnemyPartyFor(character);

        List<MenuChoice> menuChoices = new List<MenuChoice>();

        if (character.EquippedGear != null)
        {
            IGear gear = character.EquippedGear;
            IAttack specialAttack = gear.Attack;
            if (enemyParty.Characters.Count > 0)
                menuChoices.Add(new MenuChoice($"Special Attack ({specialAttack.Name} with {gear.Name})", new AttackAction(specialAttack, enemyParty.Characters[0])));
            else
                menuChoices.Add(new MenuChoice($"Special Attack ({specialAttack.Name} with {gear.Name})", null));
        }

        if (enemyParty.Characters.Count > 0)
            menuChoices.Add(new MenuChoice($"Standard Attack ({character.BasicAttack.Name})", new AttackAction(character.BasicAttack, enemyParty.Characters[0])));
        else
            menuChoices.Add(new MenuChoice($"Standard Attack ({character.BasicAttack.Name})", null));

        if (currentParty.Items.Count > 0)
        {
            menuChoices.Add(new MenuChoice($"Use Potion ({currentParty.Items.Count})", new UseItemAction(currentParty.Items[0])));
        }
        else
            menuChoices.Add(new MenuChoice($"Use Potion (0)", null));

        foreach (IGear gear in currentParty.Gears)
            menuChoices.Add(new MenuChoice($"Equip {gear.Name}", new EquipGearAction(gear)));

        menuChoices.Add(new MenuChoice("Do Nothing", new DoNothingAction()));
        return menuChoices;
    }
}

public record MenuChoice(string Description, IAction? Action)
{
    public bool Enabled => Action != null;
}
