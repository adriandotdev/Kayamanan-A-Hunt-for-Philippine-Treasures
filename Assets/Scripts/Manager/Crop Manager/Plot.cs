using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public SpriteRenderer plant;
    private bool isThereACropPlanted = false;
    private int plantStageIndex;
    float time;

    public Plant plantScriptable;

    private void Update()
    {
        if (isThereACropPlanted)
        {
            this.time -= 1 * Time.deltaTime;

            if (this.time < 0 && this.plantStageIndex < this.plantScriptable.plantStages.Length - 1)
            {
                this.plantStageIndex++;
                UpdatePlant();
                this.time = this.plantScriptable.timePerStage;
            }
        }
    }

    public void Plant()
    {
        this.plant.sprite = this.plantScriptable.plantStages[this.plantStageIndex];
        this.isThereACropPlanted = true;
        this.time = this.plantScriptable.timePerStage;
    }

    public void Harvest()
    {
        if (this.plantStageIndex == this.plantScriptable.plantStages.Length - 1)
        {
            this.plant.sprite = null;
            this.isThereACropPlanted = false;
            this.plantStageIndex = 0;
        }
    }

    void UpdatePlant()
    {
        this.plant.sprite = this.plantScriptable.plantStages[this.plantStageIndex];
    }

    private void OnMouseDown()
    {
        if (!this.isThereACropPlanted)
        {
            Plant();
        } 
        else
        {
            Harvest();
        }
    }
}
