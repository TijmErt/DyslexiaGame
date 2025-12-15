using UnityEngine;
using System.Collections;
public class CharacterDialogue : MonoBehaviour
{
    [SerializeField]
    private DialogueCharacter dialogueCharacter;
    [SerializeField]
    private InputReader inputReader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DialogueCharacterSpokenTo();
            string dialogueLine = GetDialogueLine(dialogueCharacter);
            DialogueUI dialogueUI = Object.FindFirstObjectByType<DialogueUI>();
            dialogueUI.UpdateDialogue(GetDialogueLine(dialogueCharacter));
            inputReader.EnableUI();
            dialogueCharacter.IsSpokenTo = true;
        }
    }

    public string GetDialogueLine(DialogueCharacter character)
    {
        return character.Get();
    }
    
    private void DialogueCharacterSpokenTo()
    {
        if(dialogueCharacter.IsSpokenTo)
        {
            dialogueCharacter.ChangeState("Not Found");
        }
    }
}
