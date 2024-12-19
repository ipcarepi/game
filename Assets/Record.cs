using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CoverShooter;
using UnityEngine;

public class Record : MonoBehaviour
{
    public ScoreManager scoreManager;
    public HealthBar healthBar;

    private string filePath;
    private StreamWriter writer;
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/game_data.csv";

        writer = new StreamWriter(filePath);
        writer.WriteLine("UNIX Time,Score,Health,game started");

        StartCoroutine(GameLog());
    }

    IEnumerator GameLog() 
    {
        while(true) {
            string logline = string.Format("{0}, {1}, {2}", DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds, scoreManager.score, healthBar.Value * 100);
            writer.WriteLine(logline);

            yield return new WaitForSeconds(0.2f);
        }
    }

    void OnApplicationQuit() {
        writer.Close();
    }
}
