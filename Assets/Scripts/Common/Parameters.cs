using System.IO;
using UnityEngine;

namespace Common
{
    public static class GlobalParameter
    {
        public static Texture SelectedTexture = null;
        public static string SelectedTextureName = string.Empty;
        public static string SelectedTexturePath = string.Empty;
        public static readonly string ScenePath = Path.Combine(Application.dataPath, "Scenes");
        public static readonly string ResourcePath = Path.Combine(Application.dataPath, "Resources");
    }
}