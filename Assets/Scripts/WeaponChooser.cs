using UnityEngine;
using UnityEngine.UI;

public class WeaponChooser : MonoBehaviour
{
    private GameObject[] weaponsPrefabs;
    [SerializeField] private Button[] buttons;

    public GameObject weaponChosen = null;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ChooseWeapon(int index)
    {
        weaponChosen = weaponsPrefabs[index];
    }
}
