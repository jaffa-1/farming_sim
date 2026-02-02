using UnityEngine;

public class PlantSite : MonoBehaviour, ICanInteract
{

    Plant activePlant;


    public void Interact(Player player)
    {
        if (player.HasEquippedPlant())
        {
            //player is carrying a plant
            SetPlant(player.GetEquippedPlant());    
        }
        if (player.HasEquippedTool())
        {
            //water plant and make it start growing
        }
        else
        {
            //player is not carrying a plant
        }
    }

    public void SetPlant(Plant plant)
    {
        if (plant.PlantIsSeed())
        {
            activePlant = plant;
            activePlant.setParent(this.transform);
        }
    }
}
