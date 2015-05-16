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


public class FacebookDelete : MonoBehaviour {

	public static void Item(FacebookItem item) {
		HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (FacebookValue.Url + item.id);
		request.Method = "DELETE";
		
		string authToken = "Bearer" + " " + FB.AccessToken;
		request.Headers.Add("Authorization", authToken);
		
		
		request.BeginGetResponse((IAsyncResult x) => {
			using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(x))
			{
				if(response.StatusCode == HttpStatusCode.OK) {
					string deleted = "true";
				} else {
					string notDeleted = "true";
				}
			}
		}, null);


	}

	
	
}
