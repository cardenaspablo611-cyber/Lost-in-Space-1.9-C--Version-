using UnityEngine;
using System.Reflection;
using System;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "AudioSettings")]
public class MusicSettings : ScriptableObject {
    
    public float audioEffectsVolume;
    public float musicVolume;
}
