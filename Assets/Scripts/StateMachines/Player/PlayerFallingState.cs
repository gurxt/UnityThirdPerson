using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState {
  private readonly int FALL_HASH = Animator.StringToHash("Fall");
  private Vector3 momentum;
  public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine) {}
  public override void Enter() {
    momentum = stateMachine.Controller.velocity;
    momentum.y = 0f;
    stateMachine.Animator.CrossFadeInFixedTime(FALL_HASH, 0.1f);
  }
  public override void Tick(float deltaTime) {
    Move(momentum, deltaTime);

    if (stateMachine.Controller.isGrounded) {
      ReturnToLocomotion();
    }
  }
  public override void Exit() {}
}
