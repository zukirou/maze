using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * SEを鳴らす時は必要な所で
 * Singleton<SoundPlayer>.instance.playSE("SE名")
 *  →ファイル名とサウンド名は対
 *  →いまのまんまだと、鳴らす時に読み込むので長いとちょっと読み込みで止まる
 */

public class SoundPlayer {
    BGMPlayer currentBGMPlayer;
    BGMPlayer fadeOutBGMPlayer;

    GameObject soundPlayerObj;
    AudioSource audioSource;
    Dictionary<string, AudioClipInfo> audioClips = new Dictionary<string, AudioClipInfo>();

    //AudioClipInfoはここ
    class AudioClipInfo {
        public string resourceName;
        public string name;
        public AudioClip clip;

        public AudioClipInfo(string resourceName, string name) {
            this.resourceName = resourceName;
            this.name = name;
        }
    }

    public SoundPlayer() {
        audioClips.Add("fonfonfoun", new AudioClipInfo("mazeBGM01", "fonfonfoun"));
        audioClips.Add("kekkeke", new AudioClipInfo("mazeBGM02", "kekkeke"));
    }

    public void playBGM(string bgmName, float fadeTime) {
        //古いBGMを無くす
        if (fadeOutBGMPlayer != null)
            fadeOutBGMPlayer.destory();

        //いま鳴っているBGMをフェードして消していく
        if (currentBGMPlayer != null) {
            currentBGMPlayer.stopBGM(fadeTime);
            fadeOutBGMPlayer = currentBGMPlayer;
        }

        //新たなBGMをフェードで鳴らしていく
        if(audioClips.ContainsKey(bgmName) == false) {
            //新たなBGM無し
            currentBGMPlayer = new BGMPlayer();
        }
        else {
            currentBGMPlayer = new BGMPlayer(audioClips[bgmName].resourceName);
            currentBGMPlayer.playBGM(fadeTime);
        }
    }

    public void playBGM() {
        if (currentBGMPlayer != null && currentBGMPlayer.fadeoutIs == BGMPlayer.fadeOutIs.notEnd)
            currentBGMPlayer.playBGM();
        if (fadeOutBGMPlayer != null && fadeOutBGMPlayer.fadeoutIs == BGMPlayer.fadeOutIs.notEnd)
            fadeOutBGMPlayer.playBGM();
    }

    public void pauseBGM() {
        if (currentBGMPlayer != null)
            currentBGMPlayer.pauseBGM();
        if (fadeOutBGMPlayer != null)
            fadeOutBGMPlayer.pauseBGM();
    }

    public void stopBGM(float fadeTime) {
        if (currentBGMPlayer != null)
            currentBGMPlayer.stopBGM(fadeTime);
        if (fadeOutBGMPlayer != null)
            fadeOutBGMPlayer.stopBGM(fadeTime);
    }

    public bool playSE(string seName) {
        if (audioClips.ContainsKey(seName) == false) {
            return false;
        }

        AudioClipInfo info = audioClips[seName];

        //Loadする
        if(info.clip == null) {
            info.clip = (AudioClip)Resources.Load(info.resourceName);
        }
        if(soundPlayerObj == null) {
            soundPlayerObj = new GameObject("SoundPlayer");
            audioSource = soundPlayerObj.AddComponent<AudioSource>();
        }

        //SEを再生
        audioSource.PlayOneShot(info.clip);

        return true;
    }
}
