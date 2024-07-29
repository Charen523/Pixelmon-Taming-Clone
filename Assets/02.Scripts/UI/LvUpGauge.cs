using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUpGauge : MonoBehaviour
{
    public GameObject Empty;
    public GameObject Full;
    public bool IsFull;

    public void GaugeUp()
    {
        Empty.SetActive(false);
        Full.SetActive(true);
        IsFull = true;
    }
}
