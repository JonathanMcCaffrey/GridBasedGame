using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class ArdunioControls : MonoBehaviour {
	
	public GameObject mArrowPrefab = null;
	
	private bool mMouseDown = false;
	
	float FORCE = 40;
	
	int left = 7;
	int forward = 6;
	int right = 5;
	int fire = 4;
	
	SerialPort serialPort;

	Rigidbody2D body;

	Angle angle;

	void Start () {
		serialPort = new SerialPort ("/dev/cu.usbmodem1411", 57600);
		serialPort.Open ();
		serialPort.ReadTimeout = 1;

		body = gameObject.GetComponent<Rigidbody2D> ();
		angle = gameObject.GetComponent<Angle> ();


	}
	
	
	void Update () {
		if (serialPort.IsOpen ) {
			try {
				takePortControls (serialPort.ReadByte ());
			} catch (System.Exception e) {
				
			}
		}
	}
	
	void takePortControls(int control) {
		if (control == left) {
			turn (1);
		}
		if (control == forward) {
			move ();
		}
		if (control == right) {
			turn (-1);
		}
		if (control == fire) {
			
		}
		
	}

	float rotationSpeed = 400;
	void turn(int direction) {
		angle.setDegreeValue (angle.getDegreeValue () + direction * Time.deltaTime * rotationSpeed);

	}

	float moveSpeed = 1;
	void move() {	
		this.gameObject.transform.position = this.gameObject.transform.position + (angle.getDirection () * Time.deltaTime * moveSpeed);
	}
}