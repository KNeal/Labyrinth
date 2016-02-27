using UnityEngine;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;
using InControl;

namespace Labyrinth
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    [RequireComponent(typeof(PhotonView))]
    public class PlayerController : Photon.MonoBehaviour 
    {
        public static PlayerController Instance { get; private set; }

        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private float m_MaxFallSpeed;
        [SerializeField] private float m_RotateMultiplier;
        [SerializeField] private MouseLook m_MouseLook;

        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
        [SerializeField] private Camera m_Camera;
        
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private PhotonView m_PhotonView;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private float m_StepCycle;
        private float m_NextStep;
        private AudioSource m_AudioSource;

        private enum JumpState
        {
            None,
            JumpPending,
            Jumping
        }

        private JumpState m_JumpState;


        // Use this for initialization
        private void Start()
        {
            m_PhotonView = GetComponent<PhotonView>();
            m_CharacterController = GetComponent<CharacterController>();

            gameObject.name = gameObject.name + photonView.viewID;

            if (photonView.isMine)
            {
                m_CharacterController.enabled = true;
                m_MouseLook.Init(transform, m_Camera.transform);

                Instance = this;
            }
            else
            {
                m_CharacterController.enabled = false;
                m_Camera.gameObject.SetActive(false);
            }

            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_JumpState = JumpState.None;;
            m_AudioSource = GetComponent<AudioSource>();
        }

        public void OnDestroy()
        {
            Instance = null;
        }
       

        // Update is called once per frame
        private void Update()
        {
            if (photonView.isMine)
            {
                UpdateLocalPlayerPosition();
            }
            else
            {
                UpdateRemotePlayerPosition();
            }
        }

        private void UpdateLocalPlayerPosition()
        {
            RotateView();

            // the jump state needs to read here to make sure it is not missed
            if (m_JumpState == JumpState.None)
            {
                if (InputManager.ActiveDevice.Action1.IsPressed 
                    || Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Jump Pressed");
                    m_JumpState = JumpState.JumpPending;
                }
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_JumpState = JumpState.None;
            }

            if (!m_CharacterController.isGrounded &&  m_JumpState == JumpState.None && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

        }

        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }

        private void FixedUpdate()
        {
            if (photonView.isMine)
            {
                FixedUpdateLocalPlayerPosition();
            }
            else
            {
                // Do nothing.
            }
        }

        private void FixedUpdateLocalPlayerPosition()
        {
            // Only allow movement when we are not jumping.
            if (m_JumpState == JumpState.None)
            {
                UpdateInput();

                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = m_Camera.transform.forward * m_Input.y + m_Camera.transform.right * m_Input.x;

                // get a normal for the surface that is being touched to move along it
                RaycastHit hitInfo;
                Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                    m_CharacterController.height/2f, ~0, QueryTriggerInteraction.Ignore);
                desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

                m_MoveDir.x = desiredMove.x * m_MoveSpeed;
                m_MoveDir.z = desiredMove.z * m_MoveSpeed;
            }
            else
            {
                m_MoveDir.x = 0.0f;
                m_MoveDir.z = 0.0f;
            }

            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if ( m_JumpState == JumpState.JumpPending)
                {
                    Debug.Log("Starting Jump");
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_JumpState = JumpState.Jumping;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
                if (m_MaxFallSpeed > 0 && m_MoveDir.y < -m_MaxFallSpeed)
                {
                    m_MoveDir.y = -m_MaxFallSpeed;
                }
            }
            
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle();

            m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle()
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + m_MoveSpeed)*
                                Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }
        
        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }

        private void UpdateInput()
        {
            // Read input
            float horizontal;
            if (Input.GetKeyDown(KeyCode.A))
            {
                horizontal = -1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                horizontal = 1.0f;
            }
            else
            {
                horizontal = InputManager.ActiveDevice.LeftStick.X;
            }

            float vertical;
            if (Input.GetKeyDown(KeyCode.S))
            {
                vertical = -1.0f;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                vertical = 1.0f;
            }
            else
            {
                vertical = InputManager.ActiveDevice.LeftStick.Y;
            }

            // set the desired speed to be walking or running
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }
        }


        private void RotateView()
        {
            // Read input
            float leftRight = InputManager.ActiveDevice.RightStick.X * m_RotateMultiplier;

            transform.Rotate(Vector3.up, leftRight);
        }

#region Photon
        
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation); 
        }
        else
        {
            //Network player, receive data
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

    private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this

    void UpdateRemotePlayerPosition()
    {
        // Update remote player (smooth this, this looks good, at the cost of some accuracy)
        transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
        transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
    }

#endregion
    }

}