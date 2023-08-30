using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState {
  private readonly int FREE_LOOK_SPEED_HASH = Animator.StringToHash("FreeLookSpeed");
  private readonly int FREE_LOOK_BLEND_TREE_HASH = Animator.StringToHash("FreeLookBlendTree");
  private const float animatorDampTime = 0.1f;
  public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) {}
  public override void Enter() {
    stateMachine.InputReader.TargetEvent += OnTarget;
    stateMachine.InputReader.DodgeEvent += OnDodge;
    stateMachine.InputReader.JumpEvent += OnJump;
    stateMachine.Animator.CrossFadeInFixedTime(FREE_LOOK_BLEND_TREE_HASH, 0.1f);
  }
  public override void Tick(float deltaTime) {
    if (stateMachine.InputReader.IsAttacking) {
      stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
      return;
    }

    Vector3 movement = CalculateMovement(deltaTime);

    Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
    
    if (stateMachine.InputReader.MovementValue == Vector2.zero) {
      stateMachine.Animator.SetFloat(FREE_LOOK_SPEED_HASH, 0, animatorDampTime, deltaTime);
      return;
    }

    stateMachine.Animator.SetFloat(FREE_LOOK_SPEED_HASH, 1, animatorDampTime, deltaTime);
    
    FaceMovementDirection(movement, deltaTime);
  }
  public override void Exit() {
    stateMachine.InputReader.TargetEvent -= OnTarget;
    stateMachine.InputReader.DodgeEvent -= OnDodge;
    stateMachine.InputReader.JumpEvent -= OnJump;
  }
  private void OnJump() {
    stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
  }
  private void OnDodge() {
    stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
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
  private Vector3 CalculateMovement(float deltaTime) {
    Vector3 forward = new Vector3();
    Vector3 right = new Vector3();

    forward = stateMachine.MainCameraTransform.forward;
    right = stateMachine.MainCameraTransform.right;

    forward.y = 0f;
    right.y = 0f;

    forward.Normalize();
    right.Normalize();

    return forward * stateMachine.InputReader.MovementValue.y +
           right * stateMachine.InputReader.MovementValue.x;
  }
}
