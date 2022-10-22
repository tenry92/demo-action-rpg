using UnityEngine;
using UnityEngine.Events;

namespace Tenry.DemoActionRpg.SharedEvents {
  public class VoidEventListener : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private VoidEvent voidEvent;

    [SerializeField]
    private UnityEvent eventTriggered;
    #endregion

    public UnityEvent EventTriggered => eventTriggered;

    private void OnEnable() {
      voidEvent.AddListener(this);
    }

    private void OnDisable() {
      voidEvent.RemoveListener(this);
    }

    public void OnEventTriggered() {
      eventTriggered.Invoke();
    }
  }
}
