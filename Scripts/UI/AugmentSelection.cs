using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AugmentSelection : MonoBehaviour
{
    public TowerAugmentation data;

    Image icon;
    Text textName;
    Text textDesc;

    private Button selectButton;
    private TowerAugmentation newAugmentation;
    private Turret.TowerType towerType;
    [SerializeField] private AugmentSystem augmentSystem;

    public static TowerAugmentation[] silverAugs;
    public static TowerAugmentation silverAug;

    public static int[] ran;
    public static int picked = 3;

    private void Start()
    {
        
    }
    public static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            
            arr[a] = arr[a + 1];
        }
        
        Array.Resize(ref arr, arr.Length - 1);
    }

    private void ResetAug()
    {

        /**
        if (count % 3 == 0)
        {
            silverAugs = Resources.LoadAll<TowerAugmentation>("SilverAugData");
            ran = new int[3];
            while (true)
            {
                ran[0] = UnityEngine.Random.Range(0, silverAugs.Length);
                ran[1] = UnityEngine.Random.Range(0, silverAugs.Length);
                ran[2] = UnityEngine.Random.Range(0, silverAugs.Length);

                if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                    break;
            }
        }
        **/
        //silverAugs = Resources.LoadAll<TowerAugmentation>("SilverAugData");
        if (picked >= 3)
        {
            silverAugs = Resources.LoadAll<TowerAugmentation>("SilverAugData");
            picked = 0;
            ran = new int[3];
            while (true)
            {
                ran[0] = UnityEngine.Random.Range(0, silverAugs.Length);
                ran[1] = UnityEngine.Random.Range(0, silverAugs.Length);
                ran[2] = UnityEngine.Random.Range(0, silverAugs.Length);

                if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                    break;

            }
        }
    }

    private void Awake()
    {
          
    }

    private void OnEnable()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.augUp);
        ResetAug();


        data = silverAugs[ran[picked]];
        picked += 1;
        //RemoveAt<int>(ref ran, picked);

        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.augIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textName = texts[0];
        textDesc = texts[1];
        textName.text = data.augName;


        switch (data.augType)
        {
            case TowerAugmentation.AugType.Damage:
                textDesc.text = string.Format(data.augDesc, data.DamageaddPercent * 100);
                break;
            case TowerAugmentation.AugType.AttackSpeed:
                textDesc.text = string.Format(data.augDesc, data.ASaddPercent * 100);
                break;
            case TowerAugmentation.AugType.Range:
                textDesc.text = string.Format(data.augDesc, data.RangeAddPercent * 100);
                break;
            case TowerAugmentation.AugType.Econ:
                textDesc.text = string.Format(data.augDesc, data.DiscountPercent * 100);
                break;
            default:
                textDesc.text = string.Format(data.augDesc);
                break;

        }
        
    }

    public void OnSelectButtonClicked()
    {
        augmentSystem.RegisterAugmentation(data);

        ResetAug();
    }
}


