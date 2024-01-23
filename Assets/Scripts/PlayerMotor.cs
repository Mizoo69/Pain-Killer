using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private int currentWaypointIndex = 0;

    public GameObject jumpParticleSystem;
    public int numberOfJumps = 1;
    public bool readyToJump;
    public float jumpDelay = 3f;

    public float speed = 5f;
    public float gravity = -10f;
    public float jumpHeight = 3f;

    public Transform[] waypoints;

    // Start is called before the first frame update
    void Start()
    {
        readyToJump = true;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Length > 0)
        {
            MoveTowardsWaypoint();
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.y = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;
        //playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (readyToJump && numberOfJumps > 0)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
            jumpParticleSystem.GetComponent<ParticleSystem>().Play();
            numberOfJumps--;
            if (numberOfJumps == 0)
            {
                readyToJump = false;
                Invoke("ResetJump", jumpDelay);
            }
        }
        
    }

    private void ResetJump()
    {
        readyToJump = true;
        numberOfJumps = 1;
    }

    void MoveTowardsWaypoint()
    {
        // Get the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Calculate direction to the waypoint
        Vector3 direction = targetWaypoint.position - transform.position;

        // Move towards the waypoint
        controller.Move(direction.normalized * speed * Time.deltaTime);

        // Check if close enough to the waypoint, then switch to the next one
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 10f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
