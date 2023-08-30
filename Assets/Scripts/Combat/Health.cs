using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour {
  [SerializeField] private int maxHealth = 100;
  private int health;
  public event Action OnTakeDamage;
  public event Action OnDie;
  private bool isInvulnerable;
  public bool IsDead => health == 0;
  private void Start() {
    health = maxHealth;
  }
  public void DealDamage(int damage) {
    if (health == 0) return;

    Debug.Log(health);

    if (isInvulnerable) return;

    health = Mathf.Max(health - damage, 0);

    OnTakeDamage?.Invoke();

    if (health == 0)
      OnDie?.Invoke();
  }
  public void SetInvulnerable(bool isInvulnerable) {
    this.isInvulnerable = isInvulnerable;
  }
}
