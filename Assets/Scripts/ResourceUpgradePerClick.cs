using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A specific type of upgrade that inherits from ResourceUpgrade.
// Allows creating assets directly from Unity menu: Create -> Resource Upgrade -> Resource Per Click
[CreateAssetMenu(menuName = "Resource Upgrade/Resource Per Click", fileName = "Resource Per Click")]
public class ResourceUpgradePerClick : ResourceUpgrade
{
    // Implementation of abstract method: Adds value to manual click strength
    public override void ApplyUpgrade() {
        GameManager.instance.ResourcePerClickUpgrade += UpgradeAmount;
    }
}