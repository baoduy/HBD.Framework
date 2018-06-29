namespace HBD.Framework.Data.DataReplacements
{
    public interface IJob
    {
        bool Execute();

        void Cancel();
    }
}