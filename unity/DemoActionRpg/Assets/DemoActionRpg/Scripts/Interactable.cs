using Tenry.BehaviorTree.Runtime;

using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class Interactable : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private Tenry.BehaviorTree.Runtime.BehaviorTree behavior;
    #endregion

    private BehaviorTreeController controller;

    private void Awake() {
      controller = gameObject.AddComponent<BehaviorTreeController>();
      controller.Tree = behavior;
      controller.AutoRun = false;
    }

    public void Interact() {
      controller.Run();
    }
  }
}