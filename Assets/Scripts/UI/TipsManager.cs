using System;
using TMPro;
using UnityEngine;

public class TipsManager : MonoBehaviour
{
    public static Action<string> displayTipEvent;
    public static Action disabeTipEvent;

    [SerializeField] private TMP_Text messageText;

    private Animator anim;

    private int activeTips;

    private void Start () {
        anim = GetComponent<Animator>();
    }

    private void OnEnable () {
        displayTipEvent += displayTip;
        disabeTipEvent += disableTip;
    }

    private void OnDisable () {
        displayTipEvent -= displayTip;
        disabeTipEvent -= disableTip;
    }

    private void displayTip (string message) {
        messageText.text = message;
        anim.SetInteger("state", ++activeTips);
    }

    private void disableTip () {
        anim.SetInteger("state", --activeTips);
    }
    /*public static Action<string> displayTipEvent;//делегат. отвечает за появление подсказки и принимает в качестве аргумента строчка, которая будет передаваться в подсказку
    public static Action disableTipEvent;//делегат. отвечает за исчезновение подсказки

    [SerializeField] private TMP_Text messageText;//текст, который будет в подсказке

    private Animator anim;//аниматор, который нужен, чтобы запускать анимации появления и исчезновения

    private int activeTips;//хранит в себе кол-во активных подсказок

    private void Start () {
        anim = GetComponent<Animator>();
    }

    private void OnEnable () {
        displayTipEvent += displayTip;
        disableTipEvent += disableTip;
    }

    private void OnDisable () {
        displayTipEvent -= displayTip;
        disableTipEvent -= disableTip;
    }

    private void displayTip (string message) 
    { 
        messageText.text = message;//меняет текст сообщения
        anim.SetInteger("state", ++activeTips);//увеличиваем параметр state у анимации на 1.
    }

    private void disableTip () 
    {
        anim.SetInteger("state", --activeTips);
    }*/
}
