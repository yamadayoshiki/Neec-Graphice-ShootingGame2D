using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 数字画像を分割で生成するクラス
/// [0,1,2,3,4,5,6,7,8,9]の画像を分割する
/// </summary>
public class CreateNumberSprite : MonoBehaviour
{
	/// <summary>
	/// 数字の画像
	/// </summary>
	[SerializeField]
	private Texture2D m_NumberTexture = null;

	/// <summary>
	/// ナンバースプライトリスト
	/// </summary>
	[SerializeField]
	private List<Sprite> m_Sprites = new List<Sprite>();

	/// <summary>
	/// 分割サイズ
	/// </summary>
	[SerializeField]
	private Vector2 SliceSize = Vector2.one;

	/// <summary>
	/// Pivotの位置
	/// </summary>
	[SerializeField]
	private Vector2 m_PivotPosition = new Vector2(0.5f, 0.5f);

	[SerializeField]
	private float m_PixelsPerUnit = 100.0f;

	/// <summary>
	/// 初期化
	/// </summary>
	public void Init()
	{
		//Texture2D texture = SpriteRederManager.Instance.GetTexture2D(m_NumberTexture.name);
		//if (texture != null) m_NumberTexture = texture;
		CreateSprite();
	}

	/// <summary>
	/// 数字画像を分割して生成
	/// </summary>
	private void CreateSprite()
	{
		float width = m_NumberTexture.width / SliceSize.x;
		float height = m_NumberTexture.height / SliceSize.y;
		for (int i = 0; i < 10; i++)
		{
			Sprite sprite = Sprite.Create(m_NumberTexture, new Rect(width * i, 0, width, height), new Vector2(0.5f, 0.5f), m_PixelsPerUnit);
			m_Sprites.Add(sprite);
		}
	}

	/// <summary>
	/// 数字画像を取得
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>
	public Sprite GetNumberSprite(int num)
	{
		return m_Sprites[num];
	}
}
