using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TiledCharacter))]
public class Touchplate : MonoBehaviour
{
    public GameObject Target;

    private bool locked;

    void Start()
    {
        locked = false;
    }

    public void ActivateEffect()
    {
        if (!locked)
        {
            locked = true;
            if (Target != null)
            {
                Target.SendMessage("ActivateEffect");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            ActivateEffect();
        }
    }
}
