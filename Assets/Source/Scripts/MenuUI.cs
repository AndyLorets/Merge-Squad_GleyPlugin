using D2D.Core;
using UnityEngine;
using UnityEngine.UI;

using static D2D.Utilities.CommonGameplayFacade;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private MenuUIButton fireRateIncreaseButton;
    [SerializeField] private MenuUIButton firePowerIncreaseButton;
    [SerializeField] private Button continueButton;

    private void Start()
    {
        fireRateIncreaseButton.Button.onClick.AddListener(IncreaseFireRate);
        firePowerIncreaseButton.Button.onClick.AddListener(IncreasePowerUp);

        continueButton.onClick.AddListener(() => _stateMachine.Push(new RunningState()));

        Invoke(nameof(UpdateStats), .5f);         
    }
    private void IncreasePowerUp()
    {
        _db.PowerIncreaseLevel.Value++;

        _db.Money.Value -= _gameData.PowerUpgradePrice;

        UpdateStats();
    }
    private void IncreaseFireRate()
    {
        _db.FireRateDecreaseLevel.Value++;

        _db.Money.Value -= _gameData.FireUpgradePrice;
        UpdateStats();
    }
    
    private void CheckForDeactivatingButtons()
    {
        string maxText = LanguageExample.GetCurrentLanguage("MAX", "Ã¿ —");
        bool isEnoughForRate = _db.Money.Value >= _gameData.FireNextUpgradePrice;
        bool isEnoughForPower = _db.Money.Value >= _gameData.PowerNextUpgradePrice;
        
        fireRateIncreaseButton.Button.interactable = _gameData.upgradesPercentByLevel.Length <= _db.FireRateDecreaseLevel.Value ? false : isEnoughForRate;
        firePowerIncreaseButton.Button.interactable = _gameData.upgradesPercentByLevel.Length <= _db.FireRateDecreaseLevel.Value ? false : isEnoughForPower;
        
        if (_db.FireRateDecreaseLevel.Value >= _gameData.maxLevelUpgrade)
        {
            fireRateIncreaseButton.Button.interactable = false;
            fireRateIncreaseButton.PriceText.text = maxText;
        }

        if (_db.PowerIncreaseLevel.Value >= _gameData.maxLevelUpgrade)
        {
            firePowerIncreaseButton.Button.interactable = false;
            firePowerIncreaseButton.PriceText.text = maxText;
        }
    }
    private void UpdateStats() 
    {
        _db.PowerIncreasePercent.Value = _gameData.upgradesPercentByLevel[(int) _db.PowerIncreaseLevel.Value] / 100;
        _db.FireRateDecreasePercent.Value = _gameData.upgradesPercentByLevel[(int)_db.FireRateDecreaseLevel.Value] / 100;

        string levelText = LanguageExample.GetCurrentLanguage("LEVEL", "”–Œ¬≈Õ‹"); 

        firePowerIncreaseButton.LevelText.text = $"{levelText} {_db.PowerIncreaseLevel.Value}";
        fireRateIncreaseButton.LevelText.text = $"{levelText} {_db.FireRateDecreaseLevel.Value}";
        firePowerIncreaseButton.PriceText.text = $"{_gameData.PowerNextUpgradePrice} <sprite=0>";
        fireRateIncreaseButton.PriceText.text = $"{_gameData.FireNextUpgradePrice} <sprite=0>";

        CheckForDeactivatingButtons();
    }
}