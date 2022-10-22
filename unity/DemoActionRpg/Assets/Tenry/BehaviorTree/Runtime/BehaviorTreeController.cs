using UnityEngine;

namespace Tenry.BehaviorTree.Runtime {
  public class BehaviorTreeController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private BehaviorTree tree;
    #endregion

    public BehaviorTree Tree {
      get => tree;
      set => tree = value;
    }

    private void Awake() {
      var original = tree;
      tree = tree.Clone();
      tree.name = original.name;
      tree.Controller = this;
    }

    private void Update() {
      tree.Update();
    }
  }
}
