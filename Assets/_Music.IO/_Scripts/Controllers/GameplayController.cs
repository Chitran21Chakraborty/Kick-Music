using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sourav.Engine.Core.ControllerRelated;
using Sourav.Engine.Core.NotificationRelated;
using Sourav.Engine.Editable.NotificationRelated;
using Sourav.Utilities.Extensions;
using UnityEngine;

public class GameplayController : Controller
{
    public List<Grid> grids;
    public SenderComponent senderElement;
    public ReceiverComponent receiverElement;
    public GameObject staticObject;
    [SerializeField]private List<GameObject> _unlockedDraggableItems = new List<GameObject>();
    private int index;
    
    void Start()
    {
        App.GetLevelData().hasGameStarted = true;
        StartCoroutine(StartCounter());
    }
    
    public override void OnNotificationReceived(Notification notification, NotificationParam param = null)
    {
        switch (notification)
        {
            case Notification.StartGame:
                
                if (App.GetLevelData().CurrentLevel >= App.GetStageData().levelInfo.Count)
                {
                    App.GetLevelData().CurrentLevel = 5;
                }

                if ((App.GetLevelData().CurrentLevelActual + 1) % 3 == 0)
                {
                    App.Notify(Notification.ShowInterstitial);
                }
                index = 0;
                SetStatus();
                break;

            case Notification.ResetGame:
                
                for (int i = 0; i < grids.Count; i++)
                {
                    if (grids[i].gameObject.activeInHierarchy)
                    {
                        grids[i].gameObject.Hide();
                        grids[i].isOccupied = false;
                    }
                    grids[i].dummyStair.Hide();
                }
                break;
        }
    }

    private void SetStatus()
    {
        _unlockedDraggableItems.Clear();
        for (int i = 0; i < App.GetLevelData().UnlockedElement.Count; i++)
        {
            if (App.GetLevelData().UnlockedElement[i] == 1)
            {
                _unlockedDraggableItems.Add(App.GetLevelData().draggableObjects[i]);
            }
        }
        OnEnableGame();
    }

    private void OnEnableGame()
    {
        App.GetLevelData().currentDraggableElements.Clear();
        App.GetLevelData()._receiverComponents.Clear();
        App.GetLevelData()._SenderComponents.Clear();
        App.GetLevelData().currentStageCoin = App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].coin;
        
        for (int i = 0; i < App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].gridInfo.Count; i++)
        {
            // No of Grid Generation
            grids[App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].gridInfo[i].gridStatus.y].gameObject.Show();
            grids[App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].gridInfo[i].gridStatus.y].isOccupied = false;
           

            /* FOR GENERATING DRAGGABLE ITEMS TO A PARTICULAR GRID POSITION. HERE X = 0 -> GRIDS; X = 1 -> DRAGGABLE OBJECTS, X = 2 -> STATIC OBJECTS & SO ON
             Z MEANS ARRAY INDEX OF DRAGGABLE OBJECTS */
            if (App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].gridInfo[i].gridStatus.x == 1)
            {
                if (index >= _unlockedDraggableItems.Count)
                {
                    index = 0;
                }
                GameObject go = Instantiate(_unlockedDraggableItems[index]);
                go.transform.SetParent(App.GetUiData().objParent);
                go.transform.position =
                    grids[App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].gridInfo[i].gridStatus.y]
                        .transform.position;
                go.Show();
                App.GetLevelData().currentDraggableElements.Add(go.GetComponent<DragMusicalElement>());
                index++;
            }
            
            //for static Object
            if (App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].gridInfo[i].gridStatus.x == 2)
            {
                GameObject statObj = Instantiate(staticObject, new Vector3(grids[App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].gridInfo[i].gridStatus.y].transform.position.x, grids[App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].gridInfo[i].gridStatus.y].transform.position.y, grids[App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].gridInfo[i].gridStatus.y].transform.position.z), Quaternion.identity, App.GetUiData().objParent);
                statObj.Show();
            }
        }

        //FOR GENERATING SHOOTER OBJECTS AND PROVIDE DIRECTION FROM WHERE BULLETS WILL SPAWN & ALSO PROVIDE DIRECTION OF BULLET_MOVE SCRIPT
        
        
        for (int i = 0; i < App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter.Count; i++)
        {
            ReceiverComponent receiverComponent = Instantiate(receiverElement, Vector3.Lerp(grids[App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].receiverLerpedPosition.x].transform.position, grids[App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].receiverLerpedPosition.y].transform.position, 0.5f), Quaternion.identity, App.GetUiData().objParent);
            SenderComponent senderComponent = Instantiate(senderElement, App.GetUiData().objParent) as SenderComponent;

            senderComponent.transform.position = Vector3.Lerp(grids[App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].shooterLerpedPosition.x].transform.position, grids[App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].shooterLerpedPosition.y].transform.position, 0.5f);
            senderComponent.playerTransform.eulerAngles = new Vector3(senderComponent.playerTransform.eulerAngles.x, App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].targetAngle.y, senderComponent.playerTransform.eulerAngles.z);

            float index = App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].targetAngle.y;
            if (index == -135f)
            {
                senderComponent.arrows[0].gameObject.Show();
                senderComponent.activeArrow = senderComponent.arrows[0];
            }
            if (index == 135f)
            {
                senderComponent.arrows[1].gameObject.Show();
                senderComponent.activeArrow = senderComponent.arrows[1];
            }
            if (index == 45f)
            {
                senderComponent.arrows[2].gameObject.Show();
                senderComponent.activeArrow = senderComponent.arrows[2];
            }
            if (index == -45f)
            {
                senderComponent.arrows[3].gameObject.Show();
                senderComponent.activeArrow = senderComponent.arrows[3];
            }

            senderComponent.shooter.bulletDirection.x = App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].bulletAxis.x;
            senderComponent.shooter.bulletDirection.y = App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].bulletAxis.y;
            
            senderComponent.gameObject.Show();
            receiverComponent.gameObject.Show();
            /*
             * xDegreeMultiplier & yDegreeMultiplier is used for set the axis of ball parent in which direction it will move --> MOVABLE BALL PARENT
             * angle and rotationSpeed is used for set the rotation angle of rotating ball --> ROTATING BALL
             */
            senderComponent.shooter.bullet.xDegreeMultiplier = App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].bulletAxis.x;
            
            senderComponent.shooter.bullet.yDegreeMultiplier = App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].bulletAxis.y;

            senderComponent.shooter.bullet.rotatingFootball.angle = (float)App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].ballRotationAxis.x;

            senderComponent.shooter.bullet.rotatingFootball.rotationSpeed = (float) App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].ballRotationAxis.y;
            
            receiverComponent.transform.eulerAngles = new Vector3(receiverComponent.transform.eulerAngles.x, (float)App.GetStageData().levelInfo[App.GetLevelData().CurrentLevel].shooter[i].targetAngle.x, receiverComponent.transform.eulerAngles.z);
            
            App.GetLevelData()._receiverComponents.Add(receiverComponent);
            App.GetLevelData()._SenderComponents.Add(senderComponent);
            App.GetLevelData().doNextLevel = false;
        }
        
        if (!App.GetLevelData().IsTutorialOver)
        {
            for (int i = 0; i < App.GetLevelData().currentDraggableElements.Count; i++)
            {
                App.GetLevelData().currentDraggableElements[i].isDraggable = false;
            }
            App.GetLevelData()._SenderComponents[0].shooter.canShoot = false;
            
            StartCoroutine(DoTutorialPart());
        }
    }
    
    IEnumerator DoTutorialPart()
    {
        yield return new WaitForSeconds(2.5f);
        App.GetUiData().tutorialParent.Show();
        App.GetUiData().tutoriallDragHand.gameObject.Show();
        StartCoroutine(SetDefaultDraggableItems());
    }

    IEnumerator SetDefaultDraggableItems()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < App.GetLevelData().currentDraggableElements.Count; i++)
        {
            App.GetLevelData().currentDraggableElements[i].isDraggable = true;
        }
    }
    
    IEnumerator StartCounter()
    {
        StartCoroutine(BallStatusCheck());
        
        while (App.GetLevelData().hasGameStarted)
        {
            App.GetLevelData().tick++;
            App.GetLevelData().isSpawning = true;
            yield return null;
            yield return null;
            yield return null;
            App.GetLevelData().isSpawning = false;
            yield return new WaitForSeconds(1.2f);
        }
    }

    void _OnBallReceived()
    {
        int _receivedBallCount = 0;
        for (int i = 0; i < App.GetLevelData()._receiverComponents.Count; i++)
        {
            if (App.GetLevelData()._receiverComponents[i].Received())
            {
                _receivedBallCount++;
            }
        }
        App.GetLevelData()._count = _receivedBallCount;
        App.Notify(Notification.LastObjectMovement);
    }

    WaitForSeconds ws= new WaitForSeconds(1.5f);
    IEnumerator BallStatusCheck()
    {
        while (App.GetLevelData().hasGameStarted)
        {
            yield return ws;
            _OnBallReceived();
        }
    }
}
