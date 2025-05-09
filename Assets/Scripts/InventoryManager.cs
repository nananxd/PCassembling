using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public ItemSO itemSO;
    [SerializeField] private InventoryUI currentSelectedItem;
    public List<InventoryUI> inventoryItems;

    //public List<InventoryUI> InventoryItems { get => inventoryItems; set => inventoryItems = value; }
    public InventoryUI CurrentSelectedItem { get => currentSelectedItem; set => currentSelectedItem = value; }

    private void Awake()
    {
        Instance = this;
    }

    public InventoryUI GetInventoryItemByName(string inventName)
    {
        var foundItem = inventoryItems.Find(s => s.ItemName == inventName);
        if (foundItem != null)
        {
            return foundItem;
        }
        return null;
    }
    
    //public void SetCurrentItem(InventoryItem item)
    //{
    //    CurrentSelectedItem = item;
    //}
}
