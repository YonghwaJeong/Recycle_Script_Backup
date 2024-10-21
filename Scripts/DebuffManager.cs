using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebuffCode { AirPollution, GlassBroken, Hazardous };

public class DebuffManager : MonoBehaviour
{
    [SerializeField] private GameObject airPollutionArea;
    [SerializeField] private GameObject brokenGlassArea;
    [SerializeField] private GameObject hazardousArea;

    [SerializeField] private float airPollutionDuration = 3f;
    [SerializeField] private float brokenGlassDuration = 3f;
    [SerializeField] private float hazardousDuration = 3f;

    [SerializeField] private float rangeIncrease = 0.33f;

    public void ActivateDebuff(Vector3 position, DebuffCode code)
    {
        switch (code)
        {
            case DebuffCode.AirPollution:
                if (!airPollutionArea.activeSelf)
                {
                    airPollutionArea.transform.position = position;
                    airPollutionArea.transform.localScale += new Vector3(rangeIncrease, rangeIncrease, 0);
                    StartCoroutine(DebuffCorutine(airPollutionArea, airPollutionDuration));
                }
                break;
            case DebuffCode.GlassBroken:
                if (!brokenGlassArea.activeSelf)
                {
                    brokenGlassArea.transform.position = position;
                    brokenGlassArea.transform.localScale += new Vector3(rangeIncrease, rangeIncrease, 0);
                    StartCoroutine(DebuffCorutine(brokenGlassArea, brokenGlassDuration));
                }
                break;
            case DebuffCode.Hazardous:
                if (!hazardousArea.activeSelf)
                {
                    hazardousArea.transform.position = position;
                    hazardousArea.transform.localScale += new Vector3(rangeIncrease, rangeIncrease, 0);
                    StartCoroutine(DebuffCorutine(hazardousArea, hazardousDuration));
                }
                break;
        }
    }

    public void DeactivateAll()
    {
        airPollutionArea.SetActive(false);
        brokenGlassArea.SetActive(false);
        hazardousArea.SetActive(false);
    }

    public IEnumerator DebuffCorutine(GameObject debuffArea, float duration)
    {
        debuffArea.SetActive(true);
        yield return new WaitForSeconds(duration);
        debuffArea.SetActive(false);
    }
}
