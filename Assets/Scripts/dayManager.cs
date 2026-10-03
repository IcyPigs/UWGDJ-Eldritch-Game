using UnityEngine;

public class dayManager : MonoBehaviour
{
    public dayManager instance { get; private set; }

     public bool clockOut;
     public bool success;
     public int score; // 1-5? for preformance, gets at the end
     public float patience;
     public int daysPassed;

    void Awake()
    {
        if(dayManager.instance == null)
            instance = this
        else
        {
            Destroy(gameObject)
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void resetPatience() {
        patience = 1000;
    }

    void decay()
    {
        patience -= 10f  * (1f + Mathf.Log(1f + CheckPlayerAccuracy())) / leniency;
    }

    void endDay()
    {
        clockOut = 0;
        daysPassed++;

    }
}
