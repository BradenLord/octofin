using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTrail : MonoBehaviour {

    public bool trailEnabled = false;

    public Sprite trailSprite;
    public float segmentLife = 1;
    public float spawnInterval = 0.01f;

    public float trailScale = 1.1f;
    public Color startColor = new Color(1, 1, 1, 1);
    public Color endColor = new Color(1, 1, 1, 0);

    public GameObject trailPrefab;

    public bool skeletal = true; // Position of effect should be based on skeleton

    private SkinnedMeshRenderer meshRenderer;
    private Transform sourceBone;
    private Vector3 bonePosition;
    private Quaternion boneRotation;
    private float spawnTimer = 0;

	void Start () {

        if (skeletal)
        {
            meshRenderer = GetComponent<SkinnedMeshRenderer>();
            sourceBone = meshRenderer.bones[0];

            Matrix4x4 boneMatrix = meshRenderer.sharedMesh.bindposes[0];
            bonePosition = boneMatrix.GetPosition(); 
            boneRotation = boneMatrix.GetRotation();

            trailSprite = GetComponent<SpriteMeshInstance>().spriteMesh.sprite;
        }
	}
	
	void Update () {
		
        if(trailEnabled)
        {
            spawnTimer += Time.deltaTime;

            if(spawnTimer >= spawnInterval)
            {
                GameObject trail = Instantiate(trailPrefab);

                trail.GetComponent<SpriteTrailObject>().Initialize(trailSprite, startColor, endColor, segmentLife);
                trail.GetComponent<SpriteRenderer>().material.shader = Shader.Find("GUI/Text Shader");

                Vector3 scale = transform.lossyScale * trailScale;
                float flip = Mathf.Sign(transform.lossyScale.x);
                scale.y *= flip;

                //meshRenderer.sharedMesh.bounds.center -

                trail.transform.localScale = scale;

                if (skeletal)
                {
                    Vector3 pivot = meshRenderer.sharedMesh.bounds.center;
                    Vector3 localModifier = Vector3.zero;

                    if (flip == 1)
                    {
                        localModifier += bonePosition - pivot;
                    }
                    else if(flip == -1)
                    {
                        localModifier += pivot - bonePosition;
                    }

                    trail.transform.position = sourceBone.position + localModifier;
                    trail.transform.rotation = sourceBone.rotation * boneRotation;
                }
                else
                {
                    trail.transform.position = transform.position;
                    trail.transform.rotation = transform.rotation;
                }

                spawnTimer = 0;
            }
        }
	}
}
