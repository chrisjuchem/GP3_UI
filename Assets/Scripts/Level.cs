using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour {
	public Text TextPrefab;
	public Button buttonPrefab;

	public string description;

	public string[] buttonTexts;
	public string[] links;

	private Text text;
	private List<Button> buttons = new List<Button>();

	public void Activate () {
		gameObject.SetActive (true);

		//set up the level text
		text = Instantiate (TextPrefab);
		text.rectTransform.position = new Vector3 (0, -100, 0);
		text.rectTransform.SetParent(transform, false);
		text.text = description;

		int stop = Mathf.Min (buttonTexts.Length, links.Length);
		for (int i = 0; i < stop; i++) {
			//make a new button at the right position
			Button b = Instantiate (buttonPrefab);
			b.transform.position = new Vector3 ((-100 * (stop - 1)) + (200 * i), 160, 0);
			b.transform.SetParent (transform, false);

			//set its text and behavior
			b.GetComponentInChildren<Text> ().text = buttonTexts [i];
			int capturedI = i;
			b.onClick.AddListener (() => {
				goToLevel (links [capturedI]);
			});

			//store it in a collection to be removed;
			buttons.Add (b);
		}
	}

	private void goToLevel(string suffix){
		//try to find the next level
		Transform next = transform.parent.Find ("Level" + suffix);
		if (next != null) {
			//remove created UI elements
			Object.Destroy (text.gameObject);
			foreach (Button b in buttons) {
				Object.Destroy (b.gameObject);
			}
			buttons = new List<Button> ();

			//hide this and activate next level
			gameObject.SetActive (false);
			next.gameObject.GetComponent<Level> ().Activate ();
		} else {
			Debug.Log ("Can't find Level" + suffix);
		}
	}

}