using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class Countdown : MonoBehaviour {

    public GameObject CountdownUI;
    public AudioSource GetReadyAudio;
    public AudioSource GoAudio;
    public GameObject LapTimer;
    public GameObject CarControls;

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
        GoAudio.Play();
        CarControls.GetComponent<CarUserControl>().enabled = true;
        LapTimer.SetActive(true);
    }
    
}
