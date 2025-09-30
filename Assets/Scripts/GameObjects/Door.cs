using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string _doorId;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _openAnimationName = "Open";
    [SerializeField] private string _closeAnimationName = "Close";
    private bool _isOpen = true;


    public bool IsOpen => _isOpen;


    public void CloseDoor()
    {
        Debug.Log("Attempting to close door...");
        if (!PlayerInventory.Instance.HasItem(_doorId)) return;
        if (!_isOpen) return;

        PlayerInventory.Instance.RemoveItem(_doorId);
        _animator.Play(_closeAnimationName);
        _isOpen = false;
    }
}
