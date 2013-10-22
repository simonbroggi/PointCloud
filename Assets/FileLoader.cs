using UnityEngine;
using System.Collections;

public class FileLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		dbgString = "started";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void LoadFile(string f){
		//http://answers.unity3d.com/questions/9960/how-do-i-let-the-user-select-a-local-file-for-my-a.html
		dbgString = "should load ";
	}
	string dbgString = "nothing jet";
	void OnGUI(){
		GUI.Label(new Rect(20, Screen.height-30, 220, 80), dbgString);
	}
}
