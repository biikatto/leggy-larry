using UnityEngine;

public class Reset : MonoBehaviour{

	private bool paused = false;

	void Update(){
		if(Input.GetButtonDown("Pause")){
			if(paused){
				Debug.Log("Unpaused");
				paused = false;
				Time.timeScale = 1f;
			}else{
				Debug.Log("Paused");
				paused = true;
				Time.timeScale = 0f;
			}
		}
		if(Input.GetButtonDown("Reset")){
			if(paused){
				Application.LoadLevel("demo");
				paused = false;
				Time.timeScale = 1f;
			}
		}
		if(Input.GetKey("left ctrl") & Input.GetKeyDown("q")){
			Debug.Log("Quitting");
			Application.Quit();
		}
	}
}
