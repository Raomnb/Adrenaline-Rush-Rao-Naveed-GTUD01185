using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerPlayer : Car
{
    public Texture[] texture;
    public GameObject body;
    private Renderer material;
    public WheelCollider[] wheelCollider;
    private Vector3 centerOfMass = new Vector3(0, -0.2f, 0);
    public GameObject[] wheels;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;//set center of mass on y to -0.2
        material = body.GetComponent<Renderer>(); // set material as renderer
        carAudioIdle.PlayDelayed(1); // play idle sound of car after 1 second of game start
        lap = 0; // set lap to to 0
        material.material.SetTexture("_MainTex", texture[MenuHandler.color]); // set color of car selected in main menu
    }

    private void Update()
    {
        if(startLapTime)
        {
            lapTime += Time.deltaTime; // increase lap time by time.delta time
            
        }
       
        if(startGameTime)
        {
            gameTime += Time.deltaTime; // inccrease game time by time.delta time
        }
       
       
    }

 
    void FixedUpdate()
    {
        if (raceFinished == false) // if race is not finished
        {
            currentSpeed = 2 * Mathf.PI * wheelCollider[0].radius * wheelCollider[0].rpm * 60 / 1000; // calculate current speed using rpm and radious of wheel
            gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.up * 300 * currentSpeed); // set downforce of car 
            Vector3 wheelPos;
            Quaternion wheelRotation;

            
            Drive(); // call drive function
            Brake(); // call brake function
             

            float horizontalInput = Input.GetAxis("Horizontal"); // set horizontal input as left right keys
            ApplySteer(horizontalInput); // call steer function

            for (int i = 0; i < 4; i++)
            {
                wheelCollider[i].GetWorldPose(out wheelPos, out wheelRotation); //get wheel colider position and rotation
                wheels[i].transform.position = wheelPos; // set wheels position as collider position to give real feel
                wheels[i].transform.rotation = wheelRotation; //set wheels rotation to collider rotation to give real feel
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0); // if r is pressed reset car rotation from uoside down to standard 
            }
        }
        else
        {
            wheelCollider[0].brakeTorque = 2000; // if race is finished apply breaks on all wheels
            wheelCollider[1].brakeTorque = 2000;// if race is finished apply breaks on all wheels
            wheelCollider[2].brakeTorque = 2000;// if race is finished apply breaks on all wheels
            wheelCollider[3].brakeTorque = 2000;// if race is finished apply breaks on all wheels
        }
    }
    void ApplySteer(float input)
    {
        if (currentSpeed < 25)
        {
            wheelCollider[0].steerAngle = input * 20; // if speed is less than 25 give steering angle as 20
            wheelCollider[1].steerAngle = input * 20;// if speed is less than 25 give steering angle as 20
        }
        else
        {
            wheelCollider[0].steerAngle = Mathf.Lerp(wheelCollider[0].steerAngle, input * 75 / gameObject.GetComponent<Rigidbody>().velocity.magnitude, Time.deltaTime * 5);// if speed is greater than 25 give steering angle reduced with speed the greater the speed lesser the steering angel and using mathf.lerp to interpolate steer to give smooth turning
            wheelCollider[1].steerAngle = Mathf.Lerp(wheelCollider[1].steerAngle, input * 75 / gameObject.GetComponent<Rigidbody>().velocity.magnitude, Time.deltaTime * 5);// if speed is greater than 25 give steering angle reduced with speed the greater the speed lesser the steering angel and using mathf.lerp to interpolate steer to give smooth turning
        }
    }
    public void Drive()
    {
        float input = Input.GetAxis("Vertical"); // get up down key as input
       
        if (input > 0 && currentSpeed < 100) // if up key is pressed and speed is less than 100
        {
            if(!carRunnig) 
            {
                carAudioIdle.Stop(); // stop idle sound
                carAcceleration.Play(); // play acceleration sound
                carRunnig = true; // set car runningbool to true
            }
            
                wheelCollider[2].motorTorque = input * driveTorque; // set rear wheels torque as input multiplied by drive torque to give car forward motion
                wheelCollider[3].motorTorque = input * driveTorque; // set rear wheels torque as input multiplied by drive torque to give car forward motion


        }
        else if(input<0) // if down key is pressed
        {
            wheelCollider[2].motorTorque = input * reverseTorque; // set rear wheels torque as input multiplied by reverse torque to give car backwards motion
            wheelCollider[3].motorTorque = input * reverseTorque;// set rear wheels torque as input multiplied by reverse torque to give car backwards motion
        }
        else
        {
            if (carRunnig && input==0) // if no key is pressed
            {
                carAcceleration.Stop(); //stop acceleration sound 
                carAudioIdle.Play(); // play idle sound
                carRunnig = false; // set car running top false
            }
            wheelCollider[0].motorTorque = 0; // set motor torque on all wheels to 0 to decelerate on leaving up key
            wheelCollider[1].motorTorque = 0;// set motor torque on all wheels to 0 to decelerate on leaving up key
            wheelCollider[2].motorTorque = 0;// set motor torque on all wheels to 0 to decelerate on leaving up key
            wheelCollider[3].motorTorque = 0;// set motor torque on all wheels to 0 to decelerate on leaving up key
        }
    }
    public void Brake()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            wheelCollider[2].brakeTorque = 10000; // apply brake torque
            wheelCollider[3].brakeTorque = 10000; // apply brake torque
        }
        else
        {
            wheelCollider[2].brakeTorque = 0; // set brake torque  to 0
            wheelCollider[3].brakeTorque = 0; // set brake torque to 0
        }
    }
   
}
