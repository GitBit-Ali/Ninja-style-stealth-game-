using UnityEngine;
using System;

public class Player : Singleton<Player>
{
    public static event Action OnEndOfLevelReached;

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (!collision.CompareTag("Finish")) return;

        OnEndOfLevelReached?.Invoke();
    }
}
