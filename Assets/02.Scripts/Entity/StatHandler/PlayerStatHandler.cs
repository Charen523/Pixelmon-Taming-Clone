using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    public PlayerData data;

    #region Player Status
    public float maxHp;
    public float def;
    #endregion

    public void UpdateStats()
    {
        maxHp = data.baseMaxHp;
        def = data.baseDef;

        Player.Instance.healthSystem.InitHealth(maxHp);
    }
}
