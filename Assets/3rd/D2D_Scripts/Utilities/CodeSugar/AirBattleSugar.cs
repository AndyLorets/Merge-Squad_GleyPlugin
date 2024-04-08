using System;
using D2D.Utilities;
using DG.Tweening;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public static class AirBattleSugar
    {
        public static float DoubleLerp(this float data, Vector2 input, Vector2 output)
        {
            var clamped = data.Clamp(input);
            var f = Mathf.InverseLerp(input.x, input.y, clamped);
            return Mathf.Lerp(output.x, output.y, f);
        }

        public static Tween KillTo0(this Tween t)
        {
            if (t != null)
            {
                t.Goto(0, true);
                t.Kill();
            }

            return null;
        }
        
        public static float AngleBetween360(this Vector3 from, Vector3 to)
        {
            return 360 - Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
        }
        
        public static float AngleBetween360(this Transform from, Transform to)
        {
            if (from == null || to == null) 
                throw new ArgumentNullException("AngleBetween360");
            
            return AngleBetween360(from.position, to.position);
        }
    }
}