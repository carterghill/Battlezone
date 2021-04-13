using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour { 

    #region Variables

    public Gun[] loadout;
    public Transform weaponParent;
    public Camera cam;
    public Movement movement;
    public GameObject player;
    public GameObject bulletHolePrefab;
    public LayerMask canBeShot;

    private GameObject currentWeapon;
    private int weaponIndex = 0;

    private float baseFOV;
    private float aimFOVModifier = 0.01f; 
    private float shootTime = 0.0f;

    private Rigidbody camBody;
    private Rigidbody playerBody;
    private bool shot;

    #endregion

    #region Callbacks

    void Start() {
        baseFOV = cam.fieldOfView;
        camBody = cam.GetComponent<Rigidbody>();
        playerBody = player.GetComponent<Rigidbody>();
        shot = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Equip(1);
        if (currentWeapon != null) {
            Aim(Input.GetMouseButton(1));
            //Shoot2(Input.GetMouseButton(0), Input.GetMouseButton(1));

            // Weapon position elasticity
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            currentWeapon.transform.localRotation = Quaternion.Lerp(currentWeapon.transform.localRotation, Quaternion.identity, Time.deltaTime * 4f);
        }
    }

    void FixedUpdate() {
        if (currentWeapon != null) {
            FixedAim(Input.GetMouseButton(1));
            Shoot(Input.GetMouseButton(0), Input.GetMouseButton(1));
        }
    }

    #endregion

    void Equip (int i) {

        // Delete currently equipped gun
        if (currentWeapon != null) Destroy(currentWeapon);

        // Attach the weapon to the weapon parent object
        GameObject newEquipment = Instantiate(loadout[i].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        newEquipment.transform.localPosition = Vector3.zero;
        newEquipment.transform.localEulerAngles = Vector3.zero;

        // Make the new gun the currently equipped gun
        currentWeapon = newEquipment;
        weaponIndex = i;

    }

    void Aim(bool aiming) {

        Transform anchor = currentWeapon.transform.Find("Anchor");
        Transform ads = currentWeapon.transform.Find("States/ADS");
        Transform hip = currentWeapon.transform.Find("States/Hip");

        if (aiming) {
            anchor.position = Vector3.Lerp(anchor.position, ads.position, Time.deltaTime * loadout[weaponIndex].aimSpeed);
        } else {
            anchor.position = Vector3.Lerp(anchor.position, hip.position, Time.deltaTime * loadout[weaponIndex].aimSpeed);
        }

    }

    void FixedAim(bool aiming) {
        if (aiming) {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV * aimFOVModifier, Time.fixedDeltaTime*8f);
            movement.SetSpeedModifier(loadout[weaponIndex].aimWalkModifier);
            movement.SetAiming(true);
        } else {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV, Time.fixedDeltaTime*8f);
            movement.SetSpeedModifier(loadout[weaponIndex].walkModifier);
            movement.SetAiming(false);
        }
    }


    void Shoot2(bool shooting, bool aiming) {
        if (shot) {
            // Gun kickback effect
            Gun gun = loadout[weaponIndex];
            
            shot = false;
        }
    }

    void Shoot(bool shooting, bool aiming) {
        if (shooting && shootTime < 0.001f) {

            shootTime = loadout[weaponIndex].firerate;
            float rand = loadout[weaponIndex].randomness;
            Gun gun = loadout[weaponIndex];
            Transform spawn = transform.Find("Camera");

            // Assign bloom if not aiming
            Vector3 bloom;
            if (aiming) {
                bloom = spawn.forward;
            } else {
                bloom = spawn.position + spawn.forward * 1000f;
                bloom += Random.Range(-gun.bloom, gun.bloom) * spawn.up;
                bloom += Random.Range(-gun.bloom, gun.bloom) * spawn.right;
                bloom -= spawn.position;
                bloom.Normalize();
            }

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(spawn.position, bloom, out hit, 1000f, canBeShot)) {

                GameObject newHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
                newHole.transform.LookAt(hit.point + hit.normal);
                Destroy(newHole, 10f);

            }
            
            // Add gun recoil if ADS
            if (aiming) {
                playerBody.AddTorque(transform.up * 
                    (Random.Range(loadout[weaponIndex].baseHorizontalRecoil - rand, loadout[weaponIndex].baseHorizontalRecoil + rand)) * Time.fixedDeltaTime);
                
                camBody.AddTorque(-transform.right * 
                    (Random.Range(loadout[weaponIndex].baseVerticalRecoil - rand, loadout[weaponIndex].baseVerticalRecoil + rand)) 
                    * Time.fixedDeltaTime); // Goes up
            }
            
            // Gun kickback effect
            float recoil;
            if (aiming) {
                recoil = gun.visualRecoilADS;
            } else {
                recoil = gun.visualRecoil;
            }
            currentWeapon.transform.Rotate(-recoil, 0, 0);
            currentWeapon.transform.position -= currentWeapon.transform.forward * gun.kickback;

            // Correctly camera angle from gun recoil
            cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, player.transform.eulerAngles.y, 0);

        }
        shootTime = shootTime - Time.fixedDeltaTime;
        //player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.y, player.transform.eulerAngles.y, 0);
    }

}
