using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AugmentHUD : MonoBehaviour
{
    public enum InfoType { Aug, Level }
    public InfoType type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Aug:
                float curAug = AugmentSystem.instance.augmentGauge;
                float maxAug = AugmentSystem.instance.maxAugmentGauge;
                mySlider.value = curAug / maxAug;
                break;
            
            case InfoType.Level:

                break;

        }
    }
}
