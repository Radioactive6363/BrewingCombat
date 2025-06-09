using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class EffectDemo : MonoBehaviour {
	public const string EffectAssetPath = "Assets/Prefab/";
	[FormerlySerializedAs("m_EffectPrefabList")] public List<GameObject> mEffectPrefabList = new List<GameObject> ();
	[FormerlySerializedAs("m_LookAtEffect")] public bool mLookAtEffect = true;
	private GameObject _mNowShowEffect = null;
	private int _mNowIndex = 0;
	private string _mNowEffectName;
	// Use this for initialization
	void Awake () {
        #if (UNITY_EDITOR_WIN && !UNITY_WEBPLAYER)
		    mEffectPrefabList.Clear();
		    string[] aPrefabFiles = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);
            foreach (string prefabFile in aPrefabFiles)
		    {
			    string assetPath = "Assets" + prefabFile.Replace(Application.dataPath, "").Replace('\\', '/');
                if(assetPath.Contains("_noshow"))
                {
                    continue;
                }
			    GameObject sourcePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
			    mEffectPrefabList.Add (sourcePrefab);
		    }
        #endif
        if (Application.isPlaying == false)
			return;
		_mNowIndex = 1;
		GenPrevEffect ();
	}
	
	void OnDestroy(){
		Object.DestroyImmediate (_mNowShowEffect);	
	}
	
	void LateUpdate(){
		if (Application.isPlaying == false)
			return;
		if (mLookAtEffect && _mNowShowEffect) {
			transform.LookAt (_mNowShowEffect.transform.position);			
		}
	}
	
	// Update is called once per frame
	void OnGUI() {
		if (Application.isPlaying == false)
			return;
		if (GUI.Button (new Rect (0, 25, 80, 50), "Prev")) {
			GenPrevEffect ();
		}
		if (GUI.Button (new Rect (90, 25, 80, 50), "Next")) {
			GenNextEffect ();
		}
		GUI.Label (new Rect (5, 0, 350, 50), _mNowEffectName);
	}
	
	void GenPrevEffect(){
		_mNowIndex--;
		if (_mNowIndex < 0) {
			_mNowIndex = 0;
			return;	
		}
		if (_mNowShowEffect != null) {
			Object.Destroy (_mNowShowEffect);	
		}
		_mNowShowEffect =  Instantiate(mEffectPrefabList [_mNowIndex]);
		_mNowEffectName = _mNowShowEffect.name;
	}
	
	void GenNextEffect(){
		_mNowIndex++;
		if (_mNowIndex >= mEffectPrefabList.Count) {
			_mNowIndex = mEffectPrefabList.Count - 1;	
			return;
		}
		if (_mNowShowEffect != null) {
			Object.Destroy (_mNowShowEffect);	
		}
		_mNowShowEffect =  Instantiate(mEffectPrefabList [_mNowIndex]);		
		_mNowEffectName = _mNowShowEffect.name;
	}
}
