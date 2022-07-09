using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** The ConfirmButton SCRIPT is attached to the confirm button at 
    CharacterCreation Scene
 */
public class ConfirmButton : MonoBehaviour
{
    [Header("Confirm Panel")]
    [SerializeField] private RectTransform confirmPanel;

    [Header("Character Creation Canvas Group")]
    public CanvasGroup characterCreationGroup;

    public TMPro.TMP_InputField inputField;

    private void Start()
    {
        // Get the canvas group to disable the back button and confirm button.
        this.characterCreationGroup = GameObject.Find("Character Creation Canvas Group").GetComponent<CanvasGroup>();
        this.inputField = GameObject.Find("Character Name").GetComponent<TMPro.TMP_InputField>();
    }

    public void ShowConfirmation(bool confirm)
    {
        if (confirm && this.inputField.text.Length != 0)
        {
            this.confirmPanel.gameObject.SetActive(confirm);
            this.characterCreationGroup.interactable = false;
            LeanTween.scale(confirmPanel.gameObject, new Vector2(0.80713f, 0.80713f), .3f);
        }
        else
        {
            LeanTween.scale(confirmPanel.gameObject, Vector2.zero, .3f)
                .setOnComplete(() => {

                    confirmPanel.gameObject.SetActive(false);
                    this.characterCreationGroup.interactable = true;
                });
        }
    }

    public void CreateNewProfile()
    {
        DataPersistenceManager.instance.ConfirmCharacter();
    }
}
