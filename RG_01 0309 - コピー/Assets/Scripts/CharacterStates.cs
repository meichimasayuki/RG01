public class CharacterStates
{
    public CharacterStates() { }
    // データベースから（ローカルから渡さないとデータベースを直でいじってしまう）
    public CharacterStates(CharacterStates states)
    {
        int lv = states.LV;
        int hp = states.HP;
        int mp = states.MP;
        int sp = states.SP;
        int atk = states.ATK;
        int def = states.DEF;
        int iint = states.INT;
        int mnd = states.MND;
        int exp = states.EXP;
        _lv = lv; _hp = hp; _mp = mp; _sp = sp; _atk = atk; _def = def; _int = iint; _mnd = mnd; _exp = exp;
    }
    ~CharacterStates() { }

    private int _lv;    // レベル
    private int _hp;    // 体力
    private int _mp;    // 魔力
    private int _sp;    // 技力
    private int _atk;   // 攻撃力
    private int _def;   // 防御力
    private int _int;   // 魔法攻撃力
    private int _mnd;   // 魔法防御力
    private int _exp;   // 経験値
    public int LV
    {
        get { return _lv; }
        set { _lv = value; }
    }
    public int HP
    {
        get { return _hp; }
        set { _hp = value; }
    }
    public int MP
    {
        get { return _mp; }
        set { _mp = value; }
    }
    public int SP
    {
        get { return _sp; }
        set { _sp = value; }
    }
    public int ATK
    {
        get { return _atk; }
        set { _atk = value; }
    }
    public int DEF
    {
        get { return _def; }
        set { _def = value; }
    }
    public int INT
    {
        get { return _int; }
        set { _int = value; }
    }
    public int MND
    {
        get { return _mnd; }
        set { _mnd = value; }
    }
    public int EXP
    {
        get { return _exp; }
        set { _exp = value; }
    }
}
namespace CharacterStatesCategory
{
    public static class States
    {
        public enum CATEGORY
        {
            LV = 0,
            HP,
            MP,
            SP,
            ATK,
            DEF,
            INT,
            MND,
            EXP,
            NONE1,
            NONE2,
            NONE3,
            NONE4,
        }
    }
}