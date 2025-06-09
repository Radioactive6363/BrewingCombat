using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class CameraTarget : MonoBehaviour {
	[FormerlySerializedAs("m_TargetOffset")] public Transform mTargetOffset;

	void LateUpdate(){
		transform.LookAt (mTargetOffset);
	}

}
