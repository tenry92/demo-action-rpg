using System;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

namespace Tenry.DemoActionRpg {
  public class Damageable : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private FloatingText damageTextPrefab;

    [SerializeField]
    private Transform damageTextSpawnPoint;

    [SerializeField]
    private GameObject damageEffectPrefab;

    [SerializeField]
    private DamageType[] damagedBy;

    [SerializeField]
    private bool destroyOnDeath = false;

    [SerializeField]
    private AudioSource hurtAudioSource;
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

          if (this.destroyOnDeath) {
            this.gameObject.SetActive(false);
          }
        }
      }
    }

    public int MaxHealth => this.maxHealth;

    private float activeDamageCooldown = 0f;

    public bool IsAlive => this.health > 0;

    public bool IsDead => !this.IsAlive;

    public UnityEvent<int> Damaged;

    public UnityEvent Destroyed;

    private void Awake() {
      this.health = this.maxHealth;
    }

    private void Update() {
      this.activeDamageCooldown = Mathf.Clamp(this.activeDamageCooldown - Time.deltaTime, 0f, this.activeDamageCooldown);
    }

    // private void OnEnable() {
    //   this.Damaged.AddListener(this.SpawnDamageText);
    // }

    // private void OnDisable() {
    //   this.Damaged.RemoveListener(this.SpawnDamageText);
    // }

    public bool CanTakeDamageFrom(DamageType type) {
      if (damagedBy == null) {
        return false;
      }

      return damagedBy.Contains(type);
    }

    public void Damage(int amount) {
      if (this.IsDead) {
        Debug.Log("Is already dead");
        return;
      }

      amount = Mathf.Min(this.Health, amount);

      if (amount > 0) {
        if (this.hurtAudioSource != null) {
          this.hurtAudioSource.Play();
        }

        this.Health -= amount;
        this.Damaged?.Invoke(amount);
      }

      // if (this.damageEffectPrefab != null) {
      //   Instantiate(this.damageEffectPrefab, this.damageTextSpawnPoint?.position ?? this.transform.position, Quaternion.identity);
      // }
    }

    // private void SpawnDamageText(int damage) {
    //   if (this.damageTextPrefab == null) {
    //     return;
    //   }

    //   var damageText = Instantiate(this.damageTextPrefab, this.damageTextSpawnPoint?.position ?? this.transform.position, Quaternion.identity);
    //   damageText.Text = $"-{damage}";
    // }
  }
}
