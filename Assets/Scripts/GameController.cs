using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{ 
    FreeRoam,  //マップ移動
    Battle,
}

public class GameController : MonoBehaviour
{
    //ゲームの状態を管理
    [SerializeField] Player player;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state = GameState.FreeRoam;

    private void Start()
    {
        player.OnEncounted += StartBattle;
        battleSystem.BattleOver += EndBattle;
    }

    public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = player.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

        battleSystem.StartBattle(playerParty,wildPokemon);
    }

    public void EndBattle()
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        if(state==GameState.FreeRoam)
        {
            player.HandleUpdate();
        }
        else if(state==GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }
}
