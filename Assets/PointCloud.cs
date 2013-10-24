using UnityEngine;
using System.Collections;

public class PointCloud : MonoBehaviour {

	private ParticleSystem.Particle[] particles; //probably need to keep an own struct array of points in memory. scaling!
	private float oldStartSize = 1f;
	private Vector3 centerPos; //center of all points
	// Update is called once per frame
	void Update () {
		//int n = particleSystem.GetParticles(points);
		if(particles!=null && oldStartSize!=particleSystem.startSize){			
			for(int i=0; i<particles.Length; i++){
				particles[i].size = particleSystem.startSize;
			}
			particleSystem.SetParticles(particles, particles.Length);
			oldStartSize = particleSystem.startSize;
		}
	}
	
	private struct Point{
		public Vector3 pos;
		public Color col;
	}
	private Point[] points;
	
	public void LoadPoints(string f){
		string[] lines = f.Split('\n');
		
		int startLine = 0;
		int numPoints = lines.Length - startLine;
		
		if(lines[0].Trim().Split(' ').Length == 1){
			//parse numPoints from first line
			numPoints = int.Parse(lines[0].Trim());
			
			//numPoints = numPoints-1;
			startLine = 1;
		}
		
		Debug.Log("num points: "+numPoints);
		if(numPoints <= 0){
			Debug.LogWarning("no points in the file");
			return;
		}
		
		points = new Point[numPoints];
		
//		float increment = 1f / (numPoints - 1);
		/*
		for (int i = 0; i < numPoints; i++) {
			float x = i * increment;
			points[i].position = new Vector3(x, 0f, 0f);
			points[i].color = new Color(x, 0f, 0f);
			points[i].size = 0.1f;
		}
		*/
		
		float minX=float.PositiveInfinity, maxX=float.NegativeInfinity, minY=float.PositiveInfinity, maxY=float.NegativeInfinity, minZ=float.PositiveInfinity, maxZ=float.NegativeInfinity;
		
		for(int i=0; i<numPoints; i++){
		//for(int i=startLine; i<lines.Length; i++){
			string [] values = lines[i+startLine].Split(' ');
			//Debug.Log("values = "+lines[i+startLine]);
			
			//rotate 90
			points[i].pos = new Vector3( float.Parse(values[0]), float.Parse(values[2]), -float.Parse(values[1]) );
			points[i].col = new Color32(byte.Parse(values[4]), byte.Parse(values[5]), byte.Parse(values[6]), byte.MaxValue);
			
			//set min max
			if(points[i].pos.x > maxX) maxX = points[i].pos.x;
			if(points[i].pos.x < minX) minX = points[i].pos.x;
			if(points[i].pos.y > maxY) maxY = points[i].pos.y;
			if(points[i].pos.y < minY) minY = points[i].pos.y;
			if(points[i].pos.z > maxZ) maxZ = points[i].pos.z;
			if(points[i].pos.z < minZ) minZ = points[i].pos.z;
		}
		
		centerPos = new Vector3((minX+maxX)/2f, (minY+maxY)/2f, (minZ+maxZ)/2f);
		Debug.Log("center:"+centerPos);
		//set particles
		particles = new ParticleSystem.Particle[points.Length];
		for(int i=0; i<points.Length; i++){
			particles[i].position = points[i].pos - centerPos;
			particles[i].color = points[i].col;
			particles[i].size = particleSystem.startSize;
			//if(i%10==0) Debug.Log(points[i].pos);
		}
		
		particleSystem.SetParticles(particles, points.Length);
	}
	
	
}
