using UnityEngine.UI;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public Item currentItem;
    public Image itemIcon;
    public Text itemName,itemPrice;
    public bool ShowPrice;

    private Animator Anim;
    private InventoryDisplay inventoryDisplay;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        itemPrice.text = "";

        if (currentItem != null)
            SetupItem(currentItem);
    }

    public void SetupItem(Item item)
    {
        currentItem = item;

        itemIcon.sprite = Resources.Load<Sprite>("Icons/"+item.ItemName); // Loading respective sprite from "Ressources" folder
        itemName.text = item.ItemName;

        if (item.ItemTier == Item.Tier.Rare)
            itemName.color = Color.blue;
        else if (item.ItemTier == Item.Tier.Epic)
            itemName.color = Color.yellow;
        else if (item.ItemTier == Item.Tier.Legendary)
            itemName.color = Color.magenta;
        else if (item.ItemTier == Item.Tier.Lethal)
            itemName.color = Color.red;

        if (ShowPrice)
            itemPrice.text = item.ItemPrice.ToString();
    }

    public void OnEnter()
    {
        if (currentItem == null)
            return;
        Anim.SetBool("OnHover", true);
    }

    public void OnExit()
    {
        if (currentItem == null)
            return;
        Anim.SetBool("OnHover", false);
    }

    public void OnClick()
    {
        //Debug.Log("Click");
        if (currentItem == null)
            return;

        if (GetComponentInParent<InventoryDisplay>().inventory.AddItem(currentItem, true))
            Debug.Log("Item bought");
        else
            Debug.Log("Can't purchase item");
    }
}
