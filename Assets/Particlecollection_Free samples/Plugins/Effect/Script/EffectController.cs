using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

[System.Serializable]
public class EffectData{
	[FormerlySerializedAs("m_bFoldoutOpen")] public bool mBFoldoutOpen = true;

	[FormerlySerializedAs("m_fTimeSec")] public float mFTimeSec = 0.0f;
	[FormerlySerializedAs("m_goEffect")] public GameObject mGoEffect = null;

	[FormerlySerializedAs("m_bTransformFoldout")] public bool mBTransformFoldout = true;
	[FormerlySerializedAs("m_goPos")] public Vector3 mGoPos = new Vector3 (0, 0, 0);
	[FormerlySerializedAs("m_goRotation")] public Vector3 mGoRotation = new Vector3 (0, 0, 0);
	[FormerlySerializedAs("m_goScale")] public Vector3 mGoScale = new Vector3 (1, 1, 1);

	[FormerlySerializedAs("m_bSortingFoldout")] public bool mBSortingFoldout = true;
	[FormerlySerializedAs("m_SortingLayerID")] public int mSortingLayerID;
	[FormerlySerializedAs("m_SortingOrder")] public int mSortingOrder;
}

public class EffectController : MonoBehaviour {
	[FormerlySerializedAs("m_nNumOfEffects")] public int mNNumOfEffects = 0;			///< 特效數量.
	[FormerlySerializedAs("m_bLockNums")] public bool mBLockNums = false;		///< 特效數量鎖定.

	[FormerlySerializedAs("m_kEffectGenList")] public List<EffectData> mKEffectGenList = new List<EffectData>();		///< 特效設定清單.
	private int _mNNowIndex = 0;

	void Awake()
	{
		for (int i = 0; i < mKEffectGenList.Count; i++) {
			Invoke ("GenEffect", mKEffectGenList [i].mFTimeSec);
		}

		Comp comparer = new Comp ();			///< 時間Comparer.
		mKEffectGenList.Sort (comparer);		///< 依時間排序.
	}

	void Update()
	{
		CheckTransfromUpdate ();
	}

	/// <summary>
	/// 特效生成.
	/// </summary>
	void GenEffect()
	{
		EffectData effectData = mKEffectGenList[_mNNowIndex];
		if (effectData == null)
			return;

		if(effectData.mGoEffect != null) {
			GameObject go = Instantiate (effectData.mGoEffect);
			go.transform.parent = transform;
			go.name = _mNNowIndex.ToString ();	///< 上編號.
			UpdateEffectTransformByIndex (_mNNowIndex);
			UPdateRenderLayerByIndex (_mNNowIndex);
		}
		_mNNowIndex++;
	}

	/// <summary>
	/// 原生功能更改值.
	/// </summary>
	void CheckTransfromUpdate()
	{
		foreach (Transform tf in transform) {
			int nIndex = int.Parse (tf.name);
			EffectData effectData = mKEffectGenList[nIndex];
			if (effectData == null)
				return;

			if (tf.position != effectData.mGoPos)
				effectData.mGoPos = tf.position;
			if (tf.localRotation.eulerAngles != effectData.mGoRotation)
				effectData.mGoRotation = tf.localRotation.eulerAngles;
			if (tf.localScale != effectData.mGoScale)
				effectData.mGoScale = tf.localScale;
		}
	}

	/// <summary>
	/// 更新對應編號特效之Transform數值.
	/// </summary>
	/// <param name="nIndex">特效編號.</param>
	public void UpdateEffectTransformByIndex(int nIndex)
	{
		/// 取得特效資料.
		Transform tf = this.transform.Find (nIndex.ToString());
		if (tf == null)
			return;
		EffectData effectData = mKEffectGenList[nIndex];
		if (effectData == null)
			return;

		/// 設定特效物件Transform.
		tf.position = effectData.mGoPos;
		Quaternion effectObjRotation = new Quaternion ();
		effectObjRotation.eulerAngles = effectData.mGoRotation;
		tf.localRotation = effectObjRotation;
		tf.localScale = effectData.mGoScale;
	}

	/// <summary>
	/// 檢查對應編號特效是否含有粒子系統.
	/// </summary>
	/// <returns><c>true</c>,有Particle System, <c>false</c> 沒article System.</returns>
	/// <param name="nIndex">特效編號.</param>
	public ParticleSystem CheckHasParticleSystem(int nIndex)
	{
		/// 取得特效物件.
		Transform tf = this.transform.Find (nIndex.ToString());
		if (tf == null)
			return null;

		/// 取得粒子系統.
		ParticleSystem particleSystem = tf.gameObject.GetComponent<ParticleSystem> ();
		return particleSystem;
	}

	/// <summary>
	/// 檢查對應編號特效是否使用RenderEffect.
	/// </summary>
	/// <returns>RenderEffect元件.</returns>
	/// <param name="nIndex">特效編號.</param>
	public RenderEffect CheckHasRenderEffectScript(int nIndex)
	{
		/// 取得特效物件.
		Transform tf = this.transform.Find (nIndex.ToString());
		if (tf == null)
			return null;

		/// 取得RenderEffect元件.
		RenderEffect renderEffect = tf.gameObject.GetComponent<RenderEffect> ();
		return renderEffect;
	}

	/// <summary>
	/// 更新對應編號特效物件Render Layer.
	/// </summary>
	/// <param name="nIndex">特效編號.</param>
	public void UPdateRenderLayerByIndex(int nIndex)
	{
		/// 取得特效物件.
		Transform tf = this.transform.Find (nIndex.ToString());
		if (tf == null)
			return;
		EffectData effectData = mKEffectGenList[nIndex];
		if (effectData == null)
			return;

		/// Render Layer 更新.
		Renderer render = tf.gameObject.GetComponent<Renderer>();
		render.sortingLayerID = effectData.mSortingLayerID;
		render.sortingOrder = effectData.mSortingOrder;
	}
}

/// <summary>
/// Effect Data Time comparer.
/// </summary>
public class Comp : IComparer<EffectData>
{
	public int Compare(EffectData x, EffectData y)
	{
		if (x == null) {
			if (y == null)
				return 0;
			else
				return 1;
		} else {
			if (y == null) {
				return -1;
			} else {
				float fDiff = x.mFTimeSec.CompareTo (y.mFTimeSec);
				if (fDiff > 0)
					return 1;
				else if (fDiff < 0)
					return -1;
				else
					return 0;
			}
		}
	}
}