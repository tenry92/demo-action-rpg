using System.Threading.Tasks;

using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class EnemyBehavior : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private GameObject perishEffectPrefab;
    #endregion

    private Damageable damage;

    private Animator animator;

    private void Awake() {
      this.damage = this.GetComponent<Damageable>();
      this.animator = this.GetComponent<Animator>();
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
  }
}
