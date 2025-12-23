using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioSwitching : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private AudioMixer mixer;
    public void Volume(float value)
    {
        mixer.SetFloat("Volume", value);
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void Back()
    {
        SceneManager.LoadScene(0); // Load the main menu scene
    }
    
    public void BacktoPause()
    {
        pauseMenu.SetActive(true); // Return to the pause menu
        audioMenu.SetActive(false); // Hide the audio settings menu
    }
}
