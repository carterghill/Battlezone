using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject {
    
    // Main Gun Variables
    new public string name;
    public float firerate;
    public float aimSpeed;
    public GameObject prefab;
    public float walkModifier;
    public float aimWalkModifier;

    // Recoil Variables
    public float bloom;
    public float kickback;
    public float visualRecoil;
    public float visualRecoilADS;
    public float baseHorizontalRecoil;
    public float baseVerticalRecoil;
    public float randomness;

}
