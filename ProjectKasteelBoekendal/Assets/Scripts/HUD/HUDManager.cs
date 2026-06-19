using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public Image playerInfoLabel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //INSERT CODE: Change playername to player's name
        //INSERT CODE: Change coins to player's current/saved coins
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCoinsUI(int coins)
    {
        string coinText = coins.ToString();
        playerInfoLabel.transform.Find("coinsamount").GetComponent<TMP_Text>().text = coinText;
    }

    public void LoadPauseMenu()
    {
        //INSERT CODE: Load pause menu
    }

    public void LoadVirtualCoach()
    {
        //INSERT CODE: Load virtual coach
    }
}
