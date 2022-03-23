using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchImage : MonoBehaviour
{
	[SerializeField]
	private Image m_ImageOne = null;

	[SerializeField]
	private Image m_ImageTwo = null;

	private void Start()
	{
		m_ImageOne.enabled = true;
		m_ImageTwo.enabled = false;
	}

	/// <summary>
	/// ‰æ‘œØ‚è‘Ö‚¦
	/// </summary>
	public void Switch()
	{
		m_ImageOne.enabled = !m_ImageOne.enabled;
		m_ImageTwo.enabled = !m_ImageTwo.enabled;
	}
}
