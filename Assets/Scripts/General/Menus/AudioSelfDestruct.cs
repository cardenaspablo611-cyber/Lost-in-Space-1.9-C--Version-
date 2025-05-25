using UnityEngine;

public class AudioSelfDestruct : MonoBehaviour {
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (!audioSource.isPlaying) {
            Destroy(gameObject);
        }
    }

    public void PlayAudioClip(AudioClip clip, float volume) {

        audioSource.volume = volume;
        audioSource.clip = clip;    
        audioSource.Play();         
    }
}
