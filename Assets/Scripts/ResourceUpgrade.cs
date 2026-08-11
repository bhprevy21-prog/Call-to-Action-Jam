using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract base class representing a generic upgrade.
// Uses ScriptableObject so individual upgrades can be created as data assets in Unity project files.
public abstract class ResourceUpgrade : ScriptableObject
{
    public float UpgradeAmount;
    
    public double OrignalUpgradeCost = 100;
    public double CurrentUpgradeCost = 100;
    public double CostIncreaseMultiplierPerPurchase = 0.05f; // Cost increases by 5% each purchase
    
    public string UpgradeButtonText;
    [TextArea(3, 10)] // Gives multi-line text input field inside Unity Inspector
    public string UpgradeButtonDescription;
    
    // Abstract method: Every child class must define what actually happens when purchased
    public abstract void ApplyUpgrade();

    // Runs automatically in the Unity Editor whenever values are changed in the Inspector
    private void OnValidate() {
        CurrentUpgradeCost = OrignalUpgradeCost;
    }
}