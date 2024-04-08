using System;
using Animancer;
using Cysharp.Threading.Tasks;
using D2D.Core;
using D2D.Utilities;
using UnityEngine;

namespace D2D
{
    public class PlayAnimationClipOnGameState : GameStateMachineUser
    {
        [SerializeField] private float _delay;
        [SerializeField] private float _blend = .3f;
        [SerializeField] private AnimationClip[] _onStart;
        [SerializeField] private AnimationClip[] _onRunning;
        [SerializeField] private AnimationClip[] _onWin;
        [SerializeField] private AnimationClip[] _onLose;
        [SerializeField] private AnimationClip[] _onGameFinish;

        private AnimancerComponent _animancer;

        private void Awake()
        {
            _animancer = Get<AnimancerComponent>();
        }

        private void Start() => PlayAnimation(_onStart);

        protected override void OnGameRun() => PlayAnimation(_onRunning);
        
        protected override void OnGameWin() => PlayAnimation(_onWin);
        
        protected override void OnGameLose() => PlayAnimation(_onLose);
        
        protected override void OnGameFinish() => PlayAnimation(_onGameFinish);

        private async UniTaskVoid PlayAnimation(AnimationClip[] clips)
        {
            if (clips.IsNullOrEmpty())
                return;
            
            await _delay.Seconds();

            _animancer.Play(clips.GetRandomElement(), _blend);
        }
    }
}