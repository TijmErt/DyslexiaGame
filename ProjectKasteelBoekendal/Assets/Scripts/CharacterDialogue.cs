using UnityEngine;
using System.Collections;
public class CharacterDialogue : MonoBehaviour, IInteractable 
{
    [SerializeField]
    private DialogueCharacter dialogueCharacter;
    [SerializeField]
    private InputReader inputReader;
    [SerializeField]
    private Transform playerPositionPoint;


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

    public Vector3 GetPlayerPosPoint(PlayerInteraction player)
    {
        return playerPositionPoint.position;
    }

    public void Interact(PlayerInteraction player)
    {
        DialogueCharacterSpokenTo();
        string dialogueLine = GetDialogueLine(dialogueCharacter);
        DialogueUI dialogueUI = Object.FindFirstObjectByType<DialogueUI>();
        dialogueUI.UpdateDialogue(GetDialogueLine(dialogueCharacter));
        inputReader.EnableUI();
        dialogueCharacter.IsSpokenTo = true;
    }
}
