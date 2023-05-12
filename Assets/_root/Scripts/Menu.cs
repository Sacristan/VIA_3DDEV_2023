using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        Debug.Log(nameof(Play));
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        Debug.Log(nameof(Settings));
    }

    public void Quit()
    {
        Debug.Log(nameof(Quit));
        Application.Quit();
    }
}
