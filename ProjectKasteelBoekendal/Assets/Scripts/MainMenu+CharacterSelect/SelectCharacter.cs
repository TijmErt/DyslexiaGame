using System;
using UnityEngine;
using System.Collections;
using Managers.Saving;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour, ISaveable
{
    public GameObject[] characters; // Array to hold character GameObjects
    public int Number; // Index of the currently selected character
    public GameObject confirmationPanel; // Panel to confirm character selection
    [SerializeField] private AudioSource buttonClickSound;
    public string UID => "SelectCharacter";
    
    /*
        
        
        This class is partial broken, it uses PlayerPrefs to store the Character Select State between scene's, this worked at first but suddenly decided it didn't want to work like this anymore.
        
        No Idea how to fix this, so this is your responsibility now.
        
         
     */
    
    private void Start()
    {
        // Only hide panel if it exists
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }
        if (PlayerPrefs.HasKey("SelectedCharacter"))
            Debug.Log("Key exists!");
        else
            Debug.Log("Key DOES NOT exist!");

        Debug.Log(PlayerPrefs.GetInt("SelectedCharacter", -1));

        Number = PlayerPrefs.GetInt("SelectedCharacter");
        Debug.Log("PlayerPref Start num: " + Number);
        ShowCharacter(); // Display the currently selected character
    }
    private void PlayButtonSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
    }
    public void ChangeCharacter(int Num)
    {
        PlayButtonSound();

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }

        Number += Num;

        if (Number > characters.Length - 1)
        {
            Number = 0;
        }

        if (Number < 0)
        {
            Number = characters.Length - 1;
        }

        characters[Number].SetActive(true);
        Debug.Log("PlayerPref Change num: " + Number);
    }

    void ShowCharacter()
    {
        if (characters == null || characters.Length == 0)
            return;
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
        Debug.Log("PlayerPref Show num: " + Number);
        
        characters[Number].SetActive(true);
        
        if(GetComponent<PlayerMovement>())
        {
            Animator animator = characters[Number].GetComponent<Animator>();
            GetComponent<PlayerMovement>().SetAnimator(animator);
        }
 
    }

    public void ConfirmCharacter()
    {
        PlayButtonSound();
        // Only show panel if it exists
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(true); // Show the confirmation panel
        }
    }

    public void ConfirmSelection()
    {
        PlayButtonSound();
        Debug.Log("PlayerPref Confirm num: " + Number);
        PlayerPrefs.SetInt("SelectedCharacter", Number); // Save the selected character index
        PlayerPrefs.Save(); 
        SceneManager.LoadScene("KitchenArea"); // Load the first area scene
    }
    public void CancelSelection()
    {
        PlayButtonSound();
        // Only hide panel if it exists
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false); // Hide the confirmation panel
        }
    }

    private void OnValidate() //Editor only function, allows of live visual change of character model
    {
        PlayerPrefs.SetInt("SelectedCharacter", Number);
        if (characters == null || characters.Length == 0)
            return;
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
        PlayerPrefs.SetInt("SelectedCharacter", data.IndexCharacter);
        ShowCharacter();
    }
    private struct SelectCharacterSaveData
    {
        public int IndexCharacter;

    } 

    #endregion

}
