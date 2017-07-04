using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteTrailObject : MonoBehaviour {

    private float displayTime;
    private float timeAlive;

    private Color startColor, endColor;

    private bool initialized = false;

    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Sprite sprite, Color startColor, Color endColor, float displayTime)
    {
        this.displayTime = displayTime;
        spriteRenderer.sprite = sprite;

        this.startColor = startColor;
        this.endColor = endColor;

        timeAlive = 0;
        initialized = true;
    }

    void Update () {
		
        if(initialized)
        {
            timeAlive += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColor, endColor, timeAlive / displayTime);

            if(timeAlive > displayTime)
            {
                Destroy(gameObject);
            }
        }
	}
}