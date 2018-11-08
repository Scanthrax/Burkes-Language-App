using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelection : MonoBehaviour
{

    public static GameObject currentMenu;

    enum Menu { Welcome, GameSelect }

    RectTransform startMenu;

    Dictionary<Menu, RectTransform> menuDictionary = new Dictionary<Menu, RectTransform>();

    public AnimationCurve animCurve;

    void Start()
    {
        // add the menus to the dictionary
        menuDictionary.Add(Menu.Welcome, GameObject.FindGameObjectWithTag("Welcome").GetComponent<RectTransform>());
        menuDictionary.Add(Menu.GameSelect, GameObject.FindGameObjectWithTag("GameSelect").GetComponent<RectTransform>());


        // put all menus off to the side
        foreach (KeyValuePair<Menu, RectTransform> entry in menuDictionary)
        {
            entry.Value.localPosition = new Vector2(750, 0);
            //entry.Value.gameObject.SetActive(false);
        }

        // declare which menu we will be starting at
        startMenu = menuDictionary[Menu.Welcome];

        // put menu at center of screen
        //startMenu.gameObject.SetActive(true);
        startMenu.localPosition = new Vector2(0, 0);

    }





    public void GoToNextMenu()
    {
        MoveTo(startMenu, 0.75f);
        MoveTo(menuDictionary[Menu.GameSelect], 0.75f);
    }

    void MoveTo(RectTransform obj, float duration)
    {
        StartCoroutine(AnimateMove(obj, obj.position, new Vector2(obj.position.x - 750, obj.position.y), duration));
    }

    IEnumerator AnimateMove(RectTransform obj, Vector2 origin, Vector2 target, float duration)
    {
        float journey = 0f;
        float percent = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            percent = Mathf.Clamp01(journey / duration);

            float curvePercent = animCurve.Evaluate(percent);
            obj.transform.position = Vector2.LerpUnclamped(origin, target, curvePercent);

            yield return null;
        }
    }

    
}
