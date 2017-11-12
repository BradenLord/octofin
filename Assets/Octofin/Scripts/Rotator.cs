using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public float min = -90;
    public float max = 90;
    public float multiplier = 1.0f;

    public Quaternion euler;
    public float angle;

    // Update is called once per frame
    public void Rotate(float angle) {

        angle *= multiplier;
        angle = angle > max ? max : angle;
        angle = angle < min ? min : angle;

        this.angle = angle;
        euler = Quaternion.Euler(0, 0, angle);

        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
