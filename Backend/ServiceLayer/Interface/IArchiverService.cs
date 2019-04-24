using System.Collections.Generic;

namespace Gucci.ServiceLayer.Interface
{
    public interface IArchiverService
    {
        List<string> GetLogsFilename();
        bool IsLogOlderThan30Days(string filename);
        List<string> GetOldLogs();
    }
}
