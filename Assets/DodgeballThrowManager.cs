using System.Collections.Generic;
using UnityEngine;

public class DodgeballThrowManager : MonoBehaviour
{
    public static DodgeballThrowManager Instance;

    private Queue<DodgeballEnemy> throwQueue = new Queue<DodgeballEnemy>();

    void Start()
    {
        Invoke(nameof(StartThrowingCycle), 1f);
    }


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterEnemy(DodgeballEnemy enemy)
    {
        throwQueue.Enqueue(enemy);
    }

    public void EnemyFinishedThrowing()
    {
        DodgeballEnemy next = throwQueue.Dequeue();
        throwQueue.Enqueue(next);
        next.BeginThrow(); // Sýradaki düþmana sinyal gönder
    }

    public void StartThrowingCycle()
    {
        if (throwQueue.Count > 0)
        {
            throwQueue.Peek().BeginThrow();
        }
    }
}
