using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Tenry.BehaviorTree.Runtime {
  public abstract class CompositeNode : Node {
    #region Serialized Fields
    [SerializeField]
    [HideInInspector]
    private List<Node> children = new();

    [SerializeField]
    private AbortType abortType = AbortType.None;
    #endregion

    public List<Node> Children => children;

    public AbortType AbortType {
      get => abortType;
      set => abortType = value;
    }

    public override Node Clone() {
      var copy = Instantiate(this);
      copy.children = children.ConvertAll(child => child?.Clone());

      return copy;
    }

    public override void Abort() {
      foreach (var child in children) {
        if (child.IsRunning) {
          child.Abort();
        }
      }

      base.Abort();
    }

    public override void AddChild(Node child) {
      // todo: disallow adding a (distant) parent to be added as a child
      // (or disconnect from parent otherwise?)

      if (children.Contains(child)) {
        children.Remove(child);
      }

      children.Add(child);
    }

    public override void RemoveChild(Node child) {
      children.Remove(child);
    }

    public override IEnumerable<Node> GetChildren() {
      return children.Where(child => child != null);
    }

    public override void SortChildren() {
      // remove nulls
      children = children.Where(child => child != null).ToList();

      children.Sort((a, b) => a.Position.x < b.Position.x ? -1 : 1);
    }

    protected bool CheckAbort(int lastBranchIndex, out int abortingBranchIndex) {
      abortingBranchIndex = -1;

      if (AbortType.HasFlag(AbortType.Self)) {
        for (abortingBranchIndex = 0; abortingBranchIndex <= lastBranchIndex; ++abortingBranchIndex) {
          var previousBranchNode = Children[abortingBranchIndex];

          var previousStatus = previousBranchNode.Status;
          var newStatus = previousBranchNode.Evaluate();

          if (newStatus != previousStatus) {
            return true;
          }
        }
      }

      return false;
    }
  }
}
