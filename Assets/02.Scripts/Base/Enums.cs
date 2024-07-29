
public enum PixelmonState
{
    Idle,
    Move,
    Attack
}

public enum FieldState
{
    Locked,
    Buyable,
    Empty,
    Seeded,
    Harvest
}

public enum AbilityType
{
    Attack,
    Trait,
    BaseAtkSpd,
    AddCri,
    AddCriDmg,
    AddDmg,
    AddSDmg,
    AddSCri,
    AddSCriDmg,
    End
}

public enum PassiveType
{
    Attack,
    Buff,
    Farming
}

public enum PixelmonRank
{
    Common,
    Advanced,
    Rare,
    Epic,
    Legendary,
    Unique
}

public enum DirtyUI
{
    Gold,
    Diamond,
    Seed,
    Food,
    UserExp,
    EggCount
}