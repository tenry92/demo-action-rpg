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
      if (once && triggered) {
        return;
      }

      if (filterTag != "" && filterTag != "Untagged") {
        if (other.tag != filterTag) {
          return;
        }
      }

      Triggered?.Invoke();

      triggered = true;
    }
  }
}
