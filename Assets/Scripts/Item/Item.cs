using UnityEngine;

public class Item : MonoBehaviour, ICollectableItem
{
    public virtual string GetKey()
    {
        return "";
    }
}
