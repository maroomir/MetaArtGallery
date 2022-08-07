using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    public interface ISpawner
    {
        public void Init();
        public void UpdateArts(DirectoryInfo pDirectory);
        public void UpdateArts([NotNull] IEnumerable<FileInfo> pFiles);
        public void SpawnArt(FileInfo pFile, int nIndex);
    }

    public interface IArt
    {
        public Texture Texture { get; }
        public string Tag { get; }
        public string AccessPath { get; }
        public void UpdateArt(FileInfo pFile);
    }
}