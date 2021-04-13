using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float sprintModifier;
    public float fallSpeed;
    public float jumpForce;
    public Camera normalCam;
    public Transform groundDetector;

    private Rigidbody rig;

    private float baseFOV;
    private float sprintFOVModifier = 1.25f;
    private float speedModifier = 1f;
    private bool aiming;

    // Start is called before the first frame update
    private void Start()
    {   
        baseFOV = normalCam.fieldOfView;
        Camera.main.gameObject.SetActive(false);
        rig = GetComponent<Rigidbody>();
        aiming = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check Grounded
        bool grounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.2f);

        // Controls
        float hMove = Input.GetAxisRaw("Horizontal");
        float vMove = Input.GetAxisRaw("Vertical");

        bool jump = Input.GetKey(KeyCode.Space) && grounded;
        bool sprint = Input.GetKey(KeyCode.LeftShift) && vMove > 0;

        Vector3 direction = new Vector3(hMove, 0, vMove);
        direction.Normalize();

        // Adjust speed based on sprinting modifier
        float newSpeed;
        if (aiming) {
            newSpeed = speed * speedModifier;
            sprint = false;
            //Debug.Log("grounded");
        } else {
            newSpeed = speed;
        }

        if (sprint) {
             newSpeed *= sprintModifier;
             normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.fixedDeltaTime*8f);
        } else {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.fixedDeltaTime*8f);
        }

        // Give player velocity based on input
        Vector3 moveVelocity = transform.TransformDirection(direction) * newSpeed * Time.fixedDeltaTime;
        moveVelocity.y = rig.velocity.y;
        rig.velocity = moveVelocity;

        // Apply gravity
        //rig.AddForce(0, -fallSpeed, 0, ForceMode.Acceleration);

        // Apply jump force
        if (jump) {
            rig.AddForce(Vector3.up * jumpForce);
            //Debug.Log("Jump!");
        } 
    }

    public void SetSpeedModifier(float s) {
        this.speedModifier = s;
    }

    public void SetAiming(bool a) {
        this.aiming = a;
    }

}
