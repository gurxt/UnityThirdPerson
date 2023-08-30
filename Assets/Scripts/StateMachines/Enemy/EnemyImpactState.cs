using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState{
  private readonly int IMPACT_HASH = Animator.StringToHash("Impact");
  private float duration = 1f;
  public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) {}
  public override void Enter() {
    stateMachine.Animator.CrossFadeInFixedTime(IMPACT_HASH, 0.1f);
  }
  public override void Tick(float deltaTime) {
    Move(deltaTime);

    duration -= deltaTime;
    
    if (duration <= 0f) {
      stateMachine.SwitchState(new EnemyIdleState(stateMachine));
    }
  }
  public override void Exit() {}
}
