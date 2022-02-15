using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunnerAttributeList", menuName = "RunnerAttributeList")]
public class RunnerAttributeList : ScriptableObject {
    public GameObject[] CarList;
    public string[] RunnerNameList;
}
