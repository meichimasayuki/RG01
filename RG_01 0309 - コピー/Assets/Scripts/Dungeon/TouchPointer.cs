using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPointer : MonoBehaviour
{
    [SerializeField]
    private GameObject touchPoint;
    private Player player;
    GameObject touchObj = null;

    Vector3 targetPos;  // タッチされたポイント座標
    private Vector3 downTouchPosition = Vector3.zero;
    private Vector3 upTouchPosition = Vector3.zero;
    private float touchTime = 0.0f;
    private bool isTouch = false;

    void Start()
    {
        player = GetComponent<Player>();
    }

    public void Reset()
    {
        if (touchObj) { Destroy(touchObj.gameObject); touchObj = null; }
        targetPos = Vector3.zero;
        downTouchPosition = Vector3.zero;
        upTouchPosition = Vector3.zero;
        touchTime = 0.0f;
        isTouch = false;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTouch)
        {
#if UNITY_ANDROID
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
#else
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
#endif
            isTouch = true;
            touchTime = 0.0f;
            downTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) && isTouch)
        {
            touchTime += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0) && isTouch)
        {
            upTouchPosition = Input.mousePosition;
            isTouch = false;
            if (touchTime > 0.1f)
            {
                if (Vector3.Distance(downTouchPosition, upTouchPosition) > 50.0f) return;
            }

            int index = -1; // -1 は未割り当て
            RaycastHit[] hit_objs;
            // マウスの位置からRayを発射
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit_objs = Physics.RaycastAll(ray, 10.0f);
            for (int i = 0; i < hit_objs.Length; i++)
            {
                if (hit_objs[i].collider.name.Contains("Floor"))
                {
                    index = i;
                    break;
                }
                else if (hit_objs[i].collider.name.Contains("Wall"))
                {
                    RaycastHit p_hit;
                    Vector3 test1 = new Vector3(hit_objs[i].point.x, 0.0f, hit_objs[i].point.z);
                    Vector3 test2 = new Vector3(transform.position.x, 0.0f, transform.position.z);
                    //Vector3 test3 = new Vector3(Camera.main.transform.position.x, 0.0f, Camera.main.transform.position.z);
                    Vector3 p_vec = Vector3.Normalize(test1 - test2);
                    Ray p_ray = new Ray(test2, p_vec);
                    if (Physics.Raycast(p_ray, out p_hit, 10f))
                    {
                        //Debug.Log("normal : " + hit_objs[i].normal + ", normal : " + p_hit.normal);
                        //if (hit_objs[i].normal == p_hit.normal)
                        index = i;
                        //break;
                    }
                }
            }
            if (index < 0)
            {
                return;
            }
            if (touchObj) { Destroy(touchObj.gameObject); touchObj = null; }

            RaycastHit hit = hit_objs[index];

            targetPos = new Vector3(hit.point.x, 0, hit.point.z);
            if (hit.collider.name.Contains("Wall")) targetPos += (hit.normal * 0.04f);

            touchObj = Instantiate(touchPoint, targetPos, Quaternion.Euler(0, 0, 0)) as GameObject;
            touchObj.gameObject.GetComponent<Renderer>().material.color = Color.black;
            player.OnTouch();
        }
    }

    public bool IsTouchPointer() { return touchObj != null; }
    public Vector3 GetPointerPosition() { return targetPos; }
    public void ApproachePointer() { Destroy(touchObj.gameObject); }
}
