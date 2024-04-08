using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using D2D;
using D2D.Utilities;
using DG.Tweening;
using UnityEngine;

namespace D2D.Animations
{
    public enum AnimationPlayMode
    {
        PlayOnEnable,
        // PlayOnVisible,
        PlayByScript
    }

    public enum AnimationLooping
    {
        None,
        Pong,
        ToTo,
    }

    public abstract class DAnimation : MonoBehaviour
    {
        #region Variables

        [Tooltip("Random end point of the animation (from x to y)")] 
        [SerializeField] private Vector2 _to = new Vector2(1, 1);
        [SerializeField] private float to = 1;

        [Tooltip("Random (from x to y) duration of animation")] 
        [SerializeField] private Vector2 _duration = new Vector2(.5f, .5f);
        [SerializeField] private float duration = .5f;
        [SerializeField] private DurationData _durationData;

        [SerializeField] private Vector2 _startDelay;
        [SerializeField] private float startDelay;

        [SerializeField] private Vector2 _delayBetweenYoyo;
        [SerializeField] private float delayBetweenYoyo; 

        [Tooltip("Delay between animation cycles")] 
        [SerializeField] private Vector2 _delayBetweenCycles;
        [SerializeField] private float delayBetweenCycles;

        [Space(10)] 
        
        [Tooltip("You can set it to -1 for infinite yoyo animation")]
        [SerializeField] public int _loops = 1;

        [SerializeField] public bool _isRelative;

        [Tooltip("Need to kill this GO after animation (not looped) completed?")]
        [SerializeField] public bool _destroyOnFinish;

        [SerializeField] private Ease _ease = Ease.InOutSine;
        
        [SerializeField] public AnimationPlayMode _playMode = AnimationPlayMode.PlayOnEnable;

        [SerializeField] private DAnimation[] _onComplete;
        [SerializeField] public AnimationCurve curve = null;

        public event Action Tick;
        public event Action Completed;
        
        public bool _isFrom;
        public bool _isLocal;

        public bool isRandomnessSupported;
        public bool isAdvancedInfoVisible = true;
        public bool isOnCompleteVisible;
        public bool isDurationDataMode;
        public bool _isPlayingInEditor;
        
        public bool isTargetVisibleInEditor;
        [ContextMenu("Toggle target visibility")]
        private void ToggleTargetVisibility() => isTargetVisibleInEditor = !isTargetVisibleInEditor;

        public Vector3 beforePlayPosition;
        public Vector3 beforePlayRotation;
        public Vector3 beforePlayScale;

        public Ease Ease => _ease;
        
        public bool IsPlaying => CurrentTween != null && CurrentTween.IsPlaying();

        public bool IsJustBorn => _plays == 0 || CurrentTween == null;

        /// <summary>
        /// Simple recursive algorithm which calculates whole chain duration
        /// (with all _onCompletes).
        /// </summary>
        public float ChainDuration
        {
            get
            {
                return _onComplete.IsNullOrEmpty()
                    ? CalculatedDuration
                    : CalculatedDuration + _onComplete.Max(t => t.ChainDuration);
            }
        }

        /// <summary>
        /// For now very stupid recursive algorithm.
        /// For more complex solution it is recommended to find last tween in chain by hand. 
        /// </summary>
        public DAnimation LastTweenInChain
        {
            get
            {
                return _onComplete.IsNullOrEmpty() 
                    ? this 
                    : _onComplete.OrderBy(t => t.CalculatedDuration).First();
            }
        }

        public bool IsLooped => _loops > 1 || _loops == -1;

        public bool HasStartDelay => StartDelay != Vector2.zero;
        
        public Tween CurrentTween { get; private set; }
        
        [HideInInspector] public bool isIncremental;
        
        public float CalculatedDuration
        {
            get
            {
                if (_calculatedDuration < 0)
                    _calculatedDuration = Duration.RandomFloat();

                return _calculatedDuration;
            }
            set => _calculatedDuration = value;
        }
        private float _calculatedDuration = -1;
        
        private bool _isCustomDuration;
        
        protected float CalculatedTo
        {
            get
            {
                if (_calculatedTo < 0)
                    _calculatedTo = To.RandomFloat();

                return _calculatedTo;
            }
            set => _calculatedTo = value;
        }
        private float _calculatedTo = -1;

        public Transform Target
        {
            get
            {
                if (_target == null)
                    Target = transform;

                return _target;
            }
            private set => _target = value;
        }
        public Transform _target;

        private Vector2 To 
            => isRandomnessSupported ? _to : new Vector2(to, to);

        private Vector2 Duration
        {
            get
            {
                if (isDurationDataMode)
                {
                    if (_durationData == null)
                        throw new Exception("No duration data provided!");

                    return _durationData.Value;
                }
                
                return isRandomnessSupported ? _duration : new Vector2(duration, duration);
            }
        }

        private Vector2 StartDelay =>
            isRandomnessSupported ? _startDelay : new Vector2(startDelay, startDelay);
        
        private Vector2 DelayBetweenCycles =>
            isRandomnessSupported ? _delayBetweenCycles : new Vector2(delayBetweenCycles, delayBetweenCycles);

        private Vector2 DelayBetweenYoyo =>
            isRandomnessSupported ? _delayBetweenYoyo : new Vector2(delayBetweenYoyo, delayBetweenYoyo);

        // For looped animations
        private Coroutine _loopCoroutine;
        private Coroutine _basePlayAndReplayCoroutine;
        private int _animationLoops;
        private int _plays;
        
        #endregion

        private void OnValidate()
        {
            _plays = 0;

            SetStaticRecursively(gameObject, false);
            
            if (!_onComplete.IsNullOrEmpty())
                _onComplete.ForEach(a => a._playMode = AnimationPlayMode.PlayByScript);
        }
        
        private static void SetStaticRecursively(GameObject go, bool isStatic)
        {
            foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.isStatic = isStatic;
            }
        }

        private void OnEnable()
        {
            if (_playMode != AnimationPlayMode.PlayOnEnable)
                return;
            
            // Set immediately tween back to initial state (0%)
            if (CurrentTween != null)
            {
                CurrentTween.Rewind(false);
                
                // Reinitialize everything
                ReInit(true);
                CalculatedDuration = -1;
                CalculatedTo = -1;
            }

            Play();
        }

        #region Initializing

        /// <summary>
        /// Full animation initialize.
        /// </summary>
        public void InitAnimation()
        {
            if (!_isCustomDuration)
                _calculatedDuration = -1;
            
            if (CurrentTween != null)
                CurrentTween.onUpdate -= BroadcastTick;
            
            CurrentTween = CreateTween();
            
            CurrentTween.onUpdate += BroadcastTick;
            
            CurrentTween.SetRelative(_isRelative).SetAutoKill(false);
            
            if (Ease == Ease.INTERNAL_Custom)
                CurrentTween.SetEase(curve);
            else
                CurrentTween.SetEase(_ease);

            if (_destroyOnFinish && _loops == 1)
                DestroySelfAfterDelay();

            if (isIncremental)
            {
                CurrentTween.SetLoops(-1, LoopType.Incremental);
            }
        }

        private void BroadcastTick()
        {
            Tick?.Invoke();
        }

        private void DestroySelfAfterDelay()
        {
            Target.gameObject.KillSelf(CalculatedDuration + TweenSettings.Instance.killDelay);
        }

        /// <summary>
        /// Actual children class tween specific implementation.
        /// </summary>
        protected abstract Tween CreateTween();

        /// <summary>
        /// Destroys the old animation and creates a new tween with updates parameters.
        /// Mostly resets random parameters.
        /// </summary>
        public DAnimation ReInit(bool killPrevious = false)
        {
            _plays = 0;
            OnReset();

            if (killPrevious) 
                CurrentTween?.Kill();
            
            InitAnimation();

            return this;
        }
        
        #endregion

        #region Setters

        public DAnimation SetTarget(Transform newTarget)
        {
            Target = newTarget;
            
            OnSetTarget();

            return this;
        }
        
        public DAnimation SetUpdateType(UpdateType type)
        {
            if (CurrentTween == null)
            {
                throw new Exception("Cant set update type for tween if CurrentTween = null");
            }
            
            CurrentTween.SetUpdate(type);

            return this;
        }
        
        public DAnimation SetDuration(float d)
        {
            _isCustomDuration = true;
            _calculatedDuration = d;
            return this;
        }
        
        /// <summary>
        /// Yeah, we can: https://stackoverflow.com/questions/20078025/can-somehow-a-method-from-base-class-return-child-class
        /// But we want to easily serialize DAnimation in inspector => avoid generics
        /// </summary>
        public virtual DAnimation SetEndPoint(Vector3 endPoint)
        {
            return this;
        }

        #endregion

        #region Play methods

        public DAnimation Play()
        {
            if (_basePlayAndReplayCoroutine != null)
                StopCoroutine(_basePlayAndReplayCoroutine);
            
            _basePlayAndReplayCoroutine = 
                StartCoroutine(BasePlayAndReplay(PlayCurrentTween)); 

            return this;
        }

        private void PlayCurrentTween() => CurrentTween.Play();
        
        public DAnimation Play(Vector3 endPoint, Action onComplete = null)
        {
            var a = SetEndPoint(endPoint).ReInit().Play();
            
            if (onComplete != null)
                a.Completed += onComplete;

            return a;
        }

        public DAnimation Restart()
        {
            if (_basePlayAndReplayCoroutine != null)
                StopCoroutine(_basePlayAndReplayCoroutine);
            
            _basePlayAndReplayCoroutine = 
                StartCoroutine(BasePlayAndReplay(() => CurrentTween.Restart()));

            return this;
        }

        public void KillTo0()
        {
            if (CurrentTween != null)
            {
                CurrentTween.Goto(0, true);
                CurrentTween.Kill();
                CurrentTween = null;
            }
        }

        public void Kill()
        {
            if (CurrentTween != null)
            {
                CurrentTween.Kill();
                CurrentTween = null;
            }
        }

        protected virtual void OnLoop() { }

        private IEnumerator PlayLooped()
        {
            _animationLoops = 0;
            while (_loops == -1 || _animationLoops < _loops)
            {
                CurrentTween.Restart();
                
                yield return new WaitForSeconds(CalculatedDuration);

                yield return new WaitForSeconds(DelayBetweenYoyo.RandomFloat());

                OnLoop();

                CurrentTween.PlayBackwards();
                
                yield return new WaitForSeconds(CalculatedDuration);

                _animationLoops += 2;

                if (_animationLoops == _loops && _destroyOnFinish)
                {
                    RestartChainedIfHave();
                    InvokeCompletion();
                    
                    DestroySelfAfterDelay();
                }
                
                yield return new WaitForSeconds(DelayBetweenCycles.RandomFloat());
            }
        }

        public DAnimation Stop()
        {
            if (CurrentTween == null)
                return null;

            if (DelayBetweenCycles.RandomFloat() > 0)
            {
                if (_loopCoroutine != null)
                    StopCoroutine(_loopCoroutine);
            }
            else
            {
                CurrentTween.Pause();
            }
            
            return this;
        }

        private IEnumerator BasePlayAndReplay(Action callback)
        {
            bool needDelay = false;
            
            if (IsJustBorn)
            {
                OnBeforeFirstPlayAndInit();
                needDelay = true;
            }

            OnBeforePlayAndInit();
            
            if (!Target.gameObject.activeSelf)
                yield break;
            
            if (HasStartDelay && needDelay)
                yield return new WaitForSeconds(StartDelay.RandomFloat());

            if (CurrentTween == null || _isRelative)
                InitAnimation();
            
            if (IsJustBorn)
            {
                if (_loops == 1)
                {
                    CurrentTween.onComplete += RestartChainedIfHave;
                    CurrentTween.onComplete += InvokeCompletion;
                }
            }

            _plays++;
            
            if (IsLooped)
            {
                if (_loopCoroutine != null)
                    StopCoroutine(_loopCoroutine);
                
                _loopCoroutine = StartCoroutine(PlayLooped());
            }
            else
            {
                callback?.Invoke();
            }
        }

        private void InvokeCompletion()
        {
            Completed?.Invoke();
        }

        private void RestartChainedIfHave()
        {
            if (!_onComplete.IsNullOrEmpty())
                _onComplete.ForEach(c => c.Restart());
        }

        #endregion
        
        protected virtual void OnReset() { }
        
        protected virtual void OnBeforePlayAndInit() { }

        protected virtual void OnBeforeFirstPlayAndInit() { }
        
        protected virtual void OnSetTarget() { }
    }
}
