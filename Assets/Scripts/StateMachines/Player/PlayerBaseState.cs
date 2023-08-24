using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State {
  protected PlayerStateMachine state_machine;

  public PlayerBaseState(PlayerStateMachine state_machine) {
    this.state_machine = state_machine;
  }

}
