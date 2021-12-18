using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAI : MonoBehaviour
{
    [SerializeField] private LayerMask carMask;
    [SerializeField] private GameObject weaponBarrel;
    [SerializeField] private GameObject weaponBarrelEnd;

    [SerializeField] private float shootRate = 0.07f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float shootRadius = 10f;
    [SerializeField] private WeaponState weaponState = WeaponState.idle;
    [SerializeField] private Animator weaponAnimator;

    private Collider[] hitColliders;
    private int hitColliderLength;
    private float[] distances;
    private int smallestDistanceIndex;
    private int i;
    private Transform directionPoint;

    void Start()
    {

    }

    private void OnEnable()
    {

    }

    void FixedUpdate()
    {
        ChooseTarget();
    }

    private void ChooseTarget()
    {
        hitColliders = Physics.OverlapSphere(transform.position, shootRadius, carMask);
        //List<Collider> hitCollidersList = new List<Collider>(hitColliders);
        //int selfColliderRemoveindex = 0;

        /*
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].transform.root == transform.root)
            {
                selfColliderRemoveindex = i;
            }
        }
        hitColliders.Remove(selfColliderRemoveindex);*/

        hitColliderLength = hitColliders.Length;

        if (hitColliderLength > 1)
        {
            weaponState = WeaponState.target;
            distances = new float[hitColliderLength - 1];
            smallestDistanceIndex = 0;

            for (i = 0; i < hitColliderLength - 1; i++)
            {
                distances[i] = (weaponBarrelEnd.transform.position - hitColliders[i + 1].transform.position).sqrMagnitude; // sqrMagnitude is more efficient than Vector3.Distance
            }

            for (i = 0; i < hitColliderLength - 1; i++)
            {
                if (distances[i] < distances[smallestDistanceIndex])
                {
                    smallestDistanceIndex = i;
                }
            }

            if (hitColliders[smallestDistanceIndex])
            {
                directionPoint = hitColliders[smallestDistanceIndex].transform;
                weaponBarrel.transform.LookAt(directionPoint);
                //weaponBarrel.transform.Rotate(10f, 0f, 0f);
                weaponAnimator.Play("Shoot");
                StartCoroutine(Shoot(shootRate));
            }
        }
        else
        {
            weaponAnimator.Play("Idle");
            weaponState = WeaponState.idle;
        }
    }

    IEnumerator Shoot(float firingRate)
    {
        //Dar dano

        yield return new WaitForSeconds(firingRate);
    }
}
