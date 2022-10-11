using UnityEngine;
using UnityEngine.Pool;

namespace Tenry.DemoActionRpg.Pool {
  public class ObjectPoolController : MonoBehaviour {
    public ObjectPoolLink Link { get; set; }

    private IObjectPool<GameObject> pool;

    public IObjectPool<GameObject> Pool {
      get {
        if (pool == null) {
          CreatePool();
        }

        return pool;
      }
    }

    private void CreatePool() {
      if (Link.Type == ObjectPoolLink.PoolType.Stack) {
        pool = new UnityEngine.Pool.ObjectPool<GameObject>(
          CreateObject,
          OnTakeFromPool,
          OnReturnedToPool,
          OnDestroyPoolObject,
          false, // collectionCheck
          Link.DefaultCapacity,
          Link.MaxSize
        );
      } else {
        pool = new LinkedPool<GameObject>(
          CreateObject,
          OnTakeFromPool,
          OnReturnedToPool,
          OnDestroyPoolObject,
          false, // collectionCheck
          Link.MaxSize
        );
      }
    }

    private GameObject CreateObject() {
      var go = Instantiate(Link.Prefab);

      ReturnToPool returnToPool;

      if (!go.TryGetComponent<ReturnToPool>(out returnToPool)) {
        returnToPool = go.AddComponent<ReturnToPool>();
      }

      returnToPool.Pool = this.Pool;

      return go;
    }

    private void OnDestroyPoolObject(GameObject go) {
      Destroy(go);
    }

    private void OnReturnedToPool(GameObject go) {
      go.SetActive(false);
      go.transform.SetParent(this.transform, false);
    }

    private void OnTakeFromPool(GameObject go) {
      go.transform.SetParent(null, false);
      go.SetActive(true);
    }
  }
}
