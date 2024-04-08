using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using UnityEngine.EventSystems;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class DragByMouseOnSurface : SmartScript
    {
        [SerializeField] private LayerMask _surface;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector3 _originalRotation;
        
        [HideInInspector] public bool canDrag = true;

        public bool IsDragging { get; private set; }
        private Rigidbody _rb;
        private LayerMask _defaultLayer;

        private void Start()
        {
            _rb = Get<Rigidbody>();
            _defaultLayer = gameObject.layer;
        }

        public void OnClickDown()
        {
            if (!canDrag)
                return;

            // gameObject.layer = _gameData.draggableLayer.ToLayer();

            // Only 1 time
            if (IsDragging)
                return;
            
            IsDragging = true;
            
            _rb.IfNotNull(r => r.constraints = RigidbodyConstraints.FreezeRotation);
            // _rb.isKinematic = true;

            transform.eulerAngles = _originalRotation;
        }

        private void OnClickUp()
        {
            // Only 1 time
            if (!IsDragging)
                return;
            
            IsDragging = false;

            // gameObject.layer = _gameData.braceletPart.partsLayer.ToLayer();
            
            _rb.IfNotNull(r => r.constraints = RigidbodyConstraints.None);
            // _rb.isKinematic = false;
        }

        private void Update()
        {
            if (DInput.IsMouseReleased || !DInput.IsMousePressing)
                OnClickUp();
            
            if (!DInput.IsMousePressing || !IsDragging || !canDrag)
                return;

            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 999f, _surface))
            {
                transform.position = hitInfo.point + _offset;
            }
        }
    }
}