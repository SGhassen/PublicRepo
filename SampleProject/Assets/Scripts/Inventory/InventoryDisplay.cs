using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class InventoryDisplay : MonoBehaviour
{
    public Inventory inventory;
    public Text GoldText,SizeText;

    public GameObject ItemUIPrefab;
    public Transform ItemContainer;

    public List<Item> PlayerItems;

    // Start is called before the first frame update
    void Start()
    {
        if(inventory == null)
        {
            inventory = FindObjectOfType<Inventory>();
            Debug.Log("Unassigned inventory");
        }
        GoldText.text = inventory.Gold.ToString();

        UpdateInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (GoldText.text != inventory.Gold.ToString())
        {
            GoldText.text = inventory.Gold.ToString();
            UpdateInventory();
        }

        if (inventory.GetRefreshItemList())
        {
            inventory.SetRefreshItemList(false);
            UpdateInventory();
        }
    }

    public void UpdateInventory()
    {
        foreach (ItemDisplay ID in ItemContainer.GetComponentsInChildren<ItemDisplay>())
            Destroy(ID.gameObject);

        foreach(Item item in inventory.OriginalPlayerItems)
        {
            GameObject G = Instantiate(ItemUIPrefab, ItemContainer);
            G.GetComponent<ItemDisplay>().SetupItem(item);
        }

        SizeText.text = "INVENTORY " + inventory.OriginalPlayerItems.Count.ToString() + "/" + inventory.InventoryMaxSize.ToString();
    }
}
