using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LifeGauge : MonoBehaviour
{
	/// <summary>
	/// �ϋv�l�N���X
	/// </summary>
	[SerializeField]
	public Life Life = null;

	/// <summary>
	/// �Q�[�W�摜
	/// </summary>
	[SerializeField]
	private Image m_GaugeImage = null;

	private void Update()
	{
		//�摜�ɑϋv�l�̊�����n��
		m_GaugeImage.fillAmount = Life.LifeRate;
	}
}
