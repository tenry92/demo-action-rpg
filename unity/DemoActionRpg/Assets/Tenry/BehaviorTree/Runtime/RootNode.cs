using System.Collections.Generic;

using UnityEngine;

namespace Tenry.BehaviorTree.Runtime {
  public class RootNode : Node {
    #region Serialized Fields
    [SerializeField]
    [HideInInspector]
    private Node child;
    #endregion

    public Node Child => child;

    public override Node Clone() {
      var copy = Instantiate(this);
      copy.child = child.Clone();

      return copy;
    }

    protected override void OnStart() {}

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      return Child.Evaluate();
    }

    public override void AddChild(Node child) {
      this.child = child;
    }

    public override void RemoveChild(Node child) {
      if (this.child == child) {
        this.child = null;
      }
    }

    public override IEnumerable<Node> GetChildren() {
      if (child != null) {
        yield return child;
      }
    }

    public override void SortChildren() {}
  }
}
