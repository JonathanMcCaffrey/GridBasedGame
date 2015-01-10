
//Code originally taken from FriendsSmash Demo: https://github.com/fbsamples/friendsmash-unity

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.MiniJSON;

public class FacebookUtils : ScriptableObject {
    public static string GetPictureURL(string facebookID, int? width = null, int? height = null, string type = null) {
        string url = string.Format("/{0}/picture", facebookID);
        string query = width != null ? "&width=" + width.ToString() : "";
        query += height != null ? "&height=" + height.ToString() : "";
        query += type != null ? "&type=" + type : "";
        query += "&redirect=false";
        if (query != "") url += ("?g" + query);
        return url;
    }

    public static Dictionary<string, string> RandomFriend(List<object> friends) {
        var fd = ((Dictionary<string, object>)(friends[Random.Range(0, friends.Count)]));
        var friend = new Dictionary<string, string>();
        friend["id"] = (string)fd["id"];
        friend["first_name"] = (string)fd["first_name"];
        var pictureDict = ((Dictionary<string, object>)(fd["picture"]));
        var pictureDataDict = ((Dictionary<string, object>)(pictureDict["data"]));
        friend["image_url"] = (string)pictureDataDict["url"];
        return friend;
    }

    public static Dictionary<string, string> DeserializeJSONProfile(string response) {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object nameH;
        var profile = new Dictionary<string, string>();
        if (responseObject.TryGetValue("first_name", out nameH))
        {
            profile["first_name"] = (string)nameH;
        }
        return profile;
    }
    
    public static List<object> DeserializeScores(string response)  {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object scoresh;
        var scores = new List<object>();
        if (responseObject.TryGetValue ("data", out scoresh)) 
        {
            scores = (List<object>) scoresh;
        }

        return scores;
    }

    public static List<object> DeserializeJSONFriends(string response) {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object friendsH;
        var friends = new List<object>();
        if (responseObject.TryGetValue("invitable_friends", out friendsH)) {
            friends = (List<object>)(((Dictionary<string, object>)friendsH)["data"]);
        }
        if (responseObject.TryGetValue("friends", out friendsH)) {
            friends.AddRange((List<object>)(((Dictionary<string, object>)friendsH)["data"]));
        }
        return friends;
    }

    public static string DeserializePictureURLString(string response) {
        return DeserializePictureURLObject(Json.Deserialize(response));
    }

    public static string DeserializePictureURLObject(object pictureObj) {
        var picture = (Dictionary<string, object>)(((Dictionary<string, object>)pictureObj)["data"]);
        object urlH = null;
        if (picture.TryGetValue("url", out urlH)) {
            return (string)urlH;
        }
        
        return null;
    }


}
