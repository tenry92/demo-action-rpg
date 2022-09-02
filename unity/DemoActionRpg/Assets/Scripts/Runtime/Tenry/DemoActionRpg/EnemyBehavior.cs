using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

using Tenry.Common.BehaviorTree;

namespace Tenry.DemoActionRpg {
  public class EnemyBehavior : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private GameObject perishEffectPrefab;
    #endregion

    private Damageable damage;

    private Animator animator;

    private PlayerController controller;

    private void Awake() {
      this.damage = this.GetComponent<Damageable>();
      this.animator = this.GetComponent<Animator>();
      this.controller = this.GetComponent<PlayerController>();
    }

    private void Start() {
      
    }

    private void Update() {
      
    }

    private void OnEnable() {
      if (this.damage != null) {
        this.damage.Damaged += this.OnDamage;
        this.damage.Destroyed += this.Perish;
      }
    }

    private void OnDisable() {
      if (this.damage != null) {
        this.damage.Damaged -= this.OnDamage;
        this.damage.Destroyed -= this.Perish;
      }
    }

    public async void Perish() {
      if (this.animator == null) {
        return;
      }

      this.animator.SetInteger("DamageType", 1);
      this.animator.SetTrigger("Damage");

      await Task.Delay(500);
      Destroy(this.gameObject);

      if (this.perishEffectPrefab != null) {
        Instantiate(this.perishEffectPrefab, this.transform.position, Quaternion.identity);
      }
    }

    private void OnDamage(int amount) {
      if (this.damage.IsDead) {
        return;
      }

      if (this.animator == null) {
        return;
      }

      this.animator.SetInteger("DamageType", 0);
      this.animator.SetTrigger("Damage");
    }

    private void AlertObservers() {
      Debug.Log("AlertObservers");
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

    public float GetDistanceToPlayer() {
      if (this.TryGetNearestPlayer(out var player, out var distance)) {
        return distance;
      }

      return Mathf.Infinity;
    }
  }
}
