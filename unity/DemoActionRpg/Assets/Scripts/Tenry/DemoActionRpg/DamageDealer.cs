using System.Collections.Generic;

using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class DamageDealer : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    [Min(0.1f)]
    private float cooldown = 0.5f;

    [SerializeField]
    private DamageableTypes damageTargets;
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

    public DamageableTypes DamageTargets {
      get {
        return this.damageTargets;
      }
      set {
        this.damageTargets = value;
      }
    }

    public float Cooldown => this.cooldown;

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
      if (target == null || this.damagedTargets.Contains(target)) {
        return false;
      }

      return (this.DamageTargets & target.DamageableType) != DamageableTypes.None;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos() {
      var collider = this.GetComponent<Collider>();

      if (collider != null) {
        switch (LayerMask.LayerToName(this.gameObject.layer)) {
          case "Player":
            Gizmos.color = new Color(1f, 0.5f, 0f);
            break;
          case "Enemy":
            Gizmos.color = Color.red;
            break;
          default:
            Gizmos.color = Color.gray;
            break;
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
