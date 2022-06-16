using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDataPersistence
{ 
    [Header("UI")]
    public TMPro.TextMeshProUGUI characterNameText;
    public Joystick joystick;

    public Animator animator;
    public float speed = 4;
    Vector3 movement;

    // Player Data
    public PlayerData playerData;

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        if (horizontal != 0 || vertical != 0)
        {
            animator.SetFloat("Speed", 1);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        movement = new Vector3(horizontal, vertical) * speed * Time.deltaTime;

        transform.position += movement;
    }

    public void LoadPlayerData(PlayerData playerData)
    {
        Debug.Log("DATA PERSIST LOADED : CHARACTER CLASS");
        this.playerData = playerData;

        this.characterNameText.text = this.playerData.name;
    }

    public void SavePlayerData()
    {
        throw new System.NotImplementedException();
    }
    public void LoadSlotsData(Slots slots)
    {
        throw new System.NotImplementedException();
    }
    public void SaveSlotsData(ref Slots slots)
    {
        throw new System.NotImplementedException();
    }
}
