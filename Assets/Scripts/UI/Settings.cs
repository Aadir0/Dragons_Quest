using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject settings;

    private void Awake()
    {
        settings.SetActive(true);
    }
    public void Volume()
    {
        
    }

    public void Music()
    {
        
    }
    
    public void Keybinds()
    {
        
    }

    public void Back()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("0"); // Load the main menu scene
    }
}
