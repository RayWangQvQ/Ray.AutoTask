namespace Ray.Serilog.Sinks.CoolPushBatched
{
    public enum CoolPushBatchedStateType
    {
        Start = 1,
        End = 2,
    }

    public class CoolPushBatchedConstants
    {
        public const string CoolPushBatchedStatePropertyKey = "BatchedState";
    }
}
