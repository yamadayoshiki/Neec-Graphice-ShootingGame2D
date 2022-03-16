using UnityEngine;
using UnityEditor;
using YY.Sound;

public class RegisterSoundDataWindow : EditorWindow
{
	/// <summary>
	/// サウンドの種類
	/// </summary>
	SoundType m_SoundType = SoundType.BGM;

	/// <summary>
	/// オーディオ読み込み先ファイルパス
	/// </summary>
	private string m_LoadFilePath = "Sound/";

	/// <summary>
	/// ファイルの名前
	/// </summary>
	private string m_FileName = "SoundList";

	/// <summary>
	/// 保存先ファイルパス
	/// </summary>
	private string m_ExportFilePath = "Assets/Resources/Data/Sound/";

	/// <summary>
	/// Enumの名前
	/// </summary>
	private string m_EnumFileName = "SoundID";

	/// <summary>
	/// enumファイルの作成場所ばでのパス
	/// </summary>
	private string m_EnumFileExportPath = "Assets/Script/Sound/Enum/";


	[MenuItem("YY-Tool/Register SoundData")]
	private static void Open()
	{
		GetWindow<RegisterSoundDataWindow>("サウンドデータの登録");
	}


	private void OnGUI()
	{
		//読み込むファイルを指定する
		EditorGUILayout.LabelField("読み込むオーディオファイルのパス");
		m_LoadFilePath = EditorGUILayout.TextField(m_LoadFilePath);

		//サウンドの種類を選択
		EditorGUILayout.LabelField("サウンドの種類");
		m_SoundType = (SoundType)EditorGUILayout.EnumPopup("SoundType", m_SoundType);

		//保存するファイルの名前を決める
		EditorGUILayout.LabelField("保存するファイルの名前");
		m_FileName = EditorGUILayout.TextField(m_FileName);

		//サウンドデータリスト保存先を記入する
		EditorGUILayout.LabelField("サウンドデータリストの保存先ファイルパス");
		m_ExportFilePath = EditorGUILayout.TextField(m_ExportFilePath);

		//保存するファイルの名前を決める
		EditorGUILayout.LabelField("Enumの名前");
		m_EnumFileName = EditorGUILayout.TextField(m_EnumFileName);

		//Enumの保存先を記入する
		EditorGUILayout.LabelField("Enumの保存先ファイルパス");
		m_EnumFileExportPath = EditorGUILayout.TextField(m_EnumFileExportPath);

		//ボタンにサウンドデータリストを作成機能をつける
		if (GUILayout.Button("登録"))
		{
			//サウンドデータリストを作成
			var soundDatas = RegisterSoundData.Register(m_FileName, m_ExportFilePath, m_LoadFilePath, m_SoundType);
			//サウンドデータリストの作成に失敗したら処理しない
			if (soundDatas == null) return;
			//サウンドIDをEnumで作成
			RegisterSoundData.CreateEnum(m_EnumFileName, soundDatas, m_EnumFileExportPath);
		}

	}
}
