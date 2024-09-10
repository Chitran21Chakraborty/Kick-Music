using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DG.Tweening;
using Sourav.Engine.Core.ControllerRelated;
using Sourav.Engine.Core.NotificationRelated;
using Sourav.Engine.Editable.DataRelated;
using Sourav.Engine.Editable.NotificationRelated;
using Sourav.Utilities.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIController : Controller
{
    [SerializeField]private float lastObjTargetYPos = -420f;
    public RectTransform target;
    [SerializeField]private List<GameObject> _unlockedInstruments = new List<GameObject>();
    private Coroutine routine;
    private bool isGameOver = false;
    public AudioSource calculationAudio;
    [SerializeField]private List<MusicElement> musicItems;
    
    [SerializeField]private float decreaseTime;

    public override void OnNotificationReceived(Notification notification, NotificationParam param = null)
    {
        switch (notification)
        {
            case Notification.StartGame:

                SetLevelTexts();

                UIDataSet();

                OnEnableUIAnimation();
                break;
            
            case Notification.LastObjectMovement:
                
                if (routine != null)
                {
                    StopCoroutine(routine);
                }

                if (App.GetLevelData().doNextLevel)
                {
                    return;
                }
                routine = StartCoroutine(OnForwardMovement(App.GetLevelData()._count * 1f / App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter.Count));
                break;
            
            case Notification.ResetGame:
                t = 0;
                App.GetLevelData()._count = 0;
                App.GetLevelData().doNextLevel = true;
                App.Notify(Notification.HapticSuccess);
                App.Notify(Notification.NextButtonSoundPlay);
                isGameOver = false;
                if (routine != null)
                {
                    StopCoroutine(routine);
                }
                OnResetGame();
                decreaseTime = (float)App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].coin;
                break;

            case Notification.ClaimCoin:
                App.GetLevelData().Coins += App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].coin;
                StartCoroutine(OnClaimedCoin());
                StartCoroutine(ContiniousHaptic());
                break;
            
            case Notification.LevelComplete:
                OnLevelComplete();
                App.Notify(Notification.LevelCompleteSoundPlay);
                break;
        }
    }

    private void SetLevelTexts()
    {
        App.GetUiData().currentLevelText.text = "Level " + (App.GetLevelData().CurrentLevelActual + 1).ToString();
        App.GetUiData().levelCompleteCurrentText.text = "LEVEL " + (App.GetLevelData().CurrentLevelActual + 1).ToString();
        App.GetUiData().currentDummyLevelText.text = (App.GetLevelData().CurrentLevelActual + 1).ToString();
        App.GetUiData().currentCoinText.text = App.GetLevelData().Coins.ToString();
        App.GetUiData().levelCompleteCoinText.text ="+ " + App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].coin.ToString();
    }

    private void UIDataSet()
    {
        _unlockedInstruments.Clear();
        for (int i = 0; i < App.GetLevelData().UnlockedElement.Count; i++)
        {
            if (App.GetLevelData().UnlockedElement[i] == 1)
            {
                _unlockedInstruments.Add(App.GetLevelData().musicalDummyInstruments[i]);
            }
        }

        for (int i = 0; i < musicItems.Count; i++)
        {
            if (musicItems[i].coinNeededToPurchase <= App.GetLevelData().Coins)
            {
               App.GetLevelData().UnlockedMusicalElementIndex[i] = new Vector3Int(1, App.GetLevelData().UnlockedMusicalElementIndex[i].y, App.GetLevelData().UnlockedMusicalElementIndex[i].z);
            }
        }

        for (int i = 0; i < musicItems.Count; i++)
        {
            if (App.GetLevelData().UnlockedMusicalElementIndex[i].x == 0)
            {
                musicItems[i].upgradeButtonInactive.Show();
                musicItems[i].upgradeButtonActive.Hide();
                musicItems[i].lockObj.Show();
            }
            else if(App.GetLevelData().UnlockedMusicalElementIndex[i].x == 1)
            {
                musicItems[i].upgradeButtonActive.Show();
                musicItems[i].upgradeButtonInactive.Hide();
                musicItems[i].lockObj.Hide();
            }

            if (App.GetLevelData().UnlockedMusicalElementIndex[i].y == 0)
            {
                musicItems[i].isClicked = false;
                musicItems[i].cg.alpha = 1;
            }
            else if (App.GetLevelData().UnlockedMusicalElementIndex[i].y == 1)
            {
                musicItems[i].isClicked = true;
                musicItems[i].cg.alpha = 0.5f;
            }

            if (App.GetLevelData().UnlockedMusicalElementIndex[i] == new Vector3Int(1, 0, 0))
            {
                Debug.Log("You can now purchase " + musicItems[i].gameObject.name + "'d item");
                musicItems[i].ShowHand();
                if (!App.GetUiData().newItemUnlockedPopUp.gameObject.activeInHierarchy)
                {
                    App.GetUiData().newItemUnlockedPopUp.gameObject.Show();
                    App.GetUiData().newItemUnlockedPopUp.alpha = 0;
                }
                Sequence unlockedItemSequence = DOTween.Sequence();
                unlockedItemSequence.Append(App.GetUiData().newItemUnlockedPopUp.DOFade(1, 0.4f).SetDelay(2f)
                    .SetEase(Ease.OutSine).OnComplete(
                        () =>
                        {
                            unlockedItemSequence.Append(App.GetUiData().newItemUnlockedPopUp.DOFade(0, 0.4f)
                                .SetEase(Ease.OutSine).SetDelay(3f).OnComplete(() =>
                                {
                                    unlockedItemSequence.Kill();
                                }));
                        }));
            }
            
            else if (App.GetLevelData().UnlockedMusicalElementIndex[i] == new Vector3Int(1, 1, 0) || App.GetLevelData().UnlockedMusicalElementIndex[i] == new Vector3Int(1,1,1) || App.GetLevelData().UnlockedMusicalElementIndex[i] == new Vector3Int(1,0,1))
            {
                Debug.Log("You have purchased " + musicItems[i].gameObject.name + "'d item");
                musicItems[i].HideHand();
            }
        }
    }

    void OnEnableUIAnimation()
    {
        App.GetUiData().gameViewTransform.gameObject.Show();
        Sequence openSceneSequence = DOTween.Sequence();
        openSceneSequence.Append(App.GetUiData().gameViewTransform.DOMove(new Vector3(0, 0, 0), 0.9f).OnComplete(() =>
        {
            openSceneSequence.Append(App.GetUiData().currentLevelTransform.DOMove(
                new Vector3(App.GetUiData().levelTarget.position.x, App.GetUiData().levelTarget.position.y,
                    App.GetUiData().levelTarget.position.z), 0.3f).OnComplete(() =>
            {
                openSceneSequence.Append(App.GetUiData().settingsTransform.DOMove(new Vector3(App.GetUiData().settingTarget.position.x, App.GetUiData().settingTarget.position.y, App.GetUiData().settingTarget.position.z), 0.3f).SetEase(Ease.OutBack).OnComplete(
                    () =>
                    {
                        openSceneSequence.Append(App.GetUiData().coinTransform.DOMove(new Vector3(App.GetUiData().coinTargetTransform.position.x, App.GetUiData().coinTargetTransform.position.y, App.GetUiData().coinTargetTransform.position.z), 0.3f).SetEase(Ease.OutBack).OnComplete(
                            () =>
                            {
                                openSceneSequence.Kill();
                            }));
                    }));
            }));
            Sequence horizontalSequence = DOTween.Sequence();
            horizontalSequence.Append(App.GetUiData().musicalElementsList.DOMove(
                new Vector3(App.GetUiData().musicalItemTargetTransform.position.x,
                    App.GetUiData().musicalItemTargetTransform.position.y,
                    App.GetUiData().musicalItemTargetTransform.position.z), 0.5f).OnComplete(() =>
            {
                horizontalSequence.Kill();
                /*horizontalSequence.Append(App.GetUiData().nextArrowImage.transform
                    .DOMove(
                        new Vector3(App.GetUiData().nextTarget.position.x, App.GetUiData().nextTarget.position.y,
                            App.GetUiData().nextTarget.position.z), 0.8f).SetEase(Ease.OutSine).OnComplete(() =>
                    {
                        horizontalSequence.Kill();
                    }));*/
            }));
        }));
        int instrumentIndex = Random.Range(0, _unlockedInstruments.Count);
        for (int k = 0; k < _unlockedInstruments.Count; k++)
        {
            _unlockedInstruments[k].Hide();
        }
        _unlockedInstruments[instrumentIndex].Show();
        decreaseTime = 0;
    }
  
    float t = 0;
    IEnumerator OnForwardMovement(float step)
    {
        float a = 0;
        int value = 0;
        if (t < step)
        {
            a = Time.deltaTime * 0.4f;
            value = 1;
        }
        else if (step < t)
        {
            a = -Time.deltaTime * 0.4f;
            value = -1;
        }
        
        if (step <= 0.95f)
        {
            App.GetUiData().nextLevelButton.Hide();
            isGameOver = false;
        }

        while (value != 0)
        {
            t += a;
            /*float val = Mathf.Lerp(-420f, -235f, t);
            target.anchoredPosition = new Vector2(target.anchoredPosition.x, val);
            lastObj.transform.position = new Vector3(target.position.x, target.position.y, target.position.z);*/
            float val = Mathf.Lerp(0, 1, t);
            App.GetUiData().originalNextImg.fillAmount = val;
            if (val <= 0f)
            {
                Sequence fadeInSq = DOTween.Sequence();
                fadeInSq.Append(App.GetLevelData().arrowPump.cg.DOFade(0, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    fadeInSq.Kill();
                    App.GetLevelData().arrowPump.isStartedFading = true;
                }));
            }
            if (value == 1 && t >= step)
            {
                value = 0;
                t = step;
            }
            else if (value == -1 && t <= step)
            {
                value = 0;
                t = step;
            }
            yield return null;
        }

        if (step > 0.95f && !isGameOver)
        {
            isGameOver = true;
            App.GetUiData().nextLevelButton.Show();
            App.Notify(Notification.HapticMild);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(App.GetUiData().nextText.DOFade(0.7f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                sequence.Kill();
                for (int i = 0; i < App.GetUiData().particleConFetti.Length; i++)
                {
                    App.GetUiData().particleConFetti[i].Play();
                }
            }));
        }
    }
    

    void OnResetGame()
    {
        StartCoroutine(DisableDraggableObjects());
        lastObjTargetYPos = -420f;
        for (int i = 0; i < App.GetUiData().particleConFetti.Length; i++)
        {
            App.GetUiData().particleConFetti[i].Play();
        }

        for (int i = 0; i < musicItems.Count; i++)
        {
            if (musicItems[i].hand.gameObject.activeInHierarchy)
            {
                musicItems[i].HideHand();
            }

            if (App.GetLevelData().UnlockedMusicalElementIndex[i] == new Vector3Int(1,0,0))  // no of music item and no of UnlockedElement in Common data are same
            {
                App.GetLevelData().UnlockedMusicalElementIndex[i] = new Vector3Int(1,0,1);
            }
        }
    }

    IEnumerator DisableDraggableObjects()
    {
        for (int i = 0; i < App.GetUiData().objParent.childCount; i++)
        {
            App.GetUiData().objParent.GetChild(i).gameObject.Hide();
            yield return new WaitForSeconds(0.08f);
        }

        yield return null;
        App.GetUiData().nextLevelButton.Hide();
        App.GetUiData().nextText.color = new Color(App.GetUiData().nextText.color.r, App.GetUiData().nextText.color.g, App.GetUiData().nextText.color.b, 0);
        target.anchoredPosition = new Vector2(target.anchoredPosition.x, lastObjTargetYPos);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(/*lastObj.DOMove(new Vector3(App.GetUiData().lastObjectOriginalTransform.position.x, App.GetUiData().lastObjectOriginalTransform.position.y, App.GetUiData().lastObjectOriginalTransform.position.z), 0.8f).SetDelay(.2f).SetEase(Ease.OutBack)*/App
                .GetUiData().nextArrowImage.transform.DOMove(new Vector3(App.GetUiData().nextButtonLast.position.x, App.GetUiData().nextButtonLast.position.y, App.GetUiData().nextButtonLast.position.z), 0.4f).SetDelay(0.2f))
            .OnComplete(() => {
                sequence.Append(App.GetUiData().settingsTransform
                    .DOMove(
                        new Vector3(App.GetUiData().settingOriginal.position.x,
                            App.GetUiData().settingOriginal.position.y, App.GetUiData().settingOriginal.position.z),
                        0.2f).OnComplete(() =>
                        {
                            sequence.Append(App.GetUiData().currentLevelTransform
                                .DOMove(
                                    new Vector3(App.GetUiData().levelOriginal.position.x,
                                        App.GetUiData().levelOriginal.position.y,
                                        App.GetUiData().levelOriginal.position.z), 0.2f).OnComplete(() =>
                                {
                                    sequence.Append(App.GetUiData().coinTransform
                                        .DOMove(
                                            new Vector3(App.GetUiData().coinOriginalTransform.position.x,
                                                App.GetUiData().coinOriginalTransform.position.y,
                                                App.GetUiData().coinOriginalTransform.position.z), 0.2f).OnComplete(
                                            () =>
                                            {
                                                sequence.Append(App.GetUiData().levelCompletePanel
                                                    .DOScale(new Vector3(1, 1, 1), 0.3f).SetEase(Ease.OutSine).OnComplete(
                                                        () =>
                                                        {
                                                            App.GetLevelData().instrumentsCamera.gameObject.Show();
                                                            sequence.Kill();
                                                            StartCoroutine(OnLevelCompleteConFettiPLay());
                                                            App.GetUiData().gameViewTransform.gameObject.Hide();
                                                            App.GetUiData().gameViewTransform.position = new Vector3(1.1f, 0f,0f);
                                                            App.GetUiData().nextArrowImage.transform.position = new Vector3(App.GetUiData().nextOriginal.position.x, App.GetUiData().nextOriginal.position.y, App.GetUiData().nextOriginal.position.z);
                                                            App.GetLevelData().arrowPump.isPlaying = false;
                                                            App.GetLevelData().arrowPump.isStartedFading = false;
                                                            App.GetUiData().originalNextImg.fillAmount = 0;
                                                        }));
                                            }));
                                }));
                        }));
            });
        Sequence sq = DOTween.Sequence();
        sq.Append(App.GetUiData().musicalElementsList
            .DOMove(
                new Vector3(App.GetUiData().musicalElementsLastPos.position.x,
                    App.GetUiData().musicalElementsLastPos.position.y,
                    App.GetUiData().musicalElementsLastPos.position.z), 0.5f).SetDelay(0.5f).OnComplete(() =>
            {
                sq.Kill();
                App.GetUiData().musicalElementsList.position = new Vector3(App.GetUiData().musicalItemsOriginalTransform.position.x, App.GetUiData().musicalItemsOriginalTransform.position.y, App.GetUiData().musicalItemsOriginalTransform.position.z);
            }));
        for (int i = 0; i < App.GetUiData().objParent.childCount; i++)
        {
            Destroy(App.GetUiData().objParent.GetChild(i).gameObject);
        }
    }

    IEnumerator OnClaimedCoin()
    {
        if (App.GetLevelData().IsSfxOn)
        {
            calculationAudio.Play();
        }
        int animatedCoinNum = 0;
        while (decreaseTime >= 0)
        {
            //App.Notify(Notification.Calculation);
            decreaseTime -= Time.deltaTime * 40f;
            animatedCoinNum = (int)decreaseTime;
            App.GetUiData().levelCompleteCoinText.text = "+ " + animatedCoinNum.ToString();
            if (decreaseTime <= 0)
            {
                App.Notify(Notification.LevelComplete);
                if (App.GetLevelData().IsSfxOn)
                {
                    calculationAudio.Stop();
                }
            }
            yield return null;
        }
    }

    IEnumerator ContiniousHaptic()
    {
        while (decreaseTime >= 0)
        {
            App.Notify(Notification.HapticLight);
            yield return new WaitForSeconds(0.035f);
        }
    }

    void OnLevelComplete()
    {
        App.GetLevelData().CurrentLevel++;
        App.Notify(Notification.HapticMild);
                                    
        if (App.GetLevelData().CurrentLevel >= App.GetStageData().levelInfo.Count)
        {
            App.GetLevelData().CurrentLevel = 0;
        }
        App.GetLevelData().CurrentLevelActual++;
        Sequence lsq = DOTween.Sequence();
        App.GetLevelData().instrumentsCamera.gameObject.Hide();
        lsq.Append(App.GetUiData().levelCompletePanel.DOMove(new Vector3(App.GetUiData().levelCompleteTargetTransform.position.x, App.GetUiData().levelCompleteTargetTransform.position.y, App.GetUiData().levelCompleteTargetTransform.position.z), 0.4f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            lsq.Kill();
            App.GetUiData().levelCompletePanel.localScale = Vector3.zero;
            App.GetUiData().levelCompletePanel.position = new Vector3(App.GetUiData().levelCompleteOriginalTransform.position.x, App.GetUiData().levelCompleteOriginalTransform.position.y, App.GetUiData().levelCompleteOriginalTransform.position.z);
            App.Notify(Notification.StartGame);
        }));
    }

    IEnumerator OnLevelCompleteConFettiPLay()
    {
        for (int i = 0; i < App.GetUiData().levelCompleteParticles.Length; i++)
        {
            App.GetUiData().levelCompleteParticles[i].Play();
            yield return new WaitForSeconds(0.2f);
        }
    }
}