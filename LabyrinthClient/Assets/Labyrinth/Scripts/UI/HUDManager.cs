using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Labyrinth
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField]
        private Text m_location;

        private FrameRateTracker _frameRate = new FrameRateTracker(60*5); // avg over the last 5 seconds

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _frameRate.AddSample(1.0f/Time.deltaTime);

            if (PlayerController.Instance != null)
            {
                m_location.text = string.Format("X={0:#####}, Y={1:#####}, Z={2:#####}, Rot={3:###}, FPS={4:##}",
                    PlayerController.Instance.transform.position.x,
                    PlayerController.Instance.transform.position.y,
                    PlayerController.Instance.transform.position.z,
                    Camera.main.transform.rotation.eulerAngles.y,
                    _frameRate.GetAvg());
            }
            else
            {
                m_location.text = "";
            }

        }
    }


}

