using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Limit
{
    public float xMin, xMax, zMin, zMax;
}

public class playercontroler : MonoBehaviour
{

    private Rigidbody rb;
    public float speed;
    public Limit limit;
    public float tilt;
  
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 mov = new Vector3(moveHorizontal, 0f, moveVertical);
        rb.velocity = mov * speed;
        rb.position = new Vector3 (Mathf.Clamp(rb.position.x,limit.xMin, limit.xMax)  , 0f, Mathf.Clamp(rb.position.z, limit.zMin, limit.zMax));
        rb.rotation = Quaternion.Euler(0f, 0f, rb.velocity.x * -tilt);
       
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
