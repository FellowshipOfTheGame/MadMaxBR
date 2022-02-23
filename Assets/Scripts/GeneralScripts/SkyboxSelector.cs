using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Randomly selects a skybox to the scene.
/// </summary>
public class SkyboxSelector : MonoBehaviour {

    public List<Material> SkyboxList;
    public bool isRandom;
    public int SkyboxNumber;

    private void Awake() {
        if (isRandom) {
            RenderSettings.skybox = SkyboxList[Random.Range(0, SkyboxList.Count)];
        } else {
            RenderSettings.skybox = SkyboxList[SkyboxNumber];
        }   
    }
}
