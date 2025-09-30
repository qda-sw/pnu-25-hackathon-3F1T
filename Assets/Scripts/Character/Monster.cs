using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]
public sealed class Monster : MonoBehaviour, IDamageable
{
    [Header("Move")]
    [SerializeField, Min(0f)] private float _speed = 3f;            // 이동 속도
    [SerializeField] private Transform _target;                      // 추적 대상(비우면 Start에서 Player 자동 탐색)

    [Header("Vitals")]
    [SerializeField, Min(1)] private int _maxHealth = 100;           // 최대 체력
    [SerializeField] private int _health = 100;                       // 현재 체력(관찰용)
    [SerializeField, Min(0)] private int _defense = 0;               // 방어력: 실피해 = 피해 / max(1, 방어력), 소수점 1자리 반올림

    [Header("Contact Damage")]
    [SerializeField, Min(0)] private int _contactDamage = 10;        // 접촉 피해량(플레이어가 받는 데미지)
    [SerializeField, Min(0f)] private float _damageInterval = 0.5f;  // 접촉 피해 간격(초)
    private float _lastDamageTime = -999f;

    [Header("Key Drop (per Door)")]
    [SerializeField] private bool _isKeyBearer = false;              // 이 개체가 키 보유자인가?
    [SerializeField] private GameObject _keyPrefab;                   // 드롭할 키 프리팹
    [SerializeField] private string _doorId = "";                     // 이 몬스터가 속한 문 ID(문당 1회 드롭 보장용)

    private void Awake()
    {
        _maxHealth = Mathf.Max(1, _maxHealth);
        _health = Mathf.Clamp(_health, 1, _maxHealth);

        var col = GetComponent<BoxCollider2D>();
        col.isTrigger = true; // 접촉 데미지는 트리거로 처리
    }

    private void Start()
    {
        if (_target == null)
        {
            var p = FindFirstObjectByType<Player>();
            if (p != null) _target = p.transform;
        }

        // 스폰 시점에 이미 해당 문에서 키가 커밋되어 있으면 이 개체는 키 보유자로 동작하지 않게 함
        // if (!string.IsNullOrEmpty(_doorId) && UniqueDoorRegistry.IsKeyCommitted(_doorId))
        //     _isKeyBearer = false;
    }

    private void FixedUpdate()
    {
        if (_target == null) return;

        Vector2 dir = ((Vector2)(_target.position - transform.position)).normalized;
        transform.position += (Vector3)(dir * _speed * Time.fixedDeltaTime);
    }

    // 플레이어와 닿아있는 동안 일정 간격으로 피해를 줌
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other || !other.CompareTag("Player")) return;
        if (Time.time < _lastDamageTime + _damageInterval) return;

        // Player는 IDamageable(int)을 구현하므로 int 경로 호출
        if (other.TryGetComponent<IDamageable>(out var dmg))
        {
            dmg.TakeDamage(_contactDamage);
            _lastDamageTime = Time.time;
        }
    }

    // 외부(스포너)에서 이 몬스터를 키 보유자로 초기화하고 싶을 때 사용
    // public void InitAsKeyBearer(string doorId, GameObject keyPrefab)
    // {
    //     if (!string.IsNullOrEmpty(doorId) && !UniqueDoorRegistry.IsKeyCommitted(doorId))
    //     {
    //         _doorId = doorId;
    //         _keyPrefab = keyPrefab;
    //         _isKeyBearer = true;

    //         // 이 시점에 "이 문에서는 키 스폰/드롭이 커밋됨"으로 기록 → 문당 1회 보장
    //         UniqueDoorRegistry.MarkKeySpawnedOrDropped(doorId);
    //     }
    //     else
    //     {
    //         // 이미 커밋된 문이면 비키 보유자로 동작
    //         _isKeyBearer = false;
    //     }
    // }

    // ===== IDamageable: 몬스터가 '맞을 때' 호출됨 =====
    public void TakeDamage(int damage)
    {
        // 실피해 = damage / max(1, _defense) → 소수점 1자리 반올림 → 최종 정수 반올림
        int applied = CalculateReducedDamage(damage, _defense);
        if (applied <= 0) return;

        _health -= applied;
        if (_health <= 0) Die();
    }

    private static int CalculateReducedDamage(int rawDamage, int defense)
    {
        int raw = Mathf.Max(0, rawDamage);
        int divisor = (defense == 0) ? 1 : defense;

        float reduced = (float)raw / divisor;
        float rounded1dec = Mathf.Round(reduced * 10f) / 10f; // 소수점 1자리 반올림
        return Mathf.RoundToInt(rounded1dec);                  // 최종 정수 반올림
    }

    private void Die()
    {
        // 키 보유자라면 드롭은 1회만 — 이미 스폰 시점에 커밋했지만 안전하게 드롭도 수행
        if (_isKeyBearer && _keyPrefab != null)
        {
            Instantiate(_keyPrefab, transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false); // 풀링 고려. 필요시 Destroy(gameObject)로 교체
    }

    public void RegisterKey(string doorId, GameObject keyPrefab)
    {
        _doorId = doorId;
        _keyPrefab = keyPrefab;
        _isKeyBearer = true;

    }

    // 에디터 디버그
    private void OnDrawGizmosSelected()
    {
        var col = GetComponent<BoxCollider2D>();
        if (col)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }
    }
}
