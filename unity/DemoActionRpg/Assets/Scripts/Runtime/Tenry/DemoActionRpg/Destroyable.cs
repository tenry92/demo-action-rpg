using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class Destroyable : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
      if (other.tag == "Weapon") {
        var weapon = other.GetComponent<Weapon>();
        Debug.Assert(weapon != null);

        if (weapon.WeaponActive) {
          Destroy(this.gameObject);
        } else {
          Debug.Log("Weapon is not active");
        }
      }
    }
  }
}
