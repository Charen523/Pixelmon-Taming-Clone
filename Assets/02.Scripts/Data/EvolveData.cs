using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolveData : IData
{
    public string rcode;

    public int start1;
    public int start2;
    public int start3;
    public int start4;
    public int start5;

    public string Rcode => rcode;
    public string Rank => rcode;
}