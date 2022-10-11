using UnityEngine;
using UnityEngine.Events;

namespace Tenry.DemoActionRpg.SharedEvents {
  public class ChangeSceneEventListener : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private ChangeSceneEvent changeSceneEvent;

    [SerializeField]
    private UnityEvent<string> eventTriggered;
    #endregion

    public UnityEvent<string> EventTriggered => eventTriggered;

    private void OnEnable() {
      changeSceneEvent.AddListener(this);
    }

    private void OnDisable() {
      changeSceneEvent.RemoveListener(this);
    }

    public void OnEventTriggered(string sceneName) {
      eventTriggered.Invoke(sceneName);
    }
  }
}
