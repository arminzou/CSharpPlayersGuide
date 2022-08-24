namespace FinalBattle;

public class VinFletcher : Character
{
    public override string Name => "Vin Fletcher";

    public override IAttack BasicAttack => new Punch();

    public VinFletcher() : base(15)
    {
    }
}

public class VinsBow : IGear
{
    public string Name => "Vin’s Bow";

    public IAttack Attack { get; } = new QuickShot();
}


public class QuickShot : IAttack
{
    public string Name => "QUICK SHOT";

    public AttackData Create() => new AttackData(3, ProbabilityOfHitting: 0.5);
}