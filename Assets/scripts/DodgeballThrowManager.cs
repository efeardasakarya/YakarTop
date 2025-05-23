using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Yakar top oyununda, sahneye dinamik olarak spawn olan ve "Enemy" tagine sahip t�m d��manlardan top f�rlatmay� s�rayla y�neten manager.
/// </summary>
public class DodgeballThrowManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Her top f�rlat�m� aras�nda beklenen s�re (saniye).")]
    public float timeBetweenThrows = 2f;

    private List<DodgeballEnemy> enemies = new List<DodgeballEnemy>();
    private int currentIndex = 0;

    void Start()
    {
        // �lk listeleme
        RefreshEnemyList();

        if (enemies.Count == 0)
        {
            Debug.LogWarning($"{name}: Sahnede 'Enemy' tagine sahip DodgeballEnemy bulunamad�. Spawning sonras� otomatik eklenecek.");
        }

        StartCoroutine(ThrowRoutine());
    }

    /// <summary>
    /// Sahnedeki 'Enemy' tagl� objeleri bularak listeyi yeniler.
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
                Debug.LogWarning($"{name}: 'Enemy' tagli {go.name} �zerinde DodgeballEnemy bile�eni bulunamad�.");
        }
        Debug.Log($"{name}: {enemies.Count} adet DodgeballEnemy listeye eklendi.");
    }

    private IEnumerator ThrowRoutine()
    {
        while (true)
        {
            // Dinamik spawn edileni yakalamak i�in her d�ng�de listeyi g�ncelle
            RefreshEnemyList();

            if (enemies.Count > 0)
            {
                if (currentIndex >= enemies.Count)
                    currentIndex = 0;

                var enemy = enemies[currentIndex];
                if (enemy != null)
                {
                    enemy.BeginThrow();
                    Debug.Log($"{name}: {enemy.name} f�rlatma ba�lat�ld�.");
                    currentIndex++;
                }
            }
            else
            {
                // Liste bo�sa bir s�re bekleyip tekrar dene
                Debug.Log($"{name}: Hen�z f�rlat�lacak d��man yok, {timeBetweenThrows}s sonra tekrar kontrol edilecek.");
            }

            yield return new WaitForSeconds(timeBetweenThrows);
        }
    }
}
