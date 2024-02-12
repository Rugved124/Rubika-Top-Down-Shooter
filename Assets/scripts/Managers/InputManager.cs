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

    [SerializeField]
    SceneLoader loader;
    private void Awake()
    {
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            loader.ReloadScene();
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
    public bool IsMousePressed()
    {
        return Input.GetMouseButtonDown(0);
    }
    public float GetMovementHorizontal()
    {
        return horizontal;
    }
    public float GetMovementVertical()
    {
        return vertical;
    }

    public Vector3 GetMousePosition()
    {

        Vector3 mousePos = new Vector3(0, 0, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, groundMask))
        {
            mousePos = hit.point;

        }

        return mousePos;

    }

    public bool GetIfConsumeIsHeld()
    {
        if (Input.GetMouseButton(1))
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
}

