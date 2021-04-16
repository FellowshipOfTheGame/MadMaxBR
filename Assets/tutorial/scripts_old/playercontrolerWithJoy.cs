using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

[System.Serializable]
public class LimitJoy
{
    public float xMin, xMax, yMin, yMax;

}

public class playercontrolerWithJoy : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public float speedLeap = 0.9f;
    public LimitJoy limJoy;
    public float tilt;
    //public Camera cam1;
    public Camera cam2;
    public GameObject mira;

    public bool cameras = true;
    

    public GameObject municao;//bala
    public GameObject canoEsquerdo;//cano
    public GameObject canoDireito;//cano
    //public ParticleSystem FireParticle; // Particula de Fogo.

    public float FireRate = 0.1f;        //Tempo Para Arma ATIRAR.
    private float currentTimeToFire = 0; //Cronometro.
    private bool canFire = true;         //Poder ou Nao Atirar.


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
/*     private void FixedUpdate()
    {
        float moveHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float moveVertical = CrossPlatformInputManager.GetAxis("Vertical");
        //float moveHorizontal = joyCont.Hori();
        //float moveVertical = joyCont.Vert();
        Vector3 mov = new Vector3(moveHorizontal, moveVertical, 0f);
        rb.velocity = mov * speed;
        rb.position = new Vector3(Mathf.Clamp(rb.position.x, limJoy.xMin, limJoy.xMax), Mathf.Clamp(rb.position.y, limJoy.yMin, limJoy.yMax), 0f);
        if (moveHorizontal == 0 && moveVertical == 0)
        {
            rb.rotation = Quaternion.Lerp(rb.transform.rotation, Quaternion.Euler(rb.velocity.y * -tilt, rb.velocity.x, rb.velocity.x * -tilt), Time.time * speedLeap);
        }
        else
        {
            rb.rotation = Quaternion.Euler(rb.velocity.y * -tilt, rb.velocity.x, rb.velocity.x * -tilt);
        }
        ////rb.rotation = Quaternion.Lerp(rb.transform.rotation,Quaternion.Euler(rb.velocity.y * -tilt, rb.velocity.x, rb.velocity.x * -tilt), Time.time * speedLeap);
    } */
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /* if (CrossPlatformInputManager.GetButtonDown("Fire1"))///vefirica se a tecla foi apertado e tiver liberado pulo
         {
             Tiro();
         }*/
/*          if (CrossPlatformInputManager.GetButton("Fire1") && canFire == true)///vefirica se a tecla foi apertado e tiver liberado pulo
        {
             Tiro();
        } */

        if (canFire == false)
        { //Se Pode Atirar for Falso.
            currentTimeToFire += Time.deltaTime; //Cronometro Começa A contar.
            if (currentTimeToFire > FireRate)
            { // Se Cronometro For Maior que Tempo para Atira.
                currentTimeToFire = 0;  //Cronometro vai Zerar.
                canFire = true;   //Pode Atirar Fica True.
            }
        }
   /*      if (CrossPlatformInputManager.GetButtonDown("camera"))///inverte a camera
        {
            CameraControle(!cameras);
        } */
    }
    public void Tiro()
    {
        Instantiate(municao, canoDireito.transform.position, canoDireito.transform.rotation);
        Instantiate(municao, canoEsquerdo.transform.position, canoEsquerdo.transform.rotation);
        // Instantiate(municao, new Vector3(canoEsquerdo.transform.position.x, canoEsquerdo.transform.position.y, canoEsquerdo.transform.position.z), Quaternion.identity);
        canFire = false;   // E Pode Atirar fica False.
        //FireParticle.Emit(1); //E A Particula Aparece.
        //GetComponent<AudioSource>().Play(); // E Solta um Audio
    }
    public void CameraControle(bool b)
    {
        if (b)
        {
            //cam1.gameObject.SetActive(true);
            cam2.gameObject.SetActive(false);
            mira.gameObject.SetActive(false);
        }
        else if (!b)
        {
            cam2.gameObject. SetActive(true);
             mira.gameObject.SetActive(true);
            //cam1.gameObject.SetActive(false);
        }
        cameras = b;
    }
}
