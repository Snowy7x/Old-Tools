using System;
using System.Collections;
using Snowy.Input;
using UnityEngine;

namespace Systems.FPS_Movement.Scripts
{
    public class FpCamera : SuperCamera
    {
        [Header("References")]
        [SerializeField] private Camera cam;
        [SerializeField] private Transform playerBody;
        [SerializeField] private BasicMovement movement;
        
        [Space]
        public bool useFullBody = false;
        public Transform pivot;
        
        [Space(10)]
        [Header("Camera")]
        [SerializeField] private float mouseSensitivity = 100f;
        [SerializeField] private float yAngleClamp = 90f;
        [SerializeField] private float defaultFOV = 60f;

        [Space(10)] 
        [Header("Bobbing")]
        [SerializeField] private float bobbingSpeed = 14f;
        [SerializeField] private float bobbingAmount = 0.05f;
        private float mouseX;
        private float mouseY;
        
        public event Action OnCameraUpdate;
        #region private vars
        
        private float _xRotation;
        private bool _isFovChanged;
        private float _targetFov;
        private float _currentFov;
        private float _timer;
        private float _midpoint;
        
        Vector3 _defaultCamRot;

        #endregion

        protected override void Start()
        {
            if (!playerBody) playerBody = transform.parent;
            if (!cam) cam = GetComponentInChildren<Camera>();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            cam.fieldOfView = defaultFOV;
            _midpoint = transform.localPosition.y;
            
            mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", mouseSensitivity);
            
            base.Start();
        }

        protected override void Initialize(InputManager input)
        {
            base.Initialize(input);
            input.OnLook += Look;
        }

        private void Look(Vector2 lookInput) 
        {
            mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
            mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        }

        public void ChangeFOV(float target, float transition)
        {
            if (_isFovChanged && Math.Abs(target - _targetFov) < 0.1f) return;
            if (Math.Abs(cam.fieldOfView - target) < 0.1f) return;
            StartCoroutine(ChangeFOVCoroutine(target, transition));
        } 
        
        IEnumerator ChangeFOVCoroutine(float target, float transition)
        {
            _isFovChanged = true;
            _targetFov = target;
            _currentFov = cam.fieldOfView;
            
            float time = 0f;
            while (time < transition)
            {
                time += Time.deltaTime;
                cam.fieldOfView = Mathf.Lerp(_currentFov, _targetFov, time / transition);
                yield return null;
            }
            
            _isFovChanged = false;
        }
        
        public void ZoomIn(float zoom, float transition)
        {
            float target = zoom;
            ChangeFOV(target, transition);
        }
        
        public void ZoomOut(float transition)
        {
            ChangeFOV(defaultFOV, transition);
        }

        private void Update()
        {
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -yAngleClamp, yAngleClamp);

            if (!useFullBody)
            {
                transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            }
            else
            {
                // Rotate around the pivot
                pivot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            }
            playerBody.Rotate(Vector3.up * mouseX);
            
            // Bobbing:
            Vector3 localPosition = transform.localPosition;
            if (movement.IsMoving() && movement.IsGrounded())
            {
                _timer += Time.deltaTime * bobbingSpeed;
                transform.localPosition = new Vector3(localPosition.x,
                    _midpoint + Mathf.Sin(_timer) * bobbingAmount, localPosition.z);
            }
            else
            {
                _timer = 0;
                transform.localPosition = new Vector3(localPosition.x,
                    Mathf.Lerp(localPosition.y, _midpoint, bobbingSpeed * Time.deltaTime), localPosition.z);
            }
            
            OnCameraUpdate?.Invoke();
        }
    }
}