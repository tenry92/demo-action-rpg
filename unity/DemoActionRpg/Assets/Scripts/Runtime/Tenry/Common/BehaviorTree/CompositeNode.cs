using System.Collections.Generic;

using UnityEngine;

namespace Tenry.Common.BehaviorTree {
  public abstract class CompositeNode : Node {
    #region Serialized Fields
    [SerializeField]
    [HideInInspector]
    private List<Node> children = new List<Node>();
    #endregion

    public List<Node> Children => this.children;

    public override Node Clone() {
      var copy = Instantiate(this);
      copy.children = this.children.ConvertAll(child => child.Clone());

      return copy;
    }

    public override void AddChild(Node child) {
      // todo: disallow adding a (distant) parent to be added as a child
      // (or disconnect from parent otherwise?)

      if (this.children.Contains(child)) {
        this.children.Remove(child);
      }

      this.children.Add(child);
    }

    public override void RemoveChild(Node child) {
      this.children.Remove(child);
    }

    public override IEnumerable<Node> GetChildren() {
      return this.children.AsReadOnly();
    }
  }
}
