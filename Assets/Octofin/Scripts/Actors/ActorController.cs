﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class ActorController : MonoBehaviour {

    public float maxSpeed = 4f;

    public float moveForce = 100f;
    public float averageForce = 70f;    // Factor by which the axis force is compared to determine animation speed
    public float jumpForce = 200f;
    public float airControlRatio = 0.5f;     // Factor by which horizontal movement is damped (or enhanced) when not grouded

    public float movementRatio;  // Don't remember what this does

    public bool canMove = true;

    public bool grounded;
    public LayerMask groundLayer;

    public bool running;

    public bool crouching;
    public bool blocking;

    public bool hovering = false;  // Allows omni-directional control in the air
    public float hoverDrag = 2;

    //Meta Booleans
    public bool inCombat = false;
    public bool hurt = false;
    public bool alive = true;

    public float weaponSpeed = 1; 

    private Rigidbody2D rigidBody;
    private CircleCollider2D feetCollider;
    private Animator[] animators;

    private bool attacking = false;

	void Awake () {

        rigidBody = GetComponent<Rigidbody2D>();
        feetCollider = GetComponent<CircleCollider2D>();
        animators = GetComponentsInChildren<Animator>();

        UpdateAnimatorVariable("CanMove", canMove);
        UpdateAnimatorVariable("Alive", alive);
    }

    void FixedUpdate() {

        grounded = !hovering && feetCollider.IsTouchingLayers(groundLayer);
        UpdateAnimatorVariable("Grounded", grounded);

        if (Mathf.Abs(rigidBody.velocity.x) > maxSpeed)
        {
            rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);
        }

        if (hovering)
        { 
            if (Mathf.Abs(rigidBody.velocity.y) > maxSpeed)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Sign(rigidBody.velocity.y) * maxSpeed);
            }
        }
    }

    public void Move(Vector2 movementAxes, float focalX)
    {
        if(!canMove)
        {
            return;
        }

        float xForce = movementAxes.x * moveForce;
        float yForce = movementAxes.y * moveForce;

        if (grounded)
        {
            this.running = !hovering && (Mathf.Abs(xForce) > 0);
            UpdateAnimatorVariable("Running", running);
        }
        else
        {
            xForce *= airControlRatio;
            yForce *= airControlRatio;
        }

        if (grounded || hovering)
        {
            Vector3 scale = transform.localScale;
            float relativeFocal = focalX - rigidBody.position.x;

            if (relativeFocal > 0)
            {
                scale.x = Mathf.Abs(scale.x);
            }
            else if (relativeFocal < 0)
            {
                scale.x = -Mathf.Abs(scale.x);
            }

            transform.localScale = scale;

            //If force is zero the animator will crash
            if(Mathf.Abs(xForce) > 0.01) 
            {
                this.movementRatio = moveForce / averageForce * Mathf.Sign(scale.x) * Mathf.Sign(xForce);
                UpdateAnimatorVariable("MovementRatio", movementRatio);
            }
        }

        if (!crouching)
        {
            if (movementAxes.x * rigidBody.velocity.x < maxSpeed)
            {
                rigidBody.AddForce(Vector2.right * xForce);
            }

            if (hovering)
            {
                rigidBody.gravityScale = 0;
                rigidBody.drag = hoverDrag;

                if (movementAxes.y * rigidBody.velocity.y < maxSpeed)
                {
                    rigidBody.AddForce(Vector2.up * yForce);
                }
            }
            else
            {
                rigidBody.gravityScale = 1;
                rigidBody.drag = 0;
            }
        }
    }

    public void StartAttack()
    {
        if (!attacking && !hurt && alive)
        {
            attacking = true;
            TriggerAnimatorVariable("StartAttack");
        }
    }

    public void EndAttack()
    {
        EndAttack(false);
    }

    // True if the animation ended naturally, false if it is an interrupt
    public void EndAttack(bool animationEnded)
    {
        if(!animationEnded)
        {
            TriggerAnimatorVariable("EndAttack");
        }

        attacking = false;
    } 

    public void Paralyze()
    {
        if (canMove)
        {
            SetCanMove(false);
        }
    }

    public void Unparalyze()
    {
        SetCanMove(true);
    }

    public void Jump()
    {
        if(canMove && grounded && !hovering && !crouching)
        {
            Launch(Vector2.up * jumpForce);
        }
    }

    public void Launch(Vector2 force)
    {
        rigidBody.AddForce(force);
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
        UpdateAnimatorVariable("CanMove", canMove);
    }

    public void SetCrouching(bool crouching)
    {
        if (crouching && grounded && canMove)
        {
            this.crouching = true;
            UpdateAnimatorVariable("Crouching", crouching);
        }

        if (!crouching)
        {
            this.crouching = false;
            UpdateAnimatorVariable("Crouching", crouching);
        }
    }

    public void SetBlocking(bool blocking)
    {
        if(blocking && canMove)
        {
            this.blocking = true;
            UpdateAnimatorVariable("Blocking", blocking);
        }

        if(!blocking)
        {
            this.blocking = false;
            UpdateAnimatorVariable("Blocking", blocking);
        }
    }

    public void UpdateAnimatorVariable(string name, bool value)
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool(name, value);
        }
    }

    public void UpdateAnimatorVariable(string name, float value)
    {
        foreach (Animator animator in animators)
        {
            animator.SetFloat(name, value);
        }
    }

    public void TriggerAnimatorVariable(string name)
    {
        foreach (Animator animator in animators)
        {
            animator.ResetTrigger(name);
            animator.SetTrigger(name);
        }
    }
}
