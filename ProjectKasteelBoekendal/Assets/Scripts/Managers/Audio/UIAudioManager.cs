using UnityEngine;

namespace Managers.Audio
{
    /// <summary>
    /// Assigns the UI audio source used by the static UIAudio helper.
    /// </summary>
    public class UIAudioManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            UIAudio.Source = audioSource;
        }
    }
    /// <summary>
    /// Provides a centralized audio source for playing UI sound effects.
    /// This allows UI elements to play audio without requiring a direct
    /// reference to an AudioSource component.
    /// </summary>
    public static class UIAudio
    {
        public static AudioSource Source;

        /// <summary>
        /// Plays a UI sound effect through the shared UI audio source.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        public static void Play(AudioClip clip)
        {
            if (clip == null || Source == null)
                return;

            Source.PlayOneShot(clip);
        }
    }
}