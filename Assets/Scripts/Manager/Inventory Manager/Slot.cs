using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (gameObject.transform.childCount < 2)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.transform.localPosition = Vector3.zero;
            }
            else
            {

            }
            //eventData.pointerDrag.GetComponent<SlotItem>().isCorrectlyDropped = true;
            //eventData.pointerDrag.GetComponent<SlotItem>().prevPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
