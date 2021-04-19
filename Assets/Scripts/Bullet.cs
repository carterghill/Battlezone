using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    GameObject bullet;
    
    void Start() {
        bullet = GetComponent<GameObject>();
    }
 
    void Update() {
        
    }

    void OnTriggerEnter( Collider col ) {

        Debug.Log("Hit!");
        Destroy(bullet);

    }

}
