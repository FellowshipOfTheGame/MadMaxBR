using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluePoolCollision : MonoBehaviour {
    /// <summary>
    /// Get Race Manager to access the cars colliders.
    /// </summary>

    private ParticleSystem ps;

    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

    void OnEnable() {
        ps = GetComponent<ParticleSystem>();

        for (int i = 0; i < RaceManager.Instance.Racers.Count; i++) {
            ps.trigger.AddCollider(RaceManager.Instance.Racers[i].GetComponent<BoxCollider>());
        }
    }

    void OnParticleTrigger() {
        // get the particles which matched the trigger conditions this frame
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter, out var colliderDataEnter);
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside, out var colliderDataInside);

        // iterate through the particles which entered the trigger
        for (int i = 0; i < numEnter; i++) {
            for (int j = 0; j < colliderDataEnter.GetColliderCount(i); j++) {
                var car = colliderDataEnter.GetCollider(i, j);
                if (car.CompareTag("Player") || car.CompareTag("AI")) {
                    car.GetComponent<CarController>().SetIsGlued(true);
                }
            }
        }

        // iterate through the particles which is colliding with the trigger
        for (int i = 0; i < numInside; i++) {
            for (int j = 0; j < colliderDataInside.GetColliderCount(i); j++) {
                var car = colliderDataInside.GetCollider(i, j);
                if (car.CompareTag("Player") || car.CompareTag("AI")) {
                    car.GetComponent<CarController>().SetIsGlued(true);
                }
            }
        }
    }
}
