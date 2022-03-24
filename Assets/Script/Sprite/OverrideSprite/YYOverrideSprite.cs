using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YYOverrideSprite : MonoBehaviour
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
	SpriteMeshType m_SpriteMeshType = SpriteMeshType.FullRect;

	/// <summary>
	/// フィルターモード
	/// </summary>
	[SerializeField]
	FilterMode m_FilterMode = FilterMode.Point;

	//スプライトレンダラー
	private SpriteRenderer m_SpriteRenderer;

	//イメージ
	private Image m_Image = null;

	//画像読み込み機能
	private SpriteRederManager m_SpriteRederManager;


	// Start is called before the first frame update
	void Start()
	{
		//スプライトレンダラーを取得
		if(TryGetComponent(out m_SpriteRenderer))
		{
			m_SpriteRenderer.sprite = Replacement(m_SpriteRenderer.sprite.texture);
		}
		//イメージを取得
		if (TryGetComponent(out m_Image))
		{
			m_Image.sprite = Replacement(m_Image.sprite.texture);
		}

	}

	/// <summary>
	/// 画像差し替え
	/// </summary>
	private Sprite Replacement(Texture2D origin)
	{
		//差し替えテクスチャの読み込み
		Texture2D texture2D = SpriteRederManager.Instance.GetTexture2D(origin.name);
		//フィルタモードをポイントに設定
		texture2D.filterMode = m_FilterMode;
		//スプライトを作成
		Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), m_PixcelParUint, m_Extrude, m_SpriteMeshType);

		return sprite;
	}

}
