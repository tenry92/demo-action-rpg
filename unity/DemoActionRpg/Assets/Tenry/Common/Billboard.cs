using UnityEngine;

namespace Tenry.Common {
  public class Billboard : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private Transform alignTo;
    #endregion

    private void LateUpdate() {
      var alignTo = this.alignTo;

      if (alignTo == null) {
        alignTo = Camera.main.transform;
      }

      if (alignTo == null) {
        return;
      }

      transform.rotation = alignTo.rotation;
    }
  }
}
