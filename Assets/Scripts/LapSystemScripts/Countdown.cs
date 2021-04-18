using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    public GameObject CountdownUI;
    public AudioSource GetReadyAudio;
    public AudioSource GoAudio;
    public GameObject[] Racers;

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
        //Player.transform.GetChild(0).GetComponent<VehicleControl>().activeControl = true;
        //PlayerControls.GetComponent<VehicleControl>().activeControl = true;
        
        for (int i = 0; i < Racers.Length; i++) {
            Racers[i].transform.GetChild(0).GetComponent<VehicleControl>().activeControl = true; // active control for the racer 'i'
            Racers[i].transform.GetChild(0).GetComponent<VehicleRaceData>().enabled = true; // active the racer 'i' data script
        }
    }
    
}
