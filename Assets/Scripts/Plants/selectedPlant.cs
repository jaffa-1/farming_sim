using UnityEngine;

public class selectedPlant : MonoBehaviour
{
    [SerializeField] Plant basePlant;
    Outline outline;

    private void Start()
    {
        Player.OnInteractableChanged += Player_OnInteractableChanged;
        outline = GetComponent<Outline>();
    }

    private void Player_OnInteractableChanged(object sender, Player.OnInteractableChangedEventArgs e)
    {
        if (e.plant == basePlant)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
    }
}
