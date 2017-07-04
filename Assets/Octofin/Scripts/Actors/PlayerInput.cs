using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorMovement))]
public class PlayerInput : MonoBehaviour {

    public float debugJump;

    private ActorMovement actor;  //Actor in scene to affect with player input

	void Awake ()
    {
        actor = GetComponent<ActorMovement>();
    }

    private void FixedUpdate()
    {
        Vector2 inputAxes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        actor.Move(inputAxes, worldMousePosition.x);

        if(Input.GetAxis("Jump") == 1)
        {
            actor.Jump();
        }

        if(Input.GetAxis("ActivatePrimary") == 1)
        {
            actor.ActivatePrimary();
        }

        if (Input.GetAxis("Crouch") == 1)
        {
            actor.SetCrouching(true);
        }
        else
        {
            actor.SetCrouching(false);
        }
    }
}
