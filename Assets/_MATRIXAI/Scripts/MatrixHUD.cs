using UnityEngine;
using TMPro;

namespace MatrixAI
{
    public class MatrixHUD : MonoBehaviour
    {
        public MatrixFlightController controller;
        public TextMeshProUGUI modeText;
        public TextMeshProUGUI altText;
        public TextMeshProUGUI spdText;

        private void Update()
        {
            if (controller == null) return;

            if (modeText != null)
            {
                modeText.text = controller.mode.ToString().ToUpper();
                modeText.color = (controller.mode == MatrixFlightController.FlightMode.Autopilot) ? Color.green : Color.yellow;
            }

            if (altText != null)
            {
                altText.text = $"ALT {(int)controller.transform.position.y}M";
            }

            if (spdText != null)
            {
                // Accessing private field currentThrottle or calculating from velocity
                float speed = controller.GetComponent<Rigidbody>().linearVelocity.magnitude;
                spdText.text = $"SPD {(int)speed}";
            }
        }
    }
}