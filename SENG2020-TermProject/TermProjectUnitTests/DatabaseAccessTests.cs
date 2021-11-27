using NUnit.Framework;
using SENG2020_TermProject.DatabaseManagement;

namespace TermProjectUnitTests
{
    class DatabaseAccessTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetAllMarketplaceRequestsHappyPath()
        {
            if(new ContractMarketAccess().GetAllMarketplaceRequests() != null)
                Assert.Pass();
            else Assert.Fail();
        }
    }
}
