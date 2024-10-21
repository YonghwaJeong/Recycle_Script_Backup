using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;
    public List<Turret> towers = new List<Turret>();
    public AugmentSystem augmentSystem;

    void Awake()
    {
        Instance = this;
    }

    /*
    public void UpdateAugmentation(Turret.TowerType towerType, TowerAugmentation augmentation)
    {
        foreach (Turret tower in towers)
        {
            if (tower.GetTowerType() == towerType)
            {
                //tower.ApplyAugmentation(augmentation);
                tower.UpdateTowerGround();
            }
        }
    }

    */

    /*
    public void CheckAugmentationAndApply(Turret tower)
    {
        Turret.TowerType towerType = tower.GetTowerType();
        if(augmentSystem.augmentationDict.ContainsKey(towerType))
        {
            //tower.ApplyAugmentation(augmentSystem.augmentationDict[towerType]);
        }
    }
    */
}
