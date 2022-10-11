using UnityEngine;
using UnityEngine.Events;

namespace Tenry.DemoActionRpg.SharedEvents {
  public class StringEventListener : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private StringEvent stringEvent;

    [SerializeField]
    private UnityEvent<string> eventTriggered;
    #endregion

    public UnityEvent<string> EventTriggered => eventTriggered;

    private void OnEnable() {
      stringEvent.AddListener(this);
    }

    private void OnDisable() {
      stringEvent.RemoveListener(this);
    }

    public void OnEventTriggered(string sceneName) {
      eventTriggered.Invoke(sceneName);
    }
  }
}
