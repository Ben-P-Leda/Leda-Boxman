using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    private static InventoryController _instance;
    
    public static void AddItem(GameObject toAdd) { _instance.AddItemToInventory(toAdd); }
    public static bool CarryingItem(GameObject KeyItem) { return _instance._carriedObjects.Contains(KeyItem); }
    public static void UseItem(GameObject KeyItem) { _instance._carriedObjects.Remove(KeyItem); }

    private List<GameObject> _carriedObjects;

    private void Awake()
    {
        _instance = this;
        _carriedObjects = new List<GameObject>();
    }

    private void Start()
    {

    }

    private void AddItemToInventory(GameObject toAdd)
    {
        _carriedObjects.Add(toAdd);
        toAdd.SetActive(false);
    }
}