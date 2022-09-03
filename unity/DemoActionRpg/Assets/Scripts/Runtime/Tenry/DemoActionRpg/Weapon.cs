using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class Weapon : MonoBehaviour {
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

    private void Awake() {
      this.collider = this.GetComponent<Collider>();
    }

    private void Start() {
      this.collider.enabled = false;
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
