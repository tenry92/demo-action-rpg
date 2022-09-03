using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class PlayerDetector : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    [Min(0.1f)]
    private float detectionRange = 1f;

    [SerializeField]
    [Min(0.1f)]
    private float maxDistance = 2f;
    #endregion

    public GameObject DetectedPlayer { get; set; }

    private void Update() {
      if (this.DetectedPlayer != null) {
        var currentDistance = Vector3.Distance(this.transform.position, this.DetectedPlayer.transform.position);

        if (currentDistance > this.maxDistance) {
          this.DetectedPlayer = null;
        }
      }

      if (this.TryGetNearestPlayer(out var player, out var distance) && distance <= this.detectionRange) {
        this.DetectedPlayer = player;
      }
    }

    public bool TryGetNearestPlayer(out GameObject player, out float distance) {
      player = null;
      distance = Mathf.Infinity;

      var players = GameObject.FindGameObjectsWithTag("Player");

      if (players.Length == 0) {
        return false;
      }

      for (int i = 0; i < players.Length; ++i) {
        var distanceToPlayer = Vector3.Distance(this.transform.position, players[i].transform.position);

        if (distanceToPlayer < distance) {
          player = players[i];
          distance = distanceToPlayer;
        }
      }

      return true;
    }
  }
}
