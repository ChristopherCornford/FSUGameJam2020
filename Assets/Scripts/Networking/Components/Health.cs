using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth;

    [SyncVar]
    private int currentHealth;
    
    public bool isDead => currentHealth == 0f;

    public override void OnStartServer() => SetHealth(maxHealth);

    [ServerCallback]

    private void OnDestroy()
    {
       // OnDeath?.Invoke(this, EventArgs.Empty);
    }

    [Server]
    private void SetHealth(int value)
    {
        currentHealth = value;
        //Event_HealthChanged?.Invoke(currentHealth, maxHealth);
    }

    [Server]
    public void Remove(float value)
    {
        value = Mathf.Max(value, 0);

        currentHealth = (int)Mathf.Max(currentHealth - value, 0);
    }

    private void HandleHealthUpdated(float oldValue, float newValue)
    {

    }

    [ClientRpc]
    private void UpdateHealth()
    {

    }

    [ClientRpc]
    private void RpcHandleDeath()
    {
        gameObject.SetActive(false);
    }
}
