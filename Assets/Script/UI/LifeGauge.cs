using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LifeGauge : MonoBehaviour
{
	/// <summary>
	/// ‘Ï‹v’lƒNƒ‰ƒX
	/// </summary>
	[SerializeField]
	private Life m_Life = null;
	public Life Life
	{
		get { return m_Life; }
		set
		{
			m_Life = value;
			m_GaugeObjects.ForEach((obj) => obj.SetActive(true));
		}
	}

	/// <summary>
	/// ƒQ[ƒW‰æ‘œ
	/// </summary>
	[SerializeField]
	private Image m_GaugeImage = null;

	[SerializeField]
	private List<GameObject> m_GaugeObjects = new List<GameObject>();

	private void Start()
	{
		if (m_Life == null)
		{
			m_GaugeObjects.ForEach((obj) => obj.SetActive(false));
		}
	}

	private void Update()
	{
		if (Life == null) return;
		//‰æ‘œ‚É‘Ï‹v’l‚ÌŠ„‡‚ğ“n‚·
		m_GaugeImage.fillAmount = Life.LifeRate;
	}
}
