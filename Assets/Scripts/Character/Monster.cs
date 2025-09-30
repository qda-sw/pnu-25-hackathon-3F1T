using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _health = 100;
    [SerializeField] private int _damage = 10;
    [SerializeField] private Transform _target;

    private void Start()
    {
        _target ??= FindFirstObjectByType<Player>().transform;
    }
    private void FixedUpdate()
    {
        if (_target != null)
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            transform.position += (Vector3)(direction * _speed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<Player>(out Player player))
            {
                player.TakeDamage(_damage);
            }
        }
    }

    private void Die()
    {

    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }
}
