using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    START,
    COLLISION
}

[Serializable]
public class TutorialEvent
{
    public EventType Type;
    public Collider2D ColliderA;
    public Collider2D ColliderB;
    public bool Triggered = false;
    [TextArea]
    public string Message;
}
