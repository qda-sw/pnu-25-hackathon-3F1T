using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private static PlayerInventory _instance;
    public static PlayerInventory Instance => _instance;

    [SerializeField] private Dictionary<string, ItemSO> _inventory;

    private void Awake()
    {
        _instance = this;
        _inventory = new();
    }

    public bool HasItem(string itemId)
    {
        return _inventory.ContainsKey(itemId);
    }

    public void AddItem(string itemId, ItemSO item)
    {
        _inventory[itemId] = item;

        Debug.Log($"Added item: {item.ItemName} (ID: {itemId}) to inventory.");
    }

    public void RemoveItem(string itemId)
    {
        _inventory.Remove(itemId);

        Debug.Log($"Removed item with ID: {itemId} from inventory.");
    }
}
