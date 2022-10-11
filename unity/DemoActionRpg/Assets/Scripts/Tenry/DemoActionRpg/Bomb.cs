using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class Bomb : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private float explodeInSeconds = 3f;

    [SerializeField]
    private ObjectPoolLink explosionPool;
    #endregion

    private void OnEnable() {
      Invoke("Explode", explodeInSeconds);
    }

    public void Explode() {
      var returnToPool = this.GetComponent<ReturnToPool>();

      if (returnToPool) {
        returnToPool.Return();
      } else {
        Destroy(this.gameObject);
      }

      var explosion = explosionPool.Pool.Get();
      explosion.transform.position = transform.position;
      explosion.transform.rotation = Quaternion.identity;
    }
  }
}
