namespace Tenry.BehaviorTree {
  public class SelectorNode : CompositeNode {
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
            this.currentBranchIndex = branchIndex + 1;
            break;
          case NodeStatus.Success:
            this.currentBranchIndex = branchIndex;
            return NodeStatus.Success;
        }
      }

      while (!this.HasReachedEnd) {
        switch (this.CurrentBranchNode.Evaluate()) {
          case NodeStatus.Running:
            return NodeStatus.Running;
          case NodeStatus.Failure:
            ++this.currentBranchIndex;
            continue;
          case NodeStatus.Success:
            return NodeStatus.Success;
        }
      }

      return NodeStatus.Failure;
    }
  }
}
