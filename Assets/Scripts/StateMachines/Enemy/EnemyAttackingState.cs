using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState {
  private readonly int ATTACK_HASH = Animator.StringToHash("MeleeAttack");

  public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) {}

  public override void Enter() {
    stateMachine.Animator.CrossFadeInFixedTime(ATTACK_HASH, 0.1f);
  }
  public override void Tick(float deltaTime) {
  }
  public override void Exit() {}
}
