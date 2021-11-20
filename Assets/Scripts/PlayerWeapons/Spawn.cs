using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float segundoPraSpawn;
    private float segundoConfig;
    public GameObject ObjtoPraSpawn;
    // Start is called before the first frame update
    private void Start()
    {
        segundoConfig = segundoPraSpawn;
    }
    // Update is called once per frame
    void Update()
    {
        segundoPraSpawn -= Time.deltaTime;
        if (segundoPraSpawn < 0)
        {
            segundoPraSpawn = segundoConfig;
            SpawnObjeto();

        }
    }

    private void SpawnObjeto()
    {
        //Instantiate(ObjtoPraSpawn, new Vector3(UnityEngine.Random.Range(-13.0f, 13.0f), UnityEngine.Random.Range(-7.0f, 7.0f), 250), ObjtoPraSpawn.transform.rotation);
        Instantiate(ObjtoPraSpawn, transform.position, ObjtoPraSpawn.transform.rotation);
    }
}

