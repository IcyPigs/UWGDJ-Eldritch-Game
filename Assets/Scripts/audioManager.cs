using UnityEngine.Audio;
using System;
using UnityEngine;


public class audioManager : MonoBehaviour
{
    public static audioManager instance { get; private set;}
    public Sound[] sounds;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if(frequencyController.instance == null)
            instance = this;
        else 
            Destroy(gameObject);


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null) 
        {
            Debug.Log(name + "isn't real.");
        }
        s.source.Play();
    }
}

