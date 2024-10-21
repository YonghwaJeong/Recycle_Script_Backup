using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower Augmentation", menuName = "Scriptable Object/Tower Augmentation")]
public class TowerAugmentation : ScriptableObject
{
    public enum AugType
    {
        Econ, Damage, AttackSpeed, Range, Util
    }

    [Header("# Main Info")]
    public AugType augType;
    public Turret.TowerType towerType;
    public int augId;
    public string augName;
    [TextArea]
    public string augDesc;
    public Sprite augIcon;

    [Header("# Aug Data")]
    public float DamageaddPercent;
    public float ASaddPercent;
    public float RangeAddPercent;
    public float DiscountPercent;

}

