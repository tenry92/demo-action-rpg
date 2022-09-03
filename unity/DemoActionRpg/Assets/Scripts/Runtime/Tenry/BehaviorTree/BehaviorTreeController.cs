using UnityEngine;

namespace Tenry.BehaviorTree {
  public class BehaviorTreeController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private BehaviorTree tree;
    #endregion

    public BehaviorTree Tree {
      get => this.tree;
      set => this.tree = value;
    }

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
