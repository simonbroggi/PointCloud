using UnityEngine;
using System.Collections;

public class PointCloudSpawner : MonoBehaviour {
	
	private ParticleSystem.Particle[] points;
	private int numPoints;
	
	private GUIStyle guiStyle;
	
	// Use this for initialization
	void Start () {
		guiStyle = new GUIStyle();
		guiStyle.fontSize = 28;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		if(GUI.Button(new Rect(50, 50, 220, 80), "More Points")){
			particleSystem.Emit(10000);
			numPoints = particleSystem.GetParticles(points);
		}
		
		GUI.Label(new Rect(Screen.width-270, 50, 220, 80), "n: " + particleSystem.particleCount, guiStyle);
	}
}
