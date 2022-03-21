using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CreateNumberSprite))]
public class ViewNumberSprite : MonoBehaviour
{
	/// <summary>
	/// 数字画像を分割して作成
	/// </summary>
	private CreateNumberSprite m_CreateNumberSprite = null;

	/// <summary>
	/// スコア
	/// </summary>
	[SerializeField]
	private int m_Score = 0;

	/// <summary>
	/// 数字の桁を格納する配列
	/// </summary>
	[SerializeField]
	private List<int> m_Number = new List<int>();

	/// <summary>
	/// Imageリスト
	/// </summary>
	[SerializeField]
	private List<Image> m_NumberImages = new List<Image>();

	// Start is called before the first frame update
	void Start()
	{
		m_NumberImages.AddRange(GetComponentsInChildren<Image>());
		m_CreateNumberSprite = GetComponent<CreateNumberSprite>();
		m_CreateNumberSprite.Init();
		InitNumverImage();
		m_Score = GamePlayManager.Instance.Score;
		CalucGetDigits(m_Score);
	}

	// Update is called once per frame
	void Update()
	{
		m_Score = GamePlayManager.Instance.Score;
		CalucGetDigits(m_Score);
		SetNumber(m_Number);
	}

	/// <summary>
	/// 1桁ずつ計算で求める
	/// </summary>
	/// <param name="score"></param>
	private void CalucGetDigits(int score)
	{
		var digit = score;
		//要素数0には１桁目の値が格納
		m_Number.Clear();
		while (digit != 0)
		{
			score = digit % 10;
			digit = digit / 10;
			m_Number.Add(score);
		}
	}

	/// <summary>
	/// 数字の画像を設定
	/// </summary>
	/// <param name="numbers"></param>
	private void SetNumber(List<int> numbers)
	{
		InitNumverImage();
		for (int i = 0; i < numbers.Count; i++)
		{
			m_NumberImages[m_NumberImages.Count - (i + 1)].sprite = m_CreateNumberSprite.GetNumberSprite(numbers[i]);
		}
	}

	/// <summary>
	/// Imageを0画像に初期化
	/// </summary>
	private void InitNumverImage()
	{
		foreach (var image in m_NumberImages)
		{
			image.sprite = m_CreateNumberSprite.GetNumberSprite(0);
		}
	}
}
