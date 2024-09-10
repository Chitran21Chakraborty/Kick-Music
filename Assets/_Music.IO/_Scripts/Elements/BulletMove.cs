using System;
using System.Collections;
using System.Collections.Generic;
using Sourav.Engine.Core.GameElementRelated;
using Sourav.Engine.Core.NotificationRelated;
using Sourav.Engine.Editable.NotificationRelated;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletMove : GameElement
{
    private bool isMoving;
    public int xDegreeMultiplier, yDegreeMultiplier;
    public float smoothTime;
    private float angle = 45f;
    public int bulletIndex;
    float dist = 4f;
    public FootBallComponent rotatingFootball;
   
    void Start()
    {
        isMoving = true;
    }
    
    private void FixedUpdate()
    {
        if (isMoving)
        {
            float radian = angle * (Mathf.PI/180);

            transform.Translate(xDegreeMultiplier * Mathf.Cos(radian) * smoothTime * Time.deltaTime,0, yDegreeMultiplier * Mathf.Sin(radian) * smoothTime * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Horizontal"))
        {
            isMoving = false;
            int tempY = -1 * yDegreeMultiplier;
            float tempRotBallY = -1f * rotatingFootball.angle;
            rotatingFootball.angle = tempRotBallY;
            rotatingFootball.transform.rotation = Quaternion.AngleAxis(rotatingFootball.angle, Vector3.up);
            yDegreeMultiplier = tempY;
            isMoving = true;
            OnTriggerEffect(other.gameObject.transform.parent.GetComponent<DragMusicalElement>().dustParicle, other.gameObject.transform.parent.GetComponent<DragMusicalElement>().musicAnim, other.gameObject.transform.parent.GetComponent<DragMusicalElement>().statename, other.gameObject.transform.parent.GetComponent<DragMusicalElement>(), other.gameObject.transform.parent.gameObject);

        }
    
        if (other.gameObject.CompareTag("Vertical"))
        {
            isMoving = false;
            int tempX = -1 * xDegreeMultiplier;
            float tempRotBallY = -1f * rotatingFootball.angle;
            float tempRotBallX = -1f * rotatingFootball.rotationSpeed;
            rotatingFootball.angle = tempRotBallY;
            rotatingFootball.transform.rotation = Quaternion.AngleAxis(rotatingFootball.angle, Vector3.up);
            rotatingFootball.rotationSpeed = tempRotBallX;
            xDegreeMultiplier = tempX;
            isMoving = true;
            OnTriggerEffect(other.gameObject.transform.parent.GetComponent<DragMusicalElement>().dustParicle, other.gameObject.transform.parent.GetComponent<DragMusicalElement>().musicAnim, other.gameObject.transform.parent.GetComponent<DragMusicalElement>().statename, other.gameObject.transform.parent.GetComponent<DragMusicalElement>(), other.gameObject.transform.parent.gameObject);
        }

        if (other.gameObject.CompareTag("Once"))
        {
            App.Notify(Notification.PartialAchievementSoundPlay);
        }
    }

    void OnTriggerEffect(ParticleSystem particle, Animator animator, string state, DragMusicalElement dragMusicalElement, GameObject obj)
    {
        particle.Play();
        if (animator != null)
        {
            animator.Play(state);
        }

        if (dragMusicalElement.musicElement != null)
        {
            NotificationParam audioParam = new NotificationParam(Mode.gameObjectData);
            audioParam.gameObjectData.Add(obj);
            App.Notify(Notification.PlayMusicalSound, audioParam);
        }
        App.Notify(Notification.HapticLight);
    }
}