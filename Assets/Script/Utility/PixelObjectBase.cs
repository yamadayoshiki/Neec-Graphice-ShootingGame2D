using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドッド絵オブジェクトのベース
/// </summary>
public class PixelObjectBase : MonoBehaviour
{
	private Vector3 m_CashPosition = Vector3.zero;

	void LateUpdate()
	{
		m_CashPosition = transform.localPosition;
		transform.localPosition = new Vector3(
						Mathf.RoundToInt(m_CashPosition.x),
						Mathf.RoundToInt(m_CashPosition.y),
						Mathf.RoundToInt(m_CashPosition.z)
				   );
	}

	void OnRenderObject()
	{
		transform.localPosition = m_CashPosition;
	}
}
