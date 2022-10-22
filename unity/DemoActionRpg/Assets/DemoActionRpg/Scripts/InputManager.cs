using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tenry.DemoActionRpg {
  public class InputManager : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private InputActionAsset controls;
    #endregion

    private readonly Dictionary<InputActionMap, int> referenceCount = new();

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
                DebugText.Instance?.Log($"Device #{deviceIndex}: {layout}");
              }

              break;
            }
        }
      }
    }

    public InputActionMap ListenToMap(string mapName) {
      var map = controls.FindActionMap(mapName);

      if (!referenceCount.ContainsKey(map)) {
        referenceCount.Add(map, 0);
      }

      var refCount = referenceCount[map];

      if (refCount == 0) {
        foreach (var action in map.actions) {
          action.Enable();
        }
      }

      referenceCount[map] = refCount + 1;

      return map;
    }

    public void UnlistenToMap(string mapName) {
      var map = controls.FindActionMap(mapName);

      if (!referenceCount.ContainsKey(map)) {
        referenceCount.Add(map, 0);
      }

      var refCount = referenceCount[map];

      referenceCount[map] = --refCount;

      if (refCount == 0) {
        foreach (var action in map.actions) {
          action.Disable();
        }
      }
    }
  }
}
