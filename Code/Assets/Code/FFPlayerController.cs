using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
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
    public KeyCode m_ReloadKeyCode = KeyCode.R;
    public KeyCode m_RestartKeyCode = KeyCode.Escape;
    public KeyCode m_NextLevelKeyCode = KeyCode.Return;
    public KeyCode m_PruebaVida = KeyCode.H;
    bool m_AngleLocked = false;
    bool m_AimLocked = true;

    float m_VerticalSpeed = 0.0f;
    bool m_OnGround = true;

    public float m_JumpSpeed = 10.0f;

    public Transform m_LavaCheckpoint;
    //public GameObject m_DecalPrefab;

    [Header("Camera")]
    public Camera m_Camera;
    public float m_NormalMovementFOV = 60.0f;
    public float m_RunMovementFOV = 75.0f;

    [Header("Shoot")]
    public GameObject m_DecalPrefab;
    public float m_MaxShootDistance;
    public LayerMask m_ShootinLayerMask;
    public GameObject m_Bullet;
    public Transform m_BulletSpawn;    
    
    int m_CurrentMaxAmmo;
    public int m_MaxAmmo;
    public int m_AmmoCapacity;
    int m_CurrentAmmo;

    TCOObjectPool m_DecalsPool;
    public bool m_DroneGetShoot = false;

    [Header("Animations")]
    public Animation m_Animations;
    public AnimationClip m_ShootingAnimationClip;
    public AnimationClip m_IdleAnimationClip;
    public AnimationClip m_ReloadAnimationClip;
    private bool m_Shooting = false;

    [Header("UI")]
    public TextMeshProUGUI m_LifesText;
    public TextMeshProUGUI m_ShieldText;
    public TextMeshProUGUI m_PointsText;
    public TextMeshProUGUI m_CurrentAmmoText;
    public TextMeshProUGUI m_MaxAmmoText;

    float m_Life;
    float m_Points;
    float m_Shield;

    //añadir tiempo animaciones para no tener que hardcodearlo
    void Start()
    {
        GameController.GetGameController().SetPlayer(this);
        m_Life = GameController.GetGameController().GetPlayerLifes();
        m_Shield = GameController.GetGameController().GetPlayerShield();
        m_CurrentAmmo = GameController.GetGameController().GetCurrentAmmo();
        m_Points = GameController.GetGameController().GetPoints();
        m_Yaw = transform.rotation.y;
        m_Pitch = m_PitchController.localRotation.x;
        Cursor.lockState = CursorLockMode.Locked;
        m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        SetIdleWeaponAnimation();
        m_CurrentAmmo = m_AmmoCapacity;
        m_CurrentMaxAmmo = m_MaxAmmo;
        //m_LavaCheckpoint.rotation = transform.rotation;
        //m_LavaCheckpoint.position = transform.position;
        m_DecalsPool = new TCOObjectPool(5, m_DecalPrefab);
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

        m_CurrentAmmoText.text = m_CurrentAmmo.ToString();  
        m_MaxAmmoText.text = m_CurrentMaxAmmo.ToString();
        m_ShieldText.text = m_Shield.ToString();
        m_LifesText.text = "Lifes: " + GameController.GetGameController().GetPlayerLifes();
        m_PointsText.text = "Your Points: " + GameController.GetGameController().GetPoints();
        m_ShieldText.text = "Shield: " + GameController.GetGameController().GetPlayerShield();
                

        if (Input.GetMouseButton(0) && m_CurrentAmmo > 0 && CanShoot())
        {
            Shoot();            
        }
                
        if (Input.GetKeyDown(m_PruebaVida))
        {
            DecreaseShield();
        }

        //Debug.Log(m_Shield);
        //Debug.Log(m_CurrentAmmo + " current ammo ");
        //Debug.Log(m_CurrentMaxAmmo + "currentmaxammo");

        if (m_CurrentMaxAmmo > 0 && m_CurrentAmmo < m_AmmoCapacity) 
        {
            if (Input.GetKeyDown(m_ReloadKeyCode))
            {
                SetReloadAnimation();
                StartCoroutine(Reload());                
            }
            
        }

        if (m_CurrentAmmo == 0)
        {
            SetReloadAnimation();
            StartCoroutine(Reload());
            if(m_CurrentMaxAmmo == 0)
            {
                SetIdleWeaponAnimation();
            }
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
        //Vector3 direction = m_Camera.transform.TransformDirection(new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 1)); //Esto para las balas realistas
        Vector3 direction = m_Camera.transform.TransformDirection(new Vector3(0,0, 1));
        Debug.DrawRay(m_Camera.transform.position, direction * 100, Color.green, 5f);

        //GameObject m_CurrentBullet = Instantiate(m_Bullet);
        //m_CurrentBullet.transform.position = m_BulletSpawn.position;        

        if (Physics.Raycast(l_Ray, out l_RayCastHit, m_MaxShootDistance, m_ShootinLayerMask.value))
        {
            if (l_RayCastHit.collider.tag == "DroneCollider")
            {
                l_RayCastHit.collider.GetComponent<HitCollider>().Hit();
                m_DroneGetShoot = true;
            }              

            if(l_RayCastHit.collider.tag != ("EDiana") || l_RayCastHit.collider.tag != ("DDiana") || l_RayCastHit.collider.tag != ("NDiana"))
            {
                CreateShootingParticles(l_RayCastHit.collider, l_RayCastHit.point, l_RayCastHit.normal);
            } 
            
            //m_CurrentBullet.transform.LookAt(l_RayCastHit.point);            
            SetWeaponShootAnimation();
            
            if (l_RayCastHit.collider.tag == "EDiana")
            {
                l_RayCastHit.transform.gameObject.SetActive(false);
                GameController.m_GameController.HitEasyDiana();
            }
            if (l_RayCastHit.collider.tag == "NDiana")
            {
                l_RayCastHit.transform.gameObject.SetActive(false);
                GameController.m_GameController.HitNormalDiana();
            }
            if (l_RayCastHit.collider.tag == "DDiana")
            {
                l_RayCastHit.transform.gameObject.SetActive(false);
                GameController.m_GameController.HitHardDiana();
            }
        }

        else
        {
            SetWeaponShootAnimation();
            Vector3 dir = m_Camera.transform.position + direction * 10f;
            //m_CurrentBullet.transform.LookAt(dir);
        }     
        
        m_Shooting = true;
        DecreaseAmmo();
        //Debug.Log(l_RayCastHit.collider.tag);
    }

    void CreateShootingParticles(Collider _Collider, Vector3 Position, Vector3 Normal)
    {
        Debug.DrawRay(Position, Normal * 5.0f, Color.red, 2.0f);
        //GameObject m_CurrentDecal = Instantiate(m_DecalPrefab, Position, Quaternion.LookRotation(Normal));
        //GameObject.Instantiate(m_DecalPrefab, Position, Quaternion.LookRotation(Normal));
        //m_CurrentDecal.transform.SetParent(_Collider.gameObject.transform);
        GameObject l_Decal = m_DecalsPool.GetNextElement();
        l_Decal.SetActive(true);
        l_Decal.transform.position = Position;
        l_Decal.transform.rotation = Quaternion.LookRotation(Normal);
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

    void SetReloadAnimation()
    {
        m_Animations.CrossFade(m_ReloadAnimationClip.name, 0.1f);
        m_Animations.CrossFadeQueued(m_IdleAnimationClip.name, 0.1f);
    }

    IEnumerator EndShoot()
    {
        yield return new WaitForSeconds(m_ShootingAnimationClip.length);
        m_Shooting = false;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_ReloadAnimationClip.length - 1f);
        if ((m_AmmoCapacity - m_CurrentAmmo) <= m_CurrentMaxAmmo)
        {
            m_CurrentMaxAmmo -= (m_AmmoCapacity - m_CurrentAmmo);
            m_CurrentAmmo += (m_AmmoCapacity - m_CurrentAmmo);
        }
        else
        {
            m_CurrentAmmo += m_CurrentMaxAmmo;
            m_CurrentMaxAmmo = 0;
        }

    }

    public void DecreaseAmmo()
    {
        m_CurrentAmmo--;        
    }

    public void DecreaseShield()
    {
        m_Shield -= 25f;
    }

    public float GetLife()
	{
        return m_Life;
	}

    public void AddLife(float Life)
	{
        m_Life = Mathf.Clamp(m_Life + Life, 0.0f, 150.0f);
	}

    public void GetHitDrone(float damage)
    {
        m_Life = Mathf.Clamp(m_Life - damage, 0.0f, 150.0f);
    }

	public void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Item")
            other.GetComponent<Item>().Pick(this);
        Debug.Log(m_Life);
        if (other.transform.tag == "DeadZone")
        {
            Kill();
        }
    }

    private void Kill()
    {
        m_Life = 0f;
        GameController.GetGameController().RestartGame();
    }

    public void RestartGame()
    {
        m_Life = 100.0f;
        m_CharacterController.enabled = false;
        transform.position = m_LavaCheckpoint.position;
        transform.rotation = m_LavaCheckpoint.rotation;
        m_CharacterController.enabled = true;
    }
    public void ChangeLevel()
    {
        GameController.GetGameController().SetPlayerLifes(m_Life);
        GameController.GetGameController().SetPlayerShield(m_Shield);
        GameController.GetGameController().SetCurrentAmmo(m_CurrentAmmo);
        //GameController.GetGameController().SetPoints(m_Points);
    }



}
