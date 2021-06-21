using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [SerializeField] private Image progressBar;

    void Start()
    {
        // Start async operation
        StartCoroutine(LoadAysncOperation());
    }

    IEnumerator LoadAysncOperation()
    {
        // Create an async operation
        AsyncOperation gameScene = SceneManager.LoadSceneAsync(2);

        while (gameScene.progress < 1)
        {
            // Take the progress bar fill = async operation progress
            progressBar.fillAmount = gameScene.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
