using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Common
{
    public static class ArrayFunctions
    {
        public static T[] Slice<T>(this T[] pSource, int nStart, int nEnd)
        {
            if (nEnd < 0)
                nEnd = pSource.Length + nEnd;
            int nLength = nEnd - nStart;
            T[] pResult = new T[nLength];
            for (int i = 0; i < nLength; i++)
                pResult[i] = pSource[i + nStart];
            return pResult;
        }
    }

    public static class VectorFunctions
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

    public static class ImageFunctions
    {
        public static readonly string[] Extensions = {".bmp", ".png", ".jpg"};

        public static FileInfo[] GetImages(this DirectoryInfo pDirectory)
        {
            FileInfo[] pResults = pDirectory.GetFiles();
            return pResults.Where(source => Extensions.Contains(source.Extension)).ToArray();
        }

        public static FileInfo[] GetImagesAll(this DirectoryInfo pDirectory)
        {
            List<FileInfo> pResults = new List<FileInfo>();
            foreach (DirectoryInfo pDir in pDirectory.GetDirectories())
                pResults.AddRange(pDir.GetImagesAll());
            pResults.AddRange(pDirectory.GetImages());
            return pResults.ToArray();
        }

        public static Texture2D LoadTexture(FileInfo pFile)
        {
            byte[] pBitmapData = File.ReadAllBytes(pFile.FullName);
            Texture2D pTexture = new Texture2D(0, 0);
            if (pBitmapData.Length > 0)
                pTexture.LoadImage(pBitmapData);
            return pTexture;
        }

        public static Sprite LoadSprite(FileInfo pFile)
        {
            Texture2D pTexture = LoadTexture(pFile);
            return Sprite.Create(pTexture, new Rect(0, 0, pTexture.width, pTexture.height), new Vector2(0.5f, 0.5f));
        }

        public static void LoadImage(ref Image pImage, FileInfo pFile, float fMaxWidth = 1080f,
            float fMaxHeight = 1080f)
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

        public static Texture2D DecodeTexture(string strImageEncoded)
        {
            byte[] pBitmapData = Convert.FromBase64String(strImageEncoded);
            Texture2D pTexture = new Texture2D(0, 0);
            if (pBitmapData.Length > 0)
                pTexture.LoadImage(pBitmapData);
            return pTexture;
        }

        public static Sprite DecodeSprite(string strImageEncoded)
        {
            Texture2D pTexture = DecodeTexture(strImageEncoded);
            return Sprite.Create(pTexture, new Rect(0, 0, pTexture.width, pTexture.height), new Vector2(0.5f, 0.5f));
        }

        public static void DecodeImage(ref Image pImage, string strImageEncoded, float fMaxWidth = 1080f,
            float fMaxHeight = 1080f)
        {
            Sprite pSprite = DecodeSprite(strImageEncoded);
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

    public static class RestFunctions
    {
        public static class Patterns
        {
            public static string ImageExtractor = "\"([\\w\\+\\/\\-\\=]+)\"";
        }
        
        public static string[] SplitImages(string strResult)
        {
            // ['A', 'B', 'C']
            Regex pRegex = new Regex(Patterns.ImageExtractor, RegexOptions.None);
            MatchCollection pMatches = pRegex.Matches(strResult);
            string[] pResults = new string[pMatches.Count];
            for (int i = 0; i < pMatches.Count; i++)
                pResults[i] = pMatches[i].Groups[1].Value;
            return pResults;
        }

        public static bool IsEncodedImages(string strContents)
        {
            // ['A', 'B', 'C']
            Regex pRegex = new Regex(Patterns.ImageExtractor, RegexOptions.None);
            MatchCollection pMatches = pRegex.Matches(strContents);
            return pMatches.Count > 0;
        }

        public static Texture2D[] SplitTextures(string strContents)
        {
            // ['A', 'B', 'C']
            Regex pRegex = new Regex(Patterns.ImageExtractor, RegexOptions.None);
            MatchCollection pMatches = pRegex.Matches(strContents);
            Texture2D[] pResult = new Texture2D[pMatches.Count];
            for (int i = 0; i < pMatches.Count; i++)
            {
                StringBuilder pStrBuildEncoded = new StringBuilder(pMatches[i].Value, pMatches[i].Value.Length);
                pStrBuildEncoded.Replace("\r\n", string.Empty);
                pStrBuildEncoded.Replace(" ", string.Empty);
                pResult[i] = ImageFunctions.DecodeTexture(pStrBuildEncoded.ToString());
            }

            return pResult;
        }
    }

    public static class MathFunctions
    {
        public static int Sign(float fX) => fX == 0 ? 0 : (fX < 0) ? -1 : 1;
    }

    public static class CurateFunctions
    {
        public static T[] RandomCurate<T>(T[] pSources, int nArtNum)
        {
            if (pSources.Length < nArtNum)
                throw new Exception($"Not enough arts compared to the exhibition ({pSources.Length} < {nArtNum})");
            Random pRandom = new Random();
            IOrderedEnumerable<T> pRandomizedSource = pSources.OrderBy(item => pRandom.Next());
            T[] pResult = pRandomizedSource.ToArray();
            return pResult.Take(nArtNum).ToArray();
        }
    }
}