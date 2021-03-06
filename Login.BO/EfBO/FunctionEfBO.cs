﻿using Login.DAL;
using Login.DTO;
using Login.DTO.EFModel;
using Login.VO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Login.BO
{
    public class FunctionEfBO : IFunctionBO
    {
        #region 屬性

        IFunctionEfRepository _functionEfRepo;
        IRoleFunctionEfRepository _roleFunctionEfRepo;

        #endregion

        #region 建構子

        public FunctionEfBO()
        {
            _functionEfRepo = new FunctionEfRepository(new RoleBaseEntities());
            _roleFunctionEfRepo = new RoleFunctionEfRepository(new RoleBaseEntities());
        }

        public FunctionEfBO(IFunctionEfRepository functionEfRepo, IRoleFunctionEfRepository roleFunctionEfRepo)
        {
            _functionEfRepo = functionEfRepo;
            _roleFunctionEfRepo = roleFunctionEfRepo;
        }

        #endregion

        #region 方法  

        /// <summary>
        /// 取得Function資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FunctionVO> GetFunctionData(PageDataVO pageDataVO)
        {
            pageDataVO.PageSize = pageDataVO.PageSize ?? Convert.ToInt32(ConfigurationManager.AppSettings["TablePageCount"]);

            pageDataVO.DataCount = _functionEfRepo.GetFunctionCount(pageDataVO);

            if (pageDataVO.DataCount % pageDataVO.PageSize.Value == 0)
                pageDataVO.AllPageNumber = pageDataVO.DataCount / pageDataVO.PageSize.Value;
            else
                pageDataVO.AllPageNumber = pageDataVO.DataCount / pageDataVO.PageSize.Value + 1;

            pageDataVO.LowerBound = (pageDataVO.PageNumber - 1) * pageDataVO.PageSize.Value;
            pageDataVO.UpperBound = pageDataVO.LowerBound + pageDataVO.PageSize.Value + 1;
            if (pageDataVO.LowerBound >= pageDataVO.DataCount)
            {
                pageDataVO.UpperBound = pageDataVO.DataCount + 1;
                pageDataVO.LowerBound = pageDataVO.UpperBound - (pageDataVO.PageSize.Value + 1);
            }

            return Utility.MigrationIEnumerable<FunctionDTO, FunctionVO>(_functionEfRepo.GetFunctionData(pageDataVO));
        }

        /// <summary>
        /// 取得作為上層的keyValue資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePairVO> GetParentKeyValue()
        {
            return Utility.MigrationIEnumerable<KeyValuePairDTO, KeyValuePairVO>(_functionEfRepo.GetParentKeyValue());
        }

        /// <summary>
        /// 新增功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public string AddFunction(FunctionVO functionVO)
        {
            int result = _functionEfRepo.AddFunction(functionVO);

            if (result > 0)
                return "";
            else
                return "新增失敗。";
        }

        /// <summary>
        /// 刪除功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteFunction(string id)
        {
            string result = string.Empty;

            using (TransactionScope ts = new TransactionScope())
            {
                int deleteRoleFunctionResult = _roleFunctionEfRepo.DeleteRoleFunctionByFunctionID(id);

                int deleteFunctionResult = _functionEfRepo.DeleteFunction(id);

                if (deleteRoleFunctionResult >= 0 && deleteFunctionResult > 0)
                    result = "";
                else
                    result = "刪除失敗。";

                ts.Complete();
            }

            return result;
        }

        /// <summary>
        /// 編輯功能
        /// </summary>
        /// <param name="functionVO"></param>
        /// <returns></returns>
        public string EditFunction(FunctionVO functionVO)
        {
            int result = _functionEfRepo.EditFunction(functionVO);
            if (result > 0)
                return "";
            else
                return "編輯失敗";
        }

        /// <summary>
        /// 取得角色設定功能的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IEnumerable<FunctionCheckVO> GetFunctionCheckByRole(string roleID, PageDataVO pageDataVO)
        {
            return Utility.MigrationIEnumerable<FunctionCheckDTO, FunctionCheckVO>(_roleFunctionEfRepo.GetFunctionCheckByRole(roleID));
        }

        /// <summary>
        /// 角色編輯功能
        /// 儲存勾選功能時的變更
        /// </summary>
        /// <param name="functionCheckVO"></param>
        /// <returns></returns>
        public string SaveRoleFunctionSetting(IEnumerable<FunctionCheckVO> functionCheckVO)
        {
            string result = string.Empty;
            string roleID;

            if (functionCheckVO != null && functionCheckVO.Any())
            {
                roleID = functionCheckVO.First().RoleID.ToString();
                List<RoleFunctionDTO> roleFunctionDTOs = new List<RoleFunctionDTO>();
                foreach (var item in functionCheckVO)
                {
                    RoleFunctionDTO roleFunctionDTO = new RoleFunctionDTO();
                    roleFunctionDTO.RoleID = item.RoleID;
                    roleFunctionDTO.FunctionID = item.FunctionID;
                    roleFunctionDTOs.Add(roleFunctionDTO);
                }

                using (TransactionScope ts = new TransactionScope())
                {
                    int deleteResult = _roleFunctionEfRepo.DeleteRoleFunctionByRoleID(roleID);

                    if (deleteResult < 0)
                    {
                        result = "刪除失敗。";
                        return result;
                    }

                    int insertResult = 0;

                    foreach (var item in roleFunctionDTOs)
                        insertResult += _roleFunctionEfRepo.InsertRoleFunction(item);

                    ts.Complete();

                    if (insertResult < 0)
                        result = "設定失敗。";
                }
            }
            return result;
        }

        /// <summary>
        /// 角色編輯功能
        /// 儲存清空功能的變更
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public string ClearRoleFunctionByRoleID(string roleID)
        {
            string result = string.Empty;

            using (TransactionScope ts = new TransactionScope())
            {
                int deleteResult = _roleFunctionEfRepo.DeleteRoleFunctionByRoleID(roleID);

                if (deleteResult < 0)
                    result = "刪除失敗。";

                ts.Complete();
            }

            return result;
        }

        /// <summary>
        /// 取得MenuNode
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<FunctionMenuNode> GetFunctionToNode(string userID)
        {
            var menuData = Utility.MigrationIEnumerable<FunctionMenuDTO, FunctionMenuVO>(_functionEfRepo.GetMenuData(userID));

            var topData = menuData.Where(o => o.Parent == 0).ToList();

            var NotTopData = menuData.Where(o => o.Parent != 0).ToList();

            var result = new List<FunctionMenuNode>() { };

            foreach (var item in topData)
            {
                var node = new FunctionMenuNode(item) { };
                result.Add(node);
                SetNode(NotTopData, node);
            }

            return result;
        }

        /// <summary>
        /// 遞迴產生Node
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public void SetNode(List<FunctionMenuVO> vo,  FunctionMenuNode node)
        {
            var nodeData = node;
            var nextData = vo.Where(o => o.Parent == nodeData.Val.FunctionID).ToList();

            if (nextData.Any())
                node.Next = new List<FunctionMenuNode>();
            else
                return;

            for (int i = 0; i < nextData.Count(); i++)
            {
                node.Next.Add(new FunctionMenuNode(nextData[i]));
                vo.Remove(nextData[i]);
                SetNode(vo, node.Next[i]);
            }
        }

        #endregion
    }
}
