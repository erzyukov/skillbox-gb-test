using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    private ParticleSystem giftPickUp;

    private void Awake()
    {
        giftPickUp = Instantiate<GameObject>(Resources.Load<GameObject>("Effects/PickUpGiftParticles"), transform).GetComponent<ParticleSystem>();
    }

    public void GiftPickUp()
    {
        giftPickUp.Stop();
        giftPickUp.Play();
    }

}
