using System.Collections.Generic;
using System.Web.Mvc;
using TerminalArchive.Domain.DB;
using TerminalArchive.WebUI.Models;

namespace TerminalArchive.WebUI.Controllers
{
    public class NavController : Controller
    {
        [Authorize]
        public PartialViewResult Menu()
        {
            var admin = DbHelper.UserIsAdmin(User?.Identity?.Name);
            List<MenuInfo> menues = 
                new List<MenuInfo>
                {
                    new MenuInfo { Text = "Терминалы", Controller = "TerminalMonitoring", Action = "List" }
                };
            if (!admin)
            {
                menues[0].SubItems = new MenuInfo[]
                {
                    new MenuInfo {Text = "Состояние терминалов", Controller = "TerminalMonitoring", Action = "List"},
                };
            }
            else
            {
                menues[0].SubItems = new MenuInfo[]
                    {
                        new MenuInfo {Text = "Состояние терминалов", Controller = "TerminalMonitoring", Action = "List"},
                        new MenuInfo {Text = "Добавить терминал", Controller = "Terminal", Action = "Add"}
                    };
                menues.Add(new MenuInfo { Text = "Группы терминалов", Controller = "Terminal", Action = "ListGroups",
                    SubItems = new MenuInfo[] 
                    {
                        new MenuInfo {Text = "Просмотр", Controller = "Terminal", Action = "ListGroups"},
                        new MenuInfo {Text = "Добавить группу", Controller = "Terminal", Action = "AddOrEditGroup"},
                    }
                });
                menues.Add(new MenuInfo { Text = "Пользователи", Controller = "User", Action = "ListUser",
                    SubItems = new MenuInfo[]
                    {
                        new MenuInfo {Text = "Просмотр", Controller = "User", Action = "ListUser"},
                        new MenuInfo {Text = "Добавить пользователя", Controller = "User", Action = "AddOrEdit"},
                        new MenuInfo {Text = "Настройка ролей пользователей", Controller = "User", Action = "UserRoles"}
                    }
                });

                //menues.Add(new MenuInfo {Text = "Новый терминал", Controller = "Terminal", Action = "Add"});
                //menues.Add(new MenuInfo {Text = "Группы", Controller = "Terminal", Action = "ListGroups"});
                //menues.Add(new MenuInfo {Text = "Новая группа", Controller = "Terminal", Action = "AddOrEditGroup"});
                //menues.Add(new MenuInfo {Text = "Пользователи", Controller = "User", Action = "ListUser"});
                //menues.Add(new MenuInfo {Text = "Новый пользователь", Controller = "User", Action = "AddOrEdit"});
                //menues.Add(new MenuInfo {Text = "Роли пользователей", Controller = "User", Action = "UserRoles"});
                //menues.Add(new MenuInfo { Text = "Роли", Controller = "User", Action = "ListRoles" });
                //menues.Add(new MenuInfo { Text = "Новая роль пользователей", Controller = "User", Action = "AddOrEditRole" });
                //menues.Add(new MenuInfo { Text = "Права ролей", Controller = "User", Action = "RoleRights" });
            }
            return PartialView(menues);
        }
    }
}