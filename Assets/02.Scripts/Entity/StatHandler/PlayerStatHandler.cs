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

    public void UpdateStats(float perHp, float perDef, float addHp = 1)
    {
        maxHp = data.baseMaxHp * (1 + perHp / 100);
        def = data.baseDef * (1 + perDef / 100);

        Player.Instance.healthSystem.maxHealth = maxHp;
        Player.Instance.healthSystem.currentHealth += data.baseMaxHp * addHp;
    }
}
