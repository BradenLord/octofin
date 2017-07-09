using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anima2D.Runtime
{
    public class RuntimeSpriteMesh : ScriptableObject
    {
        public const int api_version = 3;

        [SerializeField]
        [HideInInspector]
        int m_ApiVersion;

        [SerializeField]
        Sprite m_Sprite;

        [SerializeField]
        Mesh m_SharedMesh;

        [SerializeField]
        Material[] m_SharedMaterials;

        public int apiVersion { get { return m_ApiVersion; } }
        public Sprite sprite { get { return m_Sprite; } }
        public Mesh sharedMesh { get { return m_SharedMesh; } }
        public Material[] sharedMaterials { get { return m_SharedMaterials; } }
        public SpriteMeshData spriteMeshData { get; set; }

        public void SetApiVersion(int apiVersion)
        {
            this.m_ApiVersion = apiVersion;
        }

        public void SetSprite(Sprite sprite)
        {
            this.m_Sprite = sprite;
        }

        public void SetSharedMesh(Mesh sharedMesh)
        {
            this.m_SharedMesh = sharedMesh;
        }

        public void SetSharedMaterials(Material[] materials)
        {
            this.m_SharedMaterials = materials;
        }
    }
}

