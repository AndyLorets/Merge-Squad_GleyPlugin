using D2D;
using D2D.Core;
using D2D.Utilities;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class GameProgress : GameStateMachineUser
{
    [SerializeField] private LevelSO[] levels;
    [SerializeField] private Vector2 levelUpTimeRange;

    private float XPforLevelUp;
    private float totalXP;

    private float needToFinish;

    private int level = 0;

    private Dictionary<int, float> xpToLevelUps = new();

    private XPPicker xpPicker;

    public Action<int> OnLevelUp;

    private float levelTime;
    private float timeForLevelUp;
    private float levelUpTimer;

    private bool isStarted = false;
    private bool isFinished = false;

    public float GetValueForFinish() => totalXP / needToFinish;
    public float GetValueForTimeFinish() => levelTime / _levelSO.TotalDuration;
    public float GetValueForLevelUP() => XPforLevelUp / xpToLevelUps[level];
    public float GetValueForLevelUPTime() => levelUpTimer / timeForLevelUp;

    private void Awake()
    {
        _levelSO = levels[0];
        _gameProgress = this;

        xpPicker = _xpPicker;

        var multiplier = Mathf.Pow(_gameData.baseXPMultiplier, _db.PassedLevels.Value);

        timeForLevelUp = levelUpTimeRange.RandomFloat();

        if (_db.PassedLevels.Value >= 4)
        {
            multiplier *= 1.5f;
            Debug.Log("Boost level 5: " + multiplier);
        }
        
        if (_db.PassedLevels.Value >= 5)
        {
            multiplier *= 1.2f;
            Debug.Log("Boost level 6: " + multiplier);
        }
        
        if (_db.PassedLevels.Value >= 6)
        {
            multiplier *= 1.2f;
            Debug.Log("Boost level 6: " + multiplier);
        }
        
        if (_db.PassedLevels.Value >= 8)
        {
            multiplier += 1;
            Debug.Log("Boost level 6: " + multiplier);
        }

        for (int i = 0; i < LevelSO.LevelUps; i++)
        {
            var xpToLevelUp = _levelSO.BaseXPToLevelUp * multiplier + (i * _levelSO.StepXPOnLevelUp * multiplier);
            needToFinish += xpToLevelUp;

            xpToLevelUps.Add(i, xpToLevelUp);
        }
        
        /// For XP based levels game
        // xpPicker.OnPickUp += CheckForLevelUp;
        // xpPicker.OnPickUp += CheckForFinish;
    }
    protected override void OnGameRun()
    {
        isStarted = true;
    }
    protected override void OnGameFinish()
    {
        isFinished = true;
    }
    private void Update()
    {
        if (isStarted && !isFinished)
        {
            levelTime += Time.deltaTime;
            levelUpTimer += Time.deltaTime;

            if (levelUpTimer >= timeForLevelUp)
            {
                levelUpTimer = 0;

                LevelUp();
            }

            if (levelTime >= _levelSO.TotalDuration)
            {
                _stateMachine.Push(new WinState());
            }
        }
    }
    private void CheckForLevelUp(float xp)
    {
        if (level + 1 >= LevelSO.LevelUps)
        {
            return;
        }

        XPforLevelUp += xp;

        if (xpToLevelUps[level] <= XPforLevelUp)
        {
            XPforLevelUp = 0;
            level++;

            _audioManager.PlayOneShot(_gameData.spawnClip, 0.4f);

            OnLevelUp?.Invoke(level);
        }
    }

    [Button]
    public void LevelUp()
    {
        XPforLevelUp = 0;
        level++;

        _audioManager.PlayOneShot(_gameData.spawnClip, 0.4f);

        OnLevelUp?.Invoke(level);
    }

    private void CheckForFinish(float xp)
    {
        totalXP += xp;

        if (totalXP >= needToFinish)
        {
            _stateMachine.Push(new WinState());
        }
    }

}