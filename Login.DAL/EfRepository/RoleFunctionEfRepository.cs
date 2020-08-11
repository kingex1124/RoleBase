using Login.DTO;
using Login.DTO.EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.DAL
{
    public class RoleFunctionEfRepository : RoleBaseEfContext<RoleFunction>, IRoleFunctionEfRepository
    {
        #region 屬性

        private readonly RoleBaseEntities _db;

        #endregion

        #region 建構子

        public RoleFunctionEfRepository(RoleBaseEntities db) : base(db)
        {
            _db = db;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取得該角色ID所具備的權限功能
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IEnumerable<SecurityRoleFunctionDTO> GetSecurityRoleFunction(string roleId)
        {
            int idData = Convert.ToInt32(roleId);

            var result = (from role in _db.Role
                          join roleFunction in _db.RoleFunction
                          on role.RoleID equals roleFunction.RoleID
                          join function in _db.Function
                          on roleFunction.FunctionID equals function.FunctionID
                          where role.RoleID == idData
                          select new SecurityRoleFunctionDTO()
                          {
                              RoleName = role.RoleName,
                              Url = function.Url,
                              Description = function.Description
                          });

            return result;
        }

        /// <summary>
        /// 透過FunctionID刪除RoleFunction資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteRoleFunctionByFunctionID(string functionID)
        {
            int functionIDData = Convert.ToInt32(functionID);
            var deleteData = _db.RoleFunction.Where(o => o.FunctionID == functionIDData);

            foreach (var item in deleteData)
                Delete(item);

            return SaveChanges();
        }

        /// <summary>
        /// 取得角色設定功能的資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<FunctionCheckDTO> GetFunctionCheckByRole(string id)
        {
            int idData = Convert.ToInt32(id);
            var result = (from function in _db.Function
                          join selectRoleFunction in (from roleFunction in _db.RoleFunction where roleFunction.RoleID == idData select roleFunction)
                          on function.FunctionID equals selectRoleFunction.FunctionID
                          into joined
                          from j in joined.DefaultIfEmpty()
                          orderby function.FunctionID
                          select new FunctionCheckDTO()
                          {
                              FunctionID = function.FunctionID,
                              Url = function.Url,
                              Description = function.Description,
                              Check = j.RoleID == null ? false : true
                          });

            return result;
        }

        /// <summary>
        /// 透過角色ID清空RoleFunciton的資料
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteRoleFunctionByRoleID(string roleID)
        {
            int roleIDData = Convert.ToInt32(roleID);
            var deleteData = _db.RoleFunction.Where(o => o.RoleID == roleIDData);

            foreach (var item in deleteData)
                Delete(item);

            return SaveChanges();
        }

        /// <summary>
        /// 透過角色ID新增RoleFunction資料
        /// </summary>
        /// <param name="roleFunctionDTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int InsertRoleFunction(RoleFunctionDTO roleFunctionDTO)
        {
            Insert(new RoleFunction()
            {
                RoleID = roleFunctionDTO.RoleID,
                FunctionID = roleFunctionDTO.FunctionID
            });

            return SaveChanges();
        }

        #endregion
    }
}
