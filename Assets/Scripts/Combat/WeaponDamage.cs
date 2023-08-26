using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour {
  [SerializeField] private Collider playerCollider;
  private List<Collider> alreadyCollidedWtith = new List<Collider>();
  private int damage;

  private void OnEnable() {
    alreadyCollidedWtith.Clear();
  }
  private void OnTriggerEnter(Collider other) {
    if (other == playerCollider) return;
    if (alreadyCollidedWtith.Contains(other)) return;

    alreadyCollidedWtith.Add(other);

    if (other.TryGetComponent<Health>(out Health health)) {
      health.DealDamage(damage);
    }
  }

  public void SetAttack(int damage) {
    this.damage = damage;
  }
}
