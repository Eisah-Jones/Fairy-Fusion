using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    // axis
    public static float MainHorizontal()
    {
        float r = 0.0f;
        r += Input.GetAxisRaw("J_MainHorizontal");
        r += Input.GetAxisRaw("K_MainHorizontal");
        return Mathf.Clamp(r, -1.0f, 1.0f);

    }
    public static float MainVertical()
    {
        float r = 0.0f;
        r += Input.GetAxisRaw("J_MainVertical");
        r += Input.GetAxisRaw("K_MainVertical");
        return Mathf.Clamp(r, -1.0f, 1.0f);

    }
    public static Vector2 MainJoystick()
    {
        return new Vector2(MainHorizontal(), MainVertical());
    }


    //    //buttons
    //public static float vacuumButton()
    //{
    //    Debug.Log("Vacuum!!!!");
    //    return Input.GetAxisRaw("VacuumButton");
    //}
    //public static float shootButton()
    //{
    //    return Input.GetAxisRaw("ShootButton");
    //}

    

}
