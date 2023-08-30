using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState {
  private readonly int LOCOMOTION_HASH = Animator.StringToHash("Locomotion");
  private readonly int SPEED_HASH = Animator.StringToHash("Speed");

  public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) {}

  public override void Enter() {
    stateMachine.Animator.CrossFadeInFixedTime(LOCOMOTION_HASH, 0.1f);
  }
  public override void Tick(float deltaTime) {
    if (!IsInChaseRange()) {
      // * reset to idle state when out of range
      stateMachine.SwitchState(new EnemyIdleState(stateMachine));
      return;
    } else if (IsInAttackRange()) {
      stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
    }

    MoveToPlayer(deltaTime);
    FacePlayer();

    stateMachine.Animator.SetFloat(SPEED_HASH, 1f, 0.1f, deltaTime);
  }
  public override void Exit() {
    // * called when out of chasing range
    stateMachine.Agent.ResetPath();
    stateMachine.Agent.velocity = Vector3.zero;
  }
  private void MoveToPlayer(float deltaTime) {
    if (stateMachine.Agent.isOnNavMesh) {
      stateMachine.Agent.destination = stateMachine.Player.transform.position;
      Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
    }
    stateMachine.Agent.velocity = stateMachine.Controller.velocity;
  }
  private bool IsInAttackRange() {
    if (stateMachine.Player.IsDead) return false;
    float playerDistanceSquare = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
    return playerDistanceSquare <= stateMachine.AttackRange * stateMachine.AttackRange;
  }
}
