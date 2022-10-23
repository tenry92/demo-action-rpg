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

    [SerializeField]
    private float damageCooldown = 0f;
    #endregion

    private int health;

    public int Health {
      get {
        return health;
      }
      set {
        if (health == 0 && value == 0) {
          return;
        }

        health = Mathf.Clamp(value, 0, maxHealth);
        Changed?.Invoke();

        if (health == 0) {
          Destroyed?.Invoke();

          if (destroyOnDeath) {
            gameObject.SetActive(false);
          }
        }
      }
    }

    public int MaxHealth => maxHealth;

    private float activeDamageCooldown = 0f;

    public bool IsAlive => health > 0;

    public bool IsDead => !IsAlive;

    public UnityEvent Changed;

    public UnityEvent<int> Damaged;

    public UnityEvent Destroyed;

    private void Awake() {
      health = maxHealth;
    }

    private void Update() {
      activeDamageCooldown = Mathf.Clamp(activeDamageCooldown - Time.deltaTime, 0f, activeDamageCooldown);
    }

    // private void OnEnable() {
    //   Damaged.AddListener(SpawnDamageText);
    // }

    // private void OnDisable() {
    //   Damaged.RemoveListener(SpawnDamageText);
    // }

    public bool CanTakeDamageFrom(DamageType type) {
      if (damagedBy == null || activeDamageCooldown > 0f) {
        return false;
      }

      return damagedBy.Contains(type);
    }

    public void Damage(int amount) {
      if (IsDead) {
        Debug.Log("Is already dead");
        return;
      }

      amount = Mathf.Min(Health, amount);

      if (amount > 0) {
        if (hurtAudioSource != null) {
          hurtAudioSource.Play();
        }

        Health -= amount;
        activeDamageCooldown = damageCooldown;
        Damaged?.Invoke(amount);
      }

      // if (damageEffectPrefab != null) {
      //   Instantiate(damageEffectPrefab, damageTextSpawnPoint?.position ?? transform.position, Quaternion.identity);
      // }
    }

    // private void SpawnDamageText(int damage) {
    //   if (damageTextPrefab == null) {
    //     return;
    //   }

    //   var damageText = Instantiate(damageTextPrefab, damageTextSpawnPoint?.position ?? transform.position, Quaternion.identity);
    //   damageText.Text = $"-{damage}";
    // }
  }
}
