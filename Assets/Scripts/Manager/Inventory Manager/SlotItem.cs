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
    public RectTransform parentOfSlot;

    public string itemName;

    private void Start()
    {
        // NEED TO CHECK IF EVERY SCENE HAS ONE GAME OBJECT NAMED "Canvas"
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentOfSlot = transform.parent.GetComponent<RectTransform>();

        if (parentOfSlot.gameObject.transform.GetSiblingIndex() >=
            DataPersistenceManager.instance.playerData.inventory.items.Count)
        {
            eventData.pointerDrag = null;
            return;
        }

        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
        this.isCorrectlyDropped = false;

        transform.SetParent(GameObject.Find("Safe Area").transform);
    }

    public void OnDrag(PointerEventData eventData)
    { 
        Touch touch = Input.GetTouch(0);

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (isCorrectlyDropped == false)
        {
            transform.SetParent(parentOfSlot);
            transform.SetAsFirstSibling();
            GetComponent<RectTransform>().anchoredPosition = parentOfSlot.GetComponent<RectTransform>().anchoredPosition;
            transform.localPosition = Vector3.zero;
        }
        else
        {
            // Find the item with the same name.
            Item item = DataPersistenceManager.instance.playerData.inventory.items.Find(itemToFind => itemToFind.itemName == this.itemName).CopyItem();

            // Remove the item with the same name
            DataPersistenceManager.instance.playerData.inventory.items.RemoveAll(itemToRemove => itemToRemove.itemName == this.itemName);

            // From  the found 'item', insert based on index.
            DataPersistenceManager.instance.playerData.inventory.items.Insert(transform.parent.GetSiblingIndex(), item);

            // Save Game
            DataPersistenceManager.instance.SaveGame();

            InventoryManager.instance.DisplayInventoryItems();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        prevPosition = transform.parent.GetComponent<RectTransform>().anchoredPosition;
        //prevPosition = GetComponent<RectTransform>().anchoredPosition;
    }
}
