using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SoundEffectManager : MonoBehaviour
{
    
    private AudioSource AudioData { get; set; }
    
    private List<AudioClip> CurrentWord { get; set; }
    
    void Start() {
        this.AudioData = GetComponent<AudioSource>();
    }

    public void AddToQueue() {
        if (this.AudioData.isPlaying) {
            
        }
    }

    public void Update() {
        
    }

    public void CorrectAnswer() {
        Debug.Log("CorrectAnswer");
        this.AudioData.clip = Resources.Load<AudioClip>("Audio/SoundEffects/CorrectAnswer");
        this.AudioData.Play();
    }

    public void WrongAnswer() {
        Debug.Log("WrongAnswer");
        this.AudioData.clip = Resources.Load<AudioClip>("Audio/SoundEffects/WrongAnswer");
        this.AudioData.Play();
    }

    public void ButtonClick() {
        Debug.Log("ButtonClick");
        this.AudioData.clip = Resources.Load<AudioClip>("Audio/SoundEffects/ButtonClick");
        this.AudioData.Play();
    }
}
