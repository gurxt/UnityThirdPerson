using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class EnemyIdleState : EnemyBaseState {
  private readonly int LOCOMOTION_HASH = Animator.StringToHash("Locomotion");
  private readonly int SPEED_HASH = Animator.StringToHash("Speed");

  public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) {}

  public override void Enter() {
    stateMachine.Animator.CrossFadeInFixedTime(LOCOMOTION_HASH, 0.1f);
  }
  public override void Tick(float deltaTime) {
    Move(deltaTime);

    if (IsInChaseRange()) {
      // transition to chasing state
      stateMachine.SwitchState(new EnemyChasingState(stateMachine));
      return;
    }

    FacePlayer();
    stateMachine.Animator.SetFloat(SPEED_HASH, 0f, 0.1f, deltaTime);

  }
  public override void Exit() {}
}
