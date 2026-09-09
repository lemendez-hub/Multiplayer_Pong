using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour{
    // Variables
    public Sound [] sounds;
    
    public static AudioManager instance;
    
    public AudioSource audioSource;

    // Awake is called before the first frame update
    void Awake(){
        if (instance == null) // If there is no instance of AudioManager, set it to this
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // If there is already an instance of AudioManager, destroy this one
            return;
        }
 
        DontDestroyOnLoad(gameObject); // Don't destroy this game object when loading a new scene

        foreach (Sound s in sounds) // For each sound in the sounds array, create an audio source and set its properties
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Play is called to play a sound by name
    public void Play(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        s.source.Play();
    }

    // GetAudioSource is called to get the audio source of a sound by name
    public AudioSource GetAudioSource(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        
        return s.source;
    }
}