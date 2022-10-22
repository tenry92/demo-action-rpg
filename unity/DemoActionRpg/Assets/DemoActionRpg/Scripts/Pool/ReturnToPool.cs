using UnityEngine;
using UnityEngine.Pool;

namespace Tenry.DemoActionRpg.Pool {
  /// <summary>
  /// This component is added to pool items so they know where they came from.
  /// </summary>
  public class ReturnToPool : MonoBehaviour {
    public IObjectPool<GameObject> Pool { get; set; }

    public void Return() {
      this.Pool.Release(gameObject);
    }
  }
}
