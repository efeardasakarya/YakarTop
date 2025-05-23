using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Yakar top oyununda, sahneye dinamik olarak spawn olan ve "Enemy" tagine sahip tüm düþmanlardan top fýrlatmayý sýrayla yöneten manager.
/// </summary>
public class DodgeballThrowManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Her top fýrlatýmý arasýnda beklenen süre (saniye).")]
    public float timeBetweenThrows = 2f;

    private List<DodgeballEnemy> enemies = new List<DodgeballEnemy>();
    private int currentIndex = 0;

    void Start()
    {
        // Ýlk listeleme
        RefreshEnemyList();

        if (enemies.Count == 0)
        {
            Debug.LogWarning($"{name}: Sahnede 'Enemy' tagine sahip DodgeballEnemy bulunamadý. Spawning sonrasý otomatik eklenecek.");
        }

        StartCoroutine(ThrowRoutine());
    }

    /// <summary>
    /// Sahnedeki 'Enemy' taglý objeleri bularak listeyi yeniler.
    /// </summary>
    private void RefreshEnemyList()
    {
        enemies.Clear();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var go in gos)
        {
            var enemy = go.GetComponent<DodgeballEnemy>();
            if (enemy != null)
                enemies.Add(enemy);
            else
                Debug.LogWarning($"{name}: 'Enemy' tagli {go.name} üzerinde DodgeballEnemy bileþeni bulunamadý.");
        }
        Debug.Log($"{name}: {enemies.Count} adet DodgeballEnemy listeye eklendi.");
    }

    private IEnumerator ThrowRoutine()
    {
        while (true)
        {
            // Dinamik spawn edileni yakalamak için her döngüde listeyi güncelle
            RefreshEnemyList();

            if (enemies.Count > 0)
            {
                if (currentIndex >= enemies.Count)
                    currentIndex = 0;

                var enemy = enemies[currentIndex];
                if (enemy != null)
                {
                    enemy.BeginThrow();
                    Debug.Log($"{name}: {enemy.name} fýrlatma baþlatýldý.");
                    currentIndex++;
                }
            }
            else
            {
                // Liste boþsa bir süre bekleyip tekrar dene
                Debug.Log($"{name}: Henüz fýrlatýlacak düþman yok, {timeBetweenThrows}s sonra tekrar kontrol edilecek.");
            }

            yield return new WaitForSeconds(timeBetweenThrows);
        }
    }
}
