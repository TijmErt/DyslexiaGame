using System;
using UnityEngine;
using System.Collections;
using Managers.Saving;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour, ISaveable
{
    public GameObject[] characters; // Array to hold character GameObjects
    public int Number; // Index of the currently selected character
    public GameObject confirmationPanel; // Panel to confirm character selection

    public string UID => "SelectCharacter";
    
    private void Start()
    {
        // Only hide panel if it exists
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }

        Number = PlayerPrefs.GetInt("SelectedCharacter");
        ShowCharacter(); // Display the currently selected character
    }
    
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

    void ShowCharacter()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }

        characters[Number].SetActive(true);
    }

    public void ConfirmCharacter()
    {
        // Only show panel if it exists
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(true); // Show the confirmation panel
        }
    }

    public void ConfirmSelection()
    {
        PlayerPrefs.SetInt("SelectedCharacter", Number); // Save the selected character index
        SceneManager.LoadScene("KitchenArea"); // Load the first area scene
    }
    public void CancelSelection()
    {
        // Only hide panel if it exists
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false); // Hide the confirmation panel
        }
    }

    private void OnValidate() //Editor only function, allows of live visual change of character model
    {
        PlayerPrefs.SetInt("SelectedCharacter", Number);
        ShowCharacter();
    }


    #region Saving

    public object CaptureState()
    {
        SelectCharacterSaveData data = new SelectCharacterSaveData
        {
            IndexCharacter = Number
        };
        return data;
    }

    public void RestoreState(string state)
    {
        SelectCharacterSaveData data = JsonUtility.FromJson<SelectCharacterSaveData>(state);
        ChangeCharacter(data.IndexCharacter);
        PlayerPrefs.SetInt("SelectedCharacter", data.IndexCharacter);
    }
    private struct SelectCharacterSaveData
    {
        public int IndexCharacter;

    } 

    #endregion

}
