using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    public void LoadPlayerData(PlayerData playerData);

    public void LoadSlotsData(Slots slots);

    public void SaveSlotsData(ref Slots slots);
}
