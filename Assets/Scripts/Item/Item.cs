using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO _itemData;
    public ItemSO ItemData => _itemData;
}
