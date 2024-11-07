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
    /*public static Action<string> displayTipEvent;//�������. �������� �� ��������� ��������� � ��������� � �������� ��������� �������, ������� ����� ������������ � ���������
    public static Action disableTipEvent;//�������. �������� �� ������������ ���������

    [SerializeField] private TMP_Text messageText;//�����, ������� ����� � ���������

    private Animator anim;//��������, ������� �����, ����� ��������� �������� ��������� � ������������

    private int activeTips;//������ � ���� ���-�� �������� ���������

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
        messageText.text = message;//������ ����� ���������
        anim.SetInteger("state", ++activeTips);//����������� �������� state � �������� �� 1.
    }

    private void disableTip () 
    {
        anim.SetInteger("state", --activeTips);
    }*/
}
