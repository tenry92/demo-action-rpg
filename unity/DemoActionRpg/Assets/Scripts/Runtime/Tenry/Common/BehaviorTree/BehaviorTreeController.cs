using UnityEngine;

namespace Tenry.Common.BehaviorTree {
  public class BehaviorTreeController : MonoBehaviour {
    [SerializeField]
    private BehaviorTree tree;

    private void Awake() {
      this.tree = this.tree.Clone();
    }

    private void Update() {
      this.tree.Update();
    }
  }
}
