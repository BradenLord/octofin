using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DynamicSpriteLoader : MonoBehaviour {

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start () {

        string testFile = "C:/Users/Braden/Pictures/birbday.jpg";
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(File.ReadAllBytes(testFile));

        spriteRenderer = GetComponent<SpriteRenderer>();
        Rect textureRect = new Rect(0, 0, texture.width, texture.height);

        Sprite sprite = Sprite.Create(texture, textureRect, new Vector2(0.3f, 0.4f), 100f);
        sprite.name = "birbday";

        spriteRenderer.sprite = sprite;
	}
}
