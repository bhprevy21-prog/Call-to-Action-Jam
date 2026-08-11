using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dynamically creates UI buttons in the upgrade menu based on data objects
public class InitializeUpgrades : MonoBehaviour
{
    // Takes an array of upgrade data, creates a button for each, and configures them
    public void Initialize(ResourceUpgrade[] upgrades, GameObject UIToSpawn, Transform spawnParent)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            int currentIndex = i;

            // Instantiate (clone) a UI button prefab into the designated layout container
            GameObject go = Instantiate(UIToSpawn, spawnParent);

            // Reset current cost back to initial starting cost
            upgrades[currentIndex].CurrentUpgradeCost = upgrades[currentIndex].OrignalUpgradeCost;

            // Grab component references on the newly created button to update its text
            UpgradeButtonReferences buttonRef = go.GetComponent<UpgradeButtonReferences>();
            buttonRef.UpgradeButtonText.text = upgrades[currentIndex].UpgradeButtonText;
            buttonRef.UpgradeDescriptionText.SetText(upgrades[currentIndex].UpgradeButtonDescription, upgrades[currentIndex].UpgradeAmount);
            buttonRef.UpgradeCostText.text = "Cost: " + upgrades[currentIndex].CurrentUpgradeCost;

            // Assign a click listener so clicking the button calls GameManager purchase logic
            buttonRef.UpgradeButton.onClick.AddListener(delegate { GameManager.instance.OnUpgradeButtonClick(upgrades[currentIndex], buttonRef); });
        }
    }
}