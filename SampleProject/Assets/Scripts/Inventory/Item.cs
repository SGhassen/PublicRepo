[System.Serializable]
public class Item
{
    //public Sprite ItemIcon;
    public string ItemName;
    public int MaxStacks = 1;
    public int CurrentStacks;
    public int ItemPrice;
    public int Damage, Defense, Vitality, CriticalChance, MovementSpeed, HealAmount, PowerPassive, ResistancePassive, MobilityPassive;
    public enum Tier {Casual, Rare, Epic, Legendary, Lethal};
    public Tier ItemTier;
    public int ItemLevel;
    public bool Weapon;

    /*public Item(string n, int maxsatcks, int price, int damage, int defense, int vitality, int critical, int speed, int heal, int powerpassive, int resistancepassive, int mobilitypassive, Tier tier, int level, bool wep)
    {
        ItemName = n;
        MaxStacks = maxsatcks;
        ItemPrice = price;
        Damage = damage;
        Defense = defense;
        Vitality = vitality;
        CriticalChance = critical;
        MovementSpeed = speed;
        HealAmount = heal;

        PowerPassive = powerpassive;
        ResistancePassive = resistancepassive;
        MobilityPassive = mobilitypassive;

        ItemTier = tier;
        ItemLevel = level;
        Weapon = wep;
    }*/
}
