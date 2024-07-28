using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    public static SoundManager Instance { get; private set; }

    [SerializeField] private SO_AudioClipRef audioClipRefSO;

    private float volume =1f;

    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSucc += DM_OnRecipeSucc;
        DeliveryManager.Instance.OnRecipeFail += DM_OnRecipeFail;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        //Player.Instance.OnPickSomething += Player_OnPickSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        playSound(audioClipRefSO.trash, (sender as TrashCounter).transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        playSound(audioClipRefSO.objectDrop, (sender as BaseCounter).transform.position);
    }

    private void Player_OnPickSomething(object sender, System.EventArgs e)
    {
        //playSound(audioClipRefSO.objectGrab, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        playSound(audioClipRefSO.chop, ((CuttingCounter)sender).transform.position);
    }

    private void DM_OnRecipeFail(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        playSound(audioClipRefSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DM_OnRecipeSucc(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        playSound(audioClipRefSO.deliverySucc, deliveryCounter.transform.position);
    }

    private void playSound(AudioClip[] audioCliparray, Vector3 postion, float valume = 1f)
    {

        playSound(audioCliparray[Random.Range(0, audioCliparray.Length)], postion, valume);
    }
    private void playSound(AudioClip audioClip, Vector3 postion, float valumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, postion, valumeMultiplier * volume);
    }

    public void PlayFootStepsSounds(Vector3 postion,float valume)
    {
        playSound(audioClipRefSO.footStep, postion, valume);
    }

    public void ChangeVolume()
    {
        volume += .1f;
        if(volume>1f)
        {
            volume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME,volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume; 
    }
}
