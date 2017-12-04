using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NamedSound {
    public string name;
    public AudioClip audio;
}

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    public static bool disabled = false;

    AudioSource speaker;

    [SerializeField]
    public NamedSound[] list;

    public AudioClip GetSoundByName(string _name) {
        foreach (NamedSound ns in list) {
            if (ns.name == _name) return ns.audio;
        }
        return null;
    }

    public void PlaySound(string name) {
        AudioClip a = GetSoundByName(name);
        if (a == null) return;
        if (disabled) return;
        speaker.clip = a;
        speaker.PlayOneShot(a);
    }

    private void Awake() {
        instance = this;
        speaker = GetComponent<AudioSource>();
    }
}