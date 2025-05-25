using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAutomaticlyVolume : MonoBehaviour {
    
    [SerializeField] private Slider MusicVolumeSlider;

    private void Update () {

        gameObject.GetComponent<BackgroundMusicController>().ChangeVolume(MusicVolumeSlider.value, 0.2f);
    }
}
