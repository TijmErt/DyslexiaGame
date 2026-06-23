using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        UIAudio.Source = audioSource;
    }
}

public static class UIAudio
{
    public static AudioSource Source;

    public static void Play(AudioClip clip)
    {
        if (clip != null)
            Source.PlayOneShot(clip);
    }
}
