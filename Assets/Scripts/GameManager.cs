using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening; // Used for smooth animations (tweens)

// The central hub of the game. It controls game state, resources, and main UI screens.
public class GameManager : MonoBehaviour {

    // Singleton pattern: Allows any script to easily access GameManager using GameManager.instance
    public static GameManager instance;
    
    // References to UI screens (Canvases)
    public GameObject MainGameCanvas;
    [SerializeField] private GameObject _upgradeCanvas;

    // References to text displays on the screen
    [SerializeField] private TextMeshProUGUI _resourceCountText;
    [SerializeField] private TextMeshProUGUI _resourcePerSecondCountText;

    // Visual objects used for click animations
    [SerializeField] private GameObject _resourceObject;
    [SerializeField] private GameObject _backgroundObject;
    
    // Reference to the helper script that sets up upgrade buttons
    private InitializeUpgrades _initializeUpgrades;

    [Space]
    // Array holding all available upgrades created in the Unity Inspector
    public ResourceUpgrade[] ResourceUpgrades;

    // Prefab and parent transform for creating upgrade UI buttons dynamically
    [SerializeField] private GameObject _upgradeUIToSpawn;
    [SerializeField] private Transform _upgradeUIParent;
    public GameObject ResourcePerSecondObjectToSpawn;
    
    // Player stats (using double for handling very large numbers)
    public double CurrentResourceCount{ get;  set; }
    public double CurrentResourcePerSecond{ get;  set; }
    
    // Bonus resources added to every manual click
    public double ResourcePerClickUpgrade { get;  set; }
    
    // Awake runs before Start when the scene loads
    private void Awake()
    {
        // Enforce the Singleton pattern so there is only ever one GameManager
        if (instance == null) {
            instance = this;
        }
        
        // Refresh text displays on screen launch
        UpdateResourceUI();
        UpdateResourcePerSecond();
        
        // Ensure player starts on the main game screen, not the upgrade menu
        _upgradeCanvas.SetActive(false);
        MainGameCanvas.SetActive(true);
        
        // Fetch the component and create all upgrade buttons on startup
        _initializeUpgrades = GetComponent<InitializeUpgrades>();
        _initializeUpgrades.Initialize(ResourceUpgrades, _upgradeUIToSpawn, _upgradeUIParent);
    }

    #region Click Actions and Animations

    // Called whenever the player clicks the main resource button
    public void OnResourceClicked() {
        IncreaseCount();

        // DOTween animations: Scale up slightly on click, then shrink back to normal size
        _resourceObject.transform.DOBlendableScaleBy(new Vector3(0.05f, 0.05f, 0.05f), 0.05f).OnComplete(ResourceScaleBack);
        _backgroundObject.transform.DOBlendableScaleBy(new Vector3(0.05f, 0.05f, 0.05f), 0.05f).OnComplete(BackgroundScaleBack);
    }

    // Helper method to reset resource size after click animation
    private void ResourceScaleBack() {
        _resourceObject.transform.DOBlendableScaleBy(new Vector3(-0.05f, -0.05f, -0.05f), 0.05f);
    }
    
    // Helper method to reset background size after click animation
    private void BackgroundScaleBack() {
        _backgroundObject.transform.DOBlendableScaleBy(new Vector3(-0.05f, -0.05f, -0.05f), 0.05f);
    }

    // Adds base click value (1) plus any upgrades, then updates the UI
    private void IncreaseCount() {
        CurrentResourceCount += 1 + ResourcePerClickUpgrade;
        UpdateResourceUI();
    }

    #endregion

    #region Update View 
    
    // Updates screen text with current total resources
    private void UpdateResourceUI() {
        _resourceCountText.text = CurrentResourceCount.ToString();
    }

    // Updates screen text with current passive generation rate
    private void UpdateResourcePerSecond() {
        _resourcePerSecondCountText.text = CurrentResourcePerSecond.ToString() + "P/S";
    }
    
    #endregion

    #region Navigation Buttons

    // Opens the upgrade menu screen
    public void OnUpgradeClicked() {
        MainGameCanvas.SetActive(false);
        _upgradeCanvas.SetActive(true);
    }
    
    // Returns to the main gameplay screen
    public void OnResumeClicked() {
        MainGameCanvas.SetActive(true);
        _upgradeCanvas.SetActive(false);
    }

    #endregion

    #region Direct Resource Adjustments

    // Adds a specific amount of resources (used by passive timers or events)
    public void SimpleResourceIncrease(double amount) {
        CurrentResourceCount += amount;
        UpdateResourceUI();
    }
    
    // Adds to the passive income per second value
    public void SimpleResourcePerSecondIncrease(double amount) {
        CurrentResourcePerSecond += amount;
        UpdateResourcePerSecond();
    }

    #endregion

    #region Purchasing Upgrades

    // Called when player attempts to buy an upgrade
    public void OnUpgradeButtonClick(ResourceUpgrade upgrade, UpgradeButtonReferences buttonRef)
    {
        // Check if player has enough resources
        if (CurrentResourceCount >= upgrade.CurrentUpgradeCost)
        {
            // Execute the specific upgrade effect
            upgrade.ApplyUpgrade();

            // Deduct cost and update main resource text
            CurrentResourceCount -= upgrade.CurrentUpgradeCost;
            UpdateResourceUI();

            // Increase the cost for the next purchase (exponential scaling)
            upgrade.CurrentUpgradeCost = Mathf.Round((float)(upgrade.CurrentUpgradeCost * (1 + upgrade.CostIncreaseMultiplierPerPurchase)));

            // Update cost text on the button
            buttonRef.UpgradeCostText.text = "Cost: " + upgrade.CurrentUpgradeCost;
        }
    }

    #endregion
}