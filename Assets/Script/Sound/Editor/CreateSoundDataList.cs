using UnityEngine;
using UnityEditor;
using YY.Sound;

public static class CreateSoundDataList
{
	/// <summary>
	/// サウンドデータリストを作成
	/// </summary>
	/// <param name="filePath"> 保存先 </param>
	/// <param name="fileName"> ファイルの名前 </param>
	/// <returns></returns>
	public static SoundDataList Create(string filePath, string fileName)
	{
		var data = ScriptableObject.CreateInstance<SoundDataList>();
		var assetsName = $"{filePath}{fileName}.asset";
		AssetDatabase.CreateAsset(data, assetsName);
		AssetDatabase.Refresh();
		return data;
	}
}
