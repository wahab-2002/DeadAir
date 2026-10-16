using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundClips : MonoBehaviour
{
    [Serializable] public class Sound { public string role; public AudioClip clip; }
    public List<Sound> sounds = new List<Sound>();
    public AudioClip Find(string role)
    {
        Sound sound = sounds.Find(item => item.role == role);
        return sound == null ? null : sound.clip;
    }
}
