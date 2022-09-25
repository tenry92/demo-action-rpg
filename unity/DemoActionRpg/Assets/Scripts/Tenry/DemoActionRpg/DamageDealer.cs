using System.Collections.Generic;

using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class DamageDealer : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private bool hurtsPlayer;
    #endregion

    public bool WeaponActive {
      get {
        return this.collider.enabled;
      }
      set {
        this.damagedTargets.Clear();
        this.collider.enabled = value;
      }
    }

    public bool HurtsPlayer => this.hurtsPlayer;

    private new Collider collider;

    private HashSet<Damageable> damagedTargets = new HashSet<Damageable>();

    private void Awake() {
      this.collider = this.GetComponent<Collider>();
    }

    private void Start() {
      this.collider.enabled = false;
    }

    private void OnTriggerStay(Collider other) {
      var target = other.GetComponent<Damageable>();

      if (this.CanDamage(target)) {
        target.Damage(1);
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

      if (this.hurtsPlayer) {
        return target.tag == "Player";
      } else {
        return target.tag != "Player";
      }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos() {
      var collider = this.GetComponent<Collider>();

      if (collider != null) {
        if (this.hurtsPlayer) {
          Gizmos.color = Color.red;
        } else {
          Gizmos.color = new Color(1f, 0.5f, 0f);
        }

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
