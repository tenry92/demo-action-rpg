using UnityEngine;
using UnityEngine.Pool;

namespace Tenry.DemoActionRpg.Pool {
  public static class GameObjectExtensions {
    public static void ReturnToPoolOrDestroy(this GameObject go) {
      var returnToPool = go.GetComponent<Pool.ReturnToPool>();

      if (returnToPool) {
        returnToPool.Return();
      } else {
        Object.Destroy(go.gameObject);
      }
    }
  }
}
