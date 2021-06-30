using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas = null;
    [SerializeField] private GameObject creditsCanvas = null;
    [SerializeField] private GameObject optionsCanvas = null;


    private void Start()
    {
        
    }

    public void CreditsButton()
    {
        menuCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void BackFromCreditsButton()
    {
        creditsCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }

    public void OptionsButton()
    {
        menuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }

    public void BackFromOptionsButton()
    {
        optionsCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }
}
