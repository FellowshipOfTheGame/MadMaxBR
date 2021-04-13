using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCam : MonoBehaviour
{
    public Transform alvo;
    private Vector3 offSet;
    // Start is called before the first frame update
    void Start()
    {
        offSet = transform.position - alvo.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = alvo.position + offSet;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        //transform.position = new Vector3(alvo.position.x,alvo.position.y,transform.position.z);
    }
}
