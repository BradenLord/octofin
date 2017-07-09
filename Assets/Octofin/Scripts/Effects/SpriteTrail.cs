using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteTrail : MonoBehaviour {

    public bool trailEnabled = false;
    public float segmentLife = 1;
    public float spawnInterval = 0.01f;

    public float trailScale = 1.1f;
    public Color startColor = new Color(1, 1, 1, 1);
    public Color endColor = new Color(1, 1, 1, 0);

    public GameObject trailPrefab;

    private Sprite trailSprite;
    private float spawnTimer = 0;

	void Start () {
        trailSprite = GetComponent<SpriteRenderer>().sprite;
	}
	
	void Update () {
		
        if(trailEnabled)
        {
            spawnTimer += Time.deltaTime;

            if(spawnTimer >= spawnInterval)
            {
                GameObject trail = Instantiate(trailPrefab);

                trail.GetComponent<SpriteTrailObject>().Initialize(trailSprite, startColor, endColor, segmentLife);

                SpriteRenderer trailRenderer = trail.GetComponent<SpriteRenderer>();
                trailRenderer.material.shader = Shader.Find("GUI/Text Shader");
                trailRenderer.sortingLayerName = "Effects";

                //Vector3 scale = transform.lossyScale * trailScale;
                //float flip = Mathf.Sign(transform.lossyScale.x);
                //scale.y *= flip;

                //meshRenderer.sharedMesh.bounds.center -

                
                trail.transform.position = transform.position;
                trail.transform.rotation = transform.rotation;
                trail.transform.localScale = transform.localScale * trailScale;

                spawnTimer = 0;
            }
        }
	}
}
