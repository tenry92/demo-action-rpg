using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class Explosion : MonoBehaviour {
    private new ParticleSystem particleSystem;

    private void Awake() {
      particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void Update() {
      if (particleSystem != null && !particleSystem.IsAlive()) {
        Destroy(this.gameObject);
      }
    }
  }
}
