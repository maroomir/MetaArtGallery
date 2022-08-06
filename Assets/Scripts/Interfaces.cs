using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEngine.EventSystems;

public interface ISpawner
{
    public void Init();
    public void UpdateArts(DirectoryInfo pDirectory);
    public void UpdateArts([NotNull] IEnumerable<FileInfo> pFiles);
    public void SpawnArt(FileInfo pFile, int nIndex);
}

public interface IArt : IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public void UpdateArt(FileInfo pFile);
}