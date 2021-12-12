using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreasePoolCollision : MonoBehaviour {
    /// <summary>
    /// Get Race Manager to access the cars colliders.
    /// </summary>
    public GameObject RaceManager;

    private ParticleSystem ps;
    private RaceManager raceManager;

    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

    void OnEnable() {
        ps = GetComponent<ParticleSystem>();
        raceManager = RaceManager.GetComponent<RaceManager>();

        for (int i = 0; i < raceManager.Racers.Count; i++) {
            ps.trigger.AddCollider(raceManager.Racers[i].GetComponent<BoxCollider>());
        }
    }

    void OnParticleTrigger() {
        // get the particles which matched the trigger conditions this frame
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter, out var colliderDataEnter);

        // iterate through the particles which entered the trigger and make them red
        for (int i = 0; i < numEnter; i++) {
            for (int j = 0; j < colliderDataEnter.GetColliderCount(i); j++) {
                var car = colliderDataEnter.GetCollider(i, j);
                if (car.CompareTag("Player") || car.CompareTag("AI")) {
                    car.GetComponent<CarController>().SetIsGreased(true);
                }
            }
        }
    }
}
