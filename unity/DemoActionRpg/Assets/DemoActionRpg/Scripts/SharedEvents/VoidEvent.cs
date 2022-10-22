using System.Collections.Generic;

using UnityEngine;

namespace Tenry.DemoActionRpg.SharedEvents {
  [CreateAssetMenu(menuName = "Demo Action RPG/Events/Void Event", fileName = "VoidEvent")]
  public class VoidEvent : ScriptableObject {
    private readonly List<VoidEventListener> listeners = new ();

    public void Trigger() {
      for (int i = listeners.Count - 1; i >= 0; --i) {
        listeners[i].OnEventTriggered();
      }
    }

    public void AddListener(VoidEventListener listener) {
      listeners.Add(listener);
    }

    public void RemoveListener(VoidEventListener listener) {
      listeners.Remove(listener);
    }
  }
}
