﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoleBase.Controllers
{
    public class HomeController : Controller
    {
        #region Action

        /// <summary>
        /// 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// 無權限頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult NoCompetence()
        {
            return View("NoCompetence");
        }

        /// <summary>
        /// 錯誤頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult ErrorPage()
        {
            return View("ErrorPage");
        }

        #endregion
    }
}
