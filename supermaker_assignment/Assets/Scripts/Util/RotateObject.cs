using UnityEngine;

namespace Util
{
    public sealed class RotateObject : MonoBehaviourBase
    {
        public float rotationSpeed = 50f; // 회전 속도 조절

        private void Update()
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }
}
