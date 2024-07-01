using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : DeathableStateMachine
{
    //public Player Player { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public bool IsAttacking { get; set; }

    public Transform MainCameraTransform { get; set; } //필요한가?

    //public PlayerStateMachine(Player player)
    //{
    //    this.Player = player;

    //    MainCameraTransform = Camera.main.transform;
    //}
}