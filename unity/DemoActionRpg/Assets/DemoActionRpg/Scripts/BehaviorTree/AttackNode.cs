using System.Threading.Tasks;

using Tenry.BehaviorTree.Runtime;

namespace Tenry.DemoActionRpg.BehaviorTree {
  public class AttackNode : ActionNode {
    private PlayerController playerController;

    private Task task = null;

    protected override void OnStart() {
      playerController = GameObject.GetComponent<PlayerController>();
      task = null;
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (task == null) {
        task = playerController.Attack();
      }

      if (task.IsCompleted) {
        return NodeStatus.Success;
      }

      return NodeStatus.Running;
    }
  }
}
