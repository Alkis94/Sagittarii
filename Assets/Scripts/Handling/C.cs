using UnityEngine;
using System.Collections;

public static class C
{
    //----------------------
    //PLAYER          
    //----------------------

    public const int PLAYER_MAXIMUM_HEALTH = 100;     //Decrease to nerf 100
    public const int PLAYER_SPEED = 10;               //Decrease to nerf 10
    public const int PLAYER_BOUNDARY = 40;            // 40

    //----------------------
    //ENEMY        
    //----------------------
    public const int ENEMY_BOUNDARY = 40; // 40


    //----------------------
    //Arrow         
    //----------------------

    public const int ARROW_PROJECTILE_SPEED = 2200;    //Decrease to nerf 2200

    //----------------------
    //Bat        
    //----------------------

    public const int BAT_HEALTH = 10;                //Decrease to nerf 10
    public const int BAT_SPEED = 3;                  //Decrease to nerf 3
    public const int BAT_PROJECTILE_SPEED = 600;     //Decrease to nerf 600
    public const float BAT_ATTACK_FREQUENCY = 5;       //Increase to nerf 5
    public const int BAT_SPAWN_FREQUENCY = 12;        //Increase to nerf 12

    //----------------------
    //Crow        
    //----------------------

    public const int CROW_HEALTH = 10;                //Decrease to nerf 10
    public const int CROW_SPEED = 5;                  //Decrease to nerf 5
    public const int CROW_PROJECTILE_SPEED = 300;     //Decrease to nerf 300
    public const float CROW_ATTACK_FREQUENCY = 3;       //Increase to nerf 3
    public const int CROW_SPAWN_FREQUENCY = 13;        //Increase to nerf 13

    //----------------------
    //Imp      
    //----------------------

    public const int IMP_HEALTH = 30;                //Decrease to nerf 20
    public const int IMP_SPEED = 4;                  //Decrease to nerf 4
    public const int IMP_PROJECTILE_SPEED = 700;     //Decrease to nerf 700
    public const float IMP_ATTACK_FREQUENCY = 5;       //Increase to nerf 5
    public const int IMP_SPAWN_FREQUENCY = 18;        //Increase to nerf  18

    //----------------------
    //Medusa     
    //----------------------

    public const int MEDUSA_HEALTH = 150;                //Decrease to nerf 150
    public const int MEDUSA_ANGRY_HEALTH = 100;           //Decrease to nerf 100
    public const int MEDUSA_VERY_ANGRY_HEALTH = 50;       //Decrease to nerf 50
    public const int MEDUSA_SPEED = 4;                  //Decrease to nerf 4
    public const int MEDUSA_PROJECTILE_SPEED = 600;     //Decrease to nerf 600
    public const float MEDUSA_ATTACK_FREQUENCY = 2;       //Increase to nerf 2
    public const int MEDUSA_SPAWN_FREQUENCY = 180;        //Increase to nerf 120


    //----------------------
    //Wolf      
    //----------------------

    public const int WOLF_HEALTH = 20;                //Decrease to nerf 20
    public const int WOLF_SPEED = 7;                  //Decrease to nerf 7
    public const int WOLF_PROJECTILE_SPEED = 1500;     //Decrease to nerf 1200
    public const float WOLF_ATTACK_FREQUENCY = 2;       //Increase to nerf 2
    public const int WOLF_SPAWN_FREQUENCY = 15;        //Increase to nerf  15

    //----------------------
    //Pickups    
    //----------------------

    public const int PICKUP_DESPAWN_DELAY = 20; //20
    public const float HEALTH_PICKUP_DROP_RATE = 0.05f; // 5%
    public const float BAT_WINGS_DROP_RATE = 0.025f; //  2.5%
    public const float WOLF_PAW_DROP_RATE = 0.01f; // 1%
    public const float DEAD_BIRD_DROP_RATE = 0.01f; // 1%
    public const float IMP_FLAME_DROP_RATE = 0.02f; // 2%

    //----------------------
    //SpawnManager
    //----------------------

    public const int DIFFICULTY_INCREASE_DELAY = 120; // 120
}
