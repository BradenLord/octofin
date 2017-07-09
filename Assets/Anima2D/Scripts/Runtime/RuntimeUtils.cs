using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Anima2D.Runtime
{
	public static class RuntimeUtils
    {
        private static readonly Dictionary<Sprite, RuntimeSpriteMesh> spriteMeshes = new Dictionary<Sprite, RuntimeSpriteMesh>();

        public class WeightedTriangle
        {
            int m_P1;
            int m_P2;
            int m_P3;
            float m_W1;
            float m_W2;
            float m_W3;
            float m_Weight;

            public int p1 { get { return m_P1; } }
            public int p2 { get { return m_P2; } }
            public int p3 { get { return m_P3; } }
            public float w1 { get { return m_W1; } }
            public float w2 { get { return m_W2; } }
            public float w3 { get { return m_W3; } }
            public float weight { get { return m_Weight; } }

            public WeightedTriangle(int _p1, int _p2, int _p3,
                                    float _w1, float _w2, float _w3)
            {
                m_P1 = _p1;
                m_P2 = _p2;
                m_P3 = _p3;
                m_W1 = _w1;
                m_W2 = _w2;
                m_W3 = _w3;
                m_Weight = (w1 + w2 + w3) / 3f;
            }
        }

        public static RuntimeSpriteMesh CreateSpriteMesh(Sprite sprite, Vector2 pivot)
        {
            if (spriteMeshes.ContainsKey(sprite))
            {
                return spriteMeshes[sprite];
            }
            else
            {
                RuntimeSpriteMesh spriteMesh = ScriptableObject.CreateInstance<RuntimeSpriteMesh>();
                InitFromSprite(spriteMesh, sprite);

                SpriteMeshData spriteMeshData = ScriptableObject.CreateInstance<SpriteMeshData>();
                spriteMeshData.name = spriteMesh.name + "_Data";
                spriteMeshData.hideFlags = HideFlags.HideInHierarchy;
                InitFromSprite(spriteMeshData, sprite, pivot);

                spriteMesh.spriteMeshData = spriteMeshData;

                UpdateAssets(spriteMesh, spriteMeshData);

                return spriteMesh;
            }
        }

        static void InitFromSprite(RuntimeSpriteMesh spriteMesh, Sprite sprite)
        {
            spriteMesh.SetSprite(sprite);
            spriteMesh.SetApiVersion(RuntimeSpriteMesh.api_version);
        }

        static void InitFromSprite(SpriteMeshData spriteMeshData, Sprite sprite, Vector2 pivot)
        {
            Vector2[] vertices;
            IndexedEdge[] edges;
            int[] indices;

            if (sprite)
            {
                GetSpriteData(sprite, out vertices, out edges, out indices);

                spriteMeshData.vertices = vertices;
                spriteMeshData.edges = edges;
                spriteMeshData.indices = indices;
                spriteMeshData.pivotPoint = pivot;
            }
        }

        public static void GetSpriteData(Sprite sprite, out Vector2[] vertices, out IndexedEdge[] edges, out int[] indices)
        {
            int width = 0;
            int height = 0;

            GetSpriteTextureSize(sprite, ref width, ref height);

            Vector2[] uvs = sprite.uv;

            vertices = new Vector2[uvs.Length];

            for (int i = 0; i < uvs.Length; ++i)
            {
                vertices[i] = new Vector2(uvs[i].x * width, uvs[i].y * height);
            }

            ushort[] l_indices = sprite.triangles;

            indices = new int[l_indices.Length];

            for (int i = 0; i < l_indices.Length; ++i)
            {
                indices[i] = (int)l_indices[i];
            }

            HashSet<IndexedEdge> edgesSet = new HashSet<IndexedEdge>();

            for (int i = 0; i < indices.Length; i += 3)
            {
                int index1 = indices[i];
                int index2 = indices[i + 1];
                int index3 = indices[i + 2];

                IndexedEdge edge1 = new IndexedEdge(index1, index2);
                IndexedEdge edge2 = new IndexedEdge(index2, index3);
                IndexedEdge edge3 = new IndexedEdge(index1, index3);

                if (edgesSet.Contains(edge1))
                {
                    edgesSet.Remove(edge1);
                }
                else
                {
                    edgesSet.Add(edge1);
                }

                if (edgesSet.Contains(edge2))
                {
                    edgesSet.Remove(edge2);
                }
                else
                {
                    edgesSet.Add(edge2);
                }

                if (edgesSet.Contains(edge3))
                {
                    edgesSet.Remove(edge3);
                }
                else
                {
                    edgesSet.Add(edge3);
                }
            }

            edges = new IndexedEdge[edgesSet.Count];
            int edgeIndex = 0;
            foreach (IndexedEdge edge in edgesSet)
            {
                edges[edgeIndex] = edge;
                ++edgeIndex;
            }
        }

        public static void GetSpriteTextureSize(Sprite sprite, ref int width, ref int height)
        {
            if (sprite)
            {
                width = (int) sprite.bounds.size.x;
                height = (int) sprite.bounds.size.y;
            }
        }

        public static void UpdateAssets(RuntimeSpriteMesh spriteMesh, SpriteMeshData spriteMeshData)
        {
            if (spriteMesh && spriteMeshData)
            {
                //string spriteMeshPath = AssetDatabase.GetAssetPath(spriteMesh);

                //SerializedObject spriteMeshSO = new SerializedObject(spriteMesh);
                //SerializedProperty sharedMeshProp = spriteMeshSO.FindProperty("m_SharedMesh");
                //SerializedProperty sharedMaterialsProp = spriteMeshSO.FindProperty("m_SharedMaterials");

                if (!spriteMesh.sharedMesh)
                {
                    Mesh mesh = new Mesh();
                    mesh.hideFlags = HideFlags.HideInHierarchy;
                    //AssetDatabase.AddObjectToAsset(mesh, spriteMeshPath);

                    //spriteMeshSO.Update();
                    //sharedMeshProp.objectReferenceValue = mesh;
                    //spriteMeshSO.ApplyModifiedProperties();
                    //EditorUtility.SetDirty(mesh);
                }

                spriteMesh.sharedMesh.name = spriteMesh.name;

                if (spriteMesh.sharedMaterials.Length == 0)
                {
                    //Material material = new Material(Shader.Find("Sprites/Default"));
                    //material.mainTexture = SpriteUtility.GetSpriteTexture(spriteMesh.sprite, false);

                    //AssetDatabase.AddObjectToAsset(material, spriteMeshPath);

                    //spriteMeshSO.Update();
                    //sharedMaterialsProp.arraySize = 1;
                    //sharedMaterialsProp.GetArrayElementAtIndex(0).objectReferenceValue = material;
                    //spriteMeshSO.ApplyModifiedProperties();
                }

                for (int i = 0; i < spriteMesh.sharedMaterials.Length; i++)
                {
                    Material material = spriteMesh.sharedMaterials[i];

                    if (material)
                    {
                        if (spriteMesh.sprite)
                        {
                            //material.mainTexture = SpriteUtility.GetSpriteTexture(spriteMesh.sprite, false);
                        }

                        material.name = spriteMesh.name + "_" + i;
                        material.hideFlags = HideFlags.HideInHierarchy;
                        //EditorUtility.SetDirty(material);
                    }
                }

                spriteMeshData.hideFlags = HideFlags.HideInHierarchy;
                //EditorUtility.SetDirty(spriteMeshData);

                int width = 0;
                int height = 0;

                GetSpriteTextureSize(spriteMesh.sprite, ref width, ref height);

                Vector3[] vertices = GetMeshVertices(spriteMesh.sprite, spriteMeshData);

                Vector2 textureWidthHeightInv = new Vector2(1f / width, 1f / height);

                Vector2[] uvs = (new List<Vector2>(spriteMeshData.vertices)).ConvertAll(v => Vector2.Scale(v, textureWidthHeightInv)).ToArray();

                Vector3[] normals = (new List<Vector3>(vertices)).ConvertAll(v => Vector3.back).ToArray();

                BoneWeight[] boneWeightsData = spriteMeshData.boneWeights;

                if (boneWeightsData.Length != spriteMeshData.vertices.Length)
                {
                    boneWeightsData = new BoneWeight[spriteMeshData.vertices.Length];
                }

                List<UnityEngine.BoneWeight> boneWeights = new List<UnityEngine.BoneWeight>(boneWeightsData.Length);

                List<float> verticesOrder = new List<float>(spriteMeshData.vertices.Length);

                for (int i = 0; i < boneWeightsData.Length; i++)
                {
                    BoneWeight boneWeight = boneWeightsData[i];

                    List<KeyValuePair<int, float>> pairs = new List<KeyValuePair<int, float>>();
                    pairs.Add(new KeyValuePair<int, float>(boneWeight.boneIndex0, boneWeight.weight0));
                    pairs.Add(new KeyValuePair<int, float>(boneWeight.boneIndex1, boneWeight.weight1));
                    pairs.Add(new KeyValuePair<int, float>(boneWeight.boneIndex2, boneWeight.weight2));
                    pairs.Add(new KeyValuePair<int, float>(boneWeight.boneIndex3, boneWeight.weight3));

                    pairs = pairs.OrderByDescending(s => s.Value).ToList();

                    UnityEngine.BoneWeight boneWeight2 = new UnityEngine.BoneWeight();
                    boneWeight2.boneIndex0 = Mathf.Max(0, pairs[0].Key);
                    boneWeight2.boneIndex1 = Mathf.Max(0, pairs[1].Key);
                    boneWeight2.boneIndex2 = Mathf.Max(0, pairs[2].Key);
                    boneWeight2.boneIndex3 = Mathf.Max(0, pairs[3].Key);
                    boneWeight2.weight0 = pairs[0].Value;
                    boneWeight2.weight1 = pairs[1].Value;
                    boneWeight2.weight2 = pairs[2].Value;
                    boneWeight2.weight3 = pairs[3].Value;

                    boneWeights.Add(boneWeight2);

                    float vertexOrder = i;

                    if (spriteMeshData.bindPoses.Length > 0)
                    {
                        vertexOrder = spriteMeshData.bindPoses[boneWeight2.boneIndex0].zOrder * boneWeight2.weight0 +
                            spriteMeshData.bindPoses[boneWeight2.boneIndex1].zOrder * boneWeight2.weight1 +
                                spriteMeshData.bindPoses[boneWeight2.boneIndex2].zOrder * boneWeight2.weight2 +
                                spriteMeshData.bindPoses[boneWeight2.boneIndex3].zOrder * boneWeight2.weight3;
                    }

                    verticesOrder.Add(vertexOrder);
                }

                List<WeightedTriangle> weightedTriangles = new List<WeightedTriangle>(spriteMeshData.indices.Length / 3);

                for (int i = 0; i < spriteMeshData.indices.Length; i += 3)
                {
                    int p1 = spriteMeshData.indices[i];
                    int p2 = spriteMeshData.indices[i + 1];
                    int p3 = spriteMeshData.indices[i + 2];

                    weightedTriangles.Add(new WeightedTriangle(p1, p2, p3,
                                                               verticesOrder[p1],
                                                               verticesOrder[p2],
                                                               verticesOrder[p3]));
                }

                weightedTriangles = weightedTriangles.OrderBy(t => t.weight).ToList();

                List<int> indices = new List<int>(spriteMeshData.indices.Length);

                for (int i = 0; i < weightedTriangles.Count; ++i)
                {
                    WeightedTriangle t = weightedTriangles[i];
                    indices.Add(t.p1);
                    indices.Add(t.p2);
                    indices.Add(t.p3);
                }

                List<Matrix4x4> bindposes = (new List<BindInfo>(spriteMeshData.bindPoses)).ConvertAll(p => p.bindPose);

                for (int i = 0; i < bindposes.Count; i++)
                {
                    Matrix4x4 bindpose = bindposes[i];

                    bindpose.m23 = 0f;

                    bindposes[i] = bindpose;
                }

                spriteMesh.sharedMesh.Clear();
                spriteMesh.sharedMesh.vertices = vertices;
                spriteMesh.sharedMesh.uv = uvs;
                spriteMesh.sharedMesh.triangles = indices.ToArray();
                spriteMesh.sharedMesh.normals = normals;
                spriteMesh.sharedMesh.boneWeights = boneWeights.ToArray();
                spriteMesh.sharedMesh.bindposes = bindposes.ToArray();
                spriteMesh.sharedMesh.RecalculateBounds();

                //RebuildBlendShapes(spriteMesh);
            }
        }

        public static Vector3[] GetMeshVertices(Sprite sprite, SpriteMeshData spriteMeshData)
        {
            float pixelsPerUnit = sprite.pixelsPerUnit;

            return (new List<Vector2>(spriteMeshData.vertices)).ConvertAll(v => TexCoordToVertex(spriteMeshData.pivotPoint, v, pixelsPerUnit)).ToArray();
        }

        public static Vector3 TexCoordToVertex(Vector2 pivotPoint, Vector2 vertex, float pixelsPerUnit)
        {
            return (Vector3)(vertex - pivotPoint) / pixelsPerUnit;
        }

        public static void RebuildBlendShapes(SpriteMesh spriteMesh)
        {
            RebuildBlendShapes(spriteMesh, spriteMesh.sharedMesh);
        }

        public static void RebuildBlendShapes(SpriteMesh spriteMesh, Mesh mesh)
        {
            if (!mesh)
                return;

            if (!spriteMesh)
                return;

            BlendShape[] blendShapes = null;

            //SpriteMeshData spriteMeshData = LoadSpriteMeshData(spriteMesh);

            //if (spriteMeshData)
            {
                //blendShapes = spriteMeshData.blendshapes;
            }

            if (spriteMesh.sharedMesh.vertexCount != mesh.vertexCount)
            {
                return;
            }

            if (blendShapes != null)
            {
#if !(UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
                List<string> blendShapeNames = new List<string>();

                mesh.ClearBlendShapes();

                Vector3[] from = mesh.vertices;

                for (int i = 0; i < blendShapes.Length; i++)
                {
                    BlendShape blendshape = blendShapes[i];

                    if (blendshape)
                    {
                        string blendShapeName = blendshape.name;

                        if (blendShapeNames.Contains(blendShapeName))
                        {
                            Debug.LogWarning("Found repeated BlendShape name '" + blendShapeName + "' in SpriteMesh: " + spriteMesh.name);
                        }
                        else
                        {
                            blendShapeNames.Add(blendShapeName);

                            for (int j = 0; j < blendshape.frames.Length; j++)
                            {
                                BlendShapeFrame l_blendshapeFrame = blendshape.frames[j];

                                if (l_blendshapeFrame && from.Length == l_blendshapeFrame.vertices.Length)
                                {
                                    //Vector3[] deltaVertices = GetDeltaVertices(from, l_blendshapeFrame.vertices);

                                    //mesh.AddBlendShapeFrame(blendShapeName, l_blendshapeFrame.weight, deltaVertices, null, null);
                                }
                            }
                        }
                    }
                }

                mesh.UploadMeshData(false);

                //EditorUtility.SetDirty(mesh);

                //HideMaterials(spriteMesh);
#endif
            }
        }
    }
}
