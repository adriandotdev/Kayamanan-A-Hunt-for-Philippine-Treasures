using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardsManager : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private string regionName;

    public RectTransform collectiblesPanel;
    public GameObject collectiblePrefab; 

    private void OnEnable()
    {
        this.canvasGroup = GameObject.Find("Safe Area of Game").GetComponent<CanvasGroup>();
        this.regionName = AssessmentManager.instance.regionName;

        this.canvasGroup.interactable = false;

        this.DisplayAllRewards();
    }

    private void DisplayAllRewards()
    {
        foreach (Collectible collectible in AssessmentManager.instance.playerData.notebook.collectibles)
        {
            if (collectible.regionName.ToUpper() == this.regionName.ToUpper())
            {
                GameObject collectibleObj = Instantiate(collectiblePrefab, collectiblesPanel.transform);

                collectibleObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(collectible.imagePath);
                collectibleObj.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = collectible.name;
            }
        }
    }

    public void Close()
    {
        this.canvasGroup.interactable = true;
        SceneManager.UnloadSceneAsync("Collectibles", UnloadSceneOptions.None);
    }
}
