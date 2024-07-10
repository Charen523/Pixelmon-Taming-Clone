using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixelmon : MonoBehaviour
{
    public PixelmonData data;
    public PixelmonFSM fsm;
    private void Start()
    {
        fsm.InitStates();
    }
    private void Update()
    {
        fsm.Update();
    }
}
