using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : Car
{
    public Transform path;
    public List<Transform> wayPoint = new List<Transform>();
    private Transform[] transforms;
    private int currentNode;
    public WheelCollider[] wheelCollider;
    public GameObject[] wheels;
    Vector3 wheelPos;
    Quaternion wheelRotation;
    float steer;
    private Vector3 centerOfMass = new Vector3(0, -0.2f, 0);
    public Color lineColor;
    private float sensorLength = 5f;
    private Vector3 frontCenter = new Vector3(0, 0.5f, 2.2f);
    private Vector3 frontSideSensor = new Vector3(0.75f, 0f, 0);
    private float frontSideAngle = 30;
    private bool avoiding = false;
    float avoidMultiplier = 0;
    float lerpSteerAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        carAudioIdle.PlayDelayed(1); // play idle sound after 1 second of game start
        GetComponent<Rigidbody>().centerOfMass = centerOfMass; //set center of mass on y to -0.2
        transforms = path.GetComponentsInChildren<Transform>(); // get transforms of all the chidren in path
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i].transform != path.transform)
            {
                wayPoint.Add(transforms[i]); // add all the children transforms to wayPoint list
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (startLapTime)
        {
            lapTime += Time.deltaTime; // increase lap time by time.delta time

        }
        if (startGameTime)
        {
            gameTime += Time.deltaTime;// inccrease game time by time.delta time
        }
    }
    void FixedUpdate()
    {
        if (raceFinished == false)
        {
            Sensors(); // call sensors function to avoid obstracles and stay on path efficiently so car can avoid side walls
            currentSpeed = 2 * Mathf.PI * wheelCollider[1].radius * wheelCollider[1].rpm * 60 / 1000; // calculate current speed using rpm and radius of wheels
            gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.up * 300 * currentSpeed); // set downforce of car 
            ApplySteer(); // call steering function
            Drive(); //call drive function
            CheckWaypointDistance(); // call function to check distance to waypoint
            LerpSteerAngle(); // call function to interpolate steering angle
            for (int i = 0; i < 4; i++)
            {
                wheelCollider[i].GetWorldPose(out wheelPos, out wheelRotation);//get wheel colider position and rotation
                wheels[i].transform.position = wheelPos;// set wheels position as collider position to give real feel
                wheels[i].transform.rotation = wheelRotation;//set wheels rotation to collider rotation to give real feel
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
    public void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(wayPoint[currentNode].position); // get transform position of waypoint on local space as relative vector so steering angle can be calculated
        if(currentSpeed<30)
        {
          steer = (relativeVector.x / relativeVector.magnitude) * 35; // increased steering angle on low speeds 
        }
        else
        {
            steer = (relativeVector.x / relativeVector.magnitude) * 10; // decreased steering angle on high speeds
        }
        
        lerpSteerAngle = steer; // set lerp steer angle to steer

    
    }
    public void Drive()
    {
       
        if(!carRunnig)
        {
            carAcceleration.PlayDelayed(2);// stop idle sound
            carAudioIdle.Stop(); // play acceleration sound
            carRunnig = true;// set car runningbool to true

        }
        if(currentSpeed<100)
        {
            wheelCollider[2].motorTorque = driveTorque;// set rear wheels torque as input multiplied by drive torque to give car forward motion
            wheelCollider[3].motorTorque = driveTorque;// set rear wheels torque as input multiplied by drive torque to give car forward motion
        }
        else
        {
            wheelCollider[2].motorTorque = 0;// set rear wheels torque as input multiplied by reverse torque to give car backwards motion
            wheelCollider[3].motorTorque = 0;// set rear wheels torque as input multiplied by reverse torque to give car backwards motion
            if (carRunnig) // if no key is pressed
            {
                carAcceleration.Stop(); //stop acceleration sound 
                carAudioIdle.Play(); // play idle sound
                carRunnig = false; // set car running top false
            }
        }          
             
    }
    private void CheckWaypointDistance()
    {
        if(Vector3.Distance(transform.position,wayPoint[currentNode].position)<2f) // if car is near to waypoint set current waypoint to next waypoint
        {
            if(currentNode == wayPoint.Count -1)
            {
                currentNode = 0; // if current node is last node set current node to 1st node
            }
            else
            {
                currentNode++; // if current node is not last node set current node to next node
                
            }
            
        }
    }
    private void Sensors()
    {
        
        RaycastHit hit; 
        
        Vector3 sensorStartingPos = transform.position; // set sensor start position to car position
        avoidMultiplier = 0;
        avoiding = false;
        sensorStartingPos += transform.forward * frontCenter.z; // setting sensor starting position on center front of car 
        sensorStartingPos += transform.up * frontCenter.y; // increase hieght of sensor starting position
       
        //front right sensor
        sensorStartingPos += transform.right * frontSideSensor.x; // set sensor starting position to right front of car
        if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength)) // start a raycast of sensor length on right front of car
        {
            avoiding = true; //if raycast is hit with any obstacle set avoiding to true
            avoidMultiplier -= 2f; // decrease 2 in avoid multiplier to steer car left to avoid obstacle
        }
       
        //front right angle sensor
        if (Physics.Raycast(sensorStartingPos, Quaternion.AngleAxis(frontSideAngle,transform.up) * transform.forward, out hit, sensorLength))
        {
            avoiding = true;//if raycast is hit with any obstacle set avoiding to true
            avoidMultiplier -= 1f;// decrease 1 in avoid multiplier to steer car left to avoid obstacle
        }
       
        //front left sensor 
        sensorStartingPos -= transform.right * 2 * frontSideSensor.x;
        if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength))
        {
            avoiding = true;//if raycast is hit with any obstacle set avoiding to true
            avoidMultiplier += 2f;// increase 2 in avoid multiplier to steer car Right to avoid obstacle

        }
      
        // front left angle sensor
        if (Physics.Raycast(sensorStartingPos, Quaternion.AngleAxis(-frontSideAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            avoiding = true;//if raycast is hit with any obstacle set avoiding to true
            avoidMultiplier += 1f;// increase 2 in avoid multiplier to steer car Right to avoid obstacle

        }
       
        
        //front center
        if(avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength))
            {
                avoiding = true;//if raycast is hit with any obstacle set avoiding to true
                if (hit.normal.x<0)
                {
                    avoidMultiplier = -2;  //if hit is on right side decrease 2 in avoid multiplier to steer car left to avoid obstacle
                }
                else
                {
                    avoidMultiplier = 2; //if hit is on left side  increase 2 in avoid multiplier to steer car Right to avoid obstacle
                }
            }
           
           
        }
        if (avoiding)
        {
            lerpSteerAngle = steer * avoidMultiplier; // if avoiding obstacle multiply steer with avoid multiplier to avoid obstacle
          
        }

    }
    void LerpSteerAngle()
    {
        wheelCollider[0].steerAngle = Mathf.Lerp(wheelCollider[0].steerAngle, lerpSteerAngle, Time.deltaTime * 5); // interpolate steer angle for smooth turning
        wheelCollider[1].steerAngle = Mathf.Lerp(wheelCollider[1].steerAngle, lerpSteerAngle, Time.deltaTime * 5);// interpolate steer angle for smooth turning
    }
    

}
