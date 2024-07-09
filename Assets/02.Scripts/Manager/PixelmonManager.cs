using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelmonManager : Singleton<PixelmonManager>
{
    [SerializeField]
    private float radius = 2.0f;
    public int currentPixelmonCount;
    public Pixelmon[] Pixelmons = new Pixelmon[5];

    

    private void Start()
    {
        LocatedPixelmon();
    }

    public void LocatedPixelmon()
    {
        int angle = 360 / currentPixelmonCount;
        int currentAngle = 90;
        for (int i = 0; i < currentPixelmonCount; i++)
        {
            Vector3 pos = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius, Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius, 0);
            Pixelmons[i].transform.position = pos;
            currentAngle += angle;
        }
    }
}
