﻿using UnityEngine;
using UnityEngine.UI;

public class DialogRenderer : MonoBehaviour {
	public float writeSpeed = 50;
	public Image panel;
	public Text output;
	public DialogButton[] buttons;

	private float timer;
	private int charI;

	private DialogNode root;
	private DialogNode node;

	private void Start() {
		root = DialogNode.Default();
		node = DialogNode.Default();
		
		panel.gameObject.SetActive(false);
	}

	public void Render(DialogNode node, bool setRoot=true) {
		charI = 0;
		timer = Time.time;

		panel.gameObject.SetActive(node != null);
		if (node == null) {
			node = DialogNode.Default();
			return;
		}

		if (setRoot) root = node;
		this.node = node;
		output.text = "";
		
		for (int i = 0; i < buttons.Length; i++) {
			bool drawOption = i < node.options.Length;
			if (drawOption) {
				buttons[i].GetComponentInChildren<Text>().text = node.options[i].prompt;
				buttons[i].node = node.options[i];
			}
			buttons[i].gameObject.SetActive(drawOption);
		}
	}

	private void Update() {
		if (panel.gameObject.activeInHierarchy && Input.GetMouseButtonDown(0)) {
			if (charI < node.message.Length) {
				charI = node.message.Length;
				output.text = node.message;
			} else if (node.options.Length == 0) {
				// Leaf reached
				Render(node.loop ? root : null);
			}
		}

		if (Time.time - timer >= 1f / writeSpeed && charI < node.message.Length) {
			timer = Time.time;
			output.text += node.message[charI];
			charI++;
		}
	}
}