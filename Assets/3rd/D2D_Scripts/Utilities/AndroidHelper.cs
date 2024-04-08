using System;
using D2D.Utilities;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class AndroidHelper : SmartScript
    {
        private const int DefaultAndroidApiLevel = 31;
        public static bool IsOldDevice { get; }
        public static int ApiLevel { get; }

        static AndroidHelper()
        {
            ApiLevel = GetApiLevel();
            IsOldDevice = ApiLevel < 26;
        }

        private static int GetApiLevel()
        {
#if UNITY_EDITOR
                return 30;
#endif
#if UNITY_ANDROID
            try
            {
                using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
                {
                    return Mathf.Clamp(version.GetStatic<int>("SDK_INT"), 1, 100);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
#else
            return DefaultAndroidApiLevel;
#endif
        }
    }
}