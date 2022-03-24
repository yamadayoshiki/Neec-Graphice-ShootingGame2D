using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class SpriteReader : MonoBehaviour
{
    public Texture2D SpriteRead(string fileName)
    {
        string path;
        if (Application.isEditor)
        {
            path = Application.dataPath + "/_ExternalAssets" + "/" + fileName;
        }
        else
        {
            path = Application.dataPath + "/../" + fileName;
        }


        Texture2D tex = ReadPng(path);
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        return tex;
    }


    byte[] ReadPngFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }

    Texture2D ReadPng(string path)
    {
        byte[] readBinary = ReadPngFile(path);
        Texture2D texture = new Texture2D(8, 8);
        texture.LoadImage(readBinary);
        return texture;
    }
}
