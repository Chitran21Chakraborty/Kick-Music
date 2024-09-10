using System;
using System.Collections;
using System.Collections.Generic;
using Sourav.Engine.Core.GameElementRelated;
using Sourav.Engine.Editable.NotificationRelated;
using Sourav.Utilities.Extensions;
using UnityEngine;

public class DragMusicalElement : GameElement
{
    private Vector3 mOffset;

    private float mZCoordinate;
    
    public int status;
    public Grid currentLastGrid;
    public BoxCollider[] touchableColliders;
    public ParticleSystem dustParicle;
    public AudioSource musicElement;
    public string statename;
    public Animator musicAnim;
    public bool isDraggable;

    private void OnMouseDown()
    {
        if (isDraggable)
        {
            transform.position += Vector3.up * 0.01f; 
        
            mZCoordinate = App.GetLevelData().dragCamera.WorldToScreenPoint(transform.position).z;

            mOffset = transform.position - GetMouseWorldPos();
            for (int i = 0; i < touchableColliders.Length; i++)
            {
                touchableColliders[i].enabled = false;
            }

            if (App.GetUiData().tutoriallDragHand.gameObject.activeInHierarchy)
            {
                App.GetUiData().tutoriallDragHand.gameObject.Hide();
                App.GetUiData().shootTutorialText.gameObject.Show();
                App.GetUiData().shootText.Show();
                App.GetLevelData()._SenderComponents[0].shooter.canShoot = true;
            }
            App.Notify(Notification.DragSoundStart);
        }
    }
    private void OnMouseDrag()
    {
        if (isDraggable)
        {
            transform.position = GetMouseWorldPos() + mOffset;
        }
    }

    private void OnMouseUp()
    {
        if (isDraggable)
        {
            this.transform.position = currentLastGrid.transform.position - Vector3.up * 0.01f;

            for (int i = 0; i < touchableColliders.Length; i++)
            {
                touchableColliders[i].enabled = true;
            }
            currentLastGrid.dummyStair.Hide(); 
            App.Notify(Notification.DragSoundEnd);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grid"))
        {
            if (!other.gameObject.GetComponent<Grid>().isOccupied)
            {
                currentLastGrid.isOccupied = false;
                currentLastGrid.dummyStair.Hide();
                currentLastGrid = other.gameObject.GetComponent<Grid>();
                currentLastGrid.isOccupied = true;
                currentLastGrid.dummyStair.Show();
            }
        }
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoordinate;
        return App.GetLevelData().dragCamera.ScreenToWorldPoint(mousePoint);
    }
}
