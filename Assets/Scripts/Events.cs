using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events
{
    public const string LEVEL_START = "LevelStart";
    public const string LEVEL_FINISHED = "LevelFinished";
    public const string LEVEL_FAILED = "LevelFailed";

    public const string PLAYER_JUMP = "PlayerJump";
    public const string PLAYER_LANDED = "PlayerLANDED";
    public const string PLAYER_DIED = "PlayerDied";
    public const string PLAYER_SHOOT = "PlayerShoot";
    public const string ATTACK_PLAYER = "AttackPlayer";
    public const string PLAYER_POSITION_CHANGE = "PlayerPositionChange";
    
    public const string ATTACK_ENEMY = "AttackEnemy";
    public const string ENEMY_DIED = "EnemyDied";

    public const string INCREASE_SCORE = "IncreaseScore";
}
