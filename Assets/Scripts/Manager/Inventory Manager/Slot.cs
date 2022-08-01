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
            if (gameObject.transform.GetSiblingIndex() >= 
                DataPersistenceManager.instance.playerData.inventory.items.Count)
            {
                eventData.pointerDrag.GetComponent<SlotItem>().isCorrectlyDropped = false;
                return;
            }
            eventData.pointerDrag.GetComponent<SlotItem>().isCorrectlyDropped = true;

            if (gameObject.transform.childCount < 2)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.transform.SetAsFirstSibling();
                eventData.pointerDrag.transform.localPosition = Vector3.zero;
            }
            else
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.transform.SetAsFirstSibling();
                eventData.pointerDrag.transform.localPosition = Vector3.zero;

                print(transform.GetChild(1).name);
          
                transform.GetChild(1).SetParent(eventData.pointerDrag.GetComponent<SlotItem>().parentOfSlot.transform);

                eventData.pointerDrag.GetComponent<SlotItem>().parentOfSlot.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition
                    = eventData.pointerDrag.GetComponent<SlotItem>().parentOfSlot.transform.GetChild(1).parent.GetComponent<RectTransform>().anchoredPosition;

                eventData.pointerDrag.GetComponent<SlotItem>().parentOfSlot.transform.GetChild(1).localPosition = Vector3.zero;
                eventData.pointerDrag.GetComponent<SlotItem>().parentOfSlot.transform.GetChild(1).SetAsFirstSibling();
            }
            
        }
        
    }
}
