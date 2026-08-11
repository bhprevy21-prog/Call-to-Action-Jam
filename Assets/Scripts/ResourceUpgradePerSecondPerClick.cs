using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

// A specific type of upgrade that adds passive income per second over time.
// Allows creating assets directly from Unity menu: Create -> Resource Upgrade -> Resource Per Second
[CreateAssetMenu(menuName = "Resource Upgrade/Resource Per Second", fileName = "Resource Per Second")]
public class ResourceUpgradePerSecondPerClick : ResourceUpgrade
{
    // Implementation of abstract method: Spawns a timer object and updates total passive income
    public override void ApplyUpgrade() {
        // Spawns (instantiates) a timer prefab into the game scene at position (0,0,0)
        GameObject go = Instantiate(GameManager.instance.ResourcePerSecondObjectToSpawn, Vector3.zero, Quaternion.identity);
        
        // Configures the spawned timer object with this upgrade's resource amount
        go.GetComponent<ResourcePerSecondTimer>().ResourcePerSecond = UpgradeAmount;
        
        // Updates GameManager's passive resource tracking and UI display
        GameManager.instance.SimpleResourcePerSecondIncrease(UpgradeAmount);
    }
}