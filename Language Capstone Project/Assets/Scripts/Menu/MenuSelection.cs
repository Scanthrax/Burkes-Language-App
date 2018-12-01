using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelection : MonoBehaviour
{
    public static MenuSelection instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }




    /// <summary>
    /// Directions for the menu transitions
    /// </summary>
    public enum Direction { Left, Right, Up, Down}

    /// <summary>
    /// This dictionary keeps track of the different menus.  Feeding in a menu name will obtain the menu
    /// </summary>
    public Dictionary<string, RectTransform> menuDictionary = new Dictionary<string, RectTransform>();

    /// <summary>
    /// This curve controls the sweeping motion between menu transitions
    /// </summary>
    public AnimationCurve animCurve;

    /// <summary>
    /// The canvas contains all the menus
    /// </summary>
    RectTransform Canvas;

    /// <summary>
    /// The duration of the menu transitions
    /// </summary>
    public float duration = 0.4f;

    public Object introScene;

    public string currentMenu;

    void Start()
    {
        Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();

        // add the menus to the dictionary
        menuDictionary.Add("welcome", Canvas.Find("Welcome").GetComponent<RectTransform>());
        menuDictionary.Add("game select", Canvas.Find("Game Container").GetComponent<RectTransform>());
        menuDictionary.Add("choose language", Canvas.Find("Choose Language").GetComponent<RectTransform>());
        menuDictionary.Add("choose difficulty", Canvas.Find("Difficulty Select").GetComponent<RectTransform>());
        menuDictionary.Add("topic select", Canvas.Find("Choose Topics").GetComponent<RectTransform>());

        // set button interactions
        // Get menu                  // locate the button & get the component   // add the function to the button listener  // out menu             // in menu                      // direction of sweep
        //menuDictionary[Menu.Welcome].Find("Button").GetComponent<Button>().onClick.AddListener(delegate { GoToNextMenu(menuDictionary[Menu.Welcome], menuDictionary[Menu.ChooseLanguage],Direction.Down); });
        //menuDictionary[Menu.GameSelect].Find("Button").GetComponent<Button>().onClick.AddListener(delegate { GoToNextMenu(menuDictionary[Menu.GameSelect], menuDictionary[Menu.Welcome],Direction.Up); });
        //menuDictionary[Menu.ChooseLanguage].Find("English").GetComponent<Button>().onClick.AddListener(delegate { GoToNextMenu(menuDictionary[Menu.ChooseLanguage], menuDictionary[Menu.Welcome], Direction.Down); });
        //menuDictionary[Menu.ChooseLanguage].Find("Spanish").GetComponent<Button>().onClick.AddListener(delegate { GoToNextMenu(menuDictionary[Menu.ChooseLanguage], menuDictionary[Menu.Welcome], Direction.Down); });

        // disable all menus
        foreach (KeyValuePair<string, RectTransform> entry in menuDictionary)
        {
            entry.Value.gameObject.SetActive(true);
            entry.Value.localPosition = new Vector2(0, 1334);
        }

        // declare which menu we will be starting at
        currentMenu = "choose language";
        RectTransform startMenu = menuDictionary[currentMenu];
        // put menu at center of screen
        startMenu.gameObject.SetActive(true);
        startMenu.localPosition = new Vector2(0, 0);

        StaticVariables.introScene = introScene;
    }





    public void GoToNextMenu(string menu)
    {
        MoveMenus(currentMenu, true);
        MoveMenus(menu, false);
    }


    /// <summary>
    /// Transition between 2 menus
    /// </summary>
    /// <param name="obj">The menu to move</param>
    /// <param name="setInactive">Do we set the menu as inactive after transitioning?</param>
    void MoveMenus(string menu, bool setInactive)
    {
        StartCoroutine(AnimateMove(menu, setInactive));
    }

    IEnumerator AnimateMove(string menu, bool setInactive)
    {
        RectTransform obj = menuDictionary[menu];

        Vector2 target;
        Vector2 origin;

        Vector2 offset;

        Direction dir = Direction.Down;
        switch (dir)
        {
            case Direction.Right:
                offset = new Vector2(750, 0);
                break;
            case Direction.Left:
                offset = new Vector2(-750, 0);
                break;
            case Direction.Up:
                offset = new Vector2(0, 1334);
                break;
            case Direction.Down:
                offset = new Vector2(0, -1334);
                break;
            default:
                offset = new Vector2(750, 0);
                break;
        }
        // If false, this means that the menu is currently disabled.  We need to enable it & set it off screen so it can swipe in
        if (!setInactive)
        {
            obj.gameObject.SetActive(true);
            origin = offset;
            target = Vector2.zero;
        }
        // otherwise, we need to tell the current menu to go off the screen
        else
        {
            origin = Vector2.zero;
            target = -offset;
        }

        // timer for moving the menu
        float journey = 0f;
        // percentage of completion, used for finding position on animation curve
        float percent = 0f;

        // keep adjusting the position while there is time
        while (journey <= duration)
        {
            // add to timer
            journey = journey + Time.deltaTime;
            // calculate percentage
            percent = Mathf.Clamp01(journey / duration);
            // find the percentage on the curve
            float curvePercent = animCurve.Evaluate(percent);
            // adjust the position of the menu
            obj.transform.localPosition = Vector2.LerpUnclamped(origin, target, curvePercent);
            // wait a frame
            yield return null;
        }

        currentMenu = menu;

        // the loop is now over, so if the menu is going out, we disable it
        if (setInactive)
            obj.gameObject.SetActive(false);

        
    }

    
}
