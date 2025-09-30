using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private bool _loadOnStart = false;
    [SerializeField] private bool _dontDestroyOnLoad = false;

    private void Start()
    {
        if (_loadOnStart)
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
        if (_dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
