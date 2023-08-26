using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState {
  private readonly int FREE_LOOK_SPEED_HASH = Animator.StringToHash("FreeLookSpeed");
  private readonly int FREE_LOOK_BLEND_TREE_HASH = Animator.StringToHash("FreeLookBlendTree");
  private const float animatorDampTime = 0.1f;

  public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) {}

  public override void Enter() {
    stateMachine.InputReader.targetEvent += OnTarget;
    stateMachine.Animator.CrossFadeInFixedTime(FREE_LOOK_BLEND_TREE_HASH, 0.1f);
  }

  public override void Tick(float deltaTime) {
    if (stateMachine.InputReader.isAttacking) {
      stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
      return;
    }

    Vector3 movement = CalculateMovement();

    Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
    
    if (stateMachine.InputReader.MovementValue == Vector2.zero) {
      stateMachine.Animator.SetFloat(FREE_LOOK_SPEED_HASH, 0, animatorDampTime, deltaTime);
      return;
    }

    stateMachine.Animator.SetFloat(FREE_LOOK_SPEED_HASH, 1, animatorDampTime, deltaTime);
    
    FaceMovementDirection(movement, deltaTime);
  }

  public override void Exit() {
    stateMachine.InputReader.targetEvent -= OnTarget;
  }

  private void OnTarget() {
    if (!stateMachine.Targeter.SelectTarget()) return;

    stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
  }

  private void FaceMovementDirection(Vector3 movement, float deltaTime) {
    stateMachine.transform.rotation = 
      Quaternion.Lerp(
        stateMachine.transform.rotation,
        Quaternion.LookRotation(movement),
        deltaTime * stateMachine.RotationDamping
      );
  }

  private Vector3 CalculateMovement() {
    Vector3 forward = stateMachine.MainCameraTransform.forward;
    Vector3 right = stateMachine.MainCameraTransform.right;

    forward.y = 0f;
    right.y = 0f;

    forward.Normalize();
    right.Normalize();

    return forward * stateMachine.InputReader.MovementValue.y +
           right * stateMachine.InputReader.MovementValue.x;
  }
}
