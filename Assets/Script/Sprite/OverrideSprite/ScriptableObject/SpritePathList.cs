using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SpriteList", menuName = "Create/SpritePathList")]
public class SpritePathList : ScriptableObject
{
    public List<SpriteDate> spritePaths = new List<SpriteDate>();
}

[Serializable]
public class SpriteDate
{
    [Tooltip("ファイル名は書かない")]
    public string spritePath = "Txetures/";
    public Texture sprite;
    public string extension = ".png";
}
