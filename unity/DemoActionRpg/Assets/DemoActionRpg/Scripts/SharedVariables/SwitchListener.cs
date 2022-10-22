using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Tenry.DemoActionRpg.SharedVariables {
  public class SwitchListener : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private Switch variable;

    [SerializeField]
    private UnityEvent<bool> changed;

    [SerializeField]
    private UnityEvent switchEnabled;

    [SerializeField]
    private UnityEvent switchDisabled;
    #endregion

    public UnityEvent<bool> Changed => changed;

    public UnityEvent SwitchEnabled => switchEnabled;

    public UnityEvent SwitchDisabled => switchDisabled;

    private void OnEnable() {
      variable.AddListener(this);
    }

    private void OnDisable() {
      variable.RemoveListener(this);
    }

    public void OnValueChanged(bool value) {
      changed.Invoke(value);

      if (value) {
        switchEnabled.Invoke();
      } else {
        switchDisabled.Invoke();
      }
    }
  }
}
