using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace D2D.Utilities
{
    public class DInput
    {
        public static Vector3 MousePosition => Input.mousePosition;
        
        public static bool IsMousePressed => Input.GetMouseButtonDown(0);
        public static bool IsMousePressing => Input.GetMouseButton(0);
        public static bool IsMouseReleased => Input.GetMouseButtonUp(0);

        public static bool IsLeftPressed => Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
        public static bool IsLeftPressing => Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        public static bool IsLeftReleased => Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A);
        
        public static bool IsRightPressed => Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
        public static bool IsRightPressing => Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        public static bool IsRightReleased => Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);
        
        public static bool IsUpPressed => Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
        public static bool IsUpPressing => Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        public static bool IsUpReleased => Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W);
        
        public static bool IsDownPressed => Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
        public static bool IsDownPressing => Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        public static bool IsDownReleased => Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S);
        
        public static bool IsMouseOverGameObject
        {
            get
            {
                var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
                {
                    position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
                };
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                return results.Count > 0;
            }
        }

        /// <summary>
        /// Converts swipe vector to 0..360 angle.
        /// </summary>
        public static float GetAngleFromSwipe(Vector2 swipe)
        {
            return 360 - MathSugar.Angle360(Vector2.up, swipe);
        }

        /// <summary>
        /// Offset by default = 0, useful for rotating by 45 degrees (like in Relaxy game)
        /// </summary>
        public static Vector2 GetDirectionVectorFromSwipe(Vector2 swipe, float offset = 0)
        {
            Vector2 direction = Vector2.zero;
            float angle = GetAngleFromSwipe(swipe);

            if (angle.Between(-45 + offset, 45 + offset))
                direction.y = -1;
            else if (angle.Between(45 + offset, 135 + offset))
                direction.x = -1;
            else if (angle.Between(135 + offset, 225 + offset))
                direction.y = 1;
            else if (angle.Between(225 + offset, 314 + offset))
                direction.x = 1;

            return direction;
        }
        
        public static Vector3 GetDirectionVectorFromArrowKeys(Vector3 swipe, bool is3d = true)
        {
            var direction = Vector3.zero;

            if (IsLeftPressing)
            {
                direction.x = -1;
            }

            if (IsRightPressing)
            {
                direction.x = 1;
            }

            if (IsUpPressing)
            {
                if (is3d)
                    direction.z = 1;
                else
                    direction.y = 1;
            }

            if (IsDownPressing)
            {
                if (is3d)
                    direction.z = -1;
                else
                    direction.y = -1;
            }

            return direction;
        }
        
        public static Vector3 GetDirectionVectorFromArrowKeys(Vector2 swipe, Action left, Action right, 
            Action up, Action down, bool is3d = true)
        {
            var direction = Vector3.zero;

            if (IsLeftPressing)
            {
                direction.x = -1;
                left?.Invoke();
            }

            if (IsRightPressing)
            {
                direction.x = 1;
                right?.Invoke();
            }

            if (IsUpPressing)
            {
                if (is3d)
                    direction.z = 1;
                else
                    direction.y = 1;
                
                up?.Invoke();
            }

            if (IsDownPressing)
            {
                if (is3d)
                    direction.z = -1;
                else
                    direction.y = -1;
                
                down?.Invoke();
            }

            return direction;
        }
    }
}