namespace BattleBase.SaveService
{
    public interface ISaveable 
    {
        public void Load();

        public void Save();
    }
}