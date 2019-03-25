using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScript : MonoBehaviour, IInputHandler
{
    public Vector3 origin_blue;
    public Vector3 origin_red;
    public Vector3 origin_unity;

    public GameObject BlueCube;
    public GameObject RedCube;
    public GameObject unitychan;

    public Animator anim;

    public void OnInputDown(InputEventData eventData)
    {
        BlueCube.transform.position = origin_blue;
        RedCube.transform.position = origin_red;
        unitychan.transform.position = origin_unity;
        //anim.Play("WAIT00", 0);
        anim.Play("female_idle_on_chair", 0);
        unitychan.GetComponent<PlayScript>().isActive = false;
    }

    public void OnInputUp(InputEventData eventData)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        BlueCube = GameObject.Find("BlueCube");
        origin_blue = BlueCube.transform.position;
        RedCube = GameObject.Find("RedCube");
        origin_red = RedCube.transform.position;
        unitychan = GameObject.Find("female_player");
        origin_unity = unitychan.transform.position;
        anim = unitychan.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
