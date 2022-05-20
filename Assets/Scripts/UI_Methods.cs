using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Methods : MonoBehaviour
{
    public void ReloadCurrentScene ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene (int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadNextScene ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit ()
    {
        Application.Quit();
    }

    public enum Scenes
    {
        MENU,
        GAME,
    }
}