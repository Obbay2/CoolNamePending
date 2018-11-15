using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelHighlightAndSelect : MonoBehaviour {

    public Button[] buttons = new Button[4];

    private int selectedButton = 0;

	// Use this for initialization
	void Start () {
        selectButton(buttons[selectedButton]);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            deselectButton(buttons[mod(selectedButton, buttons.Length)]);
            selectedButton--;
            selectButton(buttons[mod(selectedButton, buttons.Length)]);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            deselectButton(buttons[mod(selectedButton, buttons.Length)]);
            selectedButton++;
            selectButton(buttons[mod(selectedButton, buttons.Length)]);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            buttons[mod(selectedButton, buttons.Length)].onClick.Invoke();
        }
	}

    private void selectButton(Button button)
    {
        button.GetComponent<Image>().color = new Color32(0, 192, 215, 255);
    }

    private void deselectButton(Button button)
    {
        button.GetComponent<Image>().color = new Color32(0, 120, 215, 255);
    }

    private int mod(int x, int m)
    {
        int modulo = x % m;
        return modulo < 0 ? m + modulo : modulo;
    }
}
