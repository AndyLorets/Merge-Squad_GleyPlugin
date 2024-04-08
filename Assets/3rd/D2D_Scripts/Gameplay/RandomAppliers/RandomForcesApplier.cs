using D2D.Utilities;
using D2D.Utils;
using UnityEngine;

namespace D2D.Gameplay
{
    [RequireComponent(typeof(Physics))]
    public class RandomForcesApplier : MonoBehaviour
    {
        [SerializeField] private float _forceRange;
        [SerializeField] private Vector3 _forceAxes = Vector3.one;

        [Space]
        
        [SerializeField] private float _torqueRange;
        [SerializeField] private Vector3 __torqueAxes = Vector3.one;

        private void Start()
        {
            var body = GetComponent<Rigidbody>();
            
            body.AddForce(DMath.RandomPointInsideBox(_forceRange).Multiply(_forceAxes), 
                ForceMode.Impulse);
            
            body.AddTorque(DMath.RandomPointInsideBox(_torqueRange).Multiply(__torqueAxes), 
                ForceMode.Impulse);
        }
    }
}