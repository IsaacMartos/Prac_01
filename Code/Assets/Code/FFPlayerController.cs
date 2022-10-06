using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFPlayerController : MonoBehaviour
{
    float m_Yaw;
    float m_Pitch;
    public float m_YawRotationlSpeed;
    public float m_PitchRotationlSpeed;

    public float m_MinPitch;
    public float m_MaxPitch;

    public Transform m_PitchController;
    public bool m_UseYawInverted;
    public bool m_UsePitchInverted;

    public CharacterController m_CharacterController;
    public float m_Speed;
    public float m_FastSpeedMultiplier;
    public KeyCode m_LeftKeyCode;
    public KeyCode m_RightKeyCode;
    public KeyCode m_UpKeyCode;
    public KeyCode m_DownKeyCode;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_RunKeyCode = KeyCode.LeftShift;
    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;
    bool m_AngleLocked = false;
    bool m_AimLocked = true;


    float m_VerticalSpeed = 0.0f;
    bool m_OnGround = true;

    public float m_JumpSpeed = 10.0f;

    public GameObject m_DecalPrefab;

    [Header("Camera")]
    public Camera m_Camera;
    public float m_NormalMovementFOV = 60.0f;
    public float m_RunMovementFOV = 75.0f;

    [Header("Shoot")]
    public float m_MaxShootDistance;
    public LayerMask m_ShootinLayerMask;

    [Header("Animations")]
    public Animation m_Animations;
    public AnimationClip m_ShootingAnimationClip;
    public AnimationClip m_IdleAnimationClip;
    public AnimationClip m_ReloadAnimationClip;
    private bool m_Shooting = false;

    float m_Life;

    //añadir tiempo animaciones para no tener que hardcodearlo
    void Start()
    {
        m_Life = GameController.GetGameController().SetPlayerLife();
        m_Yaw = transform.rotation.y;
        m_Pitch = m_PitchController.localRotation.x;
        Cursor.lockState = CursorLockMode.Locked;
        m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        SetIdleWeaponAnimation();

    }
#if UNITY_EDITOR
    void UpdateInputDebug()
    {
        if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
        {
            m_AngleLocked = !m_AngleLocked;
        }
        if (Input.GetKeyDown(m_DebugLockKeyCode))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        }
    }
#endif

    void Update()
    {
#if UNITY_EDITOR
        UpdateInputDebug();
#endif
        //Movement
        Vector3 l_RightDirection = transform.right;
        Vector3 l_ForwardDirection = transform.forward;
        Vector3 l_Direction = Vector3.zero;
        float l_Speed = m_Speed;


        if (Input.GetKey(m_UpKeyCode))
            l_Direction = l_ForwardDirection;
        if (Input.GetKey(m_DownKeyCode))
            l_Direction = -l_ForwardDirection;
        if (Input.GetKey(m_RightKeyCode))
            l_Direction += l_RightDirection;
        if (Input.GetKey(m_LeftKeyCode))
            l_Direction -= l_RightDirection;
        if (Input.GetKeyDown(m_JumpKeyCode) && m_OnGround)
            m_VerticalSpeed = m_JumpSpeed;
        float l_FOV = m_NormalMovementFOV;
        if (Input.GetKey(m_RunKeyCode))
        {
            l_Speed = m_Speed * m_FastSpeedMultiplier;
            l_FOV = m_RunMovementFOV;
        }

        m_Camera.fieldOfView = l_FOV;

        l_Direction.Normalize();
        Vector3 l_Movement = l_Direction * l_Speed * Time.deltaTime;

        //Rotation
        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");

#if UNITY_EDITOR
        if (m_AngleLocked)
        {
            l_MouseX = 0.0f;
            l_MouseY = 0.0f;
        }
#endif

        m_Yaw = m_Yaw + l_MouseX * m_YawRotationlSpeed * Time.deltaTime * (m_UseYawInverted ? -1.0f : 1.0f);
        m_Pitch = m_Pitch + l_MouseY * m_PitchRotationlSpeed * Time.deltaTime * (m_UsePitchInverted ? -1.0f : 1.0f);
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;

        CollisionFlags l_CollisionFalgs = m_CharacterController.Move(l_Movement);
        if ((l_CollisionFalgs & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f)
            m_VerticalSpeed = 0.0f;
        if ((l_CollisionFalgs & CollisionFlags.Below) != 0)
        {
            m_VerticalSpeed = 0.0f;
            m_OnGround = true;
        }
        else
            m_OnGround = false;

        if (Input.GetMouseButtonDown(0) && CanShoot())
        {
            Shoot();
        }
    }

    private bool CanShoot()
    {
        return !m_Shooting;
    }

    void Shoot()
    {
        Ray l_Ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit l_RayCastHit;
        if (Physics.Raycast(l_Ray, out l_RayCastHit, m_MaxShootDistance, m_ShootinLayerMask.value))
            CreateShootingParticles(l_RayCastHit.collider, l_RayCastHit.point, l_RayCastHit.normal);
        SetWeaponShootAnimation();
        m_Shooting = true;
    }

    void CreateShootingParticles(Collider _Collider, Vector3 Position, Vector3 Normal)
    {
        //Debug.DrawRay(Position, Normal * 5.0f, Color.red, 2.0f);
        GameObject.Instantiate(m_DecalPrefab, Position, Quaternion.LookRotation(Normal));
    }

    void SetIdleWeaponAnimation()
    {
        m_Animations.CrossFade(m_IdleAnimationClip.name);
    }

    void SetWeaponShootAnimation()
    {
        m_Animations.CrossFade(m_ShootingAnimationClip.name, 0.1f);
        m_Animations.CrossFadeQueued(m_IdleAnimationClip.name, 0.1f);
        StartCoroutine(EndShoot());
    }

    IEnumerator EndShoot()
    {
        yield return new WaitForSeconds(m_ShootingAnimationClip.length);
        m_Shooting = false;
    }
}
