using UnityEngine;
using System;

public class Controls {

    public static int STRAFE_UP     = 0;
    public static int STRAFE_DOWN   = 1;
    public static int STRAFE_LEFT   = 2;
    public static int STRAFE_RIGHT  = 3;
    public static int MOVE_FORWARD  = 4;
    public static int MOVE_BACKWARD = 5;
    public static int BLINDED_ACTION = 6;
    public static int BIRD_ACTION = 7;
    public static int KEY_MAX       = 8;

    private static Key[] KEYS = new Key[KEY_MAX];

    public static int ROT_SPEED     = 0;
    public static int ZOOM_SPEED    = 1;
    public static int VALUES_MAX    = 2;
    private static Value[] VALUES = new Value[VALUES_MAX];

    public static Key getKey(int keyID) {
        if (keyID < 0 || keyID >= KEYS.Length) {
            return (null);
        }
        if (KEYS[keyID] == null) {
            Setup();
        }
        return (KEYS[keyID]);
    }

    public static Value getValue(int valueID) {
        if (VALUES[valueID] == null) {
            Setup();
        }
        return (VALUES[valueID]);
    }

    /** setup default key bindings */
	public static void Setup () {
        if (KEYS[STRAFE_UP] != null) {
            return;
        }
        KEYS[STRAFE_UP]     = new Key("STRAFE_UP", "Space");
        KEYS[STRAFE_DOWN]   = new Key("STRAFE_DOWN", "E");
        KEYS[STRAFE_LEFT]   = new Key("STRAFE_LEFT", "Q");
        KEYS[STRAFE_RIGHT]  = new Key("STRAFE_RIGHT", "D");
        KEYS[MOVE_FORWARD]  = new Key("MOVE_FORWARD", "Z");
        KEYS[MOVE_BACKWARD] = new Key("MOVE_BACKWARD", "S");
        KEYS[BLINDED_ACTION] = new Key("BLINDED_ACTION", "Mouse0");
        KEYS[BIRD_ACTION]       = new Key("BIRD_ACTION", "Mouse1");

        VALUES[ROT_SPEED] = new Value("ROT_SPEED", 5);
        VALUES[ZOOM_SPEED] = new Value("ZOOM_SPEED", 0.1f);
    }
}
