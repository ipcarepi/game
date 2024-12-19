using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gradualController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Areas;
    private bool isRunning = false;
    private int NowLevel;
    private float lastDDATime;
    private object queueLock = new object();

    // Start is called before the first frame update
    void Start()
    {
        NowLevel = 1;
        Areas[NowLevel - 1].SetActive(true);
        lastDDATime = Time.time;
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        while (true) {
        yield return new WaitForSeconds(60);
        DDA_up();
        }
    }

    void DDA_up() {
        if (NowLevel < 5)
        {
            Areas[NowLevel].SetActive(true);
            NowLevel += 1;
        } else {
            Time.timeScale = 0;
        }
    }
}
