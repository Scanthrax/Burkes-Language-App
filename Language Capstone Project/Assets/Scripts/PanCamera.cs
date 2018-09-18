using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using UnityEngine.UI;

public class PanCamera : MonoBehaviour
{
    // reference to camera
    Camera cam;

    // offset that pulls camera back from capsules
    public Vector3 camOffset = new Vector3(0, 3, -10);

    // offset between each capsule
    float spawnOffset = 10f;

    // amount of capsules to spawn
    public int amtOfPlayers = 3;

    // capsule prefab to spawn
    public GameObject playerPrefab;

    // point to begin spawning capsules
    public Transform spawnPoint;

    // references to each capsule
    public GameObject[] players;

    // curve that controls camera panning
    public AnimationCurve animCurve;

    // is the camera panning?
    bool cameraPanning = false;

    // which capsule does the camera target?
    int camTarget = -1;

    // duration (seconds) between camera transitions
    [SerializeField]
    public float duration = 1f;

    IntroState state = IntroState.Intro;

    public KeyCode nextState = KeyCode.Q;

    public CharacterObject[] characters;

    public GameObject textPanel;
    Text text;
    public Vector3 spawnPointCamera;

    void Start ()
    {
        // get reference to camera
        cam = Camera.main;

        // initialize array
        players = new GameObject[amtOfPlayers];

        text = textPanel.GetComponentInChildren<Text>();

        // spawn the appropriate amount of capsules
        for (int i = 0; i < amtOfPlayers; i++)
        {
            // store reference of each instanciated capsule
            players[i] = Instantiate(playerPrefab,
                // the x-position of spawn is determined by the spawn point's x-coord, the position in the array, and the offset between each capsule
                new Vector3(spawnPoint.transform.position.x + (i * spawnOffset),
                // set y & z
                spawnPoint.transform.position.y,
                spawnPoint.transform.position.z),
                // identity rotation
                Quaternion.identity);

            var capsule = players[i].GetComponent<Character>();
            capsule.character = characters[(int)Random.Range(0, characters.Length)];
            print(capsule.character.name);
        }


        var midpoint = (players[0].transform.position.x + players[players.Length - 1].transform.position.x) / 2f;
        spawnPointCamera.x = midpoint;
        spawnPointCamera.z = amtOfPlayers * -5f;

        cam.transform.position = spawnPointCamera;
    }

    void Update()
    {
        switch(state)
        {
            case IntroState.Intro:
                if (Input.GetKeyDown(nextState))
                {
                    state = IntroState.Applause;
                    textPanel.SetActive(false);
                }
                break;
            case IntroState.Applause:
                if (Input.GetKeyDown(nextState))
                {
                    state = IntroState.CameraPan;
                    NextCameraTarget();
                }
                break;
            case IntroState.CameraPan:
                // if we press key and the camera is not panning...
                if (!cameraPanning)
                {
                    // pan the camera to the next target
                    if (camTarget >= 0)
                    {
                        MoveTo(cam.gameObject, players[camTarget].transform.position + camOffset, duration);
                    }
                    else
                    {
                        MoveTo(cam.gameObject, spawnPointCamera, duration);
                    }
                }
                break;
        }

        
	}


    void NextCameraTarget()
    {
        camTarget++;
        if(camTarget == amtOfPlayers)
        {
            camTarget = -1;
        }
    }

    void MoveTo(GameObject obj, Vector3 target, float duration)
    {
        StartCoroutine(AnimateMove(obj, obj.transform.position, target, duration));
    }


    IEnumerator AnimateMove(GameObject obj, Vector3 origin, Vector3 target, float duration)
    {
        cameraPanning = true;
        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);

            float curvePercent = animCurve.Evaluate(percent);
            obj.transform.position = Vector3.LerpUnclamped(origin, target, curvePercent);

            yield return null;
        }

        cameraPanning = false;
        state = IntroState.Intro;
        textPanel.SetActive(true);
        if (camTarget >= 0)
        {
            var charTemp = players[camTarget].GetComponent<Character>().character;
            string s1 = "This contestant's name is ";
            string s2 = charTemp.name;
            string s3 = ".  ";
            string s4 = charTemp.isMale ? "His" : "Her";
            string s5 = " country of origin is ";
            string s6 = charTemp.nationality;
            string s8 = "\n";
            string s7 = string.Concat(new string[]{ s1,s2,s3,s8,s4,s5,s6,s3});

            text.text = s7;
        }
        else
        {
            text.text = "I will now present the contestants!";
        }
    }
}
