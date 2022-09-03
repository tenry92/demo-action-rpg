using System.Collections.Generic;

using UnityEngine;

namespace Tenry.BehaviorTree {
  public abstract class DecoratorNode : Node {
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
