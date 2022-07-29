using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Canvas canvas;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] RectTransform rectTransform;

    public bool isCorrectlyDropped;
    public Vector2 prevPosition;
    public GameObject parentOfSlot;

    private void Start()
    {
        // NEED TO CHECK IF EVERY SCENE HAS ONE GAME OBJECT NAMED "Canvas"
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        print("Begin Drag");
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
        //this.isCorrectlyDropped = false;

        parentOfSlot = transform.parent.gameObject;
        transform.SetParent(GameObject.Find("Save View").transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("On Drag");
        print(canvas.scaleFactor);

        Touch touch = Input.GetTouch(0);

        //Vector2 position = Camera.main.ScreenToWorldPoint(touch.deltaPosition);

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("End Drag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        //if (isCorrectlyDropped == false)
        //{
        //    GetComponent<RectTransform>().anchoredPosition = prevPosition;
        //}

        //bool itemHasBeenPlaced = false;
        //var results = new List<RaycastResult>();
        //canvas.GetComponent < Graphic.Raycast(eventData, results);
        //foreach (var hit in results)
        //{
        //    if (hit.gameObject.transform.tag == "InventorySlot")
        //    {
        //        if (hit.gameObject.transform.childCount < 1)
        //        {
        //            the inventory slot is empty, we place here the item!
        //            draggedItem.transform.SetParent(hit.gameObject.transform);
        //            draggedItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
        //            itemHasBeenPlaced = true;
        //        }
        //        else
        //        {
        //            TODO: the inventory slot is taken, we swap the items!
        //          Debug.Log("the inventory slot is taken, we swap the items!");
        //            itemHasBeenPlaced = true;
        //        }
        //    }
        //}
        //if (!itemHasBeenPlaced)
        //{
        //    the item is not released on a proper slot, we restore it's position! 
        //    draggedItem.transform.SetParent(dragStartSlot.transform);
        //    draggedItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("Pointer Down");
        prevPosition = GetComponent<RectTransform>().anchoredPosition;
    }
}
