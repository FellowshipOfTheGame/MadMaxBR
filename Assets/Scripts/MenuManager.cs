using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private string garagem;

    public void StartGame()
    {
        SceneManager.LoadScene(garagem, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
