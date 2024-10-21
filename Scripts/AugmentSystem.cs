using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Events;

public class AugmentSystem : MonoBehaviour
{
    public static AugmentSystem instance;
    public static GameObject AugmentManager;

    [SerializeField] public int maxAugmentGauge = 9;
    [SerializeField] public int augmentGauge = 0;
    [SerializeField] private UnityEvent onGaugeIsFull;
    //public Dictionary<Turret.TowerType, TowerAugmentation> augmentationDict = new Dictionary<Turret.TowerType, TowerAugmentation>();
    private Turret.TowerType towerType;

    public float ADBuff = 0f;
    public float ASBuff = 0f;
    public float RBuff = 0f;
    public float DC = 0f;

    public float PressADBuff = 0f;
    public float PressASBuff = 0f;
    public float PressRBuff = 0f;
    public float PressDC = 0f;

    public float CutterADBuff = 0f;
    public float CutterASBuff = 0f;
    public float CutterRBuff = 0;
    public float CutterDC = 0f;

    public float WasherADBuff = 0f;
    public float WasherASBuff = 0f;
    public float WasherRBuff = 0;
    public float WasherDC = 0f;

    public float IncinADBuff = 0f;
    public float IncinASBuff = 0f;
    public float IncinRBuff = 0f;
    public float IncinDC = 0f;

    public float DroneADBuff = 0f;
    public float DroneASBuff = 0f;
    public float DroneRBuff = 0f;
    public float DroneDC = 0f;

    public float PigADBuff = 0f;
    public float PigASBuff = 0f;
    public float PigRBuff = 0f;
    public float PigDC = 0f;

    public float HumanADBuff = 0f;
    public float HumanASBuff = 0f;
    public float HumanRBuff = 0f;
    public float HumanDC = 0f;

    //Metal, Paper, Plastic, Glass, Food

    public float vsMetalBuff = 0f;
    public float vsPaperBuff = 0f;
    public float vsPlasticBuff = 0f;
    public float vsGlassBuff = 0f;
    public float vsFoodBuff = 0f;


    private void Start()
    {
        instance = this;
        init();
    }

    private void init()
    {
        

    }
    public void AddAugmentGauge()
    {
        augmentGauge++;
        if (augmentGauge > maxAugmentGauge )
        {
            onGaugeIsFull.Invoke();
        }
    }

    public void TakeAugmentGauge(int gaugeLose)
    {
        augmentGauge -= gaugeLose;
    }

    public void ResetAugmentGauge()
    {
        augmentGauge = 0;
        maxAugmentGauge += 3;
    }

    public void RegisterAugmentation(TowerAugmentation newAugmentation)
    {
        towerType = newAugmentation.towerType;

        switch (towerType)
        {
            case Turret.TowerType.press:
                PressADBuff += newAugmentation.DamageaddPercent;
                PressASBuff += newAugmentation.ASaddPercent;
                PressRBuff += newAugmentation.RangeAddPercent;
                PressDC += newAugmentation.DiscountPercent;
                break;
            case Turret.TowerType.cutter:
                CutterADBuff += newAugmentation.DamageaddPercent;
                CutterASBuff += newAugmentation.ASaddPercent;
                CutterRBuff += newAugmentation.RangeAddPercent;
                CutterDC += newAugmentation.DiscountPercent;
                break;
            case Turret.TowerType.incinerator:
                IncinADBuff += newAugmentation.DamageaddPercent;
                IncinASBuff += newAugmentation.ASaddPercent;
                IncinRBuff += newAugmentation.RangeAddPercent;
                IncinDC += newAugmentation.DiscountPercent;
                break;
            case Turret.TowerType.washer:
                WasherADBuff += newAugmentation.DamageaddPercent;
                WasherASBuff += newAugmentation.ASaddPercent;
                WasherRBuff += newAugmentation.RangeAddPercent;
                WasherDC += newAugmentation.DiscountPercent;
                break;
            default:
                ADBuff += newAugmentation.DamageaddPercent;
                ASBuff += newAugmentation.ASaddPercent;
                RBuff += newAugmentation.RangeAddPercent;
                DC += newAugmentation.DiscountPercent;
                break;
        }
        AudioManager.instance.PlaySfx(AudioManager.Sfx.augUp);

        //TowerManager.Instance.UpdateAugmentation(towerType, newAugmentation);
    }
}
