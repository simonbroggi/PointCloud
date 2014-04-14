using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]

[RequireComponent (typeof(ParticleEmitter))]
[RequireComponent (typeof(ParticleRenderer))]

public class PointCloud : MonoBehaviour {

	private Particle[] particles; //probably need to keep an own struct array of points in memory. scaling!

	[System.Serializable]
	public class CloudPoint{
		public Vector3 pos;
		public Color col;
	}

	[SerializeField]
	private CloudPoint[] points = new CloudPoint[0];

	public float pointSize = 1f;

	public int nPoints{
		get{
			return points.Length;
		}
	}

	public void ResetParticles(){
		particles = new Particle[points.Length];
		// 16250 seems to be max for legacy particles...
		for(int i=0; i<points.Length; i++){
			particles[i].energy = float.PositiveInfinity;
			particles[i].position = points[i].pos;
			particles[i].color = points[i].col;
			particles[i].size = pointSize;
		}
		Debug.Log("Assigning "+particles.Length+" Particles");
		particleEmitter.particles = particles;
	}
	
	void OnEnable(){
		ResetParticles();
	}


	[SerializeField]
	private Vector3 cloudDimensions;
	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, cloudDimensions);
	}

	public void LoadPointsFromMesh(Mesh m){
		points = new CloudPoint[m.vertexCount];
		//particleSystem.maxParticles = m.vertexCount;
		Debug.Log(m.vertices.Length+" Verts;  "+m.colors.Length+" colors;  "+m.colors32.Length+" color32");
		for(int i=0; i<m.vertexCount; i++){
			points[i] = new CloudPoint();
			points[i].pos = m.vertices[i];

			if(m.colors.Length > i){
				points[i].col = m.colors[i];
			}
			else{
				points[i].col = Color.red;
			}
		}
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
		//particleSystem.maxParticles = numPoints;
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
			points[i].col = new Color32(byte.Parse(values[values.Length-3]), byte.Parse(values[values.Length-2]), byte.Parse(values[values.Length-1]), byte.MaxValue);

		    //aha! http://www.las-vegas.uni-osnabrueck.de/index.php/tutorials2/8-understanding-file-formats-pts-and-3d-files

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
