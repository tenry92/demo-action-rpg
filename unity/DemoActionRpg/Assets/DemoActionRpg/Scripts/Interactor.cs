using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tenry.DemoActionRpg {
  public class Interactor : MonoBehaviour {
    private readonly List<Interactable> nearbyInteractables = new();

    private Controls controls;

    private void Awake() {
      controls = new();
    }

    private void OnEnable() {
      controls.Game.Enable();
      controls.Game.Interact.performed += Interact;
    }

    private void OnDisable() {
      controls.Game.Disable();
      controls.Game.Interact.performed -= Interact;
    }

    private void Interact(InputAction.CallbackContext context) {
      if (nearbyInteractables.Count > 0) {
        nearbyInteractables[0].Interact();
      }
    }

    private void OnTriggerEnter(Collider other) {
      if (other.TryGetComponent<Interactable>(out var interactable)) {
        nearbyInteractables.Add(interactable);
      }
    }

    private void OnTriggerExit(Collider other) {
      if (other.TryGetComponent<Interactable>(out var interactable)) {
        nearbyInteractables.Remove(interactable);
      }
    }
  }
}