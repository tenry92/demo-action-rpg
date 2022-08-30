using System;

using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class Damageable : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private float damageCooldown = 1f;

    [SerializeField]
    private FloatingText damageTextPrefab;

    [SerializeField]
    private Transform damageTextSpawnPoint;

    [SerializeField]
    private GameObject damageEffectPrefab;
    #endregion

    private int health;

    public int Health {
      get {
        return this.health;
      }
      set {
        if (this.health == 0 && value == 0) {
          return;
        }

        this.health = Mathf.Clamp(value, 0, maxHealth);

        if (this.health == 0) {
          this.Destroyed?.Invoke();
        }
      }
    }

    private float activeDamageCooldown = 0f;

    public bool IsCooldownActive => this.activeDamageCooldown > 0f;

    public bool IsAlive => this.health > 0;

    public bool IsDead => !this.IsAlive;

    public event Action<int> Damaged;

    public event Action Destroyed;

    private void Awake() {
      this.health = this.maxHealth;
    }

    private void Update() {
      this.activeDamageCooldown = Mathf.Clamp(this.activeDamageCooldown - Time.deltaTime, 0f, this.activeDamageCooldown);
    }

    private void OnEnable() {
      this.Damaged += this.SpawnDamageText;
    }

    private void OnDisable() {
      this.Damaged -= this.SpawnDamageText;
    }

    private void OnTriggerEnter(Collider other) {
      if (other.tag == "Weapon") {
        var weapon = other.GetComponent<Weapon>();
        Debug.Assert(weapon != null);

        if (weapon.WeaponActive) {
          this.Damage(1);
        } else {
          Debug.Log("Weapon is not active");
        }
      }
    }

    private void Damage(int amount) {
      if (this.IsDead) {
        Debug.Log("Is already dead");
        return;
      }

      if (this.IsCooldownActive) {
        Debug.Log("Cooldown active");
        return;
      }

      amount = Mathf.Min(this.Health, amount);

      this.Health -= amount;
      this.SetCooldown();
      this.Damaged?.Invoke(amount);

      if (this.damageEffectPrefab != null) {
        Instantiate(this.damageEffectPrefab, damageTextSpawnPoint?.position ?? this.transform.position, Quaternion.identity);
      }
    }

    private void SetCooldown() {
      this.activeDamageCooldown = this.damageCooldown;
    }

    private void SpawnDamageText(int damage) {
      if (this.damageTextPrefab == null) {
        return;
      }

      var damageText = Instantiate(this.damageTextPrefab, damageTextSpawnPoint?.position ?? this.transform.position, Quaternion.identity);
      damageText.Text = $"-{damage}";
    }
  }
}
