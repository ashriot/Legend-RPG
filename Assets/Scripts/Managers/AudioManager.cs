using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {
  
  public Sound[] bgms;
  public Sound[] sfxs;

  protected override void Awake() {
    base.Awake();

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

  public void PlaySfx(string name, float volume = 1f, float pitch = 1f) {
    var sfx = Array.Find(sfxs, s => s.name == name);
    if (sfx is null) { return; }
    sfx.source.volume = volume;
    sfx.source.pitch = pitch;
    sfx.source.Play();
  }
}
