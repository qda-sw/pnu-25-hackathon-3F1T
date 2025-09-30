using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                Debug.Log($"Picked up item: {item.name}");
                PlayerInventory.Instance.AddItem(item.ItemData.name, item.ItemData);
                Destroy(collision.gameObject);
            }
        }
    }
}
