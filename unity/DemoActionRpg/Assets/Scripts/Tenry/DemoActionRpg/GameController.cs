using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }

    public InputManager InputManager { get; private set; }

    private void Awake() {
      Debug.Log("GameController::Awake()");
      if (Instance != null && Instance != this) {
        Debug.Log("GameController.Instance already set");
        Destroy(this);
      }

      Instance = this;
      InputManager = GetComponentInChildren<InputManager>();
      Debug.Log(InputManager);
    }
  }
}
