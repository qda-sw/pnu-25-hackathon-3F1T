using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _healthText;

    private void Start()
    {
        _player ??= FindFirstObjectByType<Player>();
        _healthText ??= GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_player != null && _healthText != null)
        {
            _healthText.text = "Health: " + _player.Health.ToString();
        }
    }
}
