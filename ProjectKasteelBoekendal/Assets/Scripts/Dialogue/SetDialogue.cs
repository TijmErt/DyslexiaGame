using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Vergeet deze niet!

[UnitTitle("Set Dialogue")]
[UnitCategory("Dialogue")]
public class SetDialogue : Unit
{
    [DoNotSerialize] [PortLabelHidden] public ControlInput InputTrigger { get; private set; }
    [DoNotSerialize] [PortLabelHidden] public ControlOutput OutputTrigger { get; private set; }
    
    [DoNotSerialize] public ValueInput DialogueOverlay;
    [DoNotSerialize] public ValueInput Text;
    [DoNotSerialize] public ValueInput Character;
    [DoNotSerialize] public ValueInput Name;
    
    [DoNotSerialize] public ValueOutput DialogueOverlayOutput;
    
    protected override void Definition()
    {
        // We veranderen ControlInput naar ControlInputCoroutine
        this.InputTrigger = ControlInputCoroutine(nameof(this.InputTrigger), Trigger);
        
        this.OutputTrigger = ControlOutput(nameof(this.OutputTrigger));
        
        this.DialogueOverlay = ValueInput<GameObject>(nameof(this.DialogueOverlay));
        this.Text = ValueInput<string>(nameof(this.Text), string.Empty);
        this.Character = ValueInput<Sprite>(nameof(this.Character), null);
        this.Name = ValueInput<string>(nameof(this.Name), string.Empty);
        
        this.DialogueOverlayOutput = ValueOutput<GameObject>(nameof(this.DialogueOverlayOutput), (flow) => flow.GetValue<GameObject>(this.DialogueOverlay));
        
        Succession(this.InputTrigger, this.OutputTrigger);
    }
    
    private System.Collections.IEnumerator Trigger(Flow flow)
    {
        // Get components
        var overlay = flow.GetValue<GameObject>(this.DialogueOverlay);
        var dialogueTextbox = overlay.transform.Find("Background/Dialogue").gameObject.GetComponent<TextMeshProUGUI>();
        var characterImage = overlay.transform.Find("Background/Character").gameObject.GetComponent<Image>();
        var nameTextbox = overlay.transform.Find("Background/NameBackground/Name").gameObject.GetComponent<TextMeshProUGUI>();
        
        // Set new values
        if (flow.GetValue<string>(this.Text) != null) {
            dialogueTextbox.text = flow.GetValue<string>(this.Text);
        }
        if (flow.GetValue<Sprite>(this.Character) != null) {
            characterImage.sprite = flow.GetValue<Sprite>(this.Character);
        }
        if (flow.GetValue<string>(this.Name) != null) {
            nameTextbox.text = flow.GetValue<string>(this.Name);
        }
        

        // Wait for input
        var mouse = Mouse.current;
        if (mouse != null)
        {
            while (mouse.leftButton.isPressed)
            {
                yield return null;
            }
            
            while (!mouse.leftButton.isPressed)
            {
                yield return null;
            }
        }
        
        yield return this.OutputTrigger;
    }
}