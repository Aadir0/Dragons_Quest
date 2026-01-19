using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioSwitching : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private AudioMixer mixer;
    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", Mathf.Log10(volume) * 20); //Set the volume in the audio mixer
    }

    public void StopSound()
    {
        SoundManager.instance.StopAll(); //Stops any currently playing sound
    }
    public void Back()
    {
        SceneManager.LoadScene(0); //Load the main menu scene
    }
    
    public void BacktoPause()
    {
        pauseMenu.SetActive(true); //Return to the pause menu
        audioMenu.SetActive(false); //Hide the audio settings menu
    }
}
