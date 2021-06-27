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

    void Start()
    {

    }

    void FixedUpdate()
    {
        ChooseTarget();
    }

    private void ChooseTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shootRadius, carMask);
        List<Collider> hitCollidersList = new List<Collider>(hitColliders);
        int selfColliderRemoveindex = 0;

        for (int i = 0; i < hitCollidersList.Count; i++)
        {
            if (hitCollidersList[i].transform.root == transform.root)
            {
                selfColliderRemoveindex = i;
            }
        }
        hitCollidersList.RemoveAt(selfColliderRemoveindex);

        if (hitCollidersList.Count > 0)
        {
            weaponState = WeaponState.target;
            float[] distances = new float[hitCollidersList.Count];
            int smallestDistanceIndex = 0;

            for (int i = 0; i < hitCollidersList.Count; i++)
            {
                distances[i] = (weaponBarrelEnd.transform.position - hitCollidersList[i].transform.position).sqrMagnitude; // sqrMagnitude is more efficient than Vector3.Distance
            }


            for (int i = 0; i < distances.Length; i++)
            {
                if (distances[i] < distances[smallestDistanceIndex])
                {
                    smallestDistanceIndex = i;
                }
            }

            if (hitCollidersList[smallestDistanceIndex])
            {
                Transform directionPoint = hitCollidersList[smallestDistanceIndex].transform;
                weaponBarrel.transform.LookAt(directionPoint);
                //weaponBarrel.transform.Rotate(10f, 0f, 0f);
                weaponAnimator.SetBool("IsShooting", true);
                //StartCoroutine(Shoot(shootRate));
            }
        }
        else
        {
            weaponAnimator.SetBool("IsShooting", false);
            weaponState = WeaponState.idle;
        }
    }

    IEnumerator Shoot(float firingRate)
    {

        yield return new WaitForSeconds(firingRate);
    }
}
