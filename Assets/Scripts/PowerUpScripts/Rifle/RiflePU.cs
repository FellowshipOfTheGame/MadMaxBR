using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePU : PowerUpBase {
    private GameObject RifleHUD; // MachineGun HUD
    [SerializeField] private GameObject WeaponManager;

    private GameObject targetCar; // the car this script is attached

    public PowerUpData PowerUpInfo;

    public int BulletAmount { get { return WeaponManager.GetComponent<Weapon>().GetMunicao(); } }
    private void Awake() {
        if (this.transform.parent.gameObject.transform.parent.gameObject.CompareTag("Player")) {
            RifleHUD = PlayerDataDisplayer.Instance.RifleCountText;
        } else {
            RifleHUD = null;
        }
    }
    public override void Activate() {
        targetCar = this.transform.parent.gameObject.transform.parent.gameObject; // get the car this script is attached
        if (RifleHUD != null) {
            RifleHUD.SetActive(true);
        }
        WeaponManager.GetComponent<Weapon>().PegarPoweUpArma(1);
        WeaponManager.GetComponent<WeaponMovement>().StartMovementUp();
    }

    public override void Deactivate() {
        this.gameObject.SetActive(false);
        if (RifleHUD != null) {
            RifleHUD.SetActive(false);
        }
        WeaponManager.GetComponent<Weapon>().PegarPoweUpArma(1);
        WeaponManager.GetComponent<Weapon>().CanShoot = false;
        WeaponManager.GetComponent<WeaponMovement>().StartMovementDown();
        targetCar.GetComponent<VehicleData>().EmptyPowerUpSlot(PowerUpName.Rifle);
    }

    public override void UsePowerUp(bool useActive) {
        WeaponManager.GetComponent<Weapon>().Shooting = useActive;
    }

    public override void Update() {
        if (WeaponManager.GetComponent<WeaponMovement>().IsAtMaxHeight) {
            WeaponManager.GetComponent<Weapon>().CanShoot = true;
        }
        if (WeaponManager.GetComponent<Weapon>().GetMunicao() == 0) {
            Deactivate();
        }
    }
}
