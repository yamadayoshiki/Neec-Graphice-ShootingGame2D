using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverrideImage : MonoBehaviour
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

	/// <summary>
	/// UIイメージクラス
	/// </summary>
	[SerializeField]
	private Image m_Image;

	/// <summary>
	/// 画像読み込み
	/// </summary>
	private SpriteRederManager m_SpriteRederManager;

	[SerializeField]
	private Texture2D m_Texture;
	public Texture2D OverrideTexture
	{
		get { return m_Texture; }
		set
		{
			m_Texture = value;
		}
	}

	private void Awake()
	{
		Init();
		OverrideTexture = m_Texture;
	}


	private void Start()
	{
		OverrideTexture = m_SpriteRederManager.GetTexture2D(OverrideTexture.name);
	   
		m_Image.sprite = Sprite.Create(OverrideTexture, new Rect(0, 0, m_Texture.width, m_Texture.height), Vector2.zero, m_PixcelParUint, m_Extrude, m_SpriteMeshType);
		m_Image.sprite.name = OverrideTexture.name;
	}


	private void OnValidate()
	{
		OverrideTexture = m_Texture;
	}

	void Init()
	{
	   
		m_SpriteRederManager = FindObjectOfType<SpriteRederManager>();
		m_Image = GetComponent<Image>();
	}
}
