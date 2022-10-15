using UnityEngine;

using Tenry.DemoActionRpg.Pool;

namespace Tenry.DemoActionRpg {
  public class Bomb : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private float explodeInSeconds = 3f;

    [SerializeField]
    private Pool.ObjectPoolLink explosionPool;
    #endregion

    private void OnEnable() {
      Invoke("Explode", explodeInSeconds);
    }

    public void Explode() {
      gameObject.ReturnToPoolOrDestroy();

      var explosion = explosionPool.Pool.Get();
      explosion.transform.position = transform.position;
      explosion.transform.rotation = Quaternion.identity;
    }
  }
}
