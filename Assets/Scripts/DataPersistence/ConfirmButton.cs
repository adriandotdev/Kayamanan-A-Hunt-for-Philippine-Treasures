using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** The ConfirmButton SCRIPT is attached to the confirm button at 
    CharacterCreation Scene
 */
public class ConfirmButton : MonoBehaviour
{
    [SerializeField] private RectTransform confirmPanel;

    public void ShowConfirmation(bool confirm)
    {
        if (confirm)
        {
            confirmPanel.gameObject.SetActive(confirm);
            LeanTween.scale(confirmPanel.gameObject, new Vector2(0.80713f, 0.80713f), .3f);
        }
        else
        {
            LeanTween.scale(confirmPanel.gameObject, Vector2.zero, .3f)
                .setOnComplete(() => confirmPanel.gameObject.SetActive(false));
        }
    }

    public void CreateNewProfile()
    {
        DataPersistenceManager.instance.ConfirmCharacter();
    }
}
