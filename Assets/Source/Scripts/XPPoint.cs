
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class XPPoint : MonoBehaviour
{
    [SerializeField] private float xp;

    public void Init(Vector3 originPoint, Vector3 playerPoint)
    {
        Vector3 force = (Mathf.Sign(playerPoint.x - originPoint.x) == -1 ? Vector3.right : Vector3.left) + Vector3.up;
        
        GetComponent<Rigidbody>().AddForce(force * _gameData.pickUpFlyForce);

        Invoke("ChangeLayerToXP", _gameData.timeBeforeXPActivate);
    }
    public float PickUp()
    {
        Destroy(gameObject);

        return xp;
    }

    private void ChangeLayerToXP()
    {
        gameObject.layer = LayerMask.NameToLayer(_gameData.XPLayer);
    }
}