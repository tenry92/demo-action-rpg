using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class ObjectSpawner : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private bool usePool = false;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private Pool.ObjectPoolLink pool;
    #endregion

    public void Spawn(out GameObject go) {
      if (usePool) {
        go = pool.Pool.Get();
        go.transform.SetParent(null);
        go.transform.position = transform.position;
        go.transform.rotation = Quaternion.identity;
      } else {
        go = Instantiate(prefab, transform.position, Quaternion.identity);
      }
    }

    public void Spawn(int spawnParameter) {
      Spawn(out var go);
      go.SendMessage("OnSpawn", spawnParameter);
    }

    public void Spawn() {
      Spawn(out _);
    }
  }
}
