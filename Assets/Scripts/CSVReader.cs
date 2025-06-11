using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CSVReader : MonoBehaviour
{
    public static IEnumerator ReadEnemySpawnData(string fileName, System.Action<List<EnemySpawnData>> onLoaded)
    {
        var list = new List<EnemySpawnData>();
        var path = Path.Combine(Application.streamingAssetsPath, fileName);

        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("CSV load failed: " + www.error);
            onLoaded?.Invoke(list);
            yield break;
        }

        string[] lines = www.downloadHandler.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var parts = line.Split(',');
            if (parts.Length < 3) continue;

            EnemySpawnData data = new EnemySpawnData()
            {
                Wave = int.Parse(parts[0]),
                Enemy = int.Parse(parts[1]),
                EnemyCount = int.Parse(parts[2])
            };
            list.Add(data);
        }

        onLoaded?.Invoke(list);
    }
}
