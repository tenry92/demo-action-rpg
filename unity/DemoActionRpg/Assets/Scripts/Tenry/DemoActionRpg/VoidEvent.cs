using System.Collections.Generic;

using UnityEngine;

namespace Tenry.DemoActionRpg {
  [CreateAssetMenu(menuName = "Demo Action RPG/Events/VoidEvent", fileName = "Void Event")]
  public class VoidEvent : ScriptableObject {
    private List<VoidEventListener> listeners = new ();

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
