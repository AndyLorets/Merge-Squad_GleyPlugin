using D2D;
using System;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;
public class XPPicker : MonoBehaviour
{
    [SerializeField] private float getDistance = 1f;
    [SerializeField] private float pickUpForce = 5f;
    [SerializeField] private GameObject pickUpVFX;

    private SexyOverlap overlap;

    public Action<float> OnPickUp;

    private float currentXPModifier = 1;

    private void Awake()
    {
        _xpPicker = this;
        overlap = GetComponent<SexyOverlap>();
    }
    private void FixedUpdate()
    {
        if (overlap.HasTouch)
        {
            foreach (var item in overlap.AllTouched)
            {
                if (item == null)
                {
                    continue;
                }

                var distance = Vector3.Distance(transform.position, item.transform.position);

                if (distance <= getDistance)
                {
                    var xp = item.GetComponent<XPPoint>().PickUp() * currentXPModifier;

                    _audioManager.PlayOneShot(_gameData.pickUpClip, .2f);
                    OnPickUp?.Invoke(xp);
                    PickVFX(item.transform.position);

                    return;
                }

                if (item.attachedRigidbody == null)
                {
                    return;
                }

                item.attachedRigidbody.isKinematic = true;

                float speed = pickUpForce - distance;
                speed = speed * Time.fixedDeltaTime;

                item.transform.position = Vector3.MoveTowards(item.transform.position, transform.position, speed);
            }
        }
    }
    private void PickVFX(Vector3 place)
    {
        var vfx = Instantiate(pickUpVFX, place, Quaternion.identity);

        Destroy(vfx, 2f);
    }
    public void IncreaseXPModifier()
    {
        currentXPModifier *= _gameData.xpModifierIncrease;
    }
}