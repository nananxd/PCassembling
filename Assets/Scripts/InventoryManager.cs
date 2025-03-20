using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public ItemSO itemSO;
    [SerializeField] private InventoryUI currentSelectedItem;
    public List<InventoryUI> inventoryItems;

    public List<InventoryUI> InventoryItems { get => inventoryItems; set => inventoryItems = value; }
    public InventoryUI CurrentSelectedItem { get => currentSelectedItem; set => currentSelectedItem = value; }

    private void Awake()
    {
        Instance = this;
    }
    
    //public void SetCurrentItem(InventoryItem item)
    //{
    //    CurrentSelectedItem = item;
    //}
}
