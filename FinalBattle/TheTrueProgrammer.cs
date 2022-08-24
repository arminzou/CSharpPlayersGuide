namespace FinalBattle;

public class TheTrueProgrammer : Character
{
    public override string Name { get; }

    public override IAttack BasicAttack => new Punch();

    public TheTrueProgrammer(string name) : base(25)
    {
        Name = name;
        EquippedGear = new Sword();
        DefensiveModifier = new ObjectSight();
    }
}

public class Punch : IAttack
{
    public string Name => "PUNCH";
    public AttackData Create() => new AttackData(1);
}

public class Hammer : IGear
{
    public string Name => "Hammer";

    public IAttack Attack { get; } = new Smash();
}

public class Smash : IAttack
{
    string IAttack.Name => "SMASH";
    public AttackData Create() => new AttackData(3);
}

public class Sword : IGear
{
    public string Name => "Sword";

    public IAttack Attack { get; } = new Slash();
}

public class Slash : IAttack
{
    string IAttack.Name => "SLASH";
    public AttackData Create() => new AttackData(2);
}

public class ObjectSight : IAttackModifier
{
    public string Name => "OBJECT SIGHT";

    public AttackData Modify(AttackData input)
    {
        if(input.Type == DamageType.Decoding)
        {
            ColoredConsole.WriteLine($"{Name} reduced the attack by 2 points.", ConsoleColor.Yellow);
            return input with { Damage = Math.Max(0, input.Damage - 2)};
        }

        return input;
    }
}