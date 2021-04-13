using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public Transform player;
    public Transform cam;
    public Transform weapon;

    public float xSensitivity;
    public float ySensitivity;
    public float maxAngle;

    private Quaternion camCenter;
    private bool cursorLocked;

    // Start is called before the first frame update
    void Start()
    {
        camCenter = cam.localRotation;   
        cursorLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorLocked) {
            SetY();
            SetX();
        }
        UpdateCursorLock();
    }

    void SetY()
    {
        float input = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;

        if (Input.GetMouseButton(1)) input *= 0.5f;

        Quaternion adj = Quaternion.AngleAxis(input, -Vector3.right);
        Quaternion delta = cam.localRotation * adj;

        if (Quaternion.Angle(camCenter, delta) < maxAngle) 
        {
            cam.localRotation = delta;
            weapon.localRotation = delta;
        }
    }

    void SetX() 
    {
        float input = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;

        if (Input.GetMouseButton(1)) input *= 0.5f;

        Quaternion adj = Quaternion.AngleAxis(input, Vector3.up);
        Quaternion delta = player.localRotation * adj;
        player.localRotation = delta;
    }

    void UpdateCursorLock()
    {
        if (cursorLocked) 
        {
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false;

            if (Input.GetKeyDown(KeyCode.Escape)) {
                cursorLocked = false;
            }
        } else {
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;

            if (Input.GetKeyDown(KeyCode.Escape)) {
                cursorLocked = true;
            }
        }
    }
}
