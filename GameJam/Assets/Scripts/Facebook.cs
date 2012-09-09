using UnityEngine;
using System.Collections;

public class Facebook : MonoBehaviour {
	
	public const string authorizations_url = "http://caldwell3rdfloor.dyndns.org:3000/authorizations";
	public const string client_id = "150345411773349";
	public const string scope = "publish_stream";
	public const int maxRetries = 10;
	public string state;
	public string token;
	public bool enabled;
	
	
	public string AuthDialogUrl(){
		this.state = System.Guid.NewGuid().ToString();
		string url = "https://www.facebook.com/dialog/oauth?" +
			"client_id=" + client_id +
			"&redirect_uri=" + authorizations_url +
			"&scope=" + scope +
			"&state=" + state;
		print ("URL: " + url);
		
		return url;
	}
	
	public IEnumerator getDataFrom(string url){
		int i = 0;
		while(i < maxRetries){
			WWW response = new WWW(url);
			
			yield return response;
			
			JSONObject data = new JSONObject(response.text);
			string error = data["error"].str;
			if(error == "" || error == null){
				this.token = data["token"].str;
			}
			i++;
		}
	}
	
	void Awake(){
		Application.OpenURL(AuthDialogUrl());
		string url = authorizations_url + "/" + this.state;
		print (url);
		
		StartCoroutine(getDataFrom(url));
		while(this.token == "" && this.enabled){
			new WaitForSeconds(1);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
