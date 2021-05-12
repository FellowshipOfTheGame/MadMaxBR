using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReplay : MonoBehaviour
{
    [HideInInspector] public List<float> hInputs = new List<float>();
    [HideInInspector] public List<float> vInputs = new List<float>();

    void FixedUpdate()
    {
        hInputs.Add(Input.GetAxis("Horizontal"));
        vInputs.Add(Input.GetAxis("Vertical"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Save();
        }
    }

    public void Save()
    {
        
        int saveIndex = 0;

        while (System.IO.File.Exists(string.Format("{0}/Save Inputs/SaveInput {1}.bin", Application.dataPath, saveIndex)))
        {
            saveIndex++;
        }

        print(hInputs.Count + "tamanhoooh");
        print(vInputs.Count + "tamanhooov");
        for (int i = 0; i < vInputs.Count; i++)
        {
            print(hInputs[i] + "h");
            print(vInputs[i] + "v");
        }
        SaveSystem.SaveInput(this, saveIndex);
    }
}
