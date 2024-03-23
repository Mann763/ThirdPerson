using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    Animator anim;

    float InputX = 0.0f;
    float InputZ = 0.0f;

    int VelocityX = Animator.StringToHash("Velocity X");
    int VelocityZ = Animator.StringToHash("Velocity Z");
    int Jump = Animator.StringToHash("Jump");
    private float acceleration = 5.0f; // Adjust acceleration for smoother movement
    private float deceleration = 5.0f; // Adjust deceleration for smoother stopping
    private float maxSpeed = 5.0f; // Adjust max speed as needed
    private float normalClamp = 0.5f;
    private float shiftClamp = 1.0f; // Adjust this for Shift key movement

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Adjust clamped range based on Shift key input
        float currentClamp = Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift) ? shiftClamp : normalClamp;

        // Clamp input values
        InputX = Mathf.Clamp(horizontalInput, -currentClamp, currentClamp);
        InputZ = Mathf.Clamp(verticalInput, -currentClamp, currentClamp);

        // Apply acceleration and deceleration
        InputX = Mathf.Lerp(anim.GetFloat(VelocityX), InputX, Time.deltaTime * acceleration);
        InputZ = Mathf.Lerp(anim.GetFloat(VelocityZ), InputZ, Time.deltaTime * acceleration);

        // Apply max speed
        InputX = Mathf.Clamp(InputX, -maxSpeed, maxSpeed);
        InputZ = Mathf.Clamp(InputZ, -maxSpeed, maxSpeed);

        // Smoothly transition to zero when there is no input
        if (Mathf.Approximately(InputX, 0f))
        {
            InputX = Mathf.Lerp(anim.GetFloat(VelocityX), InputX, Time.deltaTime * deceleration);
        }

        if (Mathf.Approximately(InputZ, 0f))
        {
            InputZ = Mathf.Lerp(anim.GetFloat(VelocityZ), InputZ, Time.deltaTime * deceleration);
        }

        anim.SetFloat(VelocityX, InputX);
        anim.SetFloat(VelocityZ, InputZ);
    }
}
