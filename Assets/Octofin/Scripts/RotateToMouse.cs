using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour {
	
	// Update is called once per frame
	private void FixedUpdate () {

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 offset = new Vector2(worldMousePosition.x - transform.position.x, worldMousePosition.y - transform.position.y);
        float angle = Mathf.Atan2(offset.x, offset.y) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
