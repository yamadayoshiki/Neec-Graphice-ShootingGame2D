using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OverrideSprite : MonoBehaviour
{
	/// <summary>
	/// ワールド空間座標の 1 単位分に相当する、スプライトのピクセル数。
	/// </summary>
	[SerializeField]
	private float m_PixcelParUint = 1;

	/// <summary>
	/// スプライトメッシュが外側に拡張する量
	/// </summary>
	[SerializeField]
	private uint m_Extrude = 1;

	/// <summary>
	/// スプライトのために生成されるメッシュタイプの制御
	/// </summary>
	[SerializeField]
	SpriteMeshType m_SpriteMeshType = SpriteMeshType.Tight;

	private SpriteRenderer m_SpriteRenderer;
	private int m_IdMainTex = Shader.PropertyToID("_MainTex");
	private MaterialPropertyBlock m_Block;


	private SpriteRederManager m_SpriteRederManager;

	[SerializeField]
	private Texture m_Texture;
	public Texture m_OverrideTexture
	{
		get { return m_Texture; }
		set
		{
			m_Texture = value;
			if (m_Block == null)
			{
				Init();
			}
			m_Block.SetTexture(m_IdMainTex, m_Texture);
		}
	}

	public void SetSpriteRender(Sprite sprite)
	{
		m_SpriteRenderer.sprite = sprite;
	}

	private void Awake()
	{
		Init();
		m_OverrideTexture = m_Texture;

	}

	private void Start()
	{
		m_OverrideTexture = m_SpriteRederManager.GetTexture(m_OverrideTexture.name);
	}

	private void LateUpdate()
	{
		m_SpriteRenderer.SetPropertyBlock(m_Block);
	}

	private void OnValidate()
	{
		m_OverrideTexture = m_Texture;
	}

	void Init()
	{
		m_Block = new MaterialPropertyBlock();
		m_SpriteRederManager = FindObjectOfType<SpriteRederManager>();
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
		m_SpriteRenderer.GetPropertyBlock(m_Block);
	}
}
