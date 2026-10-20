using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackgroundMusicController : MonoBehaviour {

    [SerializeField] private Slider MusicVolumeSlider;

    private AudioSource audioSource;

    private float maxVolumeMusic;
    private float minVolumeMusic = 0f;

    [SerializeField] private MusicSettings MusicSettingsAdmin;

    private bool outAnimation = false;

    private void Awake() {

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.0f;

        maxVolumeMusic = MusicSettingsAdmin.musicVolume;
    }
 
    private void Start() {

        fadeIn();
    }  

    private void Update() {

        if (MusicVolumeSlider != null) {
            if (!outAnimation) {
                StartCoroutine(ChangeVolume(MusicVolumeSlider.GetComponent<Slider>().value, 0.2f));

            }
        }
    }

    public void fadeOut() {
        outAnimation = true;
        StartCoroutine(ChangeVolume(minVolumeMusic, 0.5f));
    }

    private void fadeIn() {
        StartCoroutine(ChangeVolume(maxVolumeMusic, 0.5f));
    }

    public IEnumerator ChangeVolume(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Use unscaled time so music fade works even when timeScale = 0
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / duration;
            float currentVolume = Mathf.Lerp(startVolume, targetVolume, t * t); // Acceleration effect by squaring t
            audioSource.volume = currentVolume;
            yield return null;
        }

        audioSource.volume = targetVolume; // Ensure the target volume is set at the end
    }

}
