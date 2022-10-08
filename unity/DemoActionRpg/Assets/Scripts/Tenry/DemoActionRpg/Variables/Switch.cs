using System.Collections.Generic;

using UnityEngine;

namespace Tenry.DemoActionRpg.Variables {
  [CreateAssetMenu(menuName = "Demo Action RPG/Variables/Switch", fileName = "Switch")]
  public class Switch : ScriptableObject {
    #region Serialized Fields
    [SerializeField]
    private bool value = false;
    #endregion

    private List<SwitchListener> listeners = new ();

    public bool Value {
      get => value;
      set {
        if (value != this.value) {
          this.value = value;

          for (int i = listeners.Count - 1; i >= 0; --i) {
            listeners[i].OnValueChanged(value);
          }
        }
      }
    }

    public void AddListener(SwitchListener listener) {
      listeners.Add(listener);
    }

    public void RemoveListener(SwitchListener listener) {
      listeners.Remove(listener);
    }
  }
}
