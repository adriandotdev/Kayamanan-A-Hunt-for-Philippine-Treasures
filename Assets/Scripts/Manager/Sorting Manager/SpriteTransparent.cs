using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTransparent : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, .5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1f);
        }
    }
}
