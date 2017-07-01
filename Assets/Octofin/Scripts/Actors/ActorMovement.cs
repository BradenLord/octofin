using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class ActorMovement : MonoBehaviour {

    public float moveSpeed = 4f;
    public float defaultSpeed = 2f;

    public float jumpForce = 200f;
    public float airControlRatio = 0.5f;     // Factor by which horizontal movement is damped (or enhanced) when not grouded

    public bool grounded;
    public LayerMask groundLayer;

    public bool hovering = false;  // Allows 8-directional control in the air

    public bool canMove = true;  // If the actor is in the middle of another non-interruptable sequence/animation or is in hitstun

    private float launchVelocity = 0; // Consenserves horizontal momentum when in the air

    private Rigidbody2D rigidBody;
    private CircleCollider2D feetCollider;
    private Animator animator;

	void Awake () {
        rigidBody = GetComponent<Rigidbody2D>();
        feetCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }
	
	void FixedUpdate () {

        grounded = !hovering && feetCollider.IsTouchingLayers(groundLayer);

        if(grounded || hovering)
        {
            launchVelocity = 0;
        }

        animator.SetBool("Grounded", grounded);
        animator.SetBool("Hovering", hovering);
        animator.SetFloat("VelocityX", rigidBody.velocity.x);
        animator.SetFloat("VelocityY", rigidBody.velocity.y);
    }

    public void Move(Vector2 movementAxes, float focalX)
    {
        if(!canMove)
        {
            return;
        }

        float xVelocity = movementAxes.x * moveSpeed;
        float yVelocity = movementAxes.y * moveSpeed;

        if (grounded)
        {
            bool walking = !hovering && (Math.Abs(xVelocity) > 0);
            animator.SetBool("Walking", walking);
        }
        else
        {
            xVelocity *= airControlRatio;
            xVelocity += launchVelocity;
            yVelocity *= airControlRatio;
        }

        if (grounded || hovering)
        {
            Vector3 scale = transform.localScale;
            float relativeFocal = focalX - rigidBody.position.x;

            if (relativeFocal > 0)
            {
                scale.x = Math.Abs(scale.x);
            }
            else if (relativeFocal < 0)
            {
                scale.x = -Math.Abs(scale.x);
            }

            transform.localScale = scale;

            //If velocity is zero the animator will crash
            if(Math.Abs(xVelocity) > 0.01) 
            {
                float movementRatio = moveSpeed / defaultSpeed * (scale.x / Math.Abs(scale.x)) * (xVelocity / Math.Abs(xVelocity));
                animator.SetFloat("MovementRatio", movementRatio);
            }
        }

        if(hovering)
        {
            rigidBody.velocity = new Vector2(xVelocity, yVelocity);
            rigidBody.gravityScale = 0;
        }
        else
        {
            rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
            rigidBody.gravityScale = 1;
        }
    }

    public void Jump()
    {
        if(grounded && !hovering && canMove)
        {
            Launch(new Vector2(0, jumpForce));
            launchVelocity = rigidBody.velocity.x;
        }
    }

    public void Launch(Vector2 force)
    {
        rigidBody.AddForce(force);
    }
}
