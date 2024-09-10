using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sourav.Engine.Core.GameElementRelated;
using Sourav.Utilities.Extensions;
using UnityEngine;

public class SettingsShow : GameElement
{
    public GameObject settingsHideObject;
    public GameObject settingShowObject;
    public Transform settingBGTargetTransform;
    public Transform settingBGOriginalTransform;
    public Transform settingsBGTransform;

    public void ShowSettings()
    {
        settingsHideObject.Show();
        settingShowObject.Hide();
        Sequence settingsShowSequence = DOTween.Sequence();
        settingsShowSequence
            .Append(settingsBGTransform.DOMove(
                new Vector3(settingBGTargetTransform.position.x, settingBGTargetTransform.position.y,
                    settingBGTargetTransform.position.z), 0.4f)).SetEase(Ease.OutBack).OnComplete(() =>
            {
                settingsShowSequence.Kill();
            });
        for (int i = 0; i < App.GetLevelData().currentDraggableElements.Count; i++)
        {
            App.GetLevelData().currentDraggableElements[i].isDraggable = false;
        }
        for (int i = 0; i < App.GetLevelData()._SenderComponents.Count; i++)
        {
            App.GetLevelData()._SenderComponents[i].shooter.canShoot = false;
        }
    }

    public void CloseSettings()
    {
        settingsHideObject.Hide();
        settingShowObject.Show();
        Sequence settingsHideSequence = DOTween.Sequence();
        settingsHideSequence
            .Append(settingsBGTransform.DOMove(
                new Vector3(settingBGOriginalTransform.position.x, settingBGOriginalTransform.position.y,
                    settingBGOriginalTransform.position.z), 0.4f)).SetEase(Ease.OutBack).OnComplete(() =>
            {
                settingsHideSequence.Kill();
            });
        for (int i = 0; i < App.GetLevelData().currentDraggableElements.Count; i++)
        {
            App.GetLevelData().currentDraggableElements[i].isDraggable = true;
        }

        for (int i = 0; i < App.GetLevelData()._SenderComponents.Count; i++)
        {
            App.GetLevelData()._SenderComponents[i].shooter.canShoot = true;
        }
    }
}