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
    private float ySway = 10f;
    private float xSway = 10f;
    private float xSwaySpeed = 4f;
    private float curXSwaySpeed = 0f;
    Quaternion xAdj;
    Quaternion xDelta;

    // Start is called before the first frame update
    void Start()
    {
        camCenter = cam.localRotation;   
        cursorLocked = true;
        NewSwayOffset();
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorLocked) {
            SetY();
            SetX();
        }
        WeaponSway();
        UpdateCursorLock();
    }

    void NewSwayOffset() {

        ySway = -ySway;
        xSway = -xSway;

        // Left and right sway
        xAdj = Quaternion.AngleAxis(xSway, Vector3.up);
        xDelta = player.localRotation * xAdj;
        curXSwaySpeed = 0f;

    }

    void WeaponSway () {

        player.localRotation = Quaternion.Lerp(player.localRotation, xDelta, Time.deltaTime * curXSwaySpeed);
        curXSwaySpeed = Mathf.Lerp(curXSwaySpeed, xSwaySpeed, Time.deltaTime * 4f);

        Debug.Log(Quaternion.Angle(player.localRotation, xDelta));

        if (Quaternion.Angle(player.localRotation, xDelta) < 0.5f) {
            NewSwayOffset();
        }

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
        //player.localRotation = Quaternion.Lerp(player.localRotation, delta, Time.deltaTime * 4f);
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
