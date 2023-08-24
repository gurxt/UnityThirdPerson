using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour {
  [SerializeField] private State current_state;

  private void Update() {
    current_state?.Tick(Time.deltaTime);    
  }

  public void SwitchState(State new_state) {
    current_state?.Exit();
    current_state = new_state;
    current_state?.Enter();
  }
}
