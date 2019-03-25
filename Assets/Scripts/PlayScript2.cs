using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayScript2 : MonoBehaviour, IInputClickHandler
{
    public Animator anim;
    public Rigidbody rbody;

    public bool isActive = false;
    public bool enableGest = true;
    public bool sayingHi = false;
    public bool hasSaidHi;

    public float accX = 0.4f;
    public float accZ = 0.4f;
    //public float stopDist = 0.5f;

    public float inputX;
    public float inputZ;

    public GameObject sph;
    public GameObject uitext;
    public GameObject lumber;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (isActive)
        {
            if (enableGest)
            {
                if (!sayingHi)
                    StartCoroutine(Rotate(1.5f));
            }
            else
            {
                anim.Play("female_idle_breath", 0);
                isActive = false;
            }
        }
        else
        {
            anim.Play("Walk", 0);
            isActive = true;
        }
    }

    private IEnumerator Rotate(float duration)
    {
        sayingHi = true;
        Quaternion startRotation = gameObject.transform.rotation;
        Quaternion endRotation = new Quaternion(0, 180, 0, 0);
        anim.Play("female_greet", 0);
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            gameObject.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }
        gameObject.transform.rotation = startRotation;
        sayingHi = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rbody = gameObject.GetComponent<Rigidbody>();
        inputX = GameObject.Find("BlueCube").transform.position.x;
        inputZ = GameObject.Find("BlueCube").transform.position.z;
        sph = GameObject.Find("InsideOutSphere");
        uitext = GameObject.Find("HoloHUDText");
        //lumber = GameObject.Find("Lumberjack1");
    }

    // Update is called once per frame
    void Update()
    {
        inputX = GameObject.Find("BlueCube").transform.position.x;
        inputZ = GameObject.Find("BlueCube").transform.position.z;

        float positionX = gameObject.transform.position.x;
        float positionZ = gameObject.transform.position.z;

        anim.SetFloat("inputX", positionX - inputX);
        anim.SetFloat("inputY", positionZ - inputZ);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") && isActive)
        {
            int inputX2 = (positionX - inputX) < 0 ? 1 : -1;
            int inputZ2 = (positionZ - inputZ) < 0 ? 1 : -1;

            float moveX = inputX2 * accX;
            float moveZ = inputZ2 * accZ;

            rbody.velocity = new Vector3(moveX, 0, moveZ);
        }
        else if (!isActive)
        {
            rbody.velocity = Vector3.zero;
        }
        else if (sayingHi)
        {
            rbody.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
        Debug.Log(hasSaidHi);
        hasSaidHi = lumber.GetComponent<CollisionChecker>().hasSaidHello;
        if (col.gameObject.name == "BlueCube" && hasSaidHi)
        {
            Debug.Log("Collision Detected: Win");
            anim.Play("female_say_yes", 0);
            isActive = false;

            sph.GetComponent<VideoController>().initiate_player = true;

            uitext.GetComponent<UIController>().winCount += 1;
            uitext.GetComponent<UIController>().UpdateText();

            List<TodoItem> tdi = GameObject.Find("HoloHUDText2").GetComponent<LightBuzzManager>().todoItems;
            TodoItem video = tdi.Where(x => x.Text == "watch the video").SingleOrDefault();
            video.Complete = true;
            GameObject.Find("HoloHUDText2").GetComponent<LightBuzzManager>().UpdateElement(video);
        }
        else
        {
            Debug.Log("Collision Detected: Lose");
            anim.Play("female_say_no", 0);
            isActive = false;

            uitext.GetComponent<UIController>().loseCount += 1;
            uitext.GetComponent<UIController>().UpdateText();
        }
    }

}
