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
    public float visualRecoilX;
    public float visualRecoilXADS;
    public float baseHorizontalRecoil;
    public float baseVerticalRecoil;
    public float randomness;

    // Aiming sway variables
    //public float ySway = 0.75f;
    public float ySwayMax = 1f;
    public float ySwaySpeed = 0.4f;
    //private float curYSwaySpeed = 0f;
    //private float ySwayTime = 0f;
    public float ySwayTimeMax = 3.5f;
    //private float xSway = 0.75f;
    public float xSwayMax = 1f;
    public float xSwaySpeed = 0.5f;
    //private float curXSwaySpeed = 0f;
    //private float xSwayTime = 0f;
    public float xSwayTimeMax = 3.5f;

}
