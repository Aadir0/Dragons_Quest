using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverClip;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private MonoBehaviour playerAttack;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if you forgot to close pause menu then is will automatically close
            if (pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
                playerAttack.enabled = true;
            }
            else
            {
                PauseGame(true);
                playerAttack.enabled = false;
            }
        }
    }

    #region Game Over
    //Activating GameOver Screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        if (SoundManager.instance != null)
            SoundManager.instance.Playsound(gameOverClip);
    }

    //Game over functions
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Returntomenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();  //Quits the game (only works in game)

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   //Exits unity editor play mode (Can only be execuited in the editor)
        #endif
    }
    public void Volume()
    {
        audioMenu.SetActive(true); // Show the audio settings menu
        pauseScreen.SetActive(false); // Hide the pause menu
    }
    #endregion

    #region  Pause
    public void PauseGame(bool status)
    {
        //If status is true = pause || If status is false = unpause
        pauseScreen.SetActive(status);

        //If pause status is true change timescale to 0 | When pause status is false change timescale to 1
        if (status)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    #endregion
}
