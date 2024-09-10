using System.Collections;
using System.Collections.Generic;
using Sourav.Engine.Core.ControllerRelated;
using Sourav.Engine.Core.NotificationRelated;
using Sourav.Engine.Editable.NotificationRelated;
using UnityEngine;

public class MainController : Controller
{
    void Start()
    {
        LoadData();
    }

    void LoadData()
    {
        App.Notify(Notification.LoadGame);
    }
    
    public override void OnNotificationReceived(Notification notification, NotificationParam param = null)
    {
        switch (notification)
        {
            case Notification.GameLoaded:
                App.Notify(Notification.PlayGame);
                break;
            
            case Notification.PlayGame:
                App.Notify(Notification.StartGame);
                break;
        }
    }
}
