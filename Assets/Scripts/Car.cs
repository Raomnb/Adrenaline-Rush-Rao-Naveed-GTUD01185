using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public bool carRunnig = false;  
    public AudioSource carAcceleration;
    public AudioSource carAudioIdle;
    public float currentSpeed;
    public int lap = 0;
    public float lapTime = 0;
    public float gameTime = 0;
    public float bestLap = 0;
    public bool startLapTime = false;
    public bool startGameTime = false;
    public bool raceFinished = false;
    public int driveTorque = 1800;
    public int reverseTorque = 900;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public  void ApplySteer()
    {

    }
    public  void Drive()
    {

    }
    public void Brake()
    {

    }
    public void StartLapTimer()
    {
        startLapTime = true; // set start lap time bool true
    }
    public void StartGameTime()
    {
        startGameTime = true; //set start game time to true
    }
    public void EndLapTime()
    {
        startLapTime = false; //set start lap time to false so lap time can be used for best lap if its better than best lap
        if(bestLap==0)
        {
            bestLap = lapTime; // best lap is 0 set lap time to best lap as its 1st lap
        }
        else if(bestLap>lapTime)
        {
            bestLap = lapTime; // if best lap is greater than lap time means that new lap is better than best lap so set it to best lap
        }
        lapTime = 0; // set lap time 0 for next lap
    }
   
}
