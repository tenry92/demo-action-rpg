using System.Collections.Generic;

using UnityEngine;

namespace Tenry.BehaviorTree.Runtime {
  public abstract class DecoratorNode : Node {
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

    public override void Abort() {
      if (child != null && child.IsRunning) {
        child.Abort();
      }

      base.Abort();
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
