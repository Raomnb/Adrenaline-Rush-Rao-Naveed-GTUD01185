using UnityEngine;
using System.Collections;

public class Speedometer : MonoBehaviour {

	/*
		Andreas B. O. 08-Dec-2013 : Andreasboost98@gmail.com
		You are allowed to use this in your own project. Personal and commercial use.
		I would appreciate a donation, if you deside to use this code.

		

	*/

	public GameObject car;
	public Texture2D speedometer_main_tex;
	public Texture2D speedometer_needle_tex;

	Rect speedometer_main_rect;
	Rect speedometer_needle_rect;

	Vector2 needle_pivot;

	float needle_angle = 0; // Should always be 0
	public float zero_angle = -130; // zero_angle is the angle of the needle, when the vehicle is not moving
	public float speed_add_value = 1; // This is how much the needle will move in degress for each kilometer;
	float speed; // Current speed

	public bool isKph = true; // Use Kilometers Per Hour?
					   // If set to false, Miles Per Hour will be used


	// Use this for initialization
	void Start () {
	
	}
	
	void OnGUI() {

		// Calculate the desired rects and the GUI size
		float speedometer_main_size = 0.512f * ((Screen.height / 3.5f) + (Screen.width / 3.5f));
		float speedometer_needle_sizeX = (Screen.height / 110f) + (Screen.width / 110f);
		float speedometer_needle_sizeY = (Screen.height / 30f) + (Screen.width / 30f);

		speedometer_main_rect = new Rect(Screen.width - speedometer_main_size, Screen.height - speedometer_main_size, speedometer_main_size, speedometer_main_size);
		speedometer_needle_rect = new Rect(Screen.width - (speedometer_main_size / 2) - (speedometer_needle_sizeX / 2),
		                                   Screen.height - (speedometer_main_size / 2) - (speedometer_needle_sizeY),
		                                   speedometer_needle_sizeX, speedometer_needle_sizeY);

		// Pivot rotation Vector2
		needle_pivot = new Vector2(speedometer_needle_rect.x + (speedometer_needle_sizeX / 2), speedometer_needle_rect.y + (speedometer_needle_sizeY));

		// Draw the speedometer
		GUI.DrawTexture(speedometer_main_rect, speedometer_main_tex);
		
		//Get the speed of the vehicle
		if(isKph == true) {

			speed = car.GetComponent<CarControllerPlayer>().currentSpeed;

		} else if(isKph == false) {

			speed = car.GetComponent<CarControllerPlayer>().currentSpeed/1.6f;

		}

		// zero_angle is the angle of the needle, when the vehicle is not moving
		needle_angle = zero_angle + speed * speed_add_value;

		// Backup the GUI Matrix
		Matrix4x4 matrixBackup = GUI.matrix;

		// Do the actual rotation of the needle
		GUIUtility.RotateAroundPivot(needle_angle, needle_pivot);

		// Draw the needle
		GUI.DrawTexture(speedometer_needle_rect, speedometer_needle_tex);
		GUI.matrix = matrixBackup;

	}

}
