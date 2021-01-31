using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "A / Create Level Diffucult Data", fileName = "LevelDifficultData.asset")]
public class LevelDifficultData : ScriptableObject
{
    public List<LevelDifficultMap> LevelDifficultMaps;

    public int GetBagSize(LevelDifficult levelDifficult)
    {
        foreach (var item in LevelDifficultMaps)
        {
            if (item.LevelDifficult == levelDifficult)
            {
                return item.BagSize;
            }
        }
        return 2;
    }
}

public class LevelData
{
    public List<char> Items;
    public List<string> Keys;
    public int BagSize => Items.Count;

    public LevelData(List<char> items)
    {
        Items = items;
        Keys = new List<string>();
        foreach (var item in Items)
        {
            Keys.Add(CharacterItem.ITEM_PREFIX + "_" + item);
        }
    }
}

[Serializable]
public enum LevelDifficult
{
    Easy,
    Medium,
    Hard,
    VeryHard
}

[Serializable]
public class LevelDifficultMap
{
    public LevelDifficult LevelDifficult;
    public int BagSize;
}

