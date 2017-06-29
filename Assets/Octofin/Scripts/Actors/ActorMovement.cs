using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMovement : MonoBehaviour {

    public bool grounded;
    public LayerMask groundLayer;

    private CircleCollider2D feetCollider;

	void Awake () {
        feetCollider = GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        grounded = feetCollider.IsTouchingLayers(groundLayer);
	}
}
