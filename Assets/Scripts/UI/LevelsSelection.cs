using UnityEngine;

public class LevelsSelection : MonoBehaviour
{
    public void SelectLevel(int levelIndex)
    {
        if (levelIndex <= 0)
        {
            return;
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex); // Load the selected level scene
        }
    }
}
