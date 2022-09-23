using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class CheckClicks : MonoBehaviour
{
    // Normal raycasts do not work on UI elements, they require a special kind
    private int difficulty = 0;
    GraphicRaycaster raycaster;

    void Awake()
    {
        // Get both of the components we need to do this
        this.raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            pointerData.position = Input.mousePosition;
            this.raycaster.Raycast(pointerData, results);

            ////For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            //foreach (RaycastResult result in results)
            //{
            //    Debug.Log("Hit " + result.gameObject.name);
            //}

            if(results.Count > 0)
            {
                RaycastResult result = results[0];

                switch (result.gameObject.name)
                {
                    case "StartButton":
                        PlayerPrefs.SetInt("difficulty", difficulty);
                        SceneManager.LoadScene(1);
                        break;
                    case "QuitButton":
                        Application.Quit();
                        break;
                    case "DifficultyButton":
                        CycleDifficulty();
                        break;
                }
            }
        }
    }

    void CycleDifficulty()
    {
        difficulty++;
        difficulty %= 3;
        TextMeshProUGUI t = GameObject.Find("DifficultyButton").GetComponent<TextMeshProUGUI>();
        switch (difficulty)
        {
            case 0:
                t.text = "Easy";
                break;
            case 1:
                t.text = "Medium";
                break;
            case 2:
                t.text = "Hard";
                break;
        }
    }
}