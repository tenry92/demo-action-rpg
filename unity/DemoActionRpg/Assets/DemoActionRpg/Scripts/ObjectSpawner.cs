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

#if UNITY_EDITOR
    private void OnDrawGizmos() {
      Mesh mesh = null;
      Transform meshTransform = null;

      if (usePool && pool != null && pool.Prefab != null) {
        var meshFilter = pool.Prefab.GetComponentInChildren<MeshFilter>();
        if (meshFilter != null) {
          mesh = meshFilter.sharedMesh;
          meshTransform = meshFilter.transform;
        }
      }

      if (!usePool && prefab != null) {
        var meshFilter = prefab.GetComponentInChildren<MeshFilter>();
        if (meshFilter != null) {
          mesh = meshFilter.sharedMesh;
          meshTransform = meshFilter.transform;
        }
      }

      if (mesh != null) {
        Gizmos.color = new Color(0f, 0.5f, 1f);
        var rotation = Matrix4x4.Rotate(meshTransform.rotation);
        var scale = Matrix4x4.Scale(meshTransform.lossyScale);
        Gizmos.matrix = transform.localToWorldMatrix * rotation * scale;

        Gizmos.DrawWireMesh(mesh);
      }
    }
#endif
  }
}
