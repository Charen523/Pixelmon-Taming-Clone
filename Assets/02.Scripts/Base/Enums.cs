
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

public enum TraitType
{
    AddDmg,
    AddCriDmg,
    AddSDmg,
    AddSCriDmg
}

public enum AbilityType
{
    Attack,
    Trait,
    PSVAtk,
    PSVCri,
    PSVCriDmg,
    PSVAtkSpd,
    PSVDmg,
    PSVSDmg,
    PSVSCri,
    PSVSCriDmg,
    PlayerHP,
    PlayerDefense
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