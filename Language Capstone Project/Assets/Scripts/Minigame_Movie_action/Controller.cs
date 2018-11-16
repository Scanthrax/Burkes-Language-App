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

    public string wordRecognized;
    public bool recognizedNewWord = false;

    List<GameObject> actions = new List<GameObject>();


    public float timer;
    public Text timerText;
    public Object menuScene;


    public ActionObject[] actionObjects;


    public Dictionary<string, List<GameObject>> wordToObject = new Dictionary<string, List<GameObject>>();

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
                speed = 1f;
                break;
        }

        for (int i = 0; i < numberOfActions; i++)
        {
            float x = Random.Range(-screen.transform.localScale.x * 5f, screen.transform.localScale.x * 5f);
            float y = Random.Range(-screen.transform.localScale.z * 5f, screen.transform.localScale.z * 5f);

            var obj = Instantiate(action, new Vector3(screen.transform.position.x + x,screen.transform.position.y + y, screen.transform.position.z - 0.01f), Quaternion.identity).GetComponent<movement>();
            actions.Add(obj.gameObject);
            obj.speed = speed;
            obj.actionObj = actionObjects[Random.Range(0, actionObjects.Length)];

            var sentence = obj.actionObj.sentence;

            if (!wordToObject.ContainsKey(sentence))
            {
                wordToObject.Add(sentence, new List<GameObject>());
            }
            wordToObject[sentence].Add(obj.gameObject);

        }

        foreach (var item in wordToObject[actionObjects[0].sentence])
        {
            print(item.GetComponent<movement>().actionObj.name);
        }

        timer = 60f;


    }
	
	// Update is called once per frame
	void Update () {
		

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (recognizedNewWord)
        {
            if (wordToObject.ContainsKey(wordRecognized))
            {
                
                

                points[0] += RandomPosition(wordRecognized) * 10;

                text[0].text = points[0].ToString();
            }
            recognizedNewWord = false;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            RandomPosition(actionObjects[0].sentence);
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


        timer -= Time.deltaTime;
        timerText.text = Mathf.RoundToInt(timer).ToString();
        if(timer <= 0f)
        {
            EndGame();
        }

    }

    void RandomPosition()
    {
        int rand = Random.Range(0, actions.Count);

        float x = Random.Range(-screen.transform.localScale.x * 5f, screen.transform.localScale.x * 5f);
        float y = Random.Range(-screen.transform.localScale.z * 5f, screen.transform.localScale.z * 5f);

        actions[rand].transform.position = new Vector3(screen.transform.position.x + x, screen.transform.position.y + y, screen.transform.position.z - 0.01f);
    }

    int RandomPosition(string key)
    {
        int amount = wordToObject[key].Count;
        print(amount);

        foreach (GameObject item in wordToObject[key].ToArray())
        {
            float x = Random.Range(-screen.transform.localScale.x * 5f, screen.transform.localScale.x * 5f);
            float y = Random.Range(-screen.transform.localScale.z * 5f, screen.transform.localScale.z * 5f);

            item.transform.position = new Vector3(screen.transform.position.x + x, screen.transform.position.y + y, screen.transform.position.z - 0.01f);

            var temp = item.GetComponent<movement>().actionObj.sentence;


            wordToObject[temp].Remove(item);

            item.GetComponent<movement>().SetUp(actionObjects[Random.Range(0,actionObjects.Length)]);

            temp = item.GetComponent<movement>().actionObj.sentence;

            wordToObject[temp].Add(item);
        }

        return amount;
    }


    void EndGame()
    {
        StaticVariables.minigame.Scores.Add(points[0]);
        SceneManager.LoadScene(menuScene.name);
    }
}
