using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    public RaceManager RaceManager;
    public GameObject CountdownUI;
    public GameObject PlayerRaceTimeDisplayer;
    public AudioSource GetReadyAudio;
    public AudioSource GoAudio;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(CountStart());
    }
    
    IEnumerator CountStart() {
        yield return new WaitForSeconds(0.5f);
        CountdownUI.GetComponent<Text>().text = "3";
        GetReadyAudio.Play();
        CountdownUI.SetActive(true);
        yield return new WaitForSeconds(1);
        CountdownUI.SetActive(false);
        CountdownUI.GetComponent<Text>().text = "2";
        GetReadyAudio.Play();
        CountdownUI.SetActive(true);
        yield return new WaitForSeconds(1);
        CountdownUI.SetActive(false);
        CountdownUI.GetComponent<Text>().text = "1";
        GetReadyAudio.Play();
        CountdownUI.SetActive(true);
        yield return new WaitForSeconds(1);
        CountdownUI.SetActive(false);
        PlayerRaceTimeDisplayer.SetActive(true);
        GoAudio.Play();
        RaceManager.StartRace();
    }
    
}
