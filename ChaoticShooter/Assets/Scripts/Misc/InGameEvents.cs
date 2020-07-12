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
    public Vector3 direction;

    public DamageEnemyEvent(GameObject targetEnemy, float damageDealt, Vector3 direction)
    {
        this.targetEnemy = targetEnemy;
        this.damageDealt = damageDealt;
        this.direction = direction;
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
    public object param;

    public GameStateCompletedEvent(GameState gameState, object param = null)
    {
        this.gameState = gameState;
        this.param = param;
    }
}

public class ShakeCameraEvent : GameEvent
{

}

public class StartGameEvent : GameEvent
{

}

public class RestartGameEvent : GameEvent
{

}

public class UpgradeGranted : GameEvent
{
    public PlayerBehaviourType behaviourType;

    public UpgradeGranted(PlayerBehaviourType behaviourType)
    {
        this.behaviourType = behaviourType;
    }
}

public class EnemyKilledEvent : GameEvent
{

}