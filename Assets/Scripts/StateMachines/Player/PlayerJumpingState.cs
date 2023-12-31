using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState {
  private readonly int JUMP_HASH = Animator.StringToHash("Jump");
  private Vector3 momentum; 
  public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine) {
    stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);
    momentum = stateMachine.Controller.velocity;
    momentum.y = 0f;
    stateMachine.Animator.CrossFadeInFixedTime(JUMP_HASH, 0.1f);
  }
  public override void Enter() {}
  public override void Tick(float deltaTime) {
    Move(momentum, deltaTime);

    if (stateMachine.Controller.velocity.y <= 0) {
      stateMachine.SwitchState(new PlayerFallingState(stateMachine));
      return;
    }
  }
  public override void Exit() {}

}
