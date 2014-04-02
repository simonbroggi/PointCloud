using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.IO;

public class PtsImporter : AssetPostprocessor {

	static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPath){
		foreach(string s in importedAssets){
			if(s.EndsWith(".pts")){

				string prefabPath = s.Substring(0, s.Length-4) + ".prefab";
				GameObject go = new GameObject("tempObject");
				go.hideFlags = HideFlags.NotEditable;


				/*
				ParticleSystem ps = go.AddComponent<ParticleSystem>();
				ps.loop=false;
				ps.enableEmission=false;
				ps.playOnAwake=false;
				ps.renderer.castShadows=false;
				ps.renderer.receiveShadows=false;
				*/

				PointCloud pc = go.AddComponent<PointCloud>();

				go.particleSystem.renderer.material = AssetDatabase.LoadAssetAtPath("Assets/Point.mat", typeof(Material)) as Material;

				//prefab.hideFlags = HideFlags.NotEditable;
				try{
					using(StreamReader sr = new StreamReader(s)){
						Debug.Log("loading points!!!");
						pc.LoadPointsFromPts(sr.ReadToEnd());

					}
				}
				catch(Exception e){
					Debug.LogError(e);
				}

				GameObject prefab = PrefabUtility.CreatePrefab(prefabPath, go);
				GameObject.DestroyImmediate(go);

				Debug.Log("imported asset "+s);



				//AssetDatabase.CreateAsset(prefab, prefabPath);
				//AssetDatabase.AddObjectToAsset(createPointcloudFromPts(""), s);
			}
		}
		foreach(string s in deletedAssets){
			if(s.EndsWith(".pts")){
				Debug.Log("deleted asset "+s);
			}
		}
		for(int i=0; i<movedAssets.Length; i++){
			if(movedAssets[i].EndsWith(".pts")){
				Debug.Log("moved asset from "+movedFromAssetPath[i]+" to "+movedAssets[i]);
			}
		}

		Debug.Log("OnPostprocess *.pts Assets end");
	}
}
