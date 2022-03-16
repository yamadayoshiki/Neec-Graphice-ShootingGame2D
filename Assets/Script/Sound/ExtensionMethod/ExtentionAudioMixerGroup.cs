using UnityEngine;
using UnityEngine.Audio;

public static class ExtentionAudioMixerGroup
{
	//パラメータ名は"Group名＋Volume"の命名規則で設定する
	
	/// <summary>
	/// オーディオミキサーグループの音量を取得
	/// </summary>
	/// <param name="audioMixerGroup"> 取得したいグループ </param>
	/// <returns></returns>
	public static float GetVolumeByLinear(this AudioMixerGroup audioMixerGroup)
	{
		float decibel;
		audioMixerGroup.audioMixer.GetFloat(audioMixerGroup.name + "Volume", out decibel);
		return Mathf.Pow(10.0f, decibel / 20.0f);
	}

	/// <summary>
	/// オーディオミキサーグループの音量を設定
	/// </summary>
	/// <param name="audioMixerGroup"> 取得したいグループ </param>
	/// <param name="linearVolume"> 設定する値 </param>
	public static void SetVolumeByLinear(this AudioMixerGroup audioMixerGroup, float linearVolume)
	{
		float decibel = 20.0f * Mathf.Log10(linearVolume);
		if (float.IsNegativeInfinity(decibel))
		{
			decibel = -96f;
		}

		audioMixerGroup.audioMixer.SetFloat(audioMixerGroup.name + "Volume", decibel);
	}
}

