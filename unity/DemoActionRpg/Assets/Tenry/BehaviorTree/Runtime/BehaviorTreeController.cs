using UnityEngine;

namespace Tenry.BehaviorTree.Runtime {
  public class BehaviorTreeController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private BehaviorTree tree;

    [SerializeField]
    private bool autoRun = true;
    #endregion

    private bool running;

    public bool AutoRun {
      get => autoRun;
      set => autoRun = value;
    }

    public BehaviorTree Tree {
      get => tree;
      set => tree = value;
    }

    private void Start() {
      var original = tree;
      tree = tree.Clone();
      tree.name = original.name;
      tree.Controller = this;
      running = autoRun;
    }

    private void Update() {
      if (running) {
        switch (tree.Update()) {
          case NodeStatus.Success:
          case NodeStatus.Failure:
            running = false;
            break;
        }
      }
    }

    public void Run() {
      tree.Restart();
      running = true;
    }
  }
}
