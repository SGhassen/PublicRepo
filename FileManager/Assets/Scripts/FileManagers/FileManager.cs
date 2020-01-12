using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class FileManager : MonoBehaviour
{
    [Header("Test bools")]
    public bool SaveBool;
    public bool LoadBool;
    private Inventory inventory;
    // Use this for initialization
    private void Awake()
    {
        if (inventory == null)
            inventory = GetComponent<Inventory>();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if(SaveBool)
        {
            SaveBool = false;
            SavePlayerInventory(inventory.OriginalPlayerItems);
        }
        if (LoadBool)
        {
            LoadBool = false;
            LoadPlayerInventory();
        }
    }

    SaveInstance InstanceInventoryItems(List<Item> items)
    {
        SaveInstance save = new SaveInstance();
        int i = 0;
        foreach (Item item in items)
        {
            if (item != null)
            {
                save.AllItems.Add(item);
                i++;
            }
        }
        return save;
    }

    public void SavePlayerInventory(List<Item> items)
    {
        SaveInstance save = InstanceInventoryItems(items); // Acquire items

        // save em !
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerItems.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void LoadPlayerInventory()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerItems.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerItems.save", FileMode.Open);
            SaveInstance save = (SaveInstance)bf.Deserialize(file);
            file.Close();

            inventory.OriginalPlayerItems.Clear();

            if (save.AllItems.Count > 0)
            {
                foreach (Item item in save.AllItems)
                {
                    inventory.OriginalPlayerItems.Add(item);
                    Debug.Log("Loading " + item.ItemName);
                }
                Debug.Log("Player Inventory Loaded");
                inventory.SetRefreshItemList(true); // Display the items
            }
            else
            {
                Debug.Log("No loaded items found!");
            }
        }
        else
        {
            Debug.Log("No game saved! New game inventory ?");
        }
    }

    #region Test

    public void SaveInventory()
    {
        SaveBool = true;
    }

    public void LoadInventory()
    {
        LoadBool = true;
    }

    #endregion
}
