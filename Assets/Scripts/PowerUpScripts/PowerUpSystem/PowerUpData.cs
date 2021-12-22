using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New PowerUpData", menuName = "PowerUp")]
public class PowerUpData : ScriptableObject {
    public string Name;
    public string TutorialText;
    public Sprite Icon; // sprite that represents powerup
    public PowerUpName PowerUpName;
    //public PowerUpType PowerUpType;
    //public UseType UseType;
    //public DurationType DurationType;

    public int PowerUpId { get { return (int)PowerUpName; } }
}
