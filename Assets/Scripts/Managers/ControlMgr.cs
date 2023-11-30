using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ControlMgr : MonoBehaviour
{
    //*************************************//
    //           SINGLETON CODE            //
    //*************************************//
    private static ControlMgr _instance;
    public static ControlMgr Instance { get { return _instance; } }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    //*************************************//
    //             VARIABLES               //
    //*************************************//

    public float camMovementSpeed;
    public float camRotationSpeed;

    public Texture whiteTexture;

    Vector3 mouseClickPosition;
    bool mouseDown = false;

    //*************************************//
    //             FUNCTIONS               //
    //*************************************//

    public void OnStart()
    {

    }

    public void OnUpdate(float dt)
    {
        CameraControls(dt);
        KeyboardControls(dt);
        if (PermData.Instance.part != 0)
        {
            ClickControls(dt);
        }
    }

    void CameraControls(float dt)
    {
        // W, A, S, D, R, F
        List<bool> noShift = new List<bool> { false, false, false, false, false, false }; 
        List<bool> yesShift = new List<bool> { false, false, false, false, false, false };

        noShift[0] = Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftControl);
        noShift[1] = Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftControl);
        noShift[2] = Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.LeftControl);
        noShift[3] = Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftControl);
        noShift[4] = Input.GetKey(KeyCode.R) && !Input.GetKey(KeyCode.LeftControl);
        noShift[5] = Input.GetKey(KeyCode.F) && !Input.GetKey(KeyCode.LeftControl);

        yesShift[0] = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftControl);
        yesShift[1] = Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftControl);
        yesShift[2] = Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftControl);
        yesShift[3] = Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftControl);
        yesShift[4] = Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.LeftControl);
        yesShift[5] = Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.LeftControl);


        // Rotation
        Vector3 rot = new Vector3(0, 0, 0);
        
        rot.x -= yesShift[4].GetHashCode();
        rot.x += yesShift[5].GetHashCode();
        rot.y += yesShift[3].GetHashCode();
        rot.y -= yesShift[1].GetHashCode();

        rot *= dt * camRotationSpeed;

        CameraMgr.Instance.cameraRotaion += rot;
        CameraMgr.Instance.cameraRotaion.x = Mathf.Clamp(CameraMgr.Instance.cameraRotaion.x, -90.0f, 90.0f);

        // Movement
        Vector3 basicMove = new Vector3(0, 0, 0);
        Vector3 camMove = new Vector3(0, 0, 0);
        float angle = CameraMgr.Instance.cameraRotaion.y * 3.141592f / 180.0f;
        basicMove.x += noShift[3].GetHashCode();
        basicMove.x -= noShift[1].GetHashCode();
        basicMove.z += (noShift[0] || yesShift[0]).GetHashCode();
        basicMove.z -= (noShift[2] || yesShift[2]).GetHashCode();
        basicMove.Normalize();
        basicMove.y += noShift[4].GetHashCode();
        basicMove.y -= noShift[5].GetHashCode();

        basicMove *= dt * camMovementSpeed;

        camMove.x = basicMove.x * Mathf.Cos(angle) + basicMove.z * Mathf.Sin(angle);
        camMove.z = -basicMove.x * Mathf.Sin(angle) + basicMove.z * Mathf.Cos(angle);
        camMove.y = basicMove.y;
        CameraMgr.Instance.cameraPosition += camMove;
    }
    
    void ClickControls(float dt)
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftControl))
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                for (int i = 0; i < EntityMgr.Instance.selectedEntities.Count; i++)
                {
                    EntityMgr.Instance.selectedEntities[i].selectedIndicator.SetActive(false);
                }
                EntityMgr.Instance.selectedEntities.Clear();
            }


            RaycastHit[] hits;
            Ray ray = CameraMgr.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray);

            int robotIndex = -1;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.parent.GetComponent<BoatEntity>() != null)
                {
                    if (robotIndex == -1 || hits[i].distance < hits[robotIndex].distance)
                    {
                        robotIndex = i;
                    }
                }
            }

            if (robotIndex >= 0)
            {
                EntityMgr.Instance.selectedEntities.Add(hits[robotIndex].transform.parent.GetComponent<BoatEntity>());
                hits[robotIndex].transform.parent.GetComponent<BoatEntity>().selectedIndicator.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftControl))
        {
            mouseClickPosition = Input.mousePosition;
            mouseDown = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && mouseDown)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                for (int i = 0; i < EntityMgr.Instance.selectedEntities.Count; i++)
                {
                    EntityMgr.Instance.selectedEntities[i].selectedIndicator.SetActive(false);
                }
                EntityMgr.Instance.selectedEntities.Clear();
            }

            foreach (BoatEntity boat in EntityMgr.Instance.boatEntities)
            {
                if (IsWithinSelectionBounds(boat.physics.position))
                {
                    EntityMgr.Instance.selectedEntities.Add(boat);
                    boat.selectedIndicator.SetActive(true);
                }
            }
            mouseDown = false;
        }
    }

    public void OnGUI()
    {
        if (mouseDown && !PauseMgr.Instance.gamePaused)
        {
            Rect rect = Utils.GetScreenRect(mouseClickPosition, Input.mousePosition);
            GUI.color = Color.green;
            GUI.DrawTexture(rect, whiteTexture);
            GUI.color = Color.white;
        }
    }

    void KeyboardControls(float dt)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMgr.Instance.PauseGame();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(PermData.Instance.part + 1);
        }
    }

    public bool IsWithinSelectionBounds(Vector3 pos)
    {
        if (!mouseDown) { return false; }
        Bounds vpb = Utils.GetViewportBounds(Camera.main, mouseClickPosition, Input.mousePosition);
        return vpb.Contains(Camera.main.WorldToViewportPoint(pos));
    }
}
