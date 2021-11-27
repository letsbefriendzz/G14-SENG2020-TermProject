using NUnit.Framework;
using SENG2020_TermProject.Data_Logic;
using SENG2020_TermProject.DatabaseManagement;

namespace TermProjectUnitTests
{
    public class UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        #region CityList Unit Tests

        [Test]
        public void CityAtHappyPath()
        {
            if (CityList.CityAt(0) != null)
                Assert.Pass();
            else Assert.Fail();
        }

        [Test]
        public void CityAtExceptionPath()
        {
            if (CityList.CityAt(8) == null)
                Assert.Pass();
            else Assert.Fail();
        }

        [Test]
        public void ContainsCityHappyPath()
        {
            if (CityList.ContainsCity("Kingston"))
                Assert.Pass();
            else Assert.Fail();
        }

        [Test]
        public void ContainsCityExceptionCity()
        {
            if (CityList.ContainsCity("Boston") == false)
                Assert.Pass();
            else Assert.Fail();
        }

        [Test]
        public void ContainsCityExceptionPath()
        {
            if (CityList.ContainsCity(null) == false)
                Assert.Pass();
            else Assert.Fail();
        }

        [Test]
        public void LTLStopsHappyPath()
        {
            if (CityList.LTLStops("Hamilton", "Oshawa") == 1)
                Assert.Pass();
            else Assert.Fail();
        }

        #endregion

        #region Database Tests

        //why is this failing????
        [Test]
        public void GetAllMarketplaceRequestsHappyPath()
        {
            ContractMarketAccess cma = new ContractMarketAccess();
            MarketplaceRequest[] mr = null;
            mr = cma.GetAllMarketplaceRequests();
            if (mr == null)
                Assert.Fail();
            else Assert.Pass();
        }

        #endregion
    }
}