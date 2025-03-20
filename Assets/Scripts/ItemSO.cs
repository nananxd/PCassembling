using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemSO : ScriptableObject
{
    public List<InventoryItem> items;
}

[System.Serializable]
public class InventoryItem
{
    public string itemId;
    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefab;
}
