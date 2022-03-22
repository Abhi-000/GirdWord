using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private const float DEADZONE = 50f;
    public string name;
    public static GameInput input { set; get; }
    //s = Swipe;
    private bool tap, sl, sr, su, sd;
    private Vector2 sDelta, startTouch;

    public bool Tap { get { return tap; } }
    public bool Sl { get { return sl; } }
    public bool Sr { get { return sr; } }
    public bool Su { get { return su; } }
    public bool Sd { get { return sd; } }
    public Vector2 SDelta { get { return sDelta; } }

    private void Awake()
    {
        input = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        tap = sr = sl = su = sd = false;

        #region EditorInput
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.GetComponent<BoxScript>() != null)
                {
                    name = hit.collider.name;
                    //Debug.Log(name);
                    tap = true;
                        startTouch = Input.mousePosition;
                    }

                } 
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Input.touchCount > 0)
            {
                startTouch = sDelta = Vector2.zero;
            }
                
        }
#endif
        #endregion

        #region MobileInput
        if (Input.touches.Length != 0)
        {
            Debug.Log("here");
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) 
                {
                    if (hit.collider.GetComponent<BoxScript>() != null)
                    {
                        name = hit.collider.name;
                        Debug.Log(name);
                        tap = true;
                        startTouch = Input.mousePosition;
                    }
                }
                else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                {
                    startTouch = sDelta = Vector2.zero;
                }
            }
        }
        #endregion


        //Cal Dis
        sDelta = Vector2.zero;

        if (startTouch != Vector2.zero)
        {
            //Mobile
            if (Input.touches.Length != 0)
            {
                sDelta = Input.touches[0].position - startTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                sDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        //Lest Cal we beyond DEADZONE
        if (SDelta.magnitude > DEADZONE)
        {
            float x = sDelta.x;
            float y = sDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //left or right
                if (x < 0)
                    sl = true;
                else
                    sr = true;
            }
            else
            {
                //up or Down
                if (y < 0)
                    sd = true;
                else
                    su = true;
            }
            startTouch = sDelta = Vector2.zero;
        }
    }
}
