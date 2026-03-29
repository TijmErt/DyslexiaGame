using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class TutorialDialogue
{

    public string dialogue;
    
    public bool areaButtonActive = false;
    public Vector2 areaButtonPosition;
    public Vector2 areaButtonSize;
    
    [CanBeNull] public string topButtonText;
    [CanBeNull] public string bottomButtonText;
    
    [CanBeNull] public Sprite dialogueSprite;
    
    [CanBeNull] public Button.ButtonClickedEvent onTopButtonClick;
    [CanBeNull] public Button.ButtonClickedEvent onBottomButtonClick;
    [CanBeNull] public Button.ButtonClickedEvent onAreaButtonClick;
}
