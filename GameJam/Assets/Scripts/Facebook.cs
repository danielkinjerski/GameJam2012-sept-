using UnityEngine;
using System.Collections;

public class Facebook : MonoBehaviour {
	
	public const string authorizations_url = "http://caldwell3rdfloor.dyndns.org:3000/authorizations";
	public const string client_id = "150345411773349";
	public const string scope = "publish_stream";
	public const int maxRetries = 10;
	public string state;
	public string token;
	
	public void Publish(string message){
		StartCoroutine(PublishC(message));
	}
	
	protected IEnumerator PublishC(string message){
		if(this.token != ""){
			string url = "https://graph.facebook.com/me/feed";
			
			WWWForm form = new WWWForm();
			form.AddField("access_token", this.token);
			form.AddField("message", message);
			
			WWW request = new WWW(url, form);
			
			yield return request;
		}
	}
	
	public string AuthDialogUrl(){
		this.state = System.Guid.NewGuid().ToString();
		string url = "https://www.facebook.com/dialog/oauth?" +
			"client_id=" + client_id +
			"&redirect_uri=" + authorizations_url +
			"&scope=" + scope +
			"&state=" + state;
		
		return url;
	}
	
	public IEnumerator GetTokenFrom(string url){
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
	
	void GetToken(){
		Application.OpenURL(AuthDialogUrl());
		string url = authorizations_url + "/" + this.state;
		
		StartCoroutine(GetTokenFrom(url));
	}
}
