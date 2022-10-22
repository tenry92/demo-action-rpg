using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Tenry.BehaviorTree.Runtime {
  public abstract class CompositeNode : Node {
    #region Serialized Fields
    [SerializeField]
    [HideInInspector]
    private List<Node> children = new List<Node>();

    [SerializeField]
    private AbortType abortType = AbortType.None;
    #endregion

    public List<Node> Children => this.children;

    public AbortType AbortType {
      get => this.abortType;
      set => this.abortType = value;
    }

    public override Node Clone() {
      var copy = Instantiate(this);
      copy.children = this.children.ConvertAll(child => child?.Clone());

      return copy;
    }

    public override void Abort() {
      foreach (var child in this.children) {
        if (child.IsRunning) {
          child.Abort();
        }
      }

      base.Abort();
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
      return this.children.Where(child => child != null);
    }

    public override void SortChildren() {
      // remove nulls
      this.children = this.children.Where(child => child != null).ToList<Node>();

      this.children.Sort((a, b) => a.Position.x < b.Position.x ? -1 : 1);
    }

    protected bool CheckAbort(int lastBranchIndex, out int abortingBranchIndex) {
      abortingBranchIndex = -1;
      
      if (this.AbortType.HasFlag(AbortType.Self)) {
        for (abortingBranchIndex = 0; abortingBranchIndex <= lastBranchIndex; ++abortingBranchIndex) {
          var previousBranchNode = this.Children[abortingBranchIndex];

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
