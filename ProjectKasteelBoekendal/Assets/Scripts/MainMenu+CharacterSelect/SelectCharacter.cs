using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public GameObject[] characters; // Array to hold character GameObjects
    public int Number; // Index of the currently selected character
    
    public void ChangeCharacter(int Num)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false); // Deactivate all characters
        }
        
        Number += Num; // Update the character index

        if (Number > characters.Length -1)
        {
            Number = 0; // Wrap around to the first character
        }
        if (Number < 0)
        {
            Number = characters.Length -1; // Wrap around to the last character
        }

        characters[Number].SetActive(true); // Activate the selected character
    }

    public void ConfirmCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacter", Number); // Save the selected character index
        SceneManager.LoadScene("KitchenArea"); // Load the first area scene
    }
}
