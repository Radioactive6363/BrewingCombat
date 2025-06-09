using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
/// Effect controller inspector.
/// </summary>
[CustomEditor(typeof(EffectController))]
public class EffectControllerInspector : Editor
{
	private string[] m_LayerName;
	private int[] m_LayerID;

	void OnEnable()
	{
		m_LayerName = XUIUtils.GetSortingLayerNames ();
		m_LayerID = XUIUtils.GetSortingLayerUniqueIDs ();
	}

	public override void OnInspectorGUI()
	{
		bool bShowAll = false;
		bool bHideAll = false;
		
		EffectController effectCtrl = target as EffectController;

		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUI.BeginDisabledGroup (effectCtrl.mBLockNums);
		effectCtrl.mNNumOfEffects = EditorGUILayout.IntField("Effect Count", effectCtrl.mNNumOfEffects);
		EditorGUI.EndDisabledGroup();
		effectCtrl.mBLockNums = EditorGUILayout.Toggle (effectCtrl.mBLockNums);
		if (GUILayout.Button ("One Click Expansion"))
			bShowAll = true;
		else
			bShowAll = false;

		if (GUILayout.Button ("One Click Close"))
			bHideAll = true;
		else
			bHideAll = false;

		EditorGUILayout.EndHorizontal();

		int nCnt = 0;
		for (; nCnt < effectCtrl.mNNumOfEffects; nCnt++) {
			if (nCnt >= effectCtrl.mKEffectGenList.Count) {
				effectCtrl.mKEffectGenList.Add (new EffectData ());
			}

			EffectData effectData = effectCtrl.mKEffectGenList [nCnt];
			if (effectData == null)
				continue;
			if (bShowAll)
				effectData.mBFoldoutOpen = true;
			if (bHideAll)
				effectData.mBFoldoutOpen = false;
			
			effectData.mBFoldoutOpen = EditorGUILayout.Foldout (effectData.mBFoldoutOpen, ("Effect " + nCnt + " Setting"));
			if (effectData.mBFoldoutOpen) {
				effectData.mFTimeSec = EditorGUILayout.FloatField ("Shot Time", effectData.mFTimeSec);
				effectData.mGoEffect = EditorGUILayout.ObjectField ("Obj", effectData.mGoEffect, typeof(GameObject), true) as GameObject;

				EditorGUI.indentLevel++;
				/// Transform panel.
				effectData.mBTransformFoldout = EditorGUILayout.Foldout (effectData.mBTransformFoldout, "Transform");
				if (effectData.mBTransformFoldout) {
					EditorGUI.indentLevel++;
					EditorGUI.BeginChangeCheck ();
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button ("P", GUILayout.Width(25), GUILayout.ExpandWidth(false)))
						effectData.mGoPos = new Vector3 (0, 0, 0);
					effectData.mGoPos = EditorGUILayout.Vector3Field ("", effectData.mGoPos);
					GUILayout.EndHorizontal ();
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button ("R", GUILayout.Width(25), GUILayout.ExpandWidth(false)))
						effectData.mGoRotation = new Vector3 (0, 0, 0);
					effectData.mGoRotation = EditorGUILayout.Vector3Field ("", effectData.mGoRotation);
					GUILayout.EndHorizontal ();
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button ("S", GUILayout.Width(25), GUILayout.ExpandWidth(false)))
						effectData.mGoScale = new Vector3 (0, 0, 0);
					effectData.mGoScale = EditorGUILayout.Vector3Field ("", effectData.mGoScale);
					GUILayout.EndHorizontal ();
					if (EditorGUI.EndChangeCheck ()) {
						effectCtrl.UpdateEffectTransformByIndex (nCnt);
					}
					EditorGUI.indentLevel--;
				}

				ParticleSystem particleSystem = effectCtrl.CheckHasParticleSystem (nCnt);
				RenderEffect renderEffect = effectCtrl.CheckHasRenderEffectScript (nCnt);
				if (particleSystem == null) {
					effectData.mBSortingFoldout = EditorGUILayout.Foldout (effectData.mBSortingFoldout, "Sorting Layer");
					/// Sorting panel.
					if (effectData.mBSortingFoldout) {
						EditorGUI.indentLevel++;
						EditorGUI.BeginChangeCheck ();
						EditorGUILayout.BeginHorizontal ();
						EditorGUILayout.LabelField ("Sorting Layer");
						effectData.mSortingLayerID = EditorGUILayout.IntPopup (effectData.mSortingLayerID, m_LayerName, m_LayerID);
						EditorGUILayout.EndHorizontal ();

						EditorGUILayout.BeginHorizontal ();
						EditorGUILayout.LabelField ("Sorting Order");
						effectData.mSortingOrder = EditorGUILayout.IntField (effectData.mSortingOrder);
						EditorGUILayout.EndHorizontal ();
						if (EditorGUI.EndChangeCheck ()) {
							if (renderEffect != null) {
								renderEffect.mSortingLayerID = effectData.mSortingLayerID;
								renderEffect.mSortingOrder = effectData.mSortingOrder;
								renderEffect.mEnableSetSortLayer = true;
								renderEffect.UpdateRenderLayer ();
							} else {
								effectCtrl.UPdateRenderLayerByIndex (nCnt);
							}
						}
						EditorGUI.indentLevel--;
					}
				}
				EditorGUI.indentLevel--;
			}

			if (nCnt != effectCtrl.mNNumOfEffects - 1) {
				EditorGUILayout.LabelField ("", GUILayout.Height (2));
				GUILayout.Box ("", GUILayout.ExpandWidth (true), GUILayout.Height (1));
				EditorGUILayout.LabelField ("", GUILayout.Height (2));
			}
		}

		for (; nCnt < effectCtrl.mKEffectGenList.Count; nCnt++) {
			effectCtrl.mKEffectGenList.RemoveAt (nCnt);
		}

		EditorGUILayout.EndVertical ();
	}
}
