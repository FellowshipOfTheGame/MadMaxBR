using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    #region Singleton

    public static SceneManagement instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {

    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    public void LoadGarageScene()
    {
        SceneManager.LoadScene("Garage", LoadSceneMode.Single);
    }
    public void LoadLoadingScene()
    {
        SceneManager.LoadScene("Loading Scene", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
