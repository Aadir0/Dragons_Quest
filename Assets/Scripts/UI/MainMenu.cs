using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject levelSelectionMenu;
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // Load the first level scene
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true); // Show the settings menu
        optionsMenu.SetActive(false); // Hide the options menu
    }

    public void levelSelection()
    {
        levelSelectionMenu.SetActive(true); // Show the level selection menu
    }
    
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   //Exits unity editor play mode (Can only be execuited in the editor)
        #endif
    }
}
