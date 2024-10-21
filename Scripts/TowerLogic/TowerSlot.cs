using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerSlot : MonoBehaviour, IPointerClickHandler

{
    public void OnPointerClick(PointerEventData eventData) 
    {
        int index = transform.GetSiblingIndex();
        transform.parent.GetComponent<BuildManagerUI>().ResetOutline();
        BuildManager.Instance.ChangeSelectedTower(index);
        transform.GetChild(0).GetComponent<Outline>().enabled = true;
    }
}
