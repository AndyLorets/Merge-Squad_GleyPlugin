using System;
using System.Collections.Generic;
using System.Linq;
using D2D.Utilities;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace D2D.Tools
{
    [CreateAssetMenu(fileName = "AssetsPalette", menuName = "AssetsPalette", order = 0)]
    public class AssetsPalette : SingletonData<AssetsPalette>
    {
        [SerializeField] private Object[] _favourites;

        #if UNITY_EDITOR
        // ...
        
        [Serializable]
        public class ReplaceTool
        {
            [Foldout("Replace tool")]
            [SerializeField] private GameObject _replacer;

            [Foldout("Replace tool")]
            [SerializeField] private bool _destroyOldSelected;

            [Foldout("Replace tool")]
            [SerializeField] private bool _selectNewAfterSpawn;
            
            [Button("Replace")]
            public void Replace()
            {
                var instances = new List<GameObject>();
                var length = Selection.gameObjects.Length;
                for (var i = 0; i < length; i++)
                {
                    var g = Selection.gameObjects[i];
                
                    var instance = _replacer.EditorInstantiate(g.transform.position, g.transform.parent);
                    instance.transform.rotation = g.transform.rotation;
                    instances.Add(instance);

                    if (_destroyOldSelected)
                    {
                        DestroyImmediate(Selection.gameObjects[i]);
                        length--;
                        i--;
                    }
                }

                if (_selectNewAfterSpawn)
                    Selection.objects = instances.ToArray();
            }
        }
        public ReplaceTool replaceTool;
        
        [Space]
        public Object[] toExport;
        
        /*[Serializable]
        public class MaterialTool
        {
            // [Foldout("MaterialTool")]
            [SerializeField] private Color _color;

            // [Foldout("MaterialTool")]
            [SerializeField] private Texture _texture;
            
            /*[Foldout("MaterialTool")]
            [SerializeField] private Material _material;
            
            private MeshRenderer[] MeshRenderers
                => Selection.gameObjects.Select(i => i.ChildrenGets<MeshRenderer>()).SelectMany(i => i).Distinct().ToArray();

            private SkinnedMeshRenderer[] SkinnedMeshes
                => Selection.gameObjects.Select(i => i.ChildrenGets<SkinnedMeshRenderer>()).SelectMany(i => i).Distinct().ToArray();

            public void OnWindowOpen()
            {
                if (!SkinnedMeshes.IsNullOrEmpty())
                {
                    _color = SkinnedMeshes[0].sharedMaterial.color;
                    _texture = SkinnedMeshes[0].sharedMaterial.mainTexture;

                    return;
                }
                
                if (!MeshRenderers.IsNullOrEmpty())
                {
                    _color = MeshRenderers[0].sharedMaterial.color;
                    _texture = MeshRenderers[0].sharedMaterial.mainTexture;
                }
            }

            public void ApplyChanges()
            {
                if (!SkinnedMeshes.IsNullOrEmpty())
                {
                    foreach (SkinnedMeshRenderer s in SkinnedMeshes)
                    {
                        /*
                        if (_material != null)
                        {
                            s.sharedMaterials[0] = _material;
                            continue;
                        }
                        

                        s.sharedMaterial.color = _color;
                        s.sharedMaterial.mainTexture = _texture;
                    }

                    return;
                }
                
                if (!MeshRenderers.IsNullOrEmpty())
                {
                    foreach (MeshRenderer s in MeshRenderers)
                    {
                        /*if (_material != null)
                        {
                            s.sharedMaterials[0] = _material;
                            // ....
                            continue;
                        }

                        s.sharedMaterial.color = _color;
                        s.sharedMaterial.mainTexture = _texture;
                    }
                }

                // _material = null;
            }
        }
        public MaterialTool materialTool;
        
        [Serializable]
        public class MeshTool
        {
            // [Foldout("MaterialTool")]
            [SerializeField] private Mesh _mesh;
            [SerializeField] private bool _readWriteEnabled;

            /*[Foldout("MaterialTool")]
            [SerializeField] private Material _material;
            
            private MeshFilter[] MeshFilters
                => Selection.gameObjects.Select(i => i.ChildrenGets<MeshFilter>()).SelectMany(i => i).Distinct().ToArray();

            private SkinnedMeshRenderer[] SkinnedMeshes
                => Selection.gameObjects.Select(i => i.ChildrenGets<SkinnedMeshRenderer>()).SelectMany(i => i).Distinct().ToArray();

            private MeshCollider[] MeshColliders
                => Selection.gameObjects.Select(i => i.ChildrenGets<MeshCollider>()).SelectMany(i => i).Distinct().ToArray();

            
            public void OnWindowOpen()
            {
                if (!SkinnedMeshes.IsNullOrEmpty())
                {
                    _mesh = SkinnedMeshes[0].sharedMesh;
                    _readWriteEnabled = _mesh.isReadable;

                    return;
                }
                
                if (!MeshFilters.IsNullOrEmpty())
                {
                    _mesh = MeshFilters[0].sharedMesh;
                    _readWriteEnabled = _mesh.isReadable;
                }
            }

            public void ApplyChanges()
            {
                _mesh.UploadMeshData(!_readWriteEnabled);

                if (!SkinnedMeshes.IsNullOrEmpty())
                {
                    foreach (SkinnedMeshRenderer s in SkinnedMeshes)
                        s.sharedMesh = _mesh;

                    return;
                }

                if (!MeshFilters.IsNullOrEmpty())
                {
                    foreach (var s in MeshFilters)
                        s.mesh = _mesh;
                    
                    if (!MeshColliders.IsNullOrEmpty())
                    {
                        foreach (var s in MeshColliders)
                            s.sharedMesh = _mesh;
                    }
                }
            }
        }
        public MeshTool meshTool;*/
        #endif
    }
}