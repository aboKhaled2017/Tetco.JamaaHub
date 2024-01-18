using Domain.Enums;

namespace Domain.Entities.Hub.UniversityAgent.ValueObjects
{
    /// <summary>
    /// express the status of transfering information of the batch
    /// </summary>
    public sealed record BatchTransferStatus : SmartEnum<BatchTransferStatus>
    {
        public BatchTransferStatus(string key, string value) : base(key, value)
        {
        }

        public static BatchTransferStatus New = new("new","the batch is new");
        public static BatchTransferStatus InProgress = new("in-progress","the batch started transfering packages");
        public static BatchTransferStatus Completed = new("completed","the batch received all packaged");
    }
}
