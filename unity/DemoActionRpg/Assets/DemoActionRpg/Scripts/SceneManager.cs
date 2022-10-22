using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Tenry.DemoActionRpg {
  public class SceneManager : MonoBehaviour {
    private void OnEnable() {
      var eventListener = GetComponent<SharedEvents.StringEventListener>();

      if (eventListener) {
        eventListener.EventTriggered.AddListener(TransitionToScene);
      }

      UnitySceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
      var eventListener = GetComponent<SharedEvents.StringEventListener>();

      if (eventListener) {
        eventListener.EventTriggered.RemoveListener(TransitionToScene);
      }

      UnitySceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void TransitionToScene(string sceneName) {
      // todo: show loading screen
      var activeLoadingOperation = UnitySceneManager.LoadSceneAsync(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      // todo: hide loading screen
    }
  }
}
