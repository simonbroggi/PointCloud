using UnityEngine;
using System.Collections;
//[ExecuteInEditMode]
[RequireComponent (typeof(ParticleSystem))]
public class PointCloud : MonoBehaviour {

	private ParticleSystem.Particle[] particles; //probably need to keep an own struct array of points in memory. scaling!

	[System.Serializable]
	public class CloudPoint{
		public Vector3 pos;
		public Color col;
	}

	[SerializeField]
	private CloudPoint[] points = new CloudPoint[0];

	public float pointSize = 1f;

	public void ResetParticles(){
		particles = new ParticleSystem.Particle[points.Length];
		for(int i=0; i<points.Length; i++){
			if(i < 10){
				//Debug.Log("point at "+points[i].pos);
			}
			particles[i].position = points[i].pos;
			particles[i].color = points[i].col;
			particles[i].size = pointSize;
			//if(i%10==0) Debug.Log(points[i].pos);
		}
		particleSystem.SetParticles(particles, points.Length);
		//Debug.Log("reset "+points.Length+" particles");
	}
	void Awake(){
		particleSystem.loop=false;
		particleSystem.enableEmission=false;
		particleSystem.playOnAwake=false;
		particleSystem.renderer.castShadows=false;
		particleSystem.renderer.receiveShadows=false;
	}
	void OnEnable(){
		ResetParticles();
	}

	[SerializeField]
	private Vector3 cloudDimensions;
	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, cloudDimensions);
	}
	public void LoadPointsFromPts(string f){
		string[] lines = f.Split('\n');
		
		int startLine = 0;
		int numPoints = lines.Length - startLine;
		
		if(lines[0].Trim().Split(' ').Length == 1){
			//parse numPoints from first line
			numPoints = int.Parse(lines[0].Trim());
			startLine = 1;
		}

		//Debug.Log("num points: "+numPoints);
		points = new CloudPoint[numPoints];
		particleSystem.maxParticles = numPoints;
		if(numPoints <= 0){
			Debug.LogWarning("no points in the file");
			return;
		}


		//find the bounds and set transform to the center

		float minX=float.PositiveInfinity, maxX=float.NegativeInfinity, minY=float.PositiveInfinity, maxY=float.NegativeInfinity, minZ=float.PositiveInfinity, maxZ=float.NegativeInfinity;
		
		for(int i=0; i<numPoints; i++){
		//for(int i=startLine; i<lines.Length; i++){
			string [] values = lines[i+startLine].Split(' ');
			//Debug.Log("values = "+lines[i+startLine]);
			points[i]=new CloudPoint();
			//rotate 90
			points[i].pos = new Vector3( float.Parse(values[0]), float.Parse(values[2]), -float.Parse(values[1]) );
			points[i].col = new Color32(byte.Parse(values[4]), byte.Parse(values[5]), byte.Parse(values[6]), byte.MaxValue);

			//ignoring value[3] What is it???

			//set min max
			if(points[i].pos.x > maxX) maxX = points[i].pos.x;
			if(points[i].pos.x < minX) minX = points[i].pos.x;
			if(points[i].pos.y > maxY) maxY = points[i].pos.y;
			if(points[i].pos.y < minY) minY = points[i].pos.y;
			if(points[i].pos.z > maxZ) maxZ = points[i].pos.z;
			if(points[i].pos.z < minZ) minZ = points[i].pos.z;
		}
		cloudDimensions = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);
		Vector3 centerPos = new Vector3((minX+maxX)/2f, (minY+maxY)/2f, (minZ+maxZ)/2f);
		//Debug.Log("center:"+centerPos);
		foreach(CloudPoint cp in points){
			cp.pos -= centerPos;
		}
		transform.position = centerPos;

	}
	
	
}
