using System;
using CoreProject.Singleton;
using CoreProject.Resource;
using UnityEngine;
using System.Collections.Generic;

public class StateFactory : SingletonClass<StateFactory>
{
    internal MenuView CreateMenuView(Transform parent = null)
    {
        return GameObject.Instantiate(ResourceManager.Instance.GetResource<GameObject>("MenuView"), parent).GetComponent<MenuView>();
    }
    
    internal StoryView CreateStoryView(Transform parent = null)
    {
        return GameObject.Instantiate(ResourceManager.Instance.GetResource<GameObject>("StoryView"), parent).GetComponent<StoryView>();
    }

    internal SinglePlayView CreateSinglePlayView(Transform parent = null)
    {
        return GameObject.Instantiate(ResourceManager.Instance.GetResource<GameObject>("SinglePlayView"), parent).GetComponent<SinglePlayView>();
    }
}

public class PlayerFactory : SingletonClass<PlayerFactory>
{
    public Player CreatePlayer(Transform parent = null)
    {
        return GameObject.Instantiate(ResourceManager.Instance.GetResource<GameObject>("Player"), parent).GetComponent<Player>();
    }
}

public class ItemFactory : SingletonClass<ItemFactory>
{
    public CharacterItem CreateCharacterItem(char character, Transform parent = null)
    {
        CharacterItem numericItem = GameObject.Instantiate(ResourceManager.Instance.GetResource<GameObject>("CharacterItem"), parent).GetComponent<CharacterItem>();
        numericItem.Character = character;
        return numericItem;
    }

    public Item CreateItem(string key, Transform parent = null)
    {
        Item item = null;
        if (String.IsNullOrEmpty(key))
        {
            return null;
        }
        string[] arr = key.Split('_');
        if (arr.Length < 2)
        {
            return null;
        }
        switch (arr[0])
        {
            case CharacterItem.ITEM_PREFIX:
                {
                    item = CreateCharacterItem(Char.Parse(arr[1]), parent);
                    break;
                }
        }
        return item;
    }

    public string GetValue(string key)
    {
        if (String.IsNullOrEmpty(key))
        {
            return "";
        }
        string[] arr = key.Split('_');
        if (arr.Length < 2)
        {
            return "";
        }
        return arr[1];
    }

    public LevelData CreateLevelData(LevelDifficult levelDifficult)
    {
        int bagSize = ResourceManager.Instance.GetResource<LevelDifficultData>("LevelDifficultData").GetBagSize(levelDifficult);
        List<char> items = new List<char>(bagSize);
        do
        {
            string characters = "0123456789";
            char randomChar = characters[UnityEngine.Random.Range(0, characters.Length)];
            items.Add(randomChar);
        } while (items.Count < bagSize);
        LevelData levelData = new LevelData(items);
        return levelData;
    }
}