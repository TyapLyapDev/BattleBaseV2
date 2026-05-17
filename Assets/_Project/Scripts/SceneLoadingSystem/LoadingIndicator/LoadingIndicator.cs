using System.Collections;
using UnityEngine;

namespace BattleBase.SceneLoadingService
{
    public class LoadingIndicator : MonoBehaviour
    {
        private const float FullCircleDegrees = 360f;
        private const bool IsRotation = true;

        [SerializeField] private int _jumps = 8;
        [SerializeField] private float _cycleDuration = 0.4f;

        private Transform _transform;
        private float _anglePerJump;
        private float _delayBetweenJumps;
        private WaitForSecondsRealtime _timeWait; 

        private void Awake()
        {
            _transform = transform;
            _anglePerJump = FullCircleDegrees / _jumps;
            _delayBetweenJumps = _cycleDuration / _jumps;
            _timeWait = new(_delayBetweenJumps);
        }

        private void OnEnable() =>
            StartCoroutine(RotateByJumpsRoutine());

        private IEnumerator RotateByJumpsRoutine()
        {
            while (IsRotation)
            {
                for (int i = 0; i < _jumps; i++)
                {
                    _transform.Rotate(0, 0, -_anglePerJump);

                    yield return _timeWait;
                }
            }
        }
    }
}