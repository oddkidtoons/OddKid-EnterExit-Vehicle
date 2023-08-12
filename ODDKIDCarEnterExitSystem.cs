using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class ODDKIDCarEnterExitSystem : MonoBehaviour
{

    public ODDKIDEnterExit playerControls;
    private InputAction enter;
    private InputAction exit;


    [Header("Car")]
    public Ashsvp.SimcadeVehicleController CarController;
    public Transform Car;
    [SerializeField] public Transform exitPositionTransform;

    [Header("Player Game Objects")]
    public Transform Player;
    public Transform PlayerInCar;
    

    [Header("Cameras")]
    public GameObject PlayerCam;
    public GameObject CarCam;

    public GameObject DriveUi;
    private bool Candrive;

    private void Awake()
    {
        playerControls = new ODDKIDEnterExit();
    }
    

    private void OnEnable()
    {
        enter = playerControls.Player.Enter;
        enter.Enable();
        enter.performed += enterCar;

        exit = playerControls.Player.Exit;
        exit.Enable();
        exit.performed += exitCar;
    }

    private void OnDisable()
    {
        enter.Disable();
        exit.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        CarController.CanDrive = false;
        CarCam.gameObject.SetActive(false);
        PlayerCam.gameObject.SetActive(true);
        DriveUi.gameObject.SetActive(false);
        PlayerInCar.gameObject.SetActive(false);

    }


    private void enterCar(InputAction.CallbackContext Context)
    {
        if (Candrive)
        {

            DriveUi.gameObject.SetActive(false);

            // Here we parent Car with player
            Player.transform.SetParent(Car);
            Player.gameObject.SetActive(false);
            PlayerInCar.gameObject.SetActive(true);

            // Camera
            PlayerCam.gameObject.SetActive(false);
            CarCam.gameObject.SetActive(true);


            Player.position = this.exitPositionTransform.position;
          

            CarController.CanDrive = true;
        }
        
    }
    private void exitCar(InputAction.CallbackContext Context)
    {
        if (Player.transform.parent == Car) { 
        // Here We Unparent the Player with Car
        Player.transform.SetParent(null);
        Player.gameObject.SetActive(true);

        PlayerInCar.gameObject.SetActive(false);

        // Here If Player Is Not Driving So PlayerCamera turn On and Car Camera turn off

        PlayerCam.gameObject.SetActive(true);
        CarCam.gameObject.SetActive(false);

        Player.position = this.exitPositionTransform.position;


        CarController.CanDrive = false;
    }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
           DriveUi.gameObject.SetActive(true);
            Candrive = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
          DriveUi.gameObject.SetActive(false);
            Candrive = false;
        }
    }
}
