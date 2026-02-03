using UnityEngine;

public class SeedStorage : MonoBehaviour,ICanInteract
{
    [SerializeField]PlantSO plantSO;

    public void Interact(Player player)
    {
        if (!player.UpdatePlantInventory(plantSO))
        {
            GameObject plantGO = Instantiate(plantSO.plantPrefab, player.GetInteractSpawn());
            plantGO.transform.localPosition = Vector3.zero;
            player.SetEquippedPlant(plantGO.GetComponent<Plant>());
        }

    }
    

}
