using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pixelmon : MonoBehaviour
{
    public PixelmonData data;
    public PixelmonFSM fsm;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private void Start()
    {
        fsm.InitStates();
    }
    private void Update()
    {
        fsm.Update();
    }
}
