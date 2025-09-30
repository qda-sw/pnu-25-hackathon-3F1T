using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _value;

    [SerializeField] private string _id;

    public string ItemName => _itemName;
    public Sprite Icon => _icon;
    public int Value => _value;
    public string Id => _id;   
}
