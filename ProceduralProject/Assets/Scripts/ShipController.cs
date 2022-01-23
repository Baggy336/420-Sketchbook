using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public float velocity = 25f;
    public float steeringSpeed = 7.5f;
    public float lift = 10f;
    public float rotationSpeed = 80f;

    private float currentVelocity;
    private float currentSteering;
    private float currentLift;
    private float acceleration = 2f;
    private Vector2 mousePos;
    private Vector2 screenCenter;
    private Vector2 disFromCenter;

    private void Start()
    {
        // Decide how large the screen is
        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;
    }
    private void Update()
    {
        MoveShip();
        TurnShip();
    }

    private void TurnShip()
    {
        // Get the mouse x and y position on the screen
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;

        // Calculate the distance the mouse is from the center of the screen.
        disFromCenter.x = (mousePos.x - screenCenter.x) / screenCenter.y;
        disFromCenter.y = (mousePos.y - screenCenter.y) / screenCenter.y;

        // Clamp the vector to be between 0 and 1
        mousePos = Vector2.ClampMagnitude(mousePos, 1f);

        // Apply the distance of the mouse cursor to the rotation of the object
        transform.Rotate(-disFromCenter.y * rotationSpeed * Time.deltaTime, disFromCenter.x * rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    private void MoveShip()
    {
        // Smooth each axis using lerp and deltatime for acceleration
        currentVelocity = Mathf.Lerp(currentVelocity, Input.GetAxisRaw("Vertical") * velocity, acceleration * Time.deltaTime);
        currentSteering = Mathf.Lerp(currentSteering, Input.GetAxisRaw("Horizontal") * steeringSpeed, acceleration * Time.deltaTime);
        currentLift = Mathf.Lerp(currentLift, Input.GetAxisRaw("Up/Down") * lift, acceleration * Time.deltaTime);

        // Apply the new values to the object
        transform.position += transform.forward * currentVelocity * Time.deltaTime;
        transform.position += transform.right * currentSteering * Time.deltaTime;
        transform.position += transform.up * currentLift * Time.deltaTime;
    }
}
