using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {get; private set;}
    private AudioSource Source;

    private void Awake()
    {
        instance = this;
        Source = GetComponent<AudioSource>();

        //Keeps this object even we go to new scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Destroy duplicate game objects
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Playsound(AudioClip _sound)
    {
        Source.PlayOneShot(_sound);
    }
}