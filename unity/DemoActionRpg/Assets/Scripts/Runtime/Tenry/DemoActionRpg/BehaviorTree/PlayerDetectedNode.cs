using Tenry.BehaviorTree;

namespace Tenry.DemoActionRpg.BehaviorTree {
  public class PlayerDetectedNode : ActionNode {
    private PlayerDetector detector;

    protected override void OnStart() {
      this.detector = this.GameObject.GetComponent<PlayerDetector>();
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (this.detector.DetectedPlayer != null) {
        return NodeStatus.Success;
      }

      return NodeStatus.Failure;
    }
  }
}
