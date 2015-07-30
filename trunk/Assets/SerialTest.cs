using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class SerialTest : MonoBehaviour {
	
	SerialPort stream;
	
	void Start () {
		try {
			stream = new SerialPort("usbmodem1421", 9600);
			stream.Parity = Parity.None;
			stream.StopBits = StopBits.One;
			stream.DataBits = 8;
			stream.Handshake = Handshake.None;
			stream.DataReceived += new SerialDataReceivedEventHandler(DataReceviedHandler);
			
			
			stream.Open();
			Debug.Log("opened ok"); // it DOES open ok!
		} catch (Exception e){
			Debug.Log("Error opening port "+e.ToString()); // I never see this message
		}
	}
	
	void Update () { // called about 60 times/second
		try {
			// Read serialinput from COM3
			// if this next line is here, it will hang, I don't even see the startup message
			Debug.Log(stream.ReadLine());
			// Note: I've also tried ReadByte and ReadChar and the same problem, it hangs
		} catch (Exception e){
			Debug.Log("Error reading input "+e.ToString());
		}
	}
	
	private static void DataReceviedHandler(
		object sender,
		SerialDataReceivedEventArgs e)
	{
		SerialPort sp = (SerialPort)sender; // It never gets here!
		string indata = sp.ReadExisting();
		Debug.Log("Data Received:");
		Debug.Log(indata);
	}
	
	void OnGUI() // simple GUI
	{
		// Create a button that, when pressed, sends the 'ping'
		if (GUI.Button (new Rect(10,10,100,20), "Send"))
			stream.Write(" ");
	}
}