using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : Singleton<DialogManager> {

  public GameObject dialogBox;
  public Text dialogText;
  public Animator speaker;

  public float typeSpeed;

  Queue<string> sentences;
  string currentSentence;
  bool typing;

  void Start() {
    sentences = new Queue<string>();
    if (dialogBox.activeInHierarchy) {
      dialogBox.SetActive(false);
    }
  }

  void Update() {
      
  }

  public void StartDialog(Dialog dialog, Animator animator) {
    PlayerController.GetInstance().ToggleMenuButton(false);
    speaker = animator;
    Debug.Log($"Starting conversation with {dialog.name}.");

    sentences.Clear();

    foreach (var sentence in dialog.sentences) {
      sentences.Enqueue(sentence);
    }
    dialogBox.SetActive(true);
    DisplayNextSentence();
  }

  public void DisplayNextSentence() {
    if (typing) {
      StopAllCoroutines();
      if (speaker != null) {
        speaker.SetBool("moving", false);
      }
      typing = false;
      dialogText.text = currentSentence;
    } else {
      if (sentences.Count == 0) {
        EndDialog();
        return;
      }
      currentSentence = sentences.Dequeue();
      StartCoroutine(DoTypeSentence(currentSentence));
    }
  }

  public void EndDialog() {
    dialogBox.SetActive(false);
    PlayerController.GetInstance().ToggleMenuButton(true);
    Debug.Log("End of Dialog");
  }

  IEnumerator DoTypeSentence(string sentence) {
    var index = 1;
    typing = true;
    if (speaker != null){
      speaker.SetBool("moving", true);
    }
    
    while (index <= sentence.Length) {
      dialogText.text = sentence.Substring(0, index);
      dialogText.text += $"<color=#00000000>{sentence.Substring(index)}</color>";
      index++;
      yield return new WaitForSeconds(typeSpeed);
    }

    // foreach (var letter in sentence.ToCharArray()) {
    //   dialogText.text += letter;
    //   yield return new WaitForEndOfFrame();
    // }
    typing = false;
    if (speaker != null){
      speaker.SetBool("moving", false);
    }
  }
}
