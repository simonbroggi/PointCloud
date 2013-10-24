using UnityEngine;
using System.Collections;

public class FileLoader : MonoBehaviour {
	
	public PointCloud pointCloudPrefab;

	// Use this for initialization
	void Start () {
		dbgString = "started";
		//TextAsset pts = Resources.Load("SBroggiPalaceHaeusle.pts") as TextAsset;
		//TextAsset pts = Resources.Load("SBroggiPost.pts") as TextAsset;
		//LoadFile(pts.text);
		//LoadFile("21111     \n0.5955505 0.8973999 0.2449951 -1934 125 118 102\n35.5955505 -179.8973999 1861.2449951 -1934 125 118 102\n35.5770302 -179.9205170 1861.1866455 -1913 181 173 160\n35.5856400 -179.9092102 1861.1351318 -1921 161 155 141\n35.5833511 -179.9119873 1861.0806885 -1917 194 188 174\n35.5838661 -179.9111328 1861.0270996 -1912 195 189 173\n35.5815773 -179.9139099 1860.9726563 -1914 193 187 171");
	}
	
	private Vector3 offset = new Vector3(10, 0, 0);
	private int num = 1;
	
	public void LoadFile(string f){
		//http://answers.unity3d.com/questions/9960/how-do-i-let-the-user-select-a-local-file-for-my-a.html
		//dbgString = "should load "+f;
		

		
		//GameObject p = new GameObject("pointcloud");
		PointCloud pc = Instantiate(pointCloudPrefab) as PointCloud; //p.AddComponent<ParticleSystem>();
		//PointCloud pc = ps.GetComponent<PointCloud>();
		pc.LoadPoints(f);
		//pc.transform.Translate(offset*num);
		dbgString+="\nloaded file "+num;
		num++;
		
	}
	string dbgString = "nothing jet";
	void OnGUI(){
		GUI.Label(new Rect(20, Screen.height/2, 220, 80), dbgString);
	}
}
