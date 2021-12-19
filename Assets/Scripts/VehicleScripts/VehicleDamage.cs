using UnityEngine;
using System.Collections;

public class VehicleDamage : MonoBehaviour {
    public float maxMoveDelta = 1.0f; // maximum distance one vertice moves per explosion (in meters)
    public float maxCollisionStrength = 50.0f; // maximum force of the collision
    public float yForceDampening = 0.1f; // dampening of forces in the y axis, [0.0, 1.0]
    public float demolitionRange = 0.5f; // defines the spacial size of the damage
    public float impactDirManipulator = 0.0f;
    public MeshFilter[] optionalMeshList; // car mesh that can be damaged
    public AudioSource crashSound;

    private MeshFilter[] meshfilters;
    private float sqrDemRange;

    public void Start() {
        if (optionalMeshList.Length > 0) {
            meshfilters = optionalMeshList;
        } else {
            meshfilters = GetComponentsInChildren<MeshFilter>();
        }
            
        sqrDemRange = demolitionRange * demolitionRange;
    }

    private Vector3 colPointToMe;
    private float collisionStrength;

    public void OnCollisionEnter(Collision collision) {
        //  if (collision.gameObject.CompareTag("car")) return;
        Vector3 colRelativeVelocity = collision.relativeVelocity;
        //Debug.DrawRay(colRelativeVelocity, colRelativeVelocity, Color.green, 99999999, false);
        //colRelativeVelocity.y *= yForceDampening;

        //Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.green, 2, false);

        if (collision.contacts.Length > 0) {
            collisionStrength = colRelativeVelocity.magnitude;

            if (collisionStrength > 1.0f && !crashSound.isPlaying) { // if there is a collision that causes damage
                crashSound.Play();
                crashSound.volume = collisionStrength / 200; // change the volume of the crash Sound according to the strenght of the collision

                gameObject.GetComponent<VehicleData>().ReceiveDamage(CalculateHealthDamage(collisionStrength)); // decreases health of the car

                // OnMeshForce(collision.contacts[0].point, Mathf.Clamp01(collisionStrength / maxCollisionStrength));
                //OnMeshForce(collision.GetContact(0).point, Mathf.Clamp01(collisionStrength / maxCollisionStrength));
            }
        }
    }

    private float CalculateHealthDamage(float force) {
        return force;
    }
    /*
    // if called by SendMessage(), we only have 1 param
    public void OnMeshForce(Vector4 originPosAndForce) {
        OnMeshForce((Vector3)originPosAndForce, originPosAndForce.w);
    }


    public void OnMeshForce(Vector3 originPos, float force) {
        // force should be between 0.0 and 1.0
        force = Mathf.Clamp01(force);

        for (int j = 0; j < meshfilters.Length; ++j) {
            Vector3[] verts = meshfilters[j].mesh.vertices;

            for (int i = 0; i < verts.Length; ++i) {
                Vector3 scaledVert = Vector3.Scale(verts[i], transform.localScale); // scales the vertice accordingly to the car scale
                Vector3 vertWorldPos = meshfilters[j].transform.position + (meshfilters[j].transform.rotation * scaledVert); // position of the scaled vertice in the world
                Vector3 dirOriginToVert = vertWorldPos - originPos; // direction between the point of collision and the vertice analyzed
                Vector3 flatVertToCenterDir = transform.position - vertWorldPos; // direction between the center of the car and the vertice analyzed
                flatVertToCenterDir.y = 0.0f;

                // 0.5 - 1 => 45° to 0°  / current vertice is nearer to exploPos than center of bounds
                if (dirOriginToVert.sqrMagnitude < sqrDemRange) { // dot > 0.8f )
                    // defines a deformation multiplier between 0 and 1 based on how distant the vertice is, where 1 is no deformation and 0 is maximum deformation
                    float dist = Mathf.Clamp01(dirOriginToVert.sqrMagnitude / sqrDemRange);
                    // defines how much (in meters) the vertice will be moved and therefore, how much the mesh will be deformed
                    float moveDelta = force * (1.0f - dist) * maxMoveDelta; 
                    // gets the direction and magnitude of the deformation
                    Vector3 moveDir = Vector3.Slerp(dirOriginToVert, flatVertToCenterDir, impactDirManipulator).normalized * moveDelta;
                    // moves the vertide according to moveDir
                    verts[i] += Quaternion.Inverse(transform.rotation) * moveDir;
                }
            }
            meshfilters[j].mesh.vertices = verts;
            meshfilters[j].mesh.RecalculateBounds();
        }
    }
    */
}
