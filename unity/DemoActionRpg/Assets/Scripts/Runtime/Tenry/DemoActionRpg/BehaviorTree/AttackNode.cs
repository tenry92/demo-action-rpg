using System.Threading.Tasks;

using Tenry.BehaviorTree;

namespace Tenry.DemoActionRpg.BehaviorTree {
  public class AttackNode : ActionNode {
    private PlayerController controller;

    private Task task = null;

    protected override void OnStart() {
      this.controller = this.GameObject.GetComponent<PlayerController>();
      this.task = null;
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (this.task == null) {
        this.task = this.controller.Attack();
      }

      if (this.task.IsCompleted) {
        return NodeStatus.Success;
      }

      return NodeStatus.Running;
    }
  }
}
