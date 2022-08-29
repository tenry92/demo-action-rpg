using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class Weapon : MonoBehaviour {
    #region Serialized Fields
    [Min(0.1f)]
    private float cooldown = 0.5f;
    #endregion

    public bool WeaponActive {
      get {
        return this.collider.enabled;
      }
      set {
        this.collider.enabled = value;
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
  }
}
