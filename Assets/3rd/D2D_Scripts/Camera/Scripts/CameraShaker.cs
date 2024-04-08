using UnityEngine;
using Cinemachine;

namespace D2D.Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShaker : MonoBehaviour
    {
        private CinemachineVirtualCamera _vcam;
        private CinemachineBasicMultiChannelPerlin _cameraNoise;

        private ShakeProfile _shakeProfile;
        private float _shakeTimer;

        protected void Shake(ShakeProfile profile)
        {
            if (_vcam == null)
            {
                _vcam = GetComponent<CinemachineVirtualCamera>();
                _cameraNoise = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
            
            if (profile == null)
                return;
            
            _shakeProfile = profile;
            
            Invoke(nameof(ShakeUsingProfile), profile.delay);
        }

        private void ShakeUsingProfile()
        {
            _cameraNoise.m_FrequencyGain = _shakeProfile.frequency;
            _cameraNoise.m_AmplitudeGain = _shakeProfile.intensity;

            _shakeTimer = _shakeProfile.time;
        }

        private void Update()
        {
            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;
                float t = 1 - _shakeTimer / _shakeProfile.time;
                _cameraNoise.m_AmplitudeGain = Mathf.Lerp(_shakeProfile.intensity, 0, t);
            }
        }
    }
}
