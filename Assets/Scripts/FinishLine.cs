using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CarControllerPlayer>()) // if trigger object is player car
        {
            if (other.GetComponent<CarControllerPlayer>().lap == 0) 
            {
                other.GetComponent<CarControllerPlayer>().lap++; // if lap is 0 make the lap 1
                other.GetComponent<CarControllerPlayer>().StartLapTimer();// call start lap timer function
                other.GetComponent<CarControllerPlayer>().StartGameTime();
            }
            else if(other.GetComponent<CarControllerPlayer>().lap <2) 
            {
                other.GetComponent<CarControllerPlayer>().EndLapTime(); // call end lap time function
                other.GetComponent<CarControllerPlayer>().StartLapTimer();// call start lap timer function
                other.GetComponent<CarControllerPlayer>().lap++; // if lap is less than 2 then increase lap by 1
            }
            else if(other.GetComponent<CarControllerPlayer>().lap==2)
            {
                other.GetComponent<CarControllerPlayer>().startGameTime = false; // if 2 laps are completed disable game time counter
                other.GetComponent<CarControllerPlayer>().raceFinished = true; // set the race to finished state
                other.GetComponent<CarControllerPlayer>().startLapTime = false;
            }
        }
        else if(other.GetComponent<CarAI>()) //if trigger object is enemy car
        {
            if (other.GetComponent<CarAI>().lap == 0) 
            {                
                other.GetComponent<CarAI>().lap++;   // if lap is 0 make the lap 1
                other.GetComponent<CarAI>().StartLapTimer();// call start lap timer function
                other.GetComponent<CarAI>().StartGameTime();
            }
            else if (other.GetComponent<CarAI>().lap < 2)
            {
                other.GetComponent<CarAI>().EndLapTime();// call end lap time function
                other.GetComponent<CarAI>().StartLapTimer();// call start lap timer function
                other.GetComponent<CarAI>().lap++; // if lap is less than 2 then increase lap by 1
            }
            else if (other.GetComponent<CarAI>().lap == 2)
            {
                other.GetComponent<CarAI>().startGameTime = false;// if 2 laps are completed disable game time counter
                other.GetComponent<CarAI>().raceFinished = true;// set the race to finished state
                other.GetComponent<CarAI>().startLapTime = false; 
            }
        }
       
    }
    
}
