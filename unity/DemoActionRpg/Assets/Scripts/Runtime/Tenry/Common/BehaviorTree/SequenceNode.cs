namespace Tenry.Common.BehaviorTree {
  public class SequenceNode : CompositeNode {
    private int currentBranchIndex;

    private Node CurrentBranchNode => this.Children[this.currentBranchIndex];

    private bool HasReachedEnd => this.currentBranchIndex >= this.Children.Count;

    protected override void OnStart() {
      this.currentBranchIndex = 0;
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
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
