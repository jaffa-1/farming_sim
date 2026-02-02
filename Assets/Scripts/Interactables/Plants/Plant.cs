using UnityEngine;

public class Plant : MonoBehaviour,ICanInteract
{
    public void DestroySelf()
    {
        Destroy(gameObject);    
    }

    public void Interact()
    {
        
    }
}
