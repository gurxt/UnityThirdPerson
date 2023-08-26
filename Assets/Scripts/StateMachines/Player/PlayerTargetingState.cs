using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState {
  private readonly int TARGETING_BLEND_TREE_HASH = Animator.StringToHash("TargetingBlendTree");
  private readonly int TARGETING_FORWARD_HASH = Animator.StringToHash("TargetingForward");
  private readonly int TARGETING_RIGHT_HASH = Animator.StringToHash("TargetingRight");
  public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) {}

  public override void Enter() {
    stateMachine.InputReader.cancelEvent += OnCancel;
    stateMachine.Animator.CrossFadeInFixedTime(TARGETING_BLEND_TREE_HASH, 0.1f);
  }
  public override void Tick(float deltaTime) {
    if (stateMachine.InputReader.isAttacking) {
      stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
      return;
    }

    if (stateMachine.Targeter.CurrentTarget == null ) {
      stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
      return;
    }

    Vector3 movement = CalculateMovement();

    Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

    UpdateAnimator(deltaTime);

    FaceTarget();
  }
  public override void Exit() {
    stateMachine.InputReader.cancelEvent -= OnCancel;
  }
  
  private void OnCancel() {
    stateMachine.Targeter.Cancel();
    stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
  }

  private Vector3 CalculateMovement() {
    Vector3 movement = new Vector3();

    movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
    movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

    return movement;
  }

  private void UpdateAnimator(float deltaTime) {
    // forward-backward movement
    if (stateMachine.InputReader.MovementValue.y == 0f) {
      stateMachine.Animator.SetFloat(TARGETING_FORWARD_HASH, 0f, 0.1f, deltaTime);
    } else {
      float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
      stateMachine.Animator.SetFloat(TARGETING_FORWARD_HASH, value, 0.1f, deltaTime);
    }
    // left-right movement
    if (stateMachine.InputReader.MovementValue.x == 0f) {
      stateMachine.Animator.SetFloat(TARGETING_RIGHT_HASH, 0f, 0.1f, deltaTime);
    } else {
      float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
      stateMachine.Animator.SetFloat(TARGETING_RIGHT_HASH, value, 0.1f, deltaTime);
    }

  }
}