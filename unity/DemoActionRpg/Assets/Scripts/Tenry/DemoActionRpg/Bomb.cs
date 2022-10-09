using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class Bomb : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private float explodeInSeconds = 3f;

    [SerializeField]
    private GameObject explosionPrefab;
    #endregion

    private void Start() {
      Invoke("Explode", explodeInSeconds);
    }

    public void Explode() {
      Destroy(this.gameObject);
      Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
  }
}
