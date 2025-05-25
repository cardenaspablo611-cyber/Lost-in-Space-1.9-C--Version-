using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveAudioSettings : MonoBehaviour {

    [SerializeField] private Slider AudioEffectsVolumeSlider;
    [SerializeField] private Slider MusicVolumeSlider;

    [SerializeField] private MusicSettings MusicSettingsAdmin;

    private void Awake() {

       AudioEffectsVolumeSlider.GetComponent<Slider>().value = MusicSettingsAdmin.audioEffectsVolume;
        MusicVolumeSlider.GetComponent<Slider>().value = MusicSettingsAdmin.musicVolume;
    }

    private void Update() {

        MusicSettingsAdmin.audioEffectsVolume = AudioEffectsVolumeSlider.GetComponent<Slider>().value;
        MusicSettingsAdmin.musicVolume = MusicVolumeSlider.GetComponent<Slider>().value;
    }
}
