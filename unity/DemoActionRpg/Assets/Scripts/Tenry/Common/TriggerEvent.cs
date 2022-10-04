using UnityEngine;
using UnityEngine.Events;

using Tenry.Common;

namespace Tenry.Common {
  public class TriggerEvent : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private UnityEvent Triggered;

    [SerializeField]
    [TagSelector]
    private string filterTag;

    [SerializeField]
    private bool once = false;
    #endregion

    private bool triggered = false;

    private void OnTriggerEnter(Collider other) {
      if (this.once && this.triggered) {
        return;
      }

      Debug.Log($"filterTag: »{this.filterTag}«");

      if (this.filterTag != "" && this.filterTag != "Untagged") {
        if (other.tag != this.filterTag) {
          return;
        }
      }

      this.Triggered?.Invoke();

      this.triggered = true;
    }
  }
}
