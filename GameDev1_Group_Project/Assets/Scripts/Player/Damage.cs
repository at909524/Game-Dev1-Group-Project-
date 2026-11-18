
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private Player player;
    private Health health;

    [Header("Knockback Settings")]
    public float knockbackForce = 20;
    public float knockbackDuration = .2f;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (health == null)
        {
            Debug.LogError("Health component is missing!", this);
            return;
        }

        health.OnDamaged += HandleDamage;
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        if (health == null)
            return;

        health.OnDamaged -= HandleDamage;
        health.OnDeath -= HandleDeath;
    }

    private void HandleDamage(Vector2 sourcePosition)
    {
        if (player == null || player.damagedState == null)
        {
            Debug.LogError("Player or Damaged State is missing!", this);
            return;
        }

        int knockbackDir = transform.position.x > sourcePosition.x ? 1 : -1;

        player.damagedState.SetParameters(knockbackDir);
        player.ChangeState(player.damagedState);
    }

    private void HandleDeath()
    {

    }
}
