using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingSprite : MonoBehaviour
{
    public enum ESortingType
    {
        Static, Update
    }

    [SerializeField] private ESortingType sortingType;

    private SpriteSorterManager sorter;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        sorter = FindObjectOfType<SpriteSorterManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (gameObject.CompareTag("PH Flag"))
        {
            spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder;
            return;
        }
        spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (sortingType == ESortingType.Update)
        {
            spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
        }
    }
}
