using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

    [Serializable]
    public struct SpritePair
    {
        public string boneName;
        public GameObject spriteHolder;
    }

    public SkeletonType skeletonType;
    public SpritePair[] sprites;

    private readonly Dictionary<String, GameObject> spriteObjects = new Dictionary<string, GameObject>();

    void Awake ()
    {	
        foreach(SpritePair pair in sprites)
        {
            if(spriteObjects.ContainsKey(pair.boneName))
            {
                throw new ArgumentException("There were duplicate keys in the sprite array. (Game object " + gameObject.name + ")");
            }
            spriteObjects.Add(pair.boneName, pair.spriteHolder);
        }

        List<string> skeletonNames;

        switch(skeletonType)
        {
            case SkeletonType.Humanoid:
                skeletonNames = HumanoidSkeleton.getBoneNames();
                break;

            default:
                skeletonNames = null;
                break;
        }

        List<string> keyNames = new List<string>(spriteObjects.Keys);

        //Debug.Log(skeletonNames);
        //Debug.Log(keyNames);

        if(!keyNames.Equals(skeletonNames))
        {
            //throw new ArgumentException("Sprite bone names do not match provided skeleton type.");
        }
	}

    public bool EnableEquipment(string name)
    {
        if(spriteObjects.ContainsKey(name))
        {
            spriteObjects[name].transform.Find("Equipment").gameObject.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool DisableEquipment(string name)
    {
        if (spriteObjects.ContainsKey(name))
        {
            spriteObjects[name].transform.Find("Equipment").gameObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EnableDefault(string name)
    {
        if (spriteObjects.ContainsKey(name))
        {
            spriteObjects[name].transform.Find("Default").gameObject.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool DisableDefault(string name)
    {
        if (spriteObjects.ContainsKey(name))
        {
            spriteObjects[name].transform.Find("Default").gameObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }
}

public enum SkeletonType
{
    Humanoid,
}

public static class HumanoidSkeleton
{
    public static readonly string Secondary = "Secondary";

    public static List<string> getBoneNames()
    {
        return new List<string> { Secondary };
    }
}


