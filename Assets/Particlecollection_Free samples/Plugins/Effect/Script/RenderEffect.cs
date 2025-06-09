using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.Serialization;

public enum RenderBillBoardType{
    Normal,
    Horizontal,
    Vertical,
}

[System.Serializable]
public class MaterialEffect
{
    [FormerlySerializedAs("m_EffectMaterial")] public Material mEffectMaterial;
    [FormerlySerializedAs("m_EnableAlphaAnimation")] public bool mEnableAlphaAnimation = false;
    [FormerlySerializedAs("m_AlphaAnimationTimeScale")] public float mAlphaAnimationTimeScale = 1.0f;
    [FormerlySerializedAs("m_AlphaCurve")] public AnimationCurve mAlphaCurve = new AnimationCurve();
    //public bool enableSetTextureWrapMode = true;
    [FormerlySerializedAs("m_MainTexture")] public Texture mMainTexture = null;
    [FormerlySerializedAs("m_MaskTexutre")] public Texture mMaskTexutre = null;
    [FormerlySerializedAs("m_MainTexWrapMode")] public TextureWrapMode mMainTexWrapMode;
    [FormerlySerializedAs("m_MaskTexWrapMode")] public TextureWrapMode mMaskTexWrapMode;
	[FormerlySerializedAs("m_EnableUVScroll")] public bool mEnableUVScroll = false;
	[FormerlySerializedAs("m_UVScrollMainTex")] public Vector2 mUVScrollMainTex;
	[FormerlySerializedAs("m_UVScrollCutTex")] public Vector2 mUVScrollCutTex;
#if UNITY_EDITOR
    [FormerlySerializedAs("m_EditorExtend")] public bool mEditorExtend = false;
#endif

    public MaterialEffect(Material material)
    {
        
    }

    public void ReInitMaterial(Material material)
    {
        if (material == null)
            return;
        mEffectMaterial = material;
        //effectMaterial.renderQueue += renderSeqFix;
        if(material.HasProperty(EffectShaderPropertyStr.MainTexStr))
          mMainTexture = material.GetTexture(EffectShaderPropertyStr.MainTexStr);
        if (material.HasProperty(EffectShaderPropertyStr.CutTexStr))
            mMaskTexutre = material.GetTexture(EffectShaderPropertyStr.CutTexStr);
    }

    public void UpdateEffect(float execueTime)
    {
        if (mMainTexture != null && mMainTexWrapMode != mMainTexture.wrapMode)
        {
            mMainTexture.wrapMode = mMainTexWrapMode;
        }
        if (mMaskTexutre != null && mMaskTexWrapMode != mMaskTexutre.wrapMode)
        {
            mMaskTexutre.wrapMode = mMaskTexWrapMode;
        }
		if (mEnableUVScroll) {
			if(mMainTexture)
				mEffectMaterial.SetTextureOffset (EffectShaderPropertyStr.MainTexStr, mUVScrollMainTex * execueTime);
			if(mMaskTexutre)
				mEffectMaterial.SetTextureOffset (EffectShaderPropertyStr.CutTexStr, mUVScrollCutTex * execueTime);
			
		}
    }
    void SetAlpha(float value)
    {
        Color color = mEffectMaterial.color;
        color.a = value;
        mEffectMaterial.color = color;
    }
}

[ExecuteInEditMode]
//[RequireComponent(typeof(Renderer))]
public class RenderEffect : MonoBehaviour {
    [FormerlySerializedAs("m_BillBoardType")] public RenderBillBoardType mBillBoardType;
	private Camera _mReferenceCamera = null;
    [FormerlySerializedAs("m_EnableBillBoard")] public bool mEnableBillBoard = false;
    [FormerlySerializedAs("m_EnableSetSortLayer")] public bool mEnableSetSortLayer = true;
    [FormerlySerializedAs("m_Render")] public Renderer mRender;
    [FormerlySerializedAs("m_MaterialEffects")] public List<MaterialEffect> mMaterialEffects = new List<MaterialEffect>();
	private float _mTimeLine = 0.0f;
    [FormerlySerializedAs("m_SortingLayerID")] [HideInInspector]
    public int mSortingLayerID;
    [FormerlySerializedAs("m_SortingOrder")] [HideInInspector]
    public int mSortingOrder;

    void Awake()
    {
		_mReferenceCamera = Camera.main;
        mRender = GetComponent<Renderer>();
        if (mRender == null)
            return;
    }

    void OnEnable()
    {
        RefreshMaterial();
    }

    public void UpdateRenderLayer()
    {
        if(mEnableSetSortLayer)
        {
            mRender.sortingLayerID = mSortingLayerID;
            mRender.sortingOrder = mSortingOrder;
        }

    }

    public void RefreshMaterial()
    {
		if (mRender == null) {
			mRender = GetComponent<Renderer>();
			if(mRender == null)
				return;
		}
        int i = 0; 
        for(i = 0; i < mRender.sharedMaterials.Length; i++)
        {
            if (mMaterialEffects.Count <= i)
            {
                MaterialEffect matEffect = new MaterialEffect(mRender.sharedMaterials[i]);
                mMaterialEffects.Add(matEffect);
            }
            else
            {          
                mMaterialEffects[i].ReInitMaterial(mRender.sharedMaterials[i]);
            }
        }
        for (int j = mMaterialEffects.Count - 1; i <= j; j--)
        {
            mMaterialEffects.RemoveAt(j);
       }
        UpdateRenderLayer();
    }

    void UpdateBillBoard()
    {
        if (mEnableBillBoard == false)
            return;
		if (_mReferenceCamera == null)
			_mReferenceCamera = Camera.main;
        if(mBillBoardType == RenderBillBoardType.Normal)
        {
            Vector3 targetPos = transform.position + _mReferenceCamera.transform.rotation * Vector3.forward;
            Vector3 targetOrientation = _mReferenceCamera.transform.rotation * Vector3.up;
            transform.LookAt(targetPos, targetOrientation);
        }
        else if (mBillBoardType == RenderBillBoardType.Vertical)
        {
            var v = _mReferenceCamera.transform.forward;
            v.y = 0;
            transform.rotation = Quaternion.LookRotation(v, Vector3.up);
        }
        else if(mBillBoardType == RenderBillBoardType.Horizontal)
        {
            Vector3 targetPos = transform.position + _mReferenceCamera.transform.rotation * Vector3.down;
            Vector3 targetOrientation = _mReferenceCamera.transform.rotation * Vector3.up;
            transform.LookAt(targetPos, targetOrientation);
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.x = 90.0f;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
	
	// Update is called once per frame
	void Update () {
        _mTimeLine += Time.deltaTime;
        foreach(MaterialEffect matEffect in mMaterialEffects)
        {
            matEffect.UpdateEffect(_mTimeLine);
        }
    }

	void LateUpdate(){
		UpdateBillBoard();
	}

	public void Sim(float timer){
		UpdateBillBoard ();
		foreach(MaterialEffect matEffect in mMaterialEffects)
		{
			matEffect.UpdateEffect(timer);
		}
	}
}
