using System.Collections.Generic;

using UnityEngine;

namespace Tenry.BehaviorTree {
  public class RootNode : Node {
    #region Serialized Fields
    [SerializeField]
    [HideInInspector]
    private Node child;
    #endregion

    public Node Child => this.child;

    public override Node Clone() {
      var copy = Instantiate(this);
      copy.child = this.child.Clone();

      return copy;
    }

    protected override void OnStart() {}

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      return this.Child.Evaluate();
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
      if (this.child != null) {
        yield return this.child;
      }
    }

    public override void SortChildren() {}
  }
}
