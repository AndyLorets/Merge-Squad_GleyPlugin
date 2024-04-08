using AV.Inspector.Runtime;
using D2D.Core;
using D2D.Utilities;
using D2D.Utils;
using DG.DOTweenEditor;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Animations
{
    [CustomEditor(typeof(DAnimation))]
    [CanEditMultipleObjects]
    public class DAnimationEditor : SuperEditor
    {
        protected virtual bool IsFromSupported => true;
        
        protected DAnimation _target;

        protected DAnimation CalculatedTarget => (DAnimation) target;
        
        public override void OnInspectorGUI()
        {
            BeginSerialization();

            _target = (DAnimation) target;
            
            ShowButtons();

            if (_target.isRandomnessSupported)
                ShowRandomFields();
            else
                ShowDefaultFields();
            
            ShowProperty("_ease");
            
            if (_target.isOnCompleteVisible && !_target.isAdvancedInfoVisible)
                ShowProperty("_onComplete", "On Complete");

            if (_target.isAdvancedInfoVisible)
                ShowAdvancedInfo();

            EndSerialization();
        }

        private void ShowButtons()
        {
            GUILayout.BeginHorizontal();

            if (Button(_coreData.tools.icons.effects))
            {
                FlipBool("_isPlayingInEditor");
                
                var t = (DAnimation) target;

                var isPlaying = GetBool("_isPlayingInEditor");
                
                // Avoid bag with isFrom 
                if (!isPlaying)
                    SaveInitialValues();
                
                t.InitAnimation();
                DOTweenEditorPreview.PrepareTweenForPreview(t.CurrentTween);

                if (isPlaying)
                {
                    t.Target.DOKill();
                    DOTweenEditorPreview.Stop();
                    RestoreInitialValues();
                }
                else
                {
                    DOTweenEditorPreview.Start();
                }
            }

            var text = $"Loops {_target._loops}";
            if (Button(text))
                InvertInt("_loops");

            text = _target._playMode == AnimationPlayMode.PlayOnEnable ? "On Enable" : "On Script";
            if (Button(text))
                PlusEnum("_playMode", 2);

            if (Button(_coreData.tools.icons.dice))
                FlipBool("isRandomnessSupported");

            if (Button(_coreData.tools.icons.advanced))
                FlipBool("isAdvancedInfoVisible");

            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            
            if (StateButton("Local", "_isLocal"))
                FlipBool("_isLocal");
            
            if (StateButton("Relative", "_isRelative"))
                FlipBool("_isRelative");

            if (IsFromSupported)
            {
                if (StateButton("From", "_isFrom"))
                    FlipBool("_isFrom");
            }
            
            if (StateButton("Auto Kill", "_destroyOnFinish"))
                FlipBool("_destroyOnFinish");
            
            GUILayout.EndHorizontal();
        }

        protected virtual void SaveInitialValues()
        {
            SetVector("beforePlayPosition", _target.Target.localPosition);
            SetVector("beforePlayRotation", _target.Target.localEulerAngles);
            SetVector("beforePlayScale", _target.Target.localScale);
        }

        protected virtual void RestoreInitialValues()
        {
            if (GetVector("beforePlayPosition").magnitude == 0 && GetVector("beforePlayScale").magnitude == 0)
                return;
            
            _target.Target.localPosition = GetVector("beforePlayPosition");
            _target.Target.localEulerAngles = GetVector("beforePlayRotation");
            _target.Target.localScale = GetVector("beforePlayScale");
        }

        protected virtual void ShowAdvancedInfo()
        {
            ShowLoopsProperty();
            ShowIsRelativeProperty();

            Space();

            var loops = _target._loops;

            if (_target.isRandomnessSupported)
            {
                ShowProperty("_startDelay", "Start Delay");

                if (_target.isAdvancedInfoVisible)
                {
                    if (loops > 1 || loops == -1)
                        ShowProperty("_delayBetweenYoyo", "Delay Between Yoyo");

                    if (loops > 2 || loops == -1)
                        ShowProperty("_delayBetweenCycles", "Delay Between Cycles");
                }
            }
            else
            {
                if (_target.isTargetVisibleInEditor)
                    ShowProperty("_target");

                ShowProperty("startDelay", "Start Delay");

                if (_target.isAdvancedInfoVisible)
                {
                    if (loops > 1 || loops == -1)
                        ShowProperty("delayBetweenYoyo", "Delay Between Yoyo");
                
                    if (loops > 2 || loops == -1)
                        ShowProperty("delayBetweenCycles", "Delay Between Cycles");
                }
            }
            
            if (_target.isOnCompleteVisible)
                ShowProperty("_onComplete", "On Complete");
        }

        protected void ShowIsRelativeProperty()
        {
            if (CalculatedTarget.isIncremental)
            {
                serializedObject.FindProperty("_isRelative").boolValue = false;
                serializedObject.FindProperty("_loops").intValue = 1;
            }
            else
            {
                // ShowProperty("_isRelative", "Relative");
            }
        }

        protected void ShowLoopsProperty()
        {
            if (CalculatedTarget.isIncremental)
            {
                serializedObject.FindProperty("_isRelative").boolValue = false;
                serializedObject.FindProperty("_loops").intValue = 1;
            }
            else
            {
                ShowProperty("_loops", "Loops");
            }
        }

        protected virtual void ShowRandomFields()
        {
            ShowProperty("_to");
            ShowDurationProperty();
        }

        public void ShowDurationProperty()
        {
            if (_target.isDurationDataMode)
                ShowProperty("_durationData", "Duration");
            else
                ShowProperty(_target.isRandomnessSupported ? "_duration" : "duration");
        }

        protected virtual void ShowDefaultFields()
        {
            ShowProperty("to");
            ShowDurationProperty();
        }

        private static Tween _previewTween;
    }
}