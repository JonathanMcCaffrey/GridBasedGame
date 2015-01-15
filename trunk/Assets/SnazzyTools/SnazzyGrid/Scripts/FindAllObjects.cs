using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

static public class ObjectSearch
{
	static public GameObject Find(string name) 
	{
		GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
		
		foreach (GameObject object1 in objects) {
			if (object1.name == name) return object1;	
		}
		return null;
	}
	
	static public List<GameObject> FindObjects(string name) 
	{
		GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
		List<GameObject> objectList = new List<GameObject>();
		
		foreach (GameObject object1 in objects) {
			if (object1.name == name) objectList.Add(object1);	
		}
		return objectList;
	}
	
	static public Component Find (string name,Type type) 
	{
		Component[] objects = Resources.FindObjectsOfTypeAll(type) as Component[];
		
		foreach (Component object1 in objects) {
			if (object1.gameObject.name == name) return object1;	
		}
		
		return null;
	}
}
