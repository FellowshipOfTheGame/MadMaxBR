using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Car", menuName = "Car")]
public class Car : ScriptableObject {
    [SerializeField] private string carName;
    [SerializeField] private GameObject carPrefabPlayer;
    [SerializeField] private Material[] carMaterialsPlayer;
    [SerializeField] private GameObject carPrefabBot;
    [SerializeField] private Material[] carMaterialsBot;

    public string CarName { get { return carName; } }
    /// <summary>
    /// Return the prefab object of the car based on Player or AI control, 
    /// determined by the given true or false value.
    /// </summary>
    /// <param name="playerControl">If the car is controlled by player o AI.</param>
    /// <returns></returns>
    public GameObject GetCarPrefabPlayer(bool playerControl) {
        if (playerControl) {
            return carPrefabPlayer;
        } else {
            return carPrefabBot;
        }
    }
    /// <summary>
    /// Return the list of materials appliable to the object of the car 
    /// based on Player or AI control, determined by the given true or false value.
    /// </summary>
    /// <param name="playerControl">If the car is controlled by player o AI.</param>
    /// <returns></returns>
    public Material[] GetCarMaterialsPlayer(bool playerControl) {
        if (playerControl) {
            return carMaterialsPlayer;
        } else {
            return carMaterialsBot;
        }
    }
}
