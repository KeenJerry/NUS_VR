using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardController : MonoBehaviour {
    public UnityEvent callMenu;
    public UnityEvent previewManual;
    public UnityEvent nextManual;
    public UnityEvent chooseManual;
    public UnityEvent startGame;
    public UnityEvent cut;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) callMenu.Invoke();
        else if (Input.GetKeyDown(KeyCode.A)) previewManual.Invoke();
        else if (Input.GetKeyDown(KeyCode.D)) nextManual.Invoke();
        else if (Input.GetKeyDown(KeyCode.Return)) chooseManual.Invoke();
        else if (Input.GetKeyDown(KeyCode.C)) cut.Invoke();
        else if (Input.GetKeyDown(KeyCode.Space)) startGame.Invoke();
	}
}
