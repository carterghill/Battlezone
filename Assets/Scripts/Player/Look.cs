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
    private float ySway = 1f;
    private float ySwayMax = 2f;
    private float ySwaySpeed = 1f;
    private float curYSwaySpeed = 0f;
    private float xSway = 1f;
    private float xSwayMax = 1f;
    private float xSwaySpeed = 1f;
    private float curXSwaySpeed = 0f;
    Quaternion xAdj;
    Quaternion xDelta;
    Quaternion yAdj;
    Quaternion yDelta;

    // Start is called before the first frame update
    void Start()
    {
        camCenter = cam.localRotation;   
        cursorLocked = true;
        NewXSwayOffset();
        NewYSwayOffset();
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

    void NewXSwayOffset() {

        xSway = Random.Range(-xSwayMax, xSwayMax);

        xAdj = Quaternion.AngleAxis(xSway, Vector3.up);
        xDelta = player.localRotation * xAdj;
        curXSwaySpeed = 0f;

    }

    void NewYSwayOffset() {

        ySway = Random.Range(-ySwayMax, ySwayMax);

        yAdj = Quaternion.AngleAxis(ySway, -Vector3.right);
        yDelta = cam.localRotation * yAdj;
        curYSwaySpeed = 0f;

    }

    void WeaponSway () {

        // Sway horizontal
        player.localRotation = Quaternion.Lerp(player.localRotation, xDelta, Time.deltaTime * curXSwaySpeed);
        curXSwaySpeed = Mathf.Lerp(curXSwaySpeed, xSwaySpeed, Time.deltaTime * 1f);

        if (Quaternion.Angle(player.localRotation, xDelta) < 0.5f) {
            NewXSwayOffset();
        }

        // Sway vertical
        //if (Quaternion.Angle(camCenter, yDelta) < maxAngle) {
            
            cam.localRotation = Quaternion.Lerp(cam.localRotation, yDelta, Time.deltaTime * curYSwaySpeed);
            weapon.localRotation = Quaternion.Lerp(weapon.localRotation, yDelta, Time.deltaTime * curYSwaySpeed);;
            curYSwaySpeed = Mathf.Lerp(curYSwaySpeed, ySwaySpeed, Time.deltaTime * 1f);
            
            if (Quaternion.Angle(cam.localRotation, yDelta) < 1.0f) {
                NewYSwayOffset();
            }
        //}

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
