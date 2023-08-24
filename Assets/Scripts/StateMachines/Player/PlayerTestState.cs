using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestState : PlayerBaseState {
  private float time_remaining = 5.0f;

  public PlayerTestState(PlayerStateMachine state_machine) : base(state_machine) {

  }

  public override void Enter() {
    Debug.Log("Enter");
  }

  public override void Tick(float delta_time) {
    time_remaining -= delta_time;

    Debug.Log(time_remaining);

    if (time_remaining <= 0f) {
      state_machine.SwitchState(new PlayerTestState(state_machine));
    }
  }

  public override void Exit() {
    Debug.Log("Exit");
  }
}
