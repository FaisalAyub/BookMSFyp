using System.Data.SqlClient;
using Shouldly;
using Xunit;

namespace ERP.Tests.General
{
    // ReSharper disable once InconsistentNaming
    public class ConnectionString_Tests
    {
        [Fact]
        public void SqlConnectionStringBuilder_Test()
        {
            var csb = new SqlConnectionStringBuilder("Server=192.168.50.68; Database=ERPDb; user id = dev ; password = dev123;");
            csb["Database"].ShouldBe("CNSERP");
        }
    }
}
