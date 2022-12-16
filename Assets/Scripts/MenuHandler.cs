using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    
    public GameObject[] text;
    public GameObject eight;
    public GameObject oval;
    public static int color=0;
    public int scene = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject); // take set color to next scene
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Blue()
    { 
        color = 0; // set color to 0 for blue car
        for(int i=0;i<text.Length;i++)
        {
            text[i].SetActive(false); // set X mark inactive on all 
        }
        text[color].SetActive(true); // set x mark active on blue car
    }
    public void Yellow()
    {
        color = 1; // set color 1 for yellow car
        for (int i = 0; i < text.Length; i++)
        {
            text[i].SetActive(false);// set X mark inactive on all
        }
        text[color].SetActive(true);// set x mark active on yellow car
    }
    public void Red()
    {
        color = 2; // set color 2 for red car
        for (int i = 0; i < text.Length; i++)
        {
            text[i].SetActive(false);// set X mark inactive on all
        }
        text[color].SetActive(true);// set x mark active on red car
    }
    public void Grey()
    {
        color = 3; // set color 3 for grey car
        for (int i = 0; i < text.Length; i++)
        {
            text[i].SetActive(false);// set X mark inactive on all
        }
        text[color].SetActive(true);// set x mark active on grey car
    }
    public void Purple()
    {
        color = 4; // set color 4 for purple car
        for (int i = 0; i < text.Length; i++)
        {
            text[i].SetActive(false);// set X mark inactive on all
        }
        text[color].SetActive(true);// set x mark active on purple car
    }
    public void EightTrack()
    {
        scene = 1; //set scene to 1 for eight track
        oval.SetActive(false); // set shade on oval track inactive
        eight.SetActive(true); // set shade on eight track active
    }
    public void OvalTrack()
    {
        scene = 2;//set scene to 2 for oval track
        eight.SetActive(false);// set shade on eight track inactive
        oval.SetActive(true);// set shade on oval track active
    }
    public void StartGame()
    {
        SceneManager.LoadScene(scene); //load selected scene to start game
    }
}
