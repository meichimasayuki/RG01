using UnityEngine;

public class Player : FieldCharacter
{
    [SerializeField]
    private GameObject playerModel;
    [SerializeField]
    private CameraController playerCamera;

    private TouchPointer touchPointer;

    Vector3 targetPos;  // タッチされたポイント座標
    Vector3 moveVector; // 進行方向へのベクトル
    bool isVisible = false;
    bool isMove = false;

    public void Reset()
    {
        touchPointer.Reset();
        isVisible = false;
        isMove = false;
        targetPos = Vector3.zero;
        moveVector = Vector3.zero;
    }
    void Start()
    {
        touchPointer = GetComponent<TouchPointer>();
    }

    public void DungeonUpdate()
    {
        if (!isVisible) return;

        if (touchPointer.IsTouchPointer())
        {
            targetPos = touchPointer.GetPointerPosition();
            moveVector = Vector3.Normalize(targetPos - transform.position);
            
            if (IsMoveArea() && isMove)
            {
                Move();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) playerCamera.AutoChange(false);
        }
        Rotation(moveVector);
    }

    private const float MOVE_SPEED = 0.015f;
    private void Move()
    {
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            if (isMove) PlayAnimation("Stay");
            isMove = false;
            EndRotation();
            touchPointer.ApproachePointer();
            return;
        }
        transform.position += new Vector3(moveVector.x, 0.0f, moveVector.z) * MOVE_SPEED;
        if (!IsPlayingAnimation("Run")) PlayAnimation("Run");
        AutoSetUpPlayerCamera();
    }
    
    private bool IsMoveArea()
    {
        Vector3 move_pos = transform.position + (new Vector3(moveVector.x, 0.1f, moveVector.z) * MOVE_SPEED);
        Ray ray = new Ray(move_pos, -Vector3.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, 20.0f);
        if (hits.Length != 0)
        {
            bool isfloor = false;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.name.Contains("MapTrout")) 
                {
                    Trout2 trout = hits[i].collider.gameObject.GetComponent<Trout2>();
                    Manager.PlayerPass(trout);
                    continue;
                }
                if (hits[i].collider.name.Contains("Floor")) isfloor = true;
            }
            if (!isfloor)
            {
                PlayAnimation("Stay");
                isMove = false;
                touchPointer.ApproachePointer();
            }
            return isfloor;
        }
        return false;
    }

    public void FirstAreaCheck()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, 20.0f);
        if (hits.Length != 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.name.Contains("MapTrout"))
                {
                    Trout2 trout = hits[i].collider.gameObject.GetComponent<Trout2>();
                    Manager.PlayerPass(trout);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name.Contains("Enemy"))
        {
            Manager.Battle(collision.collider.gameObject);
        }
    }

    public void OnTouch()
    {
        if(!isMove) PlayAnimation("Run");
        isMove = true;
        StartRotation();
        playerCamera.AutoChange(true);
    }

    // プレイヤーモデルを可視or不可視状態にする
    public void IsVisiblePlayer(bool is_visible)
    {
        isVisible = is_visible;
        targetPos = transform.position;
        playerModel.SetActive(is_visible);
    }

    public void Respawn()
    {
        PlayerData data = GlobalDataManager.GetGlobalData().LoadPlayerData();
        transform.position = data.Position;
        transform.rotation = data.Rotation;
        IsVisiblePlayer(true);
    }

    public void ResetSetUpPlayerCamera() { playerCamera.ResetSetUp(transform.rotation.eulerAngles.y); }
    public void AutoSetUpPlayerCamera() { playerCamera.AutoSetUp(transform.rotation.eulerAngles.y); }
}
