using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTarget : MonoBehaviour
{
    [SerializeField] private LayerMask carMask;
    [SerializeField] private GameObject weaponBarrel;
    [SerializeField] private GameObject weaponBarrelEnd;
    [SerializeField] private List<Collider> hitCollidersList;
    [SerializeField] private float shootRadius = 10f;

    [SerializeField] private int targetIndex;
    private float elapsed = 0f;
    [Range(0.05f, 0.3f)]
    [SerializeField] private float updateTarget = 0.33f;
    private bool changeTargetDalay = false;


    void FixedUpdate()
    {
        detectTarget();
        AMITarget();
    }

    void Update()
    {
        if (Input.GetMouseButton(1) && hitCollidersList.Count > 1 && changeTargetDalay == false)
        {
            if (targetIndex < (hitCollidersList.Count - 1))
            {
                targetIndex++;
            }
            else
            {
                targetIndex = 0;
            }
            changeTargetDalay = true;

            StartCoroutine(changeTargetTime(updateTarget));
        }
    }

    private void detectTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shootRadius, carMask);
        hitCollidersList = new List<Collider>(hitColliders);
        int selfColliderRemoveindex = 0;

        for (int i = 0; i < hitCollidersList.Count; i++)
        {
            if (hitCollidersList[i].transform.root == transform.root)
            {
                selfColliderRemoveindex = i;
            }
        }
        hitCollidersList.RemoveAt(selfColliderRemoveindex);
    }

    private void AMITarget()
    {
        if (hitCollidersList.Count > 0)
        {
            if (hitCollidersList.Count == 1)
            {
                rotationTurret(0);
            }
            else
            {
                if (targetIndex < (hitCollidersList.Count))
                {
                    rotationTurret(targetIndex);
                }
                else
                {
                    rotationTurret(0);
                }
            }
        }
        else
        {
            // Transform directionPoint = transform;
            weaponBarrel.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    private void rotationTurret(int index)
    {
        Transform directionPoint = hitCollidersList[index].transform;
        weaponBarrel.transform.LookAt(directionPoint);
        // Vector3 turretLookDir = rectPosition - transform.position;
        // finalTurretLookDir = Vector3.Lerp(finalTurretLookDir, turretLookDir, Time.deltaTime * velocidadeRotacao);
        // transform.rotation = Quaternion.LookRotation(turretLookDir);

        // Vector3 turretLookDir = hitCollidersList[index].transform.position - weaponBarrel.transform.position;
        // weaponBarrel.transform.LookAt(Vector3.Lerp(hitCollidersList[index].transform.position, weaponBarrel.transform.position, Time.deltaTime * velocidadeRotacao));
        // weaponBarrel.transform.LookAt(finalTurretLookDir);
        // weaponBarrel.transform.rotation = Quaternion.Slerp(weaponBarrel.transform.rotation, Quaternion.Euler(), Time.deltaTime * velocidadeRotacao);
        //  weaponBarrel.transform.rotation = Quaternion.Lerp(weaponBarrel.transform.rotation, new Quaternion.Euler(hitCollidersList[index].transform.position), Time.deltaTime * velocidadeRotacao);
    }

    private IEnumerator changeTargetTime(float time)
    {
        yield return new WaitForSeconds(time);
        changeTargetDalay = false;
    }
}
