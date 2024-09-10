using System.Collections;
using System.Collections.Generic;
using Sourav.Engine.Editable.DataRelated;
using UnityEngine;
using UnityEngine.UI;

public class UIData : CommonData
{
    public Text currentLevelText;
    public Text currentCoinText;
    public Transform settingsTransform, currentLevelTransform, coinTransform, musicalElementsList;
    public GameObject nextLevelButton;
    public Text nextText;
    public Transform objParent;
    public Transform gameViewTransform;
    public Transform settingTarget, levelTarget, coinTargetTransform, musicalItemTargetTransform, musicalElementsLastPos;
    public Transform settingOriginal, levelOriginal, coinOriginalTransform, musicalItemsOriginalTransform;
    public ParticleSystem[] particleConFetti;
    public Image nextArrowImage, originalNextImg;
    public Transform nextOriginal, nextTarget, nextButtonLast;

    #region Enable screen related

    public Text currentDummyLevelText;

    #endregion

    public GameObject tutorialParent;
    public Transform tutoriallDragHand;
    public Transform shootTutorialText;
    public GameObject shootText;
    public CanvasGroup enoughCoinImg;
    public CanvasGroup newItemUnlockedPopUp;

    #region Level Complete related

    public Transform levelCompletePanel;
    public Transform levelCompleteTargetTransform, levelCompleteOriginalTransform;
    public Text levelCompleteCoinText;
    public Text levelCompleteCurrentText;
    public ParticleSystem[] levelCompleteParticles;

    #endregion
}