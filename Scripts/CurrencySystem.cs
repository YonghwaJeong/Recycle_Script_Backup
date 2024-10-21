using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencySystem : MonoBehaviour
{
    public enum ResourceType { Metal, Paper, Plastic, Glass, Food };
    [SerializeField] private int initialMoney = 500;
    [SerializeField] private int metalValue = 15;
    [SerializeField] private int paperValue = 7;
    [SerializeField] private int plasticValue = 10;
    [SerializeField] private TextMeshProUGUI currentMoneyText;
    [SerializeField] private TextMeshProUGUI currentMetalText;
    [SerializeField] private TextMeshProUGUI currentPaperText;
    [SerializeField] private TextMeshProUGUI currentPlasticText;
    [SerializeField] private Button metalSellingButton;
    [SerializeField] private Button paperSellingButton;
    [SerializeField] private Button plasticSellingButton;

    private int currentMoney;
    private int currentMetal = 0;
    private int currentPaper = 0;
    private int currentPlastic = 0;

    public static CurrencySystem Instance;
    private void Awake()
    {
        Instance = this;
        currentMoney = initialMoney;
        UpdateCurrencyText();

        metalSellingButton.interactable = false;
        paperSellingButton.interactable = false;
        plasticSellingButton.interactable = false;
    }

    public void SpendMoney(int cost)
    {
        currentMoney -= cost;
        UpdateCurrencyText();
    }

    // 돈을 보상으로 지급할 일이 있을 때 사용 요망
    public void EarnMoney(int cost)
    {
        currentMoney += cost;
        UpdateCurrencyText();
    }

    public void GetResource(ResourceType resourceType, int resourceAmount)
    {
        switch (resourceType)
        {
            case ResourceType.Metal:
                currentMetal += resourceAmount;
                currentMetalText.text = currentMetal.ToString();
                if (currentMetal >= 10)
                {
                    metalSellingButton.interactable = true;
                }
                break;

            case ResourceType.Paper:
                currentPaper += resourceAmount;
                currentPaperText.text = currentPaper.ToString();
                if (currentPaper >= 10)
                {
                    paperSellingButton.interactable = true;
                }
                break;

            case ResourceType.Plastic:
                currentPlastic += resourceAmount;
                currentPlasticText.text = currentPlastic.ToString();
                if (currentPlastic >= 10)
                {
                    plasticSellingButton.interactable = true;
                }
                break;
        }
    }

    public void SellMetal()
    {
        currentMetal -= 10;
        currentMoney += 10 * metalValue;
        currentMetalText.text = currentMetal.ToString();
        UpdateCurrencyText();
        if (currentMetal < 10)
        {
            metalSellingButton.interactable = false;
        }
    }

    public void SellPaper()
    {
        currentPaper -= 10;
        currentMoney += 10 * paperValue;
        currentPaperText.text = currentPaper.ToString();
        UpdateCurrencyText();
        if (currentPaper < 10)
        {
            paperSellingButton.interactable = false;
        }
    }

    public void SellPlastic()
    {
        currentPlastic -= 10;
        currentMoney += 10 * plasticValue;
        currentPlasticText.text = currentPlastic.ToString();
        UpdateCurrencyText();
        if (currentPlastic < 10)
        {
            plasticSellingButton.interactable = false;
        }
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    private void UpdateCurrencyText()
    {
        currentMoneyText.text = currentMoney.ToString();
    }
}
