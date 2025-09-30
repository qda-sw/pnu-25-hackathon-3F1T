using UnityEngine;
using UnityEngine.Events;

public class TriggerEnter : MonoBehaviour
{
    [SerializeField] private string _targetTag = "Player";

    public UnityEvent TriggerEntered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"TriggerEnter detected collision with {other.gameObject.name}");
        if (other.CompareTag(_targetTag))
        {
            TriggerEntered?.Invoke();
        }
    }
}
