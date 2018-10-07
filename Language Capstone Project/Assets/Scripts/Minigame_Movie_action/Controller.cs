using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    public Renderer ground, screen;
    public GameObject action;
    public int numberOfActions;
    public Difficulty difficulty;
    public GameObject[] players;
    public Text[] text;
    int[] points = new int[4];

    List<GameObject> actions = new List<GameObject>();

	// Use this for initialization
	void Start () {
        ground.material.color = Color.gray;
        screen.material.color = Color.gray;

        float speed;
        switch(difficulty)
        {
            case Difficulty.Easy:
                speed = 1f;
                break;
            case Difficulty.Intermediate:
                speed = 2f;
                break;
            case Difficulty.Difficult:
                speed = 3f;
                break;
            default:
                speed = 0f;
                break;
        }

        for (int i = 0; i < numberOfActions; i++)
        {
            float x = Random.Range(-screen.transform.localScale.x * 5f, screen.transform.localScale.x * 5f);
            float y = Random.Range(-screen.transform.localScale.z * 5f, screen.transform.localScale.z * 5f);

            var obj = Instantiate(action, new Vector3(screen.transform.position.x + x,screen.transform.position.y + y, screen.transform.position.z - 0.01f), Quaternion.identity);
            var comp = obj.GetComponent<movement>();
            actions.Add(obj);
            comp.speed = speed;

        }

        
    }
	
	// Update is called once per frame
	void Update () {
		

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            points[0] += 10;
            text[0].text = points[0].ToString();
            RandomPosition();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            points[1] += 10;
            text[1].text = points[1].ToString();
            RandomPosition();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            points[2] += 10;
            text[2].text = points[2].ToString();
            RandomPosition();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            points[3] += 10;
            text[3].text = points[3].ToString();
            RandomPosition();
        }

    }

    void RandomPosition()
    {
        int rand = Random.Range(0, actions.Count);

        float x = Random.Range(-screen.transform.localScale.x * 5f, screen.transform.localScale.x * 5f);
        float y = Random.Range(-screen.transform.localScale.z * 5f, screen.transform.localScale.z * 5f);

        actions[rand].transform.position = new Vector3(screen.transform.position.x + x, screen.transform.position.y + y, screen.transform.position.z - 0.01f);
    }
}
