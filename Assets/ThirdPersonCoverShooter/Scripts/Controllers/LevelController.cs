using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private bool isDDA = true;

    [SerializeField]
    private GameObject[] Areas;

    [SerializeField]
    private int StartLevel;

    [SerializeField]
    private ScoreManager scoreManager;
    private int NowLevel;
    public GameObject Target;
    private CharacterHealth targetCharacterHealth;
    private float playerHealth;
    private float lastHealth;
    private float lastHitHealth;
    private float lastScore;
    private float HealthValue;
    private float zombiespawn;
    private float lastHitTime;
    private float lastDDATime;
    private float healthRate;
    private float scoreRate;
    private float result;
    private int zombies;
    private int score;
    private float healthWeight = 0.2f;
    private float healthRateWeight = 0.2f;
    private float lastHitWeight = 0.2f;
    private float ScoreRateWeight = 0.2f;
    private float ZombieWeight = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < StartLevel; i++) {
            Areas[i].SetActive(true);
        }
        NowLevel = StartLevel;
        lastDDATime = Time.time;
        targetCharacterHealth = Target.GetComponent<CharacterHealth>();
        
        if (isDDA) {
            StartCoroutine(MultiplyWithDelay(1f));
            InvokeRepeating("DDA", 20f, 1f);
        }
    }

    IEnumerator MultiplyWithDelay(float delay)
    {
        while (true)
        {
            CalHealthRate();
            CalScoreRate();
            yield return new WaitForSeconds(delay);
        }
    }

    void Update() {
        score = scoreManager.score;
        if (lastHitHealth > playerHealth) {
            lastHitTime = Time.time;
        }
        lastHitHealth = playerHealth;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (targetCharacterHealth != null) {
            playerHealth = targetCharacterHealth.Health;
            HealthValue = playerHealth / targetCharacterHealth.MaxHealth;
        }
    }

    void CalHealthRate() {
        healthRate = (lastHealth - playerHealth) / targetCharacterHealth.MaxHealth;
        lastHealth = playerHealth;
    }

    void CalScoreRate() {
        scoreRate = score - lastScore;
        lastScore = score;
    }

    public void AddZombie() {
        zombies += 1;
    }

    public void SubZombie() {
        zombies -= 1;
    }

    void DDA() {
        if (Time.time - lastDDATime > 15) {
            result = HealthDDA() + HealthRateDDA() + LastHitDDA() + ScoreRateDDA() + ZombieDDA();
            if (result > 0.2f && NowLevel != 5) {
                Areas[NowLevel].SetActive(true);
                NowLevel += 1;
                lastDDATime = Time.time;
            }
            else if (result <= -0.2f && NowLevel != 1) {
                Areas[NowLevel - 1].SetActive(false);
                NowLevel -= 1;
                lastDDATime = Time.time;
            }
        }
    }

    float HealthDDA() {
        if (HealthValue < 0.2f) {
            return -2f * healthWeight;
        }
        else if (HealthValue < 0.6f) {
            return -1f * healthWeight;
        }
        return 0;
    }

    float HealthRateDDA() {
        if (healthRate > 0.1f) {
            return -1f * healthRateWeight;
        }
        return 0;
    }

    float LastHitDDA() {
        if (Time.time - lastHitTime > 15f) {
            return 1f * lastHitWeight;
        }
        return 0;
    }

    float ScoreRateDDA() {
        switch (NowLevel)
        {
            case 1:
                zombiespawn = 0.5f;
                break;
            case 2:
                zombiespawn = 1f;
                break;
            case 3:
                zombiespawn = 1.3f;
                break;
            case 4:
                zombiespawn = 1.6f;
                break;
            case 5:
                zombiespawn = 1.83f;
                break;
            default:
                zombiespawn = 0f; // 예외 처리
                break;
        }
        if (scoreRate > zombiespawn) {
            return 1f * ScoreRateWeight;
        }
        return 0;
    }

    float ZombieDDA() {
        switch (NowLevel)
        {
            case 1:
                zombiespawn = 0.5f;
                break;
            case 2:
                zombiespawn = 1f;
                break;
            case 3:
                zombiespawn = 1.3f;
                break;
            case 4:
                zombiespawn = 1.6f;
                break;
            case 5:
                zombiespawn = 1.83f;
                break;
            default:
                zombiespawn = 0f; // 예외 처리
                break;
        }
        if (zombies > zombiespawn * 30) {
            return -1f * ZombieWeight;
        }
        else if (zombies < zombiespawn * 5) {
            return 1f * ZombieWeight;
        }
        return 0;
    }
}
