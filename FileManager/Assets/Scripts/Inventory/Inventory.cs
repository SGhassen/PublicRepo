using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    FileManager fileManager;

    [Header("Inventory Data")]
    public int Gold;
    public int InventoryMaxSize;
    public List<Item> OriginalPlayerItems;

    private bool RefreshItemList = false;
    // Use this for initialization
    void Start ()
    {
        fileManager = GetComponent<FileManager>();
    }
	
	public bool AddItem(Item item, bool ShopItem) // Shop Items require gold
    {
        if (Gold < item.ItemPrice && ShopItem)
            return false;

        if (TryStackingItem(item))
        {
            if (ShopItem)
                Gold -= item.ItemPrice;
            Debug.Log("Item stacked");
            return true;
        }
        else if (TryAddingItem(item))
        {
            if (ShopItem)
                Gold -= item.ItemPrice;
            Debug.Log("Item added");
            return true;
        }

        Debug.Log("Inventory full");
        return false;
    }

    bool TryStackingItem(Item item)
    {
        if (item.MaxStacks > 1) // TRUE IF STACKABLE ITEM
        {
            foreach (Item it in OriginalPlayerItems)
            {
                if (it == item && it.CurrentStacks + item.CurrentStacks <= it.MaxStacks)
                {
                    it.CurrentStacks += item.CurrentStacks;
                    return true;
                }
            }
        }
        return false;
    }

    bool TryAddingItem(Item item)
    {
        if (OriginalPlayerItems.Count < InventoryMaxSize)
        {
            OriginalPlayerItems.Add(item);
            return true;
        }
        return false;
    }

    public bool LevelUpItem(int ItemPos)
    {
        if (OriginalPlayerItems.Count < ItemPos) // Exception
            return false;

        if (Gold < OriginalPlayerItems[ItemPos].ItemLevel * 100) // GOLD
            return false;

        if (OriginalPlayerItems[ItemPos].ItemLevel >= 10) // STATIC ITEM MAX LEVEL
            return false;

        Gold -= OriginalPlayerItems[ItemPos].ItemLevel * 100;
        OriginalPlayerItems[ItemPos].ItemLevel++;
        return true;
    }

    public bool UpgradeItem(int ItemPos)
    {
        if (OriginalPlayerItems.Count < ItemPos) // Exception
            return false;

        int RequiredGold = RequiredGoldForUpgrade(OriginalPlayerItems[ItemPos]);
        if (RequiredGold <= 0) // GOLD
            return false;

        Gold -= RequiredGold;
        OriginalPlayerItems[ItemPos].ItemTier++;
        return true;
    }

    int RequiredGoldForUpgrade(Item item)
    {
        if (item.ItemTier == Item.Tier.Casual && Gold >= 1000)
            return 1000;
        else if (item.ItemTier == Item.Tier.Rare && Gold >= 2500)
            return 2500;
        else if (item.ItemTier == Item.Tier.Epic && Gold >= 6000)
            return 6000;
        else if (item.ItemTier == Item.Tier.Legendary && Gold >= 10000 && item.Weapon)
            return 10000;
        else
            return -1;
    }

    public void SetRefreshItemList(bool b)
    {
        RefreshItemList = b;
    }

    public bool GetRefreshItemList()
    {
        return RefreshItemList;
    }

    #region Test
    public void AddGold(int Amount)
    {
        Gold += Amount;
    }

    public void RestGold()
    {
        Gold = 0;
    }
    #endregion
}
