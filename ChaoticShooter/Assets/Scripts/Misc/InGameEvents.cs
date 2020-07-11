using Core;
using UnityEngine;

public class PlayerCreatedEvent : GameEvent
{
    public Transform transform;

    public PlayerCreatedEvent(Transform transform)
    {
        this.transform = transform;
    }
}

public class PlayerDestroyedEvent : GameEvent
{
    public Transform transform;

    public PlayerDestroyedEvent(Transform transform)
    {
        this.transform = transform;
    }
}

public class DamageEnemyEvent : GameEvent
{
    public GameObject targetEnemy;
    public float damageDealt;

    public DamageEnemyEvent(GameObject targetEnemy, float damageDealt)
    {
        this.targetEnemy = targetEnemy;
        this.damageDealt = damageDealt;
    }
}

public class GameStateSetEvent: GameEvent
{
    public GameState gameState;

    public GameStateSetEvent(GameState gameState)
    {
        this.gameState = gameState;
    }
}

public class GameStateCompletedEvent : GameEvent
{
    public GameState gameState;

    public GameStateCompletedEvent(GameState gameState)
    {
        this.gameState = gameState;
    }
}