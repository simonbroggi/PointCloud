using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PointCloud))]
[CanEditMultipleObjects]
public class PointCloudEditor : Editor {

	SerializedProperty pointSize;

	void OnEnable(){
		PointCloud pc = target as PointCloud;

		pointSize = serializedObject.FindProperty("pointSize");

		pc.particleSystem.hideFlags = HideFlags.HideInInspector;
		pc.ResetParticles();
		pc.particleSystem.Pause();
	}

	public override void OnInspectorGUI(){
		serializedObject.Update ();

		//GUILayout.Label("Custom "+serializedObject.GetType().ToString());

		EditorGUILayout.Slider(pointSize, 0.01f, 2f);

		serializedObject.ApplyModifiedProperties();

		foreach(Object t in targets){
			if(t is PointCloud){
				PointCloud pc = t as PointCloud;
				GUILayout.Label(pc.transform.position.ToString());
				pc.ResetParticles();
				pc.particleSystem.Pause();
			}
		}
	}
}
