using UnityEngine;

public class Plant : MonoBehaviour, ICanInteract
{
    [SerializeField] PlantSO plantSO;
    enum GrowthLevel
    {
        seed,
        halfDeveloped,
        fullDeveloped
    }

    GrowthLevel currentGrowthLevel;

    private void Start()
    {
        currentGrowthLevel = GrowthLevel.seed;
    }

    public void Interact(Player player)
    {
        setParent(player.GetInteractSpawn());
        player.SetEquippedPlant(this);
    }

    public void setParent(Transform transform)
    {
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = Vector3.zero;
    }

    public bool PlantIsSeed()
    {
        if (currentGrowthLevel == GrowthLevel.seed)
        {
            return true;
        }
        return false;
    }



    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
