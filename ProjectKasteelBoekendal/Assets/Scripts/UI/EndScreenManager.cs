using UnityEngine;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    public GameObject endScreenPrefab;
    public Transform canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateEndScreen(string dynamicText, int dynamicNumber, int coinsCollected)
    {
        GameObject endScreen = Instantiate(endScreenPrefab, canvas);

        EndScreenEdit editscript = endScreen.GetComponent<EndScreenEdit>();
        editscript.SetEndScreen(dynamicText, dynamicNumber.ToString(), coinsCollected.ToString());
    }
}
