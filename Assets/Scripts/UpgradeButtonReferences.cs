using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Holds direct references to elements inside an Upgrade Button prefab.
// This allows other scripts to easily edit the button text without using expensive searching methods like GetComponentInChildren.
public class UpgradeButtonReferences : MonoBehaviour
{
    public Button UpgradeButton;
    public TextMeshProUGUI UpgradeButtonText;
    public TextMeshProUGUI UpgradeDescriptionText;
    public TextMeshProUGUI UpgradeCostText;
}