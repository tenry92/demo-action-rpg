using UnityEngine;

namespace Tenry.Common.BehaviorTree {
  public class BehaviorTreeController : MonoBehaviour {
    [SerializeField]
    private BehaviorTree tree;

    private void Awake() {
      var original = this.tree;
      this.tree = this.tree.Clone();
      this.tree.name = original.name;
      this.tree.Controller = this;
    }

    private void Update() {
      this.tree.Update();
    }
  }
}
