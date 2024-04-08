using System;
using UnityEngine;

public struct SwipeData
{
    public Vector2 startPosition;
    public Vector2 endPosition;
    public SwipeDirection direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

namespace D2D.Utilities
{
    public class DSwipes : MonoBehaviour, ILazy
    {
        [SerializeField] private bool detectSwipeOnlyAfterRelease;

        [SerializeField] private float minDistanceForSwipe = 10f;

        private Vector2 fingerDownPosition;
        private Vector2 fingerUpPosition;

        public static event Action<SwipeData> SwipeHappened;

        private void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUpPosition = touch.position;
                    fingerDownPosition = touch.position;
                }

                if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
                {
                    fingerDownPosition = touch.position;
                    DetectSwipe();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDownPosition = touch.position;
                    DetectSwipe();
                }
            }
        }

        private void DetectSwipe()
        {
            if (SwipeDistanceCheckMet())
            {
                if (IsVerticalSwipe())
                {
                    var direction = fingerDownPosition.y - fingerUpPosition.y > 0
                        ? SwipeDirection.Up
                        : SwipeDirection.Down;
                    SendSwipe(direction);
                }
                else
                {
                    var direction = fingerDownPosition.x - fingerUpPosition.x > 0
                        ? SwipeDirection.Right
                        : SwipeDirection.Left;
                    SendSwipe(direction);
                }

                fingerUpPosition = fingerDownPosition;
            }
        }

        private bool IsVerticalSwipe()
        {
            return VerticalMovementDistance() > HorizontalMovementDistance();
        }

        private bool SwipeDistanceCheckMet()
        {
            return VerticalMovementDistance() > minDistanceForSwipe ||
                   HorizontalMovementDistance() > minDistanceForSwipe;
        }

        private float VerticalMovementDistance()
        {
            return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
        }

        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
        }

        private void SendSwipe(SwipeDirection direction)
        {
            SwipeData swipeData = new SwipeData
            {
                direction = direction,
                startPosition = fingerDownPosition,
                endPosition = fingerUpPosition
            };

            SwipeHappened?.Invoke(swipeData);
        }
    }
}