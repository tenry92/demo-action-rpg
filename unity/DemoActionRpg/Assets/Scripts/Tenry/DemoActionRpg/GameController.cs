using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }

    public InputManager InputManager { get; private set; }

    private void Awake() {
      if (Instance != null && Instance != this) {
        Debug.Log("GameController.Instance already set");
        Destroy(this);
      }

      DontDestroyOnLoad(this.gameObject);

      Instance = this;
      InputManager = GetComponentInChildren<InputManager>();
    }

    public void ExitGame() {
      #if UNITY_EDITOR
        if (Application.isEditor) {
          UnityEditor.EditorApplication.isPlaying = false;
        } else {
          Application.Quit();
        }
      #else
        Application.Quit();
      #endif
    }
  }
}
