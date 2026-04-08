using UnityEngine;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    public GameObject endScreenPrefab;

    public Image endScreenWinBG;
    public Image endScreenLoseBG;
    public Image quitIcon;
    public Image nextIcon;
    public Image restartIcon;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WinEndScreen()
    {
        //GameObject endScreen = Instantiate(endScreenPrefab, canvas);
    }

    public void LoseEndScreen()
    {
        
    }
}
