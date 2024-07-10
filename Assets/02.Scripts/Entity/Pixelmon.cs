using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixelmon : MonoBehaviour
{
    // 픽셀몬 데이터 필요
    public PixelmonStateMachine StateMachine;
    private void Awake()
    {
        StateMachine = new PixelmonStateMachine(this);
    }
}
