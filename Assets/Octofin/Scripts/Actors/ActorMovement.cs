using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class ActorMovement : MonoBehaviour {

    public float maxSpeed = 4f;

    public float moveForce = 100f;
    public float averageForce = 70f;    // Factor by which the axis force is compared to determine animation speed
    public float jumpForce = 200f;
    public float airControlRatio = 0.5f;     // Factor by which horizontal movement is damped (or enhanced) when not grouded

    public bool canMove = true;

    public bool grounded;
    public LayerMask groundLayer;

    public bool crouching;

    public bool hovering = false;  // Allows 8-directional control in the air
    public float hoverDrag = 2;

    public float weaponSpeed = 1; // Shouldn't be here eventually

    private Rigidbody2D rigidBody;
    private CircleCollider2D feetCollider;
    private Animator animator;

	void Awake () {

        rigidBody = GetComponent<Rigidbody2D>();
        feetCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();

        animator.SetFloat("WeaponSpeed", weaponSpeed);
    }

    void FixedUpdate() {

        grounded = !hovering && feetCollider.IsTouchingLayers(groundLayer);

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

        float xForce = movementAxes.x * moveForce;
        float yForce = movementAxes.y * moveForce;

        if (grounded)
        {
            bool walking = !hovering && (Mathf.Abs(xForce) > 0);
            animator.SetBool("Walking", walking);
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
                float movementRatio = moveForce / averageForce * Mathf.Sign(scale.x) * Mathf.Sign(xForce);
                animator.SetFloat("MovementRatio", movementRatio);
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

    public void ActivatePrimary()
    {
        if (canMove)
        {
            SetCanMove(false);
            animator.SetTrigger("WeaponSwing");
        }
    }

    public void DeactivatePrimary()
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
        animator.SetBool("CanMove", canMove);
    }

    public void SetCrouching(bool crouching)
    {
        if (crouching && grounded && canMove)
        {
            this.crouching = true;
            animator.SetBool("Crouching", true);
        }

        if (!crouching)
        {
            this.crouching = false;
            animator.SetBool("Crouching", false);
        }
    }
}
