using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSwitching : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject audioMenu;
    public void Volume()
    {
        
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
