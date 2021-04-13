using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour{

    public GameObject player;

    private Transform obj;
    private float yOffset;

    // Start is called before the first frame update
    void Start()
    {
        obj = this.GetComponent<Transform>();
        yOffset = player.transform.position.y - obj.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = new Vector3(player.transform.position.x, player.transform.position.y - yOffset, player.transform.position.z);
        obj.position = temp;
    }
}
