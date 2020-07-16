using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleBase.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RoleBase.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        #region 屬性

        HomeController _target;

        #endregion

        #region 建構子

        public HomeControllerTests()
        {
            _target = new HomeController();
        }

        #endregion

        #region 測試方法

        /// <summary>
        /// 首頁畫面
        /// </summary>
        [TestMethod()]
        public void IndexTest()
        {
            // act
            var result = _target.Index() as ViewResult;

            // assert
            // 驗證 Action
            Assert.IsTrue(!string.IsNullOrEmpty(result.ViewName) && result.ViewName == "Index");
        }

        /// <summary>
        /// 無權限畫面
        /// </summary>
        [TestMethod()]
        public void NoCompetenceTest()
        {
            // act
            var result = _target.NoCompetence() as ViewResult;

            // assert
            // 驗證 Action
            Assert.IsTrue(!string.IsNullOrEmpty(result.ViewName) && result.ViewName == "NoCompetence");
        }

        #endregion
    }
}