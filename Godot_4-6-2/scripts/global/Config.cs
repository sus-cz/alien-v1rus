public static class Config{
	//SCREEN
	public static bool fullscreen=false;
	
	// GAME
	public const int TILE_SIZE = 100;
	public static float camera_speed = 3f;
	public static float camera_max_zoom_in = 0.5f;
	public static float camera_max_zoom_out = 2.2f;
	public static int maptiles_amount_x = 10;
	public static int maptiles_amount_y = 10;
	public static int home_position_x = 0;
	public static int home_position_y = 0;
	
	// ENEMY
	public static bool are_carriers_comming_from_other_nests = false;
	public static int chance_carrierspawn = 5;
	
	// BUILDINGS
	public const int DEF_HP_TOWER = 1;
	public const int DEF_HP_ACCU = 1;
	public const int DEF_HP_HOME = 1;
	public const int DEF_HP_NEST = 1;
	
	// MONEY
	public const int MONEY_BEGIN = 20000;//40;
	public const float PRICE_FACTOR_REPAIR = 2.0f;
	public const int PRICE_ACCU = 50;
	public const int PRICE_DESTROY = 10;
	public const int PRICE_PATH = 10;
	public const int PRICE_TOWER = 20;
}
