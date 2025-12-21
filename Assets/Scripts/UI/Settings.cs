using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject keybindsMenu;
    public void Volume()
    {
        audioMenu.SetActive(true); // Show the audio settings menu
    }
    
    public void Keybinds()
    {
        keybindsMenu.SetActive(true); // Show the keybinds settings menu
    }

    public void Back()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Load the main menu scene
    }
}