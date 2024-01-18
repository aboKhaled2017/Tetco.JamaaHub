using Abd.CleanArchitecture.Kernel.Domain;

namespace Domain.Entities.Hub.Log;

public sealed class HubLog : BaseEntity<long>
{
    public HubLog()
    {
        CreateionDate = DateTime.Now;
    }
    public int SysCode { get; set; }
    public string LogLevel { get; set; }
    public string LogUser { get; set; }
    public string IP { get; set; }
    public string MachineName { get; set; }
    public string LogPageUrl { get; set; }
    public string LogMessage { get; set; }
    public string LogStack { get; set; }
    public string InstituteCode { get; set; }
    public DateTime CreateionDate { get; set; }
}
