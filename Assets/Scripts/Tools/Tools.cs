using UnityEngine;

public class Tools : MonoBehaviour, ICanInteract
{
    public enum toolType
    {
        wateringCan
    }

    [SerializeField] toolType thistoolType;
    public void Interact(Player player)
    {
        //pickup watering can
        setParent(player.GetInteractSpawn());
        player.SetEquippedTool(this);
    }

    public void setParent(Transform transform)
    {
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = Vector3.zero;
    }

    public bool ToolIsWaterCan()
    {
        if (thistoolType == toolType.wateringCan)
        {
            return true;
        }
        return false;
    }
}
