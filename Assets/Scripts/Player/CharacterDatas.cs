using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatas.asset", menuName = "A/Create Character Datas")]
public class CharacterDatas : ScriptableObject
{
    public List<CharacterData> CharacterDataList;

    public CharacterData GetCharacterData(string name)
    {
        foreach (var item in CharacterDataList)
        {
            if (item.Name == name)
            {
                return item;
            }
        }
        return null;
    }
}
