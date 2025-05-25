using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class AudioManager : MonoBehaviour {

    [SerializeField] private GameObject audioPlayerPrefab; // Prefab with AudioSelfDestruct script
    [SerializeField] private List<AudioClipEntry> audioClips = new List<AudioClipEntry>();
    private Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
    [SerializeField] private MusicSettings musicSettings;

    [System.Serializable]
    public class AudioClipEntry {
        public string name; // Name of the clip
        public AudioClip clip; // The corresponding AudioClip
    }

    private void Awake() {
        foreach (var entry in audioClips) {
            clipDictionary[entry.name] = entry.clip;
        }
    }

    public void PlayEffect(string clipName) {

        if (clipDictionary.TryGetValue(clipName, out var clip)) {
            GameObject audioPlayer = Instantiate(audioPlayerPrefab);
            AudioSelfDestruct selfDestruct = audioPlayer.GetComponent<AudioSelfDestruct>();
            selfDestruct.PlayAudioClip(clip, musicSettings.audioEffectsVolume);
        } else {
            UnityEngine.Debug.LogWarning($"Audio clip '{clipName}' not found!");
        }
    }
}
