using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunnerAttributeList", menuName = "RunnerAttributeList")]
public class RunnerAttributeList : ScriptableObject {
    public ScriptableObject[] CarList;
    public string[] RunnerNameList;
}
