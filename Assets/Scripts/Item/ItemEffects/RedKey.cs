using UnityEngine;

public class RedKey : MonoBehaviour
{
    public PlayerSmooth playerSmooth;
    public float speedMod;
    public float cameraView;
    void Start()
    {
        playerSmooth = gameObject.GetComponent<PlayerSmooth>();
    }
    void Update()
    {
        if (PlayerInventory.Instance.HasItem("K_N"))
        {

        }
        if (PlayerInventory.Instance.HasItem("K_S"))
        {

        }
        if (PlayerInventory.Instance.HasItem("K_W"))
        {
            speedMod = 1.25f;
        }
        else
        {
            speedMod = 1;
        }
        if (PlayerInventory.Instance.HasItem("K_E"))
        {
            cameraView = 2;
        }
        else
        {
            cameraView = 1;
        }
    }
}
