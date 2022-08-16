using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameManager : MonoBehaviour
{
    public RectTransform buttonsContainer;
    public GameObject buttonLayout;
    public Sprite notFlippedImage;
    public List<Sprite> buttonImages;
    public int noOfClicks = 0;

    public bool firstGuess;
    public bool secondGuess;

    public string firstGuessName;
    public string secondGuessName;

    public Transform transform1;
    public Transform transform2;

    private void Awake()
    {
        this.AddAllButtons();
    }

    public void AddAllButtons()
    {
        // for testing we make it 10 now

        GameObject memoryGameBTN;

        for (int i = 0; i < 10; i++)
        {
            memoryGameBTN = Instantiate(this.buttonLayout, this.buttonsContainer, false);

            memoryGameBTN.GetComponent<Image>().sprite = this.notFlippedImage;
        }

        int index = 0;

        foreach (Transform transform in this.buttonsContainer)
        {
            if (index == this.buttonsContainer.childCount / 2 - 1)
            {
                index = 0;
            }
            else
            {
                index += 1;
            }

            transform.gameObject.name = index.ToString();
            transform.GetComponent<Button>().onClick.AddListener(() =>
            { 
                this.CheckImage(transform);
            });
        }
    }

    public void CheckImage(Transform transform)
    {
        transform.GetComponent<Button>().enabled = false;

        if (!firstGuess)
        {
            this.firstGuess = true;

            transform.GetComponent<Image>().sprite = this.buttonImages[int.Parse(transform.name)];

            this.firstGuessName = transform.GetComponent<Image>().sprite.name;

            this.transform1 = transform;
        }
        else if (!secondGuess)
        {
            this.secondGuess = true;

            transform.GetComponent<Image>().sprite = this.buttonImages[int.Parse(transform.name)];

            this.secondGuessName = transform.GetComponent<Image>().sprite.name;

            this.transform2 = transform;

            if (this.firstGuessName.ToUpper() == this.secondGuessName.ToUpper())
            {
                print("MATCHED");

                this.firstGuess = false;
                this.secondGuess = false;
            }
            else
            {
                print("NOT MATCHED");

                this.firstGuess = false;
                this.secondGuess = false;

                StartCoroutine(FlippedTwoCards());
            }
        }
    }

    IEnumerator FlippedTwoCards()
    {
        yield return new WaitForSeconds(1);

        transform1.GetComponent<Image>().sprite = this.notFlippedImage;
        transform2.GetComponent<Image>().sprite = this.notFlippedImage;

        transform1.GetComponent<Button>().enabled = true;
        transform2.GetComponent<Button>().enabled = true;

        transform1 = null;
        transform2 = null;
    }
}
