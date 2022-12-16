using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Canvas : MonoBehaviour
{
    public TextMeshProUGUI lapTime;
    public TextMeshProUGUI lap;
    public TextMeshProUGUI bestLap;
    private float bestLapTime;
    private float time;
    private float minutesFloat;
    private float minutesFloatBest;
    private float secondsFloat;
    private float secondsFloatBest;
    private int minutes;
    private int count = 1;
    private int minutesBest;
    public GameObject playerCar;
    public GameObject[] carAI;
    public GameObject finished;
    public TextMeshProUGUI position;
    public GameObject button;
    public float timer = 0;
    public GameObject mainMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time = playerCar.GetComponent<CarControllerPlayer>().lapTime; // set time equals lap time of player car
        minutesFloat = time / 60; // get minutes out of lap time
        minutes = (int)minutesFloat;// convert minutes to int
        secondsFloat = time % 60; // get seconds out of lap time
        lapTime.text = " Lap Time: " + minutes + " : " + secondsFloat.ToString("F1"); // show lap time on screen
        lap.text = "Lap: "+playerCar.GetComponent<CarControllerPlayer>().lap; // show current lap on screen
        bestLapTime = playerCar.GetComponent<CarControllerPlayer>().bestLap; // set best lap time of player car  to local best lap time variable
        if (bestLapTime != 0) // if best lap time is not 0 means atleast 1 lap is completed
        {
            minutesFloatBest = bestLapTime / 60; // get minutes out of best lap time
            minutesBest = (int)minutesFloatBest;// convert minutes to int
            secondsFloatBest = bestLapTime % 60;// get seconds out of best lap time 
            bestLap.text = "Best Lap: " + minutesBest + " : " + secondsFloatBest.ToString("F1"); // show current best lap time on screen
        }
        if (playerCar.GetComponent<CarControllerPlayer>().raceFinished)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                for (int i = 0; i < carAI.Length; i++)
                {
                    Debug.Log(carAI[i].GetComponent<CarAI>().gameTime + carAI[i].name);
                    if (playerCar.GetComponent<CarControllerPlayer>().gameTime > carAI[i].GetComponent<CarAI>().gameTime) // check if any enemy car finished before player if yes increase the position
                    {
                        count++;
                    }
                }
                finished.SetActive(true); // enable race finished text
                if (count == 1)
                {
                    position.text = "1st Position"; // if no car finished before player show 1st position
                }
                else if (count == 2)
                {
                    position.text = "2nd Position"; // if 1 car finished before player show 2nd position
                }
                else if (count == 3)
                {
                    position.text = "3rd Position";// if 2 car finished before player show 3rd position
                }
                else if (count == 4)
                {
                    position.text = "4th Position"; // if 3 car finished before player show 4th position
                }
                else if (count == 5)
                {
                    position.text = "5th Position";// if all cars finished before player show 5th position
                }
                button.SetActive(true); // set main menu button active
                mainMenu.SetActive(false);
            }
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0); // load main menu scene
    }
   
}
