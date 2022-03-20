using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LifeGauge : MonoBehaviour
{
	/// <summary>
	/// 耐久値クラス
	/// </summary>
	[SerializeField]
	public Life Life = null;

	/// <summary>
	/// ゲージ画像
	/// </summary>
	[SerializeField]
	private Image m_GaugeImage = null;

	private void Update()
	{
		//画像に耐久値の割合を渡す
		m_GaugeImage.fillAmount = Life.LifeRate;
	}
}
