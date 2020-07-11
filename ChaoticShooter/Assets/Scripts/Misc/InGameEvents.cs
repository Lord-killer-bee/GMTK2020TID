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
