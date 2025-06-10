using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CSVReader
{
    public static List<EnemySpawnData> ReadEnemySpawnData(string fileName)
    {
        var list = new List<EnemySpawnData>();
        var path = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogError("CSV file not found: " + path);
            return list;
        }

        var lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split(',');
            if (parts.Length < 3) continue;

            EnemySpawnData data = new EnemySpawnData()
            {
                Wave = int.Parse(parts[0]),
                Enemy = int.Parse(parts[1]),
                EnemyCount = int.Parse(parts[2])
            };
            list.Add(data);
        }

        return list;
    }
}
