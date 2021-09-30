using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HistoryManager : MonoBehaviour
{
    public float timeSlider = 1.0f;
    public string gameCene;

    [SerializeField] private GameObject capa = null;
    [SerializeField] private GameObject page1 = null;
    [SerializeField] private GameObject page2 = null;
    [SerializeField] private GameObject page3 = null;
    [SerializeField] private GameObject stopButton = null;


    private void Start()
    {
        StartCoroutine(Slider(timeSlider));
    }

    IEnumerator Slider(float timer)
    {
        yield return new WaitForSeconds(timer);
        Page1();
        yield return new WaitForSeconds(timer);
        Page2();
        yield return new WaitForSeconds(timer);
        Page3();
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(gameCene, LoadSceneMode.Single);

    }

    private void Page1()
    {
        capa.SetActive(false);
        stopButton.SetActive(true);
        page1.SetActive(true);
    }

    private void Page2()
    {
        page1.SetActive(false);
        page2.SetActive(true);

    }

    private void Page3()
    {
        page2.SetActive(false);
        page3.SetActive(true);
    }

    public void StopHistory()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(gameCene, LoadSceneMode.Single);
    }
}

