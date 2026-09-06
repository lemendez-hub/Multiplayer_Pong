using UnityEngine.Audio;
using UnityEngine;

[System.Serializable] // This attribute allows the Sound class to be serialized and shown in the Unity Inspector
public class Sound{
    // Variables
    public string name;
    public AudioClip clip;
    
    [Range(0f, 5f)] // Sets a range from 0 to 5 for the volume variable in the Unity Inspector
    public float volume;
    
    [Range(0.1f, 5f)] // Sets a range from 0.1 to 5 for the pitch variable in the Unity Inspector
    public float pitch;
    public bool loop;
    
    [HideInInspector] // This attribute hides the source variable from the Unity Inspector
    public AudioSource source;
}