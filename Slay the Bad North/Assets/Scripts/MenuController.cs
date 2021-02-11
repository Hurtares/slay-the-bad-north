using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Race
{
    ELF,
    ORC,
    HUMAN
}
public enum Menu
{
    MAIN,
    OPTIONS,
    RACE, DECK
};

public class MenuController : MonoBehaviour
{
    Race selectedRace;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject configMenu;
    [SerializeField] GameObject raceMenu;
    [SerializeField] GameObject deckMenu;

    public void startMenu(int iMenu)
    {
        Menu menu = (Menu)iMenu;
        desactiveAll();
        switch (menu)
        {
            case Menu.MAIN:
                mainMenu.SetActive(true);
                break;
            case Menu.OPTIONS:
                configMenu.SetActive(true);
                break;
            case Menu.RACE:
                raceMenu.SetActive(true);
                break;
            case Menu.DECK:
                deckMenu.SetActive(true);
                break;
            default:
                mainMenu.SetActive(true);
                break;
        }
    }

    public void startGame() {

    }

    public void selectRace(int race)
    {
        selectedRace = (Race)race;
        startMenu(3);
    }

    private void desactiveAll()
    {
        mainMenu.SetActive(false);
        configMenu.SetActive(false);
        raceMenu.SetActive(false);
        deckMenu.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
