using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _amount = 1;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private bool _spawnOnStart = true;
    [SerializeField] private IntEventChannelSO _nextWaveEventChannel;

    [SerializeField] private GameObject _keyPrefab;
    [SerializeField] private string _doorId = "";

    private void Start()
    {
        if (_spawnOnStart) StartSpawn(_amount);
    }

    private void OnEnable()
    {
        _nextWaveEventChannel.OnEventRaised += StartSpawn;
    }

    private void StartSpawn(int amount)
    {
        _amount = amount;

        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        int specialMonsterIndex = Random.Range(0, _amount);
        for (int i = 0; i < _amount; i++)
        {
            var monster = Instantiate(_prefab, transform.position, Quaternion.identity);
            if (i == specialMonsterIndex)
            {
                var specialMonster = monster.GetComponent<Monster>();
                if (specialMonster != null)
                {
                    specialMonster.RegisterKey(_doorId, _keyPrefab);
                }
            }
            Debug.Log($"Spawned {_prefab.name} at {transform.position} ({i + 1}/{_amount})");
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
