using System.Collections.Generic;

using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class DamageDealer : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private DamageType damageType;

    [SerializeField]
    [Min(1)]
    private int damage = 1;
    #endregion

    private HashSet<Damageable> damagedTargets = new HashSet<Damageable>();

    private void OnTriggerStay(Collider other) {
      var target = other.GetComponent<Damageable>();

      if (this.CanDamage(target)) {
        target.Damage(damage);
        this.damagedTargets.Add(target);
      }
    }

    private void OnTriggerEnter(Collider other) {
      var target = other.GetComponent<Damageable>();

      if (this.CanDamage(target)) {
        target.Damage(damage);
        this.damagedTargets.Add(target);
      }
    }

    private bool CanDamage(Damageable target) {
      if (target == null) {
        return false;
      }

      if (this.damagedTargets.Contains(target)) {
        return false;
      }

      return target.CanTakeDamageFrom(damageType);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos() {
      var collider = this.GetComponent<Collider>();

      if (collider != null) {
        Gizmos.color = new Color(1f, 0.5f, 0f);
        Gizmos.matrix = this.transform.localToWorldMatrix;

        var capsuleCollider = collider as CapsuleCollider;

        if (capsuleCollider != null) {
          var radius = capsuleCollider.radius;
          Gizmos.DrawWireSphere(capsuleCollider.center, radius);
          return;
        }

        var boxCollider = collider as BoxCollider;

        if (boxCollider != null) {
          Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
      }
    }
    #endif
  }
}
