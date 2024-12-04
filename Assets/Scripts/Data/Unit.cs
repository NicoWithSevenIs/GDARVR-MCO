using Mirror;
using UnityEngine;

public class Unit : NetworkBehaviour
{
    [SyncVar]
    private int currentPower;
    public int CurrentPower { get => currentPower; set => currentPower = value; }

    [SyncVar]
    private int currentHealth;
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

    [SyncVar]
    private int highestHealth;
    public int HighestHealth { get => highestHealth; }

    private Character character;
    public Character Character { get => character; }

    private GameObject fieldObject;
    public GameObject FieldObject { get => fieldObject; }

    public void Initialize(Character character, GameObject fieldObject)
    {
        if (!isServer) return;

        this.character = character;
        this.fieldObject = fieldObject;
        currentHealth = character.health;
        currentPower = character.power;
        highestHealth = character.health;
    }

    [Server]
    public void ChangePower(int amount)
    {
        if (!isServer) return;
        currentPower = Mathf.Max(0, currentPower + amount);
    }

    [Server]
    public void ChangeHealth(int amount, bool willKill = true)
    {
        if (!isServer) return;

        currentHealth = Mathf.Max(0, currentHealth + amount);

        if (currentHealth > highestHealth)
            highestHealth = currentHealth;

        if (currentHealth > 0)
            return;

        if (CurrentHealth <= 0)
        {
            if (willKill)
            {
                Debug.Log("Dead");
                if (fieldObject != null)
                    NetworkServer.Destroy(fieldObject);
                NetworkServer.Destroy(gameObject);
            }
            else currentHealth = 1;
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("[SERVER] Unit Spawned");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("[CLIENT] Unit Spawned");
    }
}