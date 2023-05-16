using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<Item> items = new List<Item>();

    [SerializeField] private Transform _itemContent;
    [SerializeField] private GameObject _inventoryItem;

    private GameObject _savedObject;
    private void Awake()
    {
        instance = this;
    }

    public void Add(Item item)
    {
        items.Add(item);
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public void ListItems()
    {
        CleanInventory();
        foreach (var item in items)
        {

            GameObject obj = Instantiate(_inventoryItem, _itemContent);

            obj.name = item.itemName;
            
            obj.gameObject.SetActive(true);
        }
    }

    private void CleanInventory()
    {
        foreach (Transform item in _itemContent)
        {
            Destroy(item.gameObject);
        }
    }
}
