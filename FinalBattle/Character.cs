namespace FinalBattle;

public abstract class Character
{
    public abstract string Name { get; }
    public abstract IAttack BasicAttack { get; }
    public IGear? EquippedGear { get; set; }
    public IAttackModifier? DefensiveModifier { get; set; }
    public bool IsAlive => HP > 0;

    private int _hp;
    public int MaxHP { get; }
    public int HP
    {
        get => _hp;
        set => _hp = Math.Clamp(value, 0, MaxHP);
    }
    public Character(int hp)
    {
        MaxHP = hp;
        HP = hp;
    }
}
