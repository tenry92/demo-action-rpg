using UnityEngine;
using UnityEngine.Pool;

namespace Tenry.DemoActionRpg.Pool {
  [CreateAssetMenu(menuName = "Demo Action RPG/Object Pool Link", fileName = "ObjectPool")]
  public class ObjectPoolLink : ScriptableObject {
    public enum PoolType {
      Stack,
      LinkedList,
    }

    #region Serialized Fields
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private PoolType poolType;

    // only for Stack type
    [SerializeField]
    private int defaultCapacity = 10;

    [SerializeField]
    private int maxSize = 10_000;
    #endregion

    private ObjectPoolController poolController;

    public IObjectPool<GameObject> Pool {
      get {
        if (poolController == null) {
          CreatePoolController();
        }

        return poolController.Pool;
      }
    }

    public GameObject Prefab;

    public PoolType Type => poolType;

    public int DefaultCapacity => defaultCapacity;

    public int MaxSize => maxSize;

    private void CreatePoolController() {
      var go = new GameObject("ObjectPool");
      poolController = go.AddComponent<ObjectPoolController>();
      poolController.Link = this;
    }
  }
}
