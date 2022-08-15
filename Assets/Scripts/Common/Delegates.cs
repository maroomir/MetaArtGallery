using System;
using UnityEngine;

namespace Common
{
    public class TextureArgs : EventArgs
    {
        public Texture Texture { get; }
        public string Tag { get; }
        public string AccessPath { get; }

        public TextureArgs(Texture pTexture, string strTag, string strTexturePath)
        {
            Texture = pTexture;
            Tag = strTag;
            AccessPath = strTexturePath;
        }
    }

    public delegate void DeliveryTextureHandler(object pSender, TextureArgs pArgs);
}