using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float pickupRange = 3.8f;
    [SerializeField] private float pickupForce = 150.0f;
    [SerializeField] private float throwForce = 10.0f;

    [Header("References")]
    public GameObject controls;
    public Text modeText;
    public Transform cam;

    public Animator leftHand;
    public Animator rightHand;
    public Animator leftArm;
    public Animator rightArm;
    public Transform leftHoldPoint;
    public Transform rightHoldPoint;

    private GameObject leftObj;
    private Rigidbody leftObjRB;
    private Collider leftObjCol;
    private GameObject rightObj;
    private Rigidbody rightObjRB;
    private Collider rightObjCol;

    private int currentModeInt = 0;
    private string currentMode = "Drop";
    private string[] modes = new string[] { "Drop", "Throw", "Combine" };

    void Start()
    {
        
    }

    void Update()
    {
        MyInput();
        MoveObject();
    }
    private void MyInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (controls.activeSelf)
                controls.SetActive(false);
            else if (!controls.activeSelf)
                controls.SetActive(true);
        }
        if (!controls.activeSelf)
        {
            if (Input.GetKeyUp(KeyCode.Q))
                SwitchMode(-1);
            if (Input.GetKeyUp(KeyCode.E))
                SwitchMode(1);
            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
                SwitchMode(-1);
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
                SwitchMode(1);

            if (Input.GetMouseButtonUp(0))
            {
                // grab if there is no held item
                if (leftObj == null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                    {
                        GameObject obj = hit.transform.gameObject;
                        if (obj.GetComponent<Rigidbody>())
                        {
                            leftObjRB = obj.GetComponent<Rigidbody>();
                            leftObjRB.useGravity = false;
                            leftObjRB.drag = 10;
                            leftObjRB.constraints = RigidbodyConstraints.FreezeRotation;
                            leftObjRB.transform.parent = leftHoldPoint;

                            leftObjCol = obj.GetComponent<Collider>();
                            leftObjCol.isTrigger = true;

                            leftObj = obj;

                            leftHand.Play("Grab");
                        }
                    }
                }
                // do action of there is a held item
                else
                {
                    if (currentMode == "Combine")
                    {
                        if(rightObj != null)
                        {
                            leftArm.Play("LeftCombine");
                            leftHand.Play("Ungrab");
                        }
                    }
                    else
                    {
                        leftObjCol.isTrigger = false;

                        leftObjRB.useGravity = true;
                        leftObjRB.drag = 1;
                        leftObjRB.constraints = RigidbodyConstraints.None;
                        leftObjRB.transform.parent = null;

                        if (currentMode == "Drop")
                        {
                            leftHand.Play("Ungrab");
                        }
                        else if (currentMode == "Throw")
                        {
                            leftHand.Play("Throw");
                            leftObjRB.AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
                        }

                        leftObj = null;
                    }
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (rightObj == null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                    {
                        GameObject obj = hit.transform.gameObject;
                        if (obj.GetComponent<Rigidbody>())
                        {
                            rightObjRB = obj.GetComponent<Rigidbody>();
                            rightObjRB.useGravity = false;
                            rightObjRB.drag = 10;
                            rightObjRB.constraints = RigidbodyConstraints.FreezeRotation;
                            rightObjRB.transform.parent = rightHoldPoint;

                            rightObjCol = obj.GetComponent<Collider>();
                            rightObjCol.isTrigger = true;

                            rightObj = obj;

                            rightHand.Play("Grab");
                        }
                    }
                }
                else
                {
                    if (currentMode == "Combine")
                    {
                        if (leftObj != null)
                        {
                            rightArm.Play("RightCombine");
                            rightHand.Play("Ungrab");
                        }
                    }
                    else
                    {
                        rightObjCol.isTrigger = false;

                        rightObjRB.useGravity = true;
                        rightObjRB.drag = 1;
                        rightObjRB.constraints = RigidbodyConstraints.None;
                        rightObjRB.transform.parent = null;

                        if (currentMode == "Drop")
                        {
                            rightHand.Play("Ungrab");
                        }
                        else if (currentMode == "Throw")
                        {
                            rightHand.Play("Throw");
                            rightObjRB.AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
                        }

                        rightObj = null;
                    }
                }
            }
        }
    }
    public void Combine(int hand)
    {
        if(hand == 0)
        {
            GameObject obj = leftObj;
            leftObj = null;
            Destroy(obj);
        }
        else if (hand == 1)
        {
            GameObject obj = rightObj;
            rightObj = null;
            Destroy(obj);
        }
    }
    private void SwitchMode(int mode)
    {
        currentModeInt += mode;

        if(currentModeInt > modes.Length - 1)
            currentModeInt = 0;
        else if(currentModeInt < 0)
            currentModeInt = modes.Length - 1;

        currentMode = modes[currentModeInt];
        modeText.text = modes[currentModeInt].ToString();
    }
    private void MoveObject()
    {
        if(leftObj != null)
        {
            if (Vector3.Distance(leftObj.transform.position, leftHoldPoint.position) > 0.1f)
            {
                Vector3 moveDirection = leftHoldPoint.position - leftObj.transform.position;
                leftObjRB.AddForce(moveDirection * pickupForce);
            }
        }
        if(rightObj != null)
        {
            if (Vector3.Distance(rightObj.transform.position, rightHoldPoint.position) > 0.1f)
            {
                Vector3 moveDirection = rightHoldPoint.position - rightObj.transform.position;
                rightObjRB.AddForce(moveDirection * pickupForce);
            }
        }
    }
}
