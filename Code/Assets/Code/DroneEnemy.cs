using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = System.Random;

public class DroneEnemy : MonoBehaviour
{
	public enum TState
	{
		IDLE,
		PATROL,
		ALERT,
		CHASE,
		ATTACK,
		HIT,
		DIE
	}
	public TState m_State;
	NavMeshAgent m_NavMeshAgent;
	public List<Transform> m_PatrolTargets;
	int m_CurrentPatrolTargetId = 0;
	public float m_HearRangerDistance = 2.0f;

	public float m_VisualConeAngle = 60.0f;
	public float m_SightDistance = 8.0f;
	public LayerMask m_SightLayerMask;
	public float m_EyesHeight = 1.8f;
	public float m_EyesPlayerHeight = 1.8f;
	public float m_RotationSpeed = 3f;
    public float m_DroneSpeed = 3f;
    public float m_DroneMinimumRange = 4f;
    public float m_DroneShootingRange = 4f;
	public float m_MaxChaseDistance = 50f;

	public float m_DroneLifes = 3f;
	public float m_DroneDamage = 15f;
	public float m_DroneFireRate = 2f;
	private float m_CountdowwnBetweeenFireRate = 0f;
	public bool m_WatchedPlayer = false;

    public Image m_LifeBarImage;
    public Transform m_LifeBarAnchorPosition;
    public RectTransform m_LifeBarRectPosition;
	public GameObject m_LifeBar;

	private int RndNumber;
    public UIControls m_UIControls;
    public GameObject m_LifeItem;
    public GameObject m_ShieldItem;
    public GameObject m_BulletsItem;

    public Animation m_Animation;
    public AnimationClip m_DroneAlert;
    public AnimationClip m_DroneIdle;

    private void Awake()
	{
		m_NavMeshAgent = GetComponent<NavMeshAgent>();
	}

	void Start()
	{
		SetIdelState();		
	}

	void Update()
	{
		switch (m_State)
		{
			case TState.IDLE:
				UpdateIdelState();
				break;
			case TState.PATROL:
				UpdatePatrolState();
				break;
			case TState.ALERT:
				UpdateAlertState();
				break;
			case TState.CHASE:
				UpdateChaseState();
				break;
			case TState.ATTACK:
				UpdateAttackState();
				break;
			case TState.HIT:
				UpdateHitState();
				break;
			case TState.DIE:
				UpdateDieState();
				break;
		}
		    
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
		Vector3 l_EyesPosition = transform.position + Vector3.up * m_EyesHeight;
		Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_EyesPlayerHeight;
		Debug.DrawLine(l_EyesPosition, l_PlayerEyesPosition, SeesPlayer() ? Color.red : Color.blue);
		UpdateLifeBarPosition();

		if(m_DroneLifes <= 0f)
		{
			SetDieState();
		}

		if(!ShowLifeBar())
		{
			m_LifeBar.SetActive(false);
		}
        
        Debug.Log(ShowLifeBar());
    }
	void SetIdelState()
	{
		m_State = TState.IDLE;
	}
	void UpdateIdelState()
	{
		SetPatrolState();
	}
	void SetPatrolState()
	{
		m_State = TState.PATROL;
		m_NavMeshAgent.destination = m_PatrolTargets[m_CurrentPatrolTargetId].position;
        m_NavMeshAgent.stoppingDistance = 0f;
    }
	void UpdatePatrolState()
	{
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        if (PatrolTargetPositionArrive())
		{
            MoveToNextPatrolPosition();
        }			

		if (HearsPlayer())
		{
			SetAlertState();
            //Debug.Log("pillao");
		}
		if (GameController.GetGameController().GetPlayer().m_DroneGetShoot == true && Vector3.Distance(l_PlayerPosition, transform.position) < 10f)
		{
			SetAlertState();
		}

	}
	bool HearsPlayer()
	{
		Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
		return Vector3.Distance(l_PlayerPosition, transform.position) <= m_HearRangerDistance;
	}

	bool SeesPlayer()
	{
		Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
		Vector3 l_DirectionPlayerXZ = l_PlayerPosition - transform.position;
		l_DirectionPlayerXZ.y = 0.0f;
		l_DirectionPlayerXZ.Normalize();
		Vector3 l_ForwardXZ = transform.forward;
		l_ForwardXZ.y = 0.0f;
		l_ForwardXZ.Normalize();
		Vector3 l_EyesPosition = transform.position + Vector3.up * m_EyesHeight;
		Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_EyesPlayerHeight;
		Vector3 l_Direction = l_PlayerEyesPosition - l_EyesPosition;
		//l_Direction.Normalize();
		float l_Lenght = l_Direction.magnitude;
		l_Direction /= l_Lenght;
		Ray l_Ray = new Ray(l_EyesPosition, l_Direction);
		return Vector3.Distance(l_PlayerPosition, transform.position) < m_SightDistance &&
			Vector3.Dot(l_ForwardXZ, l_DirectionPlayerXZ) > Mathf.Cos(m_VisualConeAngle * Mathf.Deg2Rad / 2.0f)
			&& !Physics.Raycast(l_Ray, l_Lenght, m_SightLayerMask.value);
	}

	void MoveToNextPatrolPosition()
	{
		++m_CurrentPatrolTargetId;
		if (m_CurrentPatrolTargetId >= m_PatrolTargets.Count)
			m_CurrentPatrolTargetId = 0;
		m_NavMeshAgent.destination = m_PatrolTargets[m_CurrentPatrolTargetId].position;
	}

	bool PatrolTargetPositionArrive()
	{
		return !m_NavMeshAgent.hasPath && !m_NavMeshAgent.pathPending && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
	}

	void SetAlertState()
	{
		m_State = TState.ALERT;
	}
	void UpdateAlertState()
	{
		m_NavMeshAgent.isStopped = true;		
		m_Animation.CrossFade(m_DroneAlert.name, 0.1f);
		if (SeesPlayer())
		{
			SetChaseState();
			m_Animation.Stop();
		}
		else
		{			
			SetPatrolState();
		}
	}

	void SetChaseState()
	{
		m_State = TState.CHASE;
	}
	void UpdateChaseState()
	{
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
		m_NavMeshAgent.isStopped = false;
		//Debug.Log(Vector3.Distance(l_PlayerPosition, transform.position));

        if (Vector3.Distance(l_PlayerPosition,transform.position) < m_MaxChaseDistance)
		{
            Vector3 dirToPlayer = transform.position - l_PlayerPosition;
            Vector3 newPos = transform.position - dirToPlayer;
            m_NavMeshAgent.SetDestination(newPos);
			m_NavMeshAgent.stoppingDistance = 2f;
            transform.LookAt(l_PlayerPosition);
        }

		if(Vector3.Distance(l_PlayerPosition, transform.position) < m_DroneMinimumRange)
		{
			SetAttackState();
		}

		else
		{
			SetPatrolState();
		}
    }
	
	void SetAttackState()
	{
		m_State = TState.ATTACK;
	}
	void UpdateAttackState()
	{
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;

        if (Vector3.Distance(l_PlayerPosition, transform.position) <= m_DroneShootingRange)
        {
			if (m_CountdowwnBetweeenFireRate <= 0)
			{
                //Debug.Log("Shooting");
                GameController.GetGameController().GetPlayer().GetHitDrone(m_DroneDamage);
				m_CountdowwnBetweeenFireRate = 1f / m_DroneFireRate;
            }
			m_CountdowwnBetweeenFireRate -= Time.deltaTime;
			
        }
		else
		{
			SetChaseState();
		}

    }
	void SetHitState()
	{
		m_State = TState.HIT;
	}
	void UpdateHitState()
	{
		SetAlertState();
	}
	void SetDieState()
	{
		m_State = TState.DIE;
	}
	void UpdateDieState()
	{
        GameController.GetGameController().AddRespawnDroneElement(this);
        gameObject.SetActive(false);
		GenerateRandomNumber();
		if(RndNumber == 0)
		{
			Instantiate(m_LifeItem, transform.position, transform.rotation);
        }
        if (RndNumber == 1)
        {
            Instantiate(m_ShieldItem, transform.position, transform.rotation);
        }
        if (RndNumber == 2)
        {
            Instantiate(m_BulletsItem, transform.position, transform.rotation);
        }
    }
	
	IEnumerator StartSeeingPlayer()
	{
        yield return new WaitForSeconds(3.5f);
		if (SeesPlayer())
		{
			m_WatchedPlayer = true;
			if (m_WatchedPlayer == true)
				SetChaseState();
		}           

    }

    public void Hit(float life)
    {
        m_DroneLifes -= life;
        m_LifeBarImage.fillAmount = m_DroneLifes;
        //Debug.Log("Hiit life" + life);
    }

    void UpdateLifeBarPosition()
    {
        Vector3 l_Position = GameController.GetGameController().GetPlayer().m_Camera.WorldToViewportPoint(m_LifeBarAnchorPosition.position);
        m_LifeBarRectPosition.anchoredPosition = new Vector3(l_Position.x * 1920f, -(1080f - l_Position.y * 1080f), 0.0f);
        m_LifeBarRectPosition.gameObject.SetActive(l_Position.z > 0.0f);
    }

	bool ShowLifeBar()
	{
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        Vector3 l_EyesPosition = transform.position + Vector3.up * m_EyesHeight;
        Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_EyesPlayerHeight;
        Vector3 l_Direction = l_PlayerEyesPosition - l_EyesPosition;
        float l_Lenght = l_Direction.magnitude;
        l_Direction /= l_Lenght;
        Ray l_Ray = new Ray(l_EyesPosition, l_Direction);
        return !Physics.Raycast(l_Ray, l_Lenght, m_SightLayerMask.value);
    }
    
	public void GenerateRandomNumber()
	{		
        Random rnd = new Random();
		RndNumber = rnd.Next(3);
		Debug.Log(RndNumber);
    }

	public void Respawn()
	{
		gameObject.SetActive(true);
	}
}
