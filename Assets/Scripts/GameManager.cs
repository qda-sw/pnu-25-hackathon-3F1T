using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _currentWave = 0;

    public int CurrentWave => _currentWave;

    [SerializeField] private IntEventChannelSO _nextWaveEventChannel;

    private void StartNextWave()
    {
        _currentWave++;
    }


}
