using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEvent : MonoBehaviour
{

    public static event System.Action OnEnemyDeath;

    public static void TriggerEnemyDeath()
    {
        OnEnemyDeath?.Invoke();
    }

}
