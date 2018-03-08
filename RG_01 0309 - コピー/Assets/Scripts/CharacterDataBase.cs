using System.Collections.Generic;
using CharacterID;

public class CharacterDataBase
{

    private List<List<CharacterStates>> _masterdata = new List<List<CharacterStates>>();
    public CharacterStates GetPlayerStates(int _lv)
    {
        return new CharacterStates(_masterdata[(int)Character.ID.PLAYER][_lv - 1]);
    }
    public CharacterStates GetSkeletonStates(int _lv)
    {
        return new CharacterStates(_masterdata[(int)Character.ID.SKELETON][_lv - 1]);
    }

    public void LoadCharacterDataBase()
    {
        CSVManager csvManager = new CSVManager();
        _masterdata = csvManager.LoadMasterDataCSV();
    }
}
