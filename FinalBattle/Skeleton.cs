namespace FinalBattle;

public class Skeleton : Character
{
    public override string Name => "SKELETON";

    public override IAttack BasicAttack => new BoneCrunch();
    public Skeleton() : base(5)
    {
    }
}

public class BoneCrunch : IAttack
{
    public string Name => "BONE CRUNCH";
    public AttackData Create() => new AttackData(new Random().Next(2));
}

public class Dagger : IGear
{
    public string Name => "DAGGER";

    public IAttack Attack { get; } = new Stab();
}

public class Stab : IAttack
{
    public string Name => "STAB";

    public AttackData Create() => new AttackData(1);
}