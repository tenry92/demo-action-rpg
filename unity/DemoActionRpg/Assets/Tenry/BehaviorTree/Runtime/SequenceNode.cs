using System.Collections.Generic;

namespace Tenry.BehaviorTree.Runtime {
  public class SequenceNode : CompositeNode {
    private int currentBranchIndex;

    private Node CurrentBranchNode => this.Children[this.currentBranchIndex];

    private bool HasReachedEnd => this.currentBranchIndex >= this.Children.Count;

    protected override void OnStart() {
      this.currentBranchIndex = 0;
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (this.CheckAbort(this.currentBranchIndex - 1, out int branchIndex)) {
        this.CurrentBranchNode.Abort();
        var abortingNode = this.Children[branchIndex];

        switch (abortingNode.Status) {
          case NodeStatus.Running:
            this.currentBranchIndex = branchIndex;
            return NodeStatus.Running;
          case NodeStatus.Failure:
            this.currentBranchIndex = branchIndex;
            return NodeStatus.Failure;
          case NodeStatus.Success:
            this.currentBranchIndex = branchIndex + 1;
            break;
        }
      }

      while (!this.HasReachedEnd) {
        switch (this.CurrentBranchNode.Evaluate()) {
          case NodeStatus.Running:
            return NodeStatus.Running;
          case NodeStatus.Failure:
            return NodeStatus.Failure;
          case NodeStatus.Success:
            ++this.currentBranchIndex;
            continue;
        }
      }

      return NodeStatus.Success;
    }
  }
}
