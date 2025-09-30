using UnityEngine;
using System;
public class Monster : MonoBehaviour, IDamageable
{
    [Header("Move")]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private Transform _target;

    [Header("Health / Damage")]
    [SerializeField] private int _health = 100;
    [SerializeField] private int _damage = 10;

    [Header("Melee Attack")]
    [SerializeField] private float _meleeRange = 1.2f;
    [SerializeField] private float _meleeCooldown = 0.8f;
    private float _lastMeleeTime = -999f;

    [Header("Skill (Optional)")]
    [SerializeField] private float _skillRange = 4.5f;
    [SerializeField] private float _skillCooldown = 3f;
    [SerializeField] private LayerMask _skillLineMask;                 // 시야 가리는 레이어(벽 등)
    [SerializeField] private GameObject _projectilePrefab;             // 투사체(선택)
    [SerializeField] private float _projectileSpeed = 8f;              // 투사체 속도
    [SerializeField] private Transform _castOrigin;                     // 투사체 발사 위치(없으면 transform)

    [Header("Key / Door Integration")]
    [SerializeField] private string _carriedKeyId = "";                // 이 몬스터가 보유한 열쇠 ID(예: "BossKey") 아이템 보유 여부
    public string CarriedKeyId => _carriedKeyId;

    public event Action<Monster> OnDied;                               // 스포너/게임매니저 등이 구독 가능


    private void Start()
    {
        if (_target == null)
        {
            var p = FindFirstObjectByType<Player>();
            if (p != null) _target = p.transform;
        }
        if (_castOrigin == null) _castOrigin = transform;
    }
    private void FixedUpdate()
    {
        if (_target != null)
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            transform.position += (Vector3)(direction * _speed * Time.fixedDeltaTime);
        }
    }
    private void Update()
    {
        if (_target == null) return;

        // 근접공격 시도
        TryMelee();

        // 스킬 시도 (투사체가 없더라도 훅은 남겨둠)
        TrySkill();
    }
    private void TryMelee()
    {
        if (Time.time < _lastMeleeTime + _meleeCooldown) return;
        if (Vector2.Distance(transform.position, _target.position) > _meleeRange) return;

        // 실제 히트는 트리거나 직접 판정으로 처리 가능—여기서는 직접 판정
        if (_target.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(_damage);
            _lastMeleeTime = Time.time;
        }
    }

    private void TrySkill()
    {
        if (_skillCooldown <= 0f) return; // 스킬 비활성화
        if (Time.time < _lastSkillTime + _skillCooldown) return;

        float dist = Vector2.Distance(transform.position, _target.position);
        if (dist > _skillRange) return;

        // 라인오브사이트(시야) 체크
        Vector2 origin = _castOrigin.position;
        Vector2 dir = (_target.position - _castOrigin.position).normalized;
        var hit = Physics2D.Raycast(origin, dir, _skillRange, _skillLineMask);
        if (hit && hit.transform != _target) return;

        // 캐스트 실행
        OnCastSkill(_target);
        _lastSkillTime = Time.time;
    }
    private float _lastSkillTime = -999f;

    // 스킬 실제 구현 훅
    private void OnCastSkill(Transform target)
    {
        // [예측/자리표시자] — 투사체가 설정되면 발사,
        // 없으면 로그만 남깁니다. 프로젝트에 맞는 이펙트/디버프/버프 등으로 교체하세요.
        if (_projectilePrefab != null)
        {
            var go = Instantiate(_projectilePrefab, _castOrigin.position, Quaternion.identity);
            // 간단 투사체 추진
            if (go.TryGetComponent<Rigidbody2D>(out var rb))
            {
                Vector2 dir = (target.position - _castOrigin.position).normalized;
                rb.linearVelocity = dir * _projectileSpeed;
            }
            // [예측/자리표시자] 투사체에 충돌 시 플레이어에 데미지 주는 스크립트를 붙이는 것을 권장합니다.
        }
        else
        {
            Debug.Log($"{name} cast skill at {target.name}"); // [예측/자리표시자]
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 트리거 기반 근접 히트가 필요한 경우(예: 무기 콜라이더), 쿨다운 적용
        if (!collision.CompareTag("Player")) return;
        if (Time.time < _lastMeleeTime + _meleeCooldown) return;

        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(_damage);
            _lastMeleeTime = Time.time;
        }
    }

    private void Die()
    {
        // [예측/자리표시자] 사망 이펙트/사운드/루팅 등
        // 열쇠를 가진 유니크 몬스터인 경우, 문 로직이 CarriedKeyId를 확인하도록 하세요.
        OnDied?.Invoke(this);

        // 기본 처리: 비활성화(원하면 Destroy 사용)
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0) Die();
    }
    // 디버그/툴링 가시화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _meleeRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_castOrigin ? _castOrigin.position : transform.position, _skillRange);

        // 시야 레이 가시화(대략적인 방향)
        if (_target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(
                _castOrigin ? _castOrigin.position : transform.position,
                _target.position
            );
        }
    }
}
