using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Labyrinth
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField]
        private Text m_location;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerController.Instance != null)
            {
                m_location.text = string.Format("X={0:#####}, Y={1:#####}, Z={2:#####}, Rot={3:###}",
                    PlayerController.Instance.transform.position.x,
                    PlayerController.Instance.transform.position.y,
                    PlayerController.Instance.transform.position.z,
                    PlayerController.Instance.transform.rotation.eulerAngles.y);
            }
            else
            {
                m_location.text = "";
            }

        }
    }


}

