using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Runtime.Serialization;

public interface FacebookReceiveListener {
	void onFacebookReceiveGiven ();
}

public class FacebookReceive : MonoBehaviour {

	public static List<FacebookItem> givenList = new List<FacebookItem>();
	public static void safeAdd(FacebookItem item) {
		foreach(FacebookItem checkItem in givenList) {
			if(item.id == checkItem.id) {
				return;
			}
		}

		givenList.Add (item);
	}

	public static void Given() {
		HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (FacebookValue.BaseUrl + "give");
		request.Method = "GET";
		
		string authToken = "Bearer" + " " + FB.AccessToken;
		request.Headers.Add("Authorization", authToken);

		request.BeginGetResponse((IAsyncResult x) => {
			using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(x)) {
				string test = x.ToString();
				
				StreamReader reader = new StreamReader(response.GetResponseStream());
				string content = reader.ReadToEnd();
				
				JSONObject rootNode = new JSONObject(content);
				
				JSONObject dataNodes = rootNode["data"];
				
				foreach(JSONObject dataNode in dataNodes.list) {
					FacebookItem item = new FacebookItem();
					
					item.sender = dataNode["from"].str;
					item.id = dataNode["id"].str;
					
					JSONObject innerDataNodes = dataNode["data"];

					foreach(string key in innerDataNodes.keys) {
						item.itemName.Add(key);
					}

					foreach(JSONObject innerDataNode in innerDataNodes.list) {
						item.itemTitle.Add(innerDataNode["id"].str);
					}
					
					safeAdd(item);
					
				}
			}

		}, null);
	}

	
	
}
