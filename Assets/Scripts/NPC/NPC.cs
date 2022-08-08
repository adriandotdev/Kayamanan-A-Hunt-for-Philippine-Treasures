using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    // Array of waypoints to walk from one to the next one
    [SerializeField]
    private Transform[] waypoints;

    // Walk speed that can be set in Inspector
    [SerializeField]
    private float moveSpeed = 2f;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;

    private Animator animator;
    // Use this for initialization
    private void Start()
    {

        // Set position of Enemy as position of the first waypoint
        //transform.position = waypoints[waypointIndex].transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Move Enemy
        this.Move();
    }

    // Method that actually make Enemy walk
    private void Move()
    {
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops
        if (this.waypointIndex <= this.waypoints.Length - 1)
        {
            // Move Enemy from current waypoint to the next one
            // using MoveTowards method
            Vector3 movement = Vector2.MoveTowards(transform.position,
               this.waypoints[this.waypointIndex].transform.position,
               this.moveSpeed * Time.deltaTime);
            transform.position = movement;

            Vector2 moveDirection = (this.waypoints[this.waypointIndex].transform.position - transform.position).normalized;
            float dotValueH = Vector2.Dot(Vector2.right, moveDirection);
            float dotValueV = Vector2.Dot(Vector2.up, moveDirection);

            animator.SetFloat("Speed", 1);
            animator.SetFloat("Horizontal", dotValueH);
            animator.SetFloat("Vertical", dotValueV);

            //float horizontal = Vector2.Dot(Vector2.right, this.waypoints[this.waypointIndex].transform.position - transform.position);
            //float vertical = Vector2.Dot(Vector2.up, this.waypoints[this.waypointIndex].transform.position - transform.position);

            //float h = 0.5f, v = 0.5f;

            //if (horizontal < 0f)
            //{
            //    h = -0.5f;
            //}
            //else if (horizontal > 0f)
            //{
            //    h = 0.5f;
            //}

            //if (vertical < 0f)
            //{
            //    v = -0.5f;
            //}
            //else if (vertical > 0f)
            //{
            //    v = 0.5f;
            //}



            //if (this.waypoints[this.waypointIndex].transform.position.x >= transform.position.x)
            //{
            //    animator.SetBool("RIGHT", true);
            //}
            //else if (this.waypoints[this.waypointIndex].transform.position.x <= transform.position.x)
            //{
            //    animator.SetBool("LEFT", true);
            //}

            //else if (this.waypoints[this.waypointIndex].transform.position.y >= transform.position.y)
            //{
            //    animator.SetBool("UP", true);
            //}
            //else
            //{
            //    animator.SetBool("DOWN", true);
            //}

            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if (transform.position.x == waypoints[this.waypointIndex].transform.position.x)
            { 
                this.waypointIndex += 1;

                if (this.waypointIndex > this.waypoints.Length - 1)
                {
                    this.waypointIndex = 0;
                }
            }
        }
    }
}
