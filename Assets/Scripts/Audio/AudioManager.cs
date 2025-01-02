using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = 1.0f;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.Play();
        }
    }

    void Start () {
        Debug.Log("i should play!");
        Play("Theme"); //bgm
    }

    public void Play (string name) {
        Debug.Log("i should play2!");
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
