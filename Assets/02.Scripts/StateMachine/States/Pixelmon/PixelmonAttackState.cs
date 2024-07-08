using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PixelmonAttackState : AttackState
{
    public PixelmonAttackState(PixelmonStateMachine stateMachine) 
        : base(stateMachine)
    {
    }

    public override void Execute()
    {
        //몹이 널이라면 상태 변화
        //널이 아니라면 공격 및 스킬
    }
}
