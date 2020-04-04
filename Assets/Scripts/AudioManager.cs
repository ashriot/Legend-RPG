using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance;

  public Sound[] bgms;
  public Sound[] sfxs;

  void Awake() {
    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(this);
      return;
    }

    foreach (var bgm in bgms) {
      bgm.source = gameObject.AddComponent<AudioSource>();
      bgm.source.clip = bgm.clip;

      bgm.source.volume = bgm.volume;
      bgm.source.pitch = bgm.pitch;
    }

    foreach (var sfx in sfxs) {
      sfx.source = gameObject.AddComponent<AudioSource>();
      sfx.source.clip = sfx.clip;

      sfx.source.volume = sfx.volume;
      sfx.source.pitch = sfx.pitch;
    }
  }

  public void PlayBgm(string name) {
    var bgm = Array.Find(bgms, s => s.name == name);
    if (bgm is null) { return; }
    bgm.source.Play();
  }

  public void PlaySfx(string name) {
    var sfx = Array.Find(sfxs, s => s.name == name);
    if (sfx is null) { return; }
    sfx.source.Play();
  }
}
