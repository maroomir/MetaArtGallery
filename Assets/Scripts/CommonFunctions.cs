
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public static class VectorFactory
{
    public static Vector3 EachDivide(this Vector3 pSource, Vector3 pTarget)
    {
        return new Vector3(pSource.x / pTarget.x, pSource.y / pTarget.y, pSource.z / pTarget.z);
    }

    public static Vector3 EachMultiply(this Vector3 pSource, Vector3 pTarget)
    {
        return new Vector3(pSource.x * pTarget.x, pSource.y * pTarget.y, pSource.z * pTarget.z);
    }
}

public static class ImageFactory
{
    public static readonly string[] Extensions = {".bmp", ".png", ".jpg"};

    public static FileInfo[] GetImages(this DirectoryInfo pDirectory)
    {
        FileInfo[] pResults = pDirectory.GetFiles();
        return pResults.Where(source => Extensions.Contains(source.Extension)).ToArray();
    }
    
    public static Texture2D LoadTexture(FileInfo pFile)
    {
        byte[] pBytes = File.ReadAllBytes(pFile.FullName);
        Texture2D pTexture = new Texture2D(0, 0);
        if (pBytes.Length > 0)
            pTexture.LoadImage(pBytes);
        return pTexture;
    }

    public static Sprite LoadSprite(FileInfo pFile)
    {
        Texture2D pTexture = LoadTexture(pFile);
        return Sprite.Create(pTexture, new Rect(0, 0, pTexture.width, pTexture.height), new Vector2(0.5f, 0.5f));
    }

    public static void LoadImage(ref Image pImage, FileInfo pFile, float fMaxWidth = 1080f, float fMaxHeight = 1080f)
    {
        Sprite pSprite = LoadSprite(pFile);
        if (pSprite.texture.width > fMaxWidth)
            pImage.rectTransform.sizeDelta =
                new Vector2(fMaxWidth, fMaxWidth / pSprite.texture.width * pSprite.texture.height);
        else if (pSprite.texture.height > fMaxHeight)
            pImage.rectTransform.sizeDelta =
                new Vector2(fMaxHeight / pSprite.texture.height * pSprite.texture.width, fMaxHeight);
        else
            pImage.rectTransform.sizeDelta = new Vector2(pSprite.texture.width, pSprite.texture.height);
        pImage.sprite = pSprite;
    }
}