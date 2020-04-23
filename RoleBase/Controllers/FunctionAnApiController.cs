﻿using LoginServerBO.Service;
using LoginVO.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RoleBase.Controllers
{
    public class FunctionAnApiController : ApiController
    {
        FunctionService functionService = new FunctionService();
        RoleService roleService = new RoleService();

        /// <summary>
        /// 取得Function資料
        /// </summary>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public IEnumerable<FunctionVO> FunctionAddEditDelete()
        {
            var functionData = functionService.GetFunctionData();
            return functionData;
        }

        /// <summary>
        /// 新增Function
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public FunctionVO AddFunction(FunctionVO functionVO)
        {
            if (!ModelState.IsValid)
                functionVO.Message = "請填寫必填欄位";
            else
            {
                var result = functionService.AddFunction(functionVO);

                if (!string.IsNullOrEmpty(result))
                    functionVO.Message = result;
            }
            return functionVO;
        }

        /// <summary>
        /// 刪除Function
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public string DeleteFunction(FunctionVO functionVO)
        {
            var result = functionService.DeleteFunction(functionVO.FunctionID.ToString());

            return result;
        }


        /// <summary>
        /// 編輯Function
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public string EditFunction(FunctionVO functionVO)
        {
            var result = functionService.EditFunction(functionVO);

            return result;
        }

        /// <summary>
        /// 取得角色資料
        /// </summary>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public IEnumerable<RoleVO> RoleFunctionEdit()
        {
            // Thread.Sleep(1000);
            var roleData = roleService.GetRoleData();
            return roleData;
        }

        /// <summary>
        /// 透過角色ID取得勾選的功能資料
        /// 編輯角色與功能的關係
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public IEnumerable<FunctionCheckVO> GetFunctionByRole(RoleVO roleVO)
        {
            var functionCheckData = functionService.GetFunctionCheckByRole(roleVO.RoleID.ToString());
            return functionCheckData;
        }

        /// <summary>
        /// 儲存RoleFunction設定的變更
        /// </summary>
        /// <param name="functionCheckVO"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public string SaveRoleFunctionSetting(IEnumerable<FunctionCheckVO> functionCheckVO)
        {
            string result = string.Empty;
            //處理有關選時的行為
            result = functionService.SaveRoleFunctionSetting(functionCheckVO);
            return result;
        }

        /// <summary>
        /// 儲存RoleFunction設定的變更，未選取的時候
        /// </summary>
        /// <param name="roleVO"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [HttpPost]
        public string NoSelectSaveRoleFunctionSetting(RoleVO roleVO)
        {
            string result = string.Empty;

            //處理清空所有check時的行為
            result = functionService.ClearRoleFunctionByRoleID(roleVO.RoleID.ToString());

            return result;
        }
    }
}