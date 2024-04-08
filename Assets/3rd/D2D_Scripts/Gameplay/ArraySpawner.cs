using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D;
using D2D.Gameplay;
using D2D.Utilities;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;
using Object = System.Object;

namespace D2D
{
    public enum AutoCountMode {None, X, XZ}
    
    public class ArraySpawner : SmartScript
    {
        [TabGroup("Basic")] [SerializeField] private Vector3 _count;
        [TabGroup("Basic")] [SerializeField] private float _step;
        [TabGroup("Basic")] [SerializeField] private GameObject[] _prefabs;

        [TabGroup("Advanced")] [SerializeField] private bool _shuffle = true;
        [TabGroup("Advanced")] [SerializeField] private bool _rebuildOnStart;
        [TabGroup("Advanced")] [SerializeField] private Transform _folder;
        [TabGroup("Advanced")] [SerializeField] private bool _fromMiddle;
        [TabGroup("Advanced")] [SerializeField] private AutoCountMode _autoCountMode;
        [TabGroup("Advanced")] [SerializeField] private bool _showGizmo = true;
        [TabGroup("Advanced")] [SerializeField] private bool _replayOnRebuild;

        #if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (!_showGizmo)
                return;

            Gizmos.color = Color.blue;
            var tp = transform.position;

            for (int i = 0; i < _count.x; i++)
                LineWithSphere(tp, tp + new Vector3(i, 0) * _step);

            for (int i = 0; i < _count.y; i++)
                LineWithSphere(tp, tp + new Vector3(0, i) * _step);
            
            for (int i = 0; i < _count.z; i++)
                LineWithSphere(tp, tp + new Vector3(0, 0, i) * _step);
            
            void LineWithSphere(Vector3 from, Vector3 to)
            {
                Gizmos.DrawLine(from, to);
                Gizmos.DrawSphere(to, .1f);
            }
        }

        [Sirenix.OdinInspector.Button("Rebuild")]
        private void Rebuild()
        {
            var folder = _folder;
            if (folder == null)
                folder = transform;
            folder.ClearChildren();
            
            if (_shuffle)
                _prefabs = _prefabs.Shuffle();

            int prefabIndex = 0;

            if (_autoCountMode == AutoCountMode.X)
            {
                _count = new Vector3(_prefabs.Length, 0);
            }
            else if (_autoCountMode == AutoCountMode.XZ)
            {
                var root = Mathf.Sqrt(_prefabs.Length).Round();
                _count = new Vector3(root, 0, root);
            }

            for (int xIndex = 0; xIndex < _count.x; xIndex++)
            {
                for (int zIndex = 0; zIndex < _count.z; zIndex++)
                {
                    int yIndex = 0;
                    if (_count.y == 0)
                    {
                        if (Spawn(xIndex, yIndex, zIndex, folder, ref prefabIndex)) 
                            return;
                    }
                    else
                    {
                        for (yIndex = 0; yIndex < _count.y; yIndex++)
                        {
                            if (Spawn(xIndex, yIndex, zIndex, folder, ref prefabIndex)) 
                                return;
                        }
                    }
                }
            }
        }

        [Sirenix.OdinInspector.Button("On All")]
        public void OnAll() => transform.GetChildTransforms().OnAll();

        private bool Spawn(int xIndex, int yIndex, int zIndex, Transform folder, ref int prefabIndex)
        {
            var spawnPosition = transform.position + new Vector3(xIndex, yIndex, zIndex) * _step;

            if (_fromMiddle)
                spawnPosition -= _count * _step / 2f;

            if (prefabIndex >= _prefabs.Length)
                prefabIndex = DMath.Random(0, _prefabs.Length - 1);
            
            var instance = _prefabs[prefabIndex].EditorInstantiate(spawnPosition, folder);

            prefabIndex++;
            if (prefabIndex == _prefabs.Length - 1)
            {
                if (_autoCountMode == AutoCountMode.None)
                    prefabIndex = 0;
                else
                    return true;
            }

            return false;
        }

        #endif
    }
    
}