using System.Collections.Generic;

using UnityEngine;

namespace Tenry.DemoActionRpg.SharedEvents {
  [CreateAssetMenu(menuName = "Demo Action RPG/Events/String Event", fileName = "StringEvent")]
  public class StringEvent : ScriptableObject {
    private readonly List<StringEventListener> listeners = new ();

    public void Trigger(string sceneName) {
      for (int i = listeners.Count - 1; i >= 0; --i) {
        listeners[i].OnEventTriggered(sceneName);
      }
    }

    public void AddListener(StringEventListener listener) {
      listeners.Add(listener);
    }

    public void RemoveListener(StringEventListener listener) {
      listeners.Remove(listener);
    }
  }
}
