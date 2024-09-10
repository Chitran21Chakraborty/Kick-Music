using DG.Tweening;
using Sirenix.OdinInspector;
using Sourav.Engine.Core.GameElementRelated;
using Sourav.Utilities.Extensions;
using UnityEngine;

namespace Sourav.UIPresets.CommonSettings.Scripts
{
    public class SettingsView : GameElement
    {
        [SerializeField] private GameObject settingsClose;
        [SerializeField] private GameObject bgShow;
        [SerializeField] private GameObject panel;
        [SerializeField] private float durationOfShow;
        [SerializeField] private float durationOfHide;
        [SerializeField] private Ease easeOnShow;
        [SerializeField] private Ease easeOnHide;
        [SerializeField] private bool animateOnClose;

        [SerializeField] [ReadOnly] private bool canCloseSettings;
        

        public void ShowSettings()
        {
            canCloseSettings = false;
            // DOTween.Restart(bgShow, "out");
            settingsClose.Show();
            settingsClose.transform.DOLocalRotate(new Vector3(0, 0, 180), durationOfShow);
            panel.Show();
            bgShow.transform.DOScale(new Vector3(1, 1, 1), durationOfShow).SetEase(easeOnShow).OnComplete(SettingsShown);
            for (int i = 0; i < App.GetLevelData().currentDraggableElements.Count; i++)
            {
                App.GetLevelData().currentDraggableElements[i].isDraggable = false;
            }
            for (int i = 0; i < App.GetLevelData()._SenderComponents.Count; i++)
            {
                App.GetLevelData()._SenderComponents[i].shooter.canShoot = false;
            }
        }
        public void SettingsShown()
        {
            // settingsClose.Show();
            settingsClose.transform.localRotation = Quaternion.identity;
            canCloseSettings = true;
        }

        public void HideSettings()
        {
            if(!canCloseSettings)
                return;
            if (!animateOnClose)
            {
                settingsClose.transform.localRotation = Quaternion.identity;
                settingsClose.Hide();
                bgShow.transform.localScale = new Vector3(1, 0, 1);
                panel.Hide();
            }
            else
            {
                canCloseSettings = false;
                settingsClose.transform.DOLocalRotate(new Vector3(0, 0, -180), durationOfHide);
                bgShow.transform.DOScale(new Vector3(1, 0, 1), durationOfHide).SetEase(easeOnHide).OnComplete(SettingsHidden);
                
            }
            
            for (int i = 0; i < App.GetLevelData().currentDraggableElements.Count; i++)
            {
                App.GetLevelData().currentDraggableElements[i].isDraggable = true;
            }

            for (int i = 0; i < App.GetLevelData()._SenderComponents.Count; i++)
            {
                App.GetLevelData()._SenderComponents[i].shooter.canShoot = true;
            }
        }

        private void SettingsHidden()
        {
            settingsClose.transform.localRotation = Quaternion.identity;
            settingsClose.Hide();
            bgShow.transform.localScale = new Vector3(1, 0, 1);
            panel.Hide();
        }
    }
}
