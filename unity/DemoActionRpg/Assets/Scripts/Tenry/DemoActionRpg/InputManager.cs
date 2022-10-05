using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tenry.DemoActionRpg {
  public class InputManager : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private InputActionAsset controls;
    #endregion

    private Dictionary<InputActionMap, int> referenceCount = new ();

    private void OnEnable() {
      InputSystem.onDeviceChange += LogDeviceChange;
    }

    private void OnDisable() {
      InputSystem.onDeviceChange -= LogDeviceChange;
    }

    private void Update() {
      for (int deviceIndex = 0; deviceIndex < InputSystem.devices.Count; ++deviceIndex) {
        var device = InputSystem.devices[deviceIndex];

        switch (device.description.deviceClass) {
          case "Keyboard":
          case "Mouse":
            break;
          default: {
            var layout = InputSystem.TryFindMatchingLayout(device.description);

            if (layout != null) {
              DebugText.Instance.Log($"Device #{deviceIndex}: {layout}");
            }

            break;
          }
        }
      }
    }

    private void LogDeviceChange(InputDevice device, InputDeviceChange change) {
      switch (change) {
        case InputDeviceChange.Added:
          Debug.Log($"Device {device} added");
          break;
        case InputDeviceChange.Removed:
          Debug.Log($"Device {device} removed");
          break;
      }

      var layout = InputSystem.TryFindMatchingLayout(device.description);

      Debug.Log($"layout: {layout}");
    }

    public InputActionMap ListenToMap(string mapName) {
      var map = controls.FindActionMap(mapName);

      if (!this.referenceCount.ContainsKey(map)) {
        this.referenceCount.Add(map, 0);
      }

      var refCount = this.referenceCount[map];

      if (refCount == 0) {
        foreach (var action in map.actions) {
          action.Enable();
        }
      }

      this.referenceCount[map] = refCount + 1;

      return map;
    }

    public void UnlistenToMap(string mapName) {
      var map = controls.FindActionMap(mapName);

      if (!this.referenceCount.ContainsKey(map)) {
        this.referenceCount.Add(map, 0);
      }

      var refCount = this.referenceCount[map];

      this.referenceCount[map] = --refCount;

      if (refCount == 0) {
        foreach (var action in map.actions) {
          action.Disable();
        }
      }
    }
  }
}
