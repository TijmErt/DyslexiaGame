using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        SceneSwitchManager.instance.LoadPreviousScene();
    }
}
