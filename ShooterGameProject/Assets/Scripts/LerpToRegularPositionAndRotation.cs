using UnityEngine;

public class LerpToRegularPositionAndRotation : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float positionLerpSpeed = 8f;
        [SerializeField, Min(0f)] private float rotationLerpSpeed = 8f;
        
        private Transform _cachedTransform;

        private Vector3 _regularLocalPosition;
        private Quaternion _regularLocalRotation;
        
        private void Awake()
        {
            _cachedTransform = transform;
            _regularLocalRotation = _cachedTransform.localRotation;
            _regularLocalPosition = _cachedTransform.localPosition;
        }

        private void LateUpdate()
        {
            var position = _cachedTransform.localPosition;
            var rotation = _cachedTransform.localRotation;

            rotation = Quaternion.Lerp(rotation, _regularLocalRotation, Time.deltaTime * rotationLerpSpeed);
            position = Vector3.Lerp(position, _regularLocalPosition, Time.deltaTime * positionLerpSpeed);

            _cachedTransform.localRotation = rotation;
            _cachedTransform.localPosition = position;
        }
    }
