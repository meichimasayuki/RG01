using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyField : FieldCharacter
{
    [SerializeField] private GameObject mapEnemy;

    enum ENEMY_FIELD_STATE
    {
        STAY,
        LOOK,
        WALK,
        RUN,
    }

    private GameObject _target = null;
    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    private List<Vector3> movePosotionList = new List<Vector3>();
    private Vector3 movePosotion;
    private Vector3 moveVector;
    private string myArea;
    private float stateTime = 0.0f;
    private const float IN_SEARCH_DISTANS = 1.0f;
    private const float OUT_SEARCH_DISTANS = 2.5f;
    private float sesrchArea = IN_SEARCH_DISTANS;
    public float THINKING_TIME = 2.0f;
    private const float WALK_SPEED = 0.0015f;
    private const float RUN_SPEED = 0.005f;

    void Start()
    {
        StateReset();
    }

    public void DungeonUpdate()
    {
        switch (State)
        {
            case (int)ENEMY_FIELD_STATE.STAY:
                Stay();
                break;
            case (int)ENEMY_FIELD_STATE.LOOK:
                Look();
                break;
            case (int)ENEMY_FIELD_STATE.WALK:
                Walk();
                break;
            case (int)ENEMY_FIELD_STATE.RUN:
                Run();
                break;
            default:
                break;
        }
        if (IsInSerach())
        {
            if(State != (int)ENEMY_FIELD_STATE.RUN)PlayAnimation("Run");
            State = (int)ENEMY_FIELD_STATE.RUN;
        }
        if(IsOutSerach() && State == (int)ENEMY_FIELD_STATE.RUN)
        {
            StateReset();
        }
        Rotation(moveVector);
    }

    private void StateReset()
    {
        stateTime = 0.0f;
        PlayAnimation("Stay");
        sesrchArea = IN_SEARCH_DISTANS;
        State = (int)ENEMY_FIELD_STATE.STAY;
    }

    private void Stay()
    {
        //Debug.Log("Stay() : Start");
        stateTime += Time.deltaTime;
        if(stateTime >= THINKING_TIME)
        {
            int rand = Random.Range(0, 10);
            if (rand < 5)
            {
                PlayAnimation("Look");
                State = (int)ENEMY_FIELD_STATE.LOOK;
            }
            else
            {
                rand = Random.Range(0, movePosotionList.Count);
                movePosotion = movePosotionList[rand];
                moveVector = Vector3.Normalize(movePosotion - transform.position);
                PlayAnimation("Walk");
                StartRotation();
                State = (int)ENEMY_FIELD_STATE.WALK;
            }
        }
        //Debug.Log("Stay() : End");
    }

    private void Look()
    {
        sesrchArea = IN_SEARCH_DISTANS * 1.5f;
        if (!IsPlayingAnimation("Look"))
        {
            StateReset();
        }
    }

    private void Walk()
    {
        transform.position += new Vector3(moveVector.x, 0.0f, moveVector.z) * WALK_SPEED;
        if (Vector3.Distance(transform.position, movePosotion) < 0.01f)
        {
            StateReset();
        }
    }

    private void Run()
    {
        Vector3 moveVector = Vector3.Normalize(Target.transform.position - transform.position);
        transform.position += new Vector3(moveVector.x, 0.0f, moveVector.z) * RUN_SPEED;
        if(!isRotation) transform.rotation = Quaternion.LookRotation(moveVector);
    }

    private bool IsInSerach()
    {
        if (Vector3.Distance(transform.position, _target.transform.position) < sesrchArea)
        {
            return true;
        }
        return false;
    }

    private bool IsOutSerach()
    {
        if (Vector3.Distance(transform.position, _target.transform.position) > OUT_SEARCH_DISTANS)
        {
            return true;
        }
        return false;
    }

    public void FirstAreaCheck()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Ray ray = new Ray(pos, -Vector3.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, 20.0f);
        if (hits.Length != 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.name.Contains("MapTrout"))
                {
                    Trout2 trout = hits[i].collider.gameObject.GetComponent<Trout2>();
                    movePosotionList = Manager.GetEnemyAreaInTroutList(trout);
                }
            }
        }
        Debug.Log("動ける範囲のマスは" + movePosotionList.Count + "マスです。");
    }

    public void MapEnemyActive(Trout2 trout)
    {
        for(int i = 0;i < movePosotionList.Count; i++)
        {
            if(trout.GetPosition() == movePosotionList[i])
            {
                mapEnemy.SetActive(true);
                break;
            }
        }
    }
}
