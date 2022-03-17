using System;
using System.Collections.Generic;
using UnityEngine;

public static class MyScreen
{
	/// <summary>
	/// メインカメラ
	/// </summary>
	private static Camera m_MainCamera = null;

	/// <summary>
	/// 右上座標
	/// </summary>
	public static Vector3 TopRight;

	/// <summary>
	/// 左下座標
	/// </summary>
	public static Vector3 BottomLeft;

	/// <summary>
	/// 初期化
	/// </summary>
	public static void Initialize()
	{
		//メインカメラを取得
		m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

		//ビューポート空間の座標をワールド空間座標に変換
		TopRight = m_MainCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, m_MainCamera.nearClipPlane));
		BottomLeft = m_MainCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, m_MainCamera.nearClipPlane));
	}
}

