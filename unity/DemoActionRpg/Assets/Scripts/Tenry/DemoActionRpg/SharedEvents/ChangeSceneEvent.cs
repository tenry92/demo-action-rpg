using System.Collections.Generic;

using UnityEngine;

namespace Tenry.DemoActionRpg.SharedEvents {
  [CreateAssetMenu(menuName = "Demo Action RPG/Events/ChangeSceneEvent", fileName = "ChangeSceneEvent")]
  public class ChangeSceneEvent : ScriptableObject {
    private List<ChangeSceneEventListener> listeners = new ();

    public void Trigger(string sceneName) {
      for (int i = listeners.Count - 1; i >= 0; --i) {
        listeners[i].OnEventTriggered(sceneName);
      }
    }

    public void AddListener(ChangeSceneEventListener listener) {
      listeners.Add(listener);
    }

    public void RemoveListener(ChangeSceneEventListener listener) {
      listeners.Remove(listener);
    }
  }
}
