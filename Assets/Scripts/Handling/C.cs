
public static class C
{
    //----------------------
    //PLAYER          
    //----------------------
    public const int PLAYER_STARTING_DAMAGE = 10;
    public const int PLAYER_JUMP_FORCE = 8;
    public const int PLAYER_MAXIMUM_HEALTH = 100;     //Decrease to nerf 
    public const int PLAYER_SPEED = 5;               //Decrease to nerf 

    //----------------------
    //ENEMY        
    //----------------------
    public const int ENEMY_BOUNDARY = 40; // 40


    //----------------------
    //Arrow         
    //----------------------

    public const int ARROW_PROJECTILE_SPEED = 800;    //Decrease to nerf 

    //----------------------
    //Bat        
    //----------------------

    public const int BAT_HEALTH = 10;                //Decrease to nerf 
    public const int BAT_SPEED = 1;                  //Decrease to nerf 
    public const int BAT_PROJECTILE_SPEED = 200;     //Decrease to nerf 
    public const float BAT_ATTACK_FREQUENCY = 5;       //Increase to nerf 
    public const int BAT_SPAWN_FREQUENCY = 8;        //Increase to nerf 

    //----------------------
    //Crow        
    //----------------------

    public const int CROW_HEALTH = 10;                //Decrease to nerf 
    public const int CROW_SPEED = 2;                  //Decrease to nerf 
    public const int CROW_PROJECTILE_SPEED = 100;     //Decrease to nerf 
    public const float CROW_ATTACK_FREQUENCY = 3;       //Increase to nerf 
    public const int CROW_SPAWN_FREQUENCY = 6;        //Increase to nerf 

    //----------------------
    //Imp      
    //----------------------

    public const int IMP_HEALTH = 30;                //Decrease to nerf 
    public const int IMP_SPEED = 2;                  //Decrease to nerf 
    public const int IMP_PROJECTILE_SPEED = 200;     //Decrease to nerf 
    public const float IMP_ATTACK_FREQUENCY = 5;       //Increase to nerf 
    public const int IMP_SPAWN_FREQUENCY = 18;        //Increase to nerf  

    //----------------------
    //Medusa     
    //----------------------

    public const int MEDUSA_HEALTH = 150;                //Decrease to nerf 
    public const int MEDUSA_ANGRY_HEALTH = 100;           //Decrease to nerf 
    public const int MEDUSA_VERY_ANGRY_HEALTH = 50;       //Decrease to nerf 
    public const int MEDUSA_SPEED = 2;                  //Decrease to nerf 
    public const int MEDUSA_PROJECTILE_SPEED = 200;     //Decrease to nerf 
    public const float MEDUSA_ATTACK_FREQUENCY = 2;       //Increase to nerf 
    public const int MEDUSA_SPAWN_FREQUENCY = 180;        //Increase to nerf 


    //----------------------
    //Wolf      
    //----------------------

    public const int WOLF_HEALTH = 10;                //Decrease to nerf 
    public const int WOLF_SPEED = 3;                  //Decrease to nerf 
    public const int WOLF_PROJECTILE_SPEED = 500;     //Decrease to nerf 
    public const float WOLF_ATTACK_FREQUENCY = 2;       //Increase to nerf 
    public const int WOLF_SPAWN_FREQUENCY = 7;        //Increase to nerf  

    //----------------------
    //Pickups    
    //----------------------

    public const int PICKUP_DESPAWN_DELAY = 20; 
    public const float HEALTH_PICKUP_DROP_RATE = 0.05f; // 5%
    public const float BAT_WINGS_DROP_RATE = 0.025f; //  2.5
    public const float WOLF_PAW_DROP_RATE = 0.01f; // 1%
    public const float DEAD_BIRD_DROP_RATE = 0.01f; // 1%
    public const float IMP_FLAME_DROP_RATE = 0.02f; // 2%

    //----------------------
    //SpawnManager
    //----------------------

    public const int DIFFICULTY_INCREASE_DELAY = 120; // 120
}
