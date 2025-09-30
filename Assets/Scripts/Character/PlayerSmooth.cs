using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSmooth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _speed = 100.0f;
    [SerializeField] private int _health = 100;

    [SerializeField] private int _score = 0;
    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _movement;
    

    public int Health => _health;
    public int Score => _score;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        _movement.Normalize();
    }

    private Vector2 acceleration = new(0, 0);
    private Vector2 velocity = new(0, 0);

    private void FixedUpdate()
    {
        acceleration = (_movement * 0.8f) + (acceleration * 0.2f);
        if (velocity.magnitude > 0)
        {
            velocity = (velocity * (System.Math.Max(0f, velocity.magnitude * 0.95f - 0.1f) / velocity.magnitude)) + (acceleration * 0.5f);
        }
        else
        {
            velocity = acceleration * 0.2f;
        }
        //Debug.Log(acceleration);
        //Debug.Log(velocity);
        //Debug.Log(_rb.position);
        _rb.MovePosition(_rb.position + velocity * _speed * Time.fixedDeltaTime);
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        gameObject.SetActive(false);
    }
    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }
    public void AddScore(int points)
    {
        _score += points;
    }
}
