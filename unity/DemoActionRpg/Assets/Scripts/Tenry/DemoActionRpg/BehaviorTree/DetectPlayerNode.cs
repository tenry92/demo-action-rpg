using UnityEngine;

using Tenry.BehaviorTree.Runtime;

namespace Tenry.DemoActionRpg.BehaviorTree {
  public class DetectPlayerNode : ActionNode {
    #region Serialized Fields
    [SerializeField]
    private float range = 1f;
    #endregion

    protected override void OnStart() {}

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (this.TryGetNearestPlayer(out var player, out var distance) && distance <= this.range) {
        this.Blackboard.Set("SeekTarget", player);
        return NodeStatus.Success;
      }

      return NodeStatus.Failure;
    }

    private bool TryGetNearestPlayer(out GameObject player, out float distance) {
      player = null;
      distance = Mathf.Infinity;

      var players = GameObject.FindGameObjectsWithTag("Player");

      if (players.Length == 0) {
        return false;
      }

      for (int i = 0; i < players.Length; ++i) {
        var distanceToPlayer = Vector3.Distance(this.GameObject.transform.position, players[i].transform.position);

        if (distanceToPlayer < distance) {
          player = players[i];
          distance = distanceToPlayer;
        }
      }

      return true;
    }
  }
}
