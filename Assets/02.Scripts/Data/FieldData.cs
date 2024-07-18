using System;
using UnityEngine;

[Serializable]
public class FieldData
{
    public PixelmonData farmer;
    public FieldState currentFieldState; //저장 데이터.
    public int yieldClass;
    public float leftTime;
    public string lastSaveTime;
}
