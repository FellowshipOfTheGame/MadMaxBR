using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityStandardAssets.CrossPlatformInput;

public class PlayerControlerMundoAberto : MonoBehaviour
{
    public LimitJoy limJoy;
    public float tilt;
    //public Camera cam1;

    //public GameObject mira;


    private Rigidbody rb;
    public float speed;
    public float speedLeap = 0.9f;
    /*
        public GameObject municao;//bala
        public GameObject canoEsquerdo;//cano
        public GameObject canoDireito;//cano
        //public ParticleSystem FireParticle; // Particula de Fogo.
    */
    public float FireRate = 0.1f;        //Tempo Para Arma ATIRAR.
    private float currentTimeToFire = 0; //Cronometro.
    private bool canFire = true;         //Poder ou Nao Atirar.


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }
  /*   private void FixedUpdate()
    {
        float moveHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float moveVertical = CrossPlatformInputManager.GetAxis("Vertical");
        //float moveHorizontal = joyCont.Hori();
        //float moveVertical = joyCont.Vert();
        Vector3 mov = new Vector3(0f, moveVertical, moveHorizontal);
        // Vector3 mov = new Vector3(moveVertical * -1, moveHorizontal * -1, 0f);
        rb.velocity = mov * speed;
        rb.transform.Translate((speed * -1) * Time.deltaTime, 0, 0);
        rb.rotation = Quaternion.Lerp(rb.transform.rotation, Quaternion.Euler(rb.velocity.y * -tilt, rb.velocity.y* -tilt, rb.velocity.x * -tilt), Time.time * speedLeap);
    } */

    // Update is called once per frame
    void Update()
    {

    }
}
