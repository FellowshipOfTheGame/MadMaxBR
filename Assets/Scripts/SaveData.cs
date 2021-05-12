using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<float> horizontalInputs = new List<float>();
    public List<float> verticalInputs = new List<float>();

    public SaveData(InputReplay inputReplay)
    {
        horizontalInputs = inputReplay.hInputs;
        verticalInputs = inputReplay.vInputs;
    }
}
