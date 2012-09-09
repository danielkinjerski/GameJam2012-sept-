using UnityEngine;
using System.Collections;

public class Facebook : MonoBehaviour {
	
	public const string authorizations_url = "http://caldwell3rdfloor.dyndns.org:3000/authorizations";
	public const string client_id = "150345411773349";
	public const string scope = "publish_stream";
	public const int retries = 50;
	public string state;
	public string token;
	public bool enabled;
	
	
	public static string AuthDialogUrl(){
		System.Guid state = System.Guid.NewGuid();
		string url = "https://www.facebook.com/dialog/oauth?" +
			"client_id=" + client_id +
			"&redirect_uri=" + authorizations_url +
			"&scope=" + scope +
			"&state=" + state.ToString();
		
		return url;
	}
	
	public IEnumerator getDataFrom(string url){
		print ("in getDataFrom");
		WWW response = new WWW(url);
		
		yield return response;
		
		print ("BODY: " + response.text);
	}
	
	void Awake(){
		Application.OpenURL(AuthDialogUrl());
		string url = authorizations_url + "/" + this.state;
		print (url);
		
		print ("Calling getDataFrom");
		StartCoroutine(getDataFrom(url));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
