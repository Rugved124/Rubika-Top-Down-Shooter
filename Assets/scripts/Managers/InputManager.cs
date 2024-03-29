using UnityEngine;

public class InputManager : MonoBehaviour
{
    //this is the code of singleton
    public static InputManager instance;

    float horizontal;

    float vertical;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private LayerMask soulMask;

    Plane plane = new Plane(Vector3.up, Vector3.zero);

    private bool usingMouseAndKeyboard = false;
    private bool usingController = false;

    [SerializeField]
    private float gamePadSensitivity;

    Vector3 aimPos;
    private void Awake()
    {
        plane = new Plane(Vector3.up, Vector3.zero);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    //till here everything is to make this a singleton script so there wont be any copies of this script and you can directly access the functions in this directly

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        CheckControllerInput();
        CheckMouseAndKeyboardInput();
    }
    public bool IsFirePressed()
    {   
        if(GameManager.Instance.currentState == GameManager.GameStates.RUNNING)
        {
            return Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1");
        }
        else
        {
            return false;
        }
    }
    public bool IsFireHeld() 
    {
        if (GameManager.Instance.currentState == GameManager.GameStates.RUNNING)
        {
            return Input.GetMouseButton(0) || Input.GetButton("Fire1");
        }
        else
        {
            return false;
        }
    }
    public float GetMovementHorizontal()
    {
        return horizontal;
    }
    public float GetMovementVertical()
    {
        return vertical;
    }
    public bool GetDashButton()
    {
        return Input.GetButtonDown("Dash");
    }
    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = new Vector3(0, 0, 0);
        if (usingMouseAndKeyboard )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (plane.Raycast(ray, out rayDistance))
            {
                mousePos = ray.GetPoint(rayDistance);
            }
            return mousePos;
        }
        else
        {
            Vector3 pcPos = FindObjectOfType<PC>().transform.position;
            float aimVertical = Input.GetAxis("Aim Vertical");
            float aimHorizontal = Input.GetAxis("Aim Horizontal");
            aimPos += new Vector3(aimHorizontal, pcPos.y, aimVertical) * Time.deltaTime * gamePadSensitivity;

            return (pcPos + Quaternion.AngleAxis(-45, Vector3.up) * aimPos);
        }
    }

    public bool GetIfConsumeIsHeld()
    {
        if (Input.GetMouseButton(1) && GameManager.Instance.currentState == GameManager.GameStates.RUNNING)
        {
            return true;
        }
        return false;
    }

    public bool GetIfShieldIsPressed()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            return true;
        }
        else
        {
            return false;
        }

    }
    void CheckMouseAndKeyboardInput()
    {
        if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            if (Input.anyKeyDown)
            {
                // Get the last key that was pressed
                KeyCode lastKeyCode = GetLastKeyCode();
                Debug.Log("Last key pressed: " + lastKeyCode.ToString());
            }
            usingMouseAndKeyboard = true;
            usingController = false;
        }
    }
     KeyCode GetLastKeyCode()
    {
        // Iterate over all possible key codes and check if they are currently down
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                return keyCode;
            }
        }
        return KeyCode.None;
    }
    void CheckControllerInput()
    {
        string[] joystickNames = Input.GetJoystickNames();
        for (int i = 0; i < joystickNames.Length; i++)
        {
            if (!string.IsNullOrEmpty(joystickNames[i]) && (Input.GetAxis("Aim Vertical") != 0 || Input.GetAxis("Aim Horizontal") != 0))
            {
                usingController = true;
                usingMouseAndKeyboard = false;
                break;
            }
        }
    }
}

