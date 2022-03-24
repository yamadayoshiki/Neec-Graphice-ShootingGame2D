using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteRederManager : SingletonMonoBehaviour<SpriteRederManager>
{
    [SerializeField]
    SpritePathList spriteDateList;

    Dictionary<string, SpriteDate> spriteDateDic = new Dictionary<string, SpriteDate>();

    [SerializeField]
    SpriteReader ioTest;

    


    private void Awake()
    {
        foreach (SpriteDate Date in spriteDateList.spritePaths)
        {
            spriteDateDic.Add(Date.sprite.name, Date);
        }
    }

    public Texture GetTexture(string spriteName)
    {
        SpriteDate data = spriteDateDic[spriteName];

        Texture2D tex = ioTest.SpriteRead(data.spritePath + "/" + spriteName + data.extension);
        return tex;
    }

    public Texture2D GetTexture2D(string imageName)
    {
        SpriteDate data = spriteDateDic[imageName];
       

        Texture2D tex = ioTest.SpriteRead(data.spritePath + "/" + imageName + data.extension );
        return tex;
    }
}
