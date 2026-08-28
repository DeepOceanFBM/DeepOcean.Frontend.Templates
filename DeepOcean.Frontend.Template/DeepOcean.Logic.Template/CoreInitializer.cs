
using CAT.Core;
using CAT.Core.Extension;
using CAT.Core.Services;
using CAT.Model;
using CAT.Plugin.Model;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/*
    Welcome to the CoreInitializer class! This class serves as the entry point for initializing your application.
    It provides essential methods for setting up your application's core functionalities, including database connections, data synchronization, and event handling.


    this Namespace 
    
    using CAT.Core;
    using CAT.Core.Extension;
    using CAT.Core.Services;
    using CAT.Model;
    using CAT.Model.Model.Basic;
    using CAT.Plugin.Model;
    using CAT.Plugin.Model.BasicDataStructures;

    Core Initializer class is designed to be flexible and extensible, allowing you to customize it according to your application's specific needs. You can add your own initialization logic, database connections, and event subscriptions as required.
 
 */

namespace CAT.LogicModule.xxx // Replace 'xxx' with your module name, e.g., InventorySystem, SalesModule, etc.
{
    // SEE docs for more information on how to use this class to initialize your application.
    // Url : https://documents.deepoceanfbm.com/#/docs/04_Module_Development_Guide
    public class CoreInitializer
    {

        //this is Exmple for Sqlite Main DB System, you can remove this if you don't need it

        private static SQLiteAsyncConnection? _dataBaseSqlite;
        internal static SQLiteAsyncConnection DataBaseSqlite
        {
            get
            {
                if (_dataBaseSqlite != null) return _dataBaseSqlite;
                var DBManager = RuntimeServiceHelper.GetService<DBConnectingManager>();
                _dataBaseSqlite = DBManager!.GetConnection(StaticVal.DB_Master,
                    // Register your model classes here for database schema creation and management
                    // is Required for proper functionality in the DeepOcean framework
                    [typeof(LogicProject.Model.Clients)]); 
                return _dataBaseSqlite;
            }
        }

        public static async Task<ServiceResponseModel<object>> PushData()
        {
            try
            {
                SQLiteAsyncConnection db = DataBaseSqlite;

                if (db.StoreDateTimeAsTicks)
                {
                    Debug.WriteLine("StoreDateTimeAsTicks is true, ensure that your DateTime properties are correctly handled.");
                }

                await db.PushDataToServer<LogicProject.Model.Clients>(ApiRoutes.SyncBasicDataStructures.Category.SetDataList);
               
                return new ServiceResponseModel<object> { Success = true, Message = "Data synchronization completed.", Data = null, CodeStatus = 200 };
            }
            catch (Exception ex)
            {
                ex.SaveError("CAT.LogicModule.InventorySystem");
                return new ServiceResponseModel<object>()
                {
                    CodeStatus = 500,
                    Data = null,
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public static async Task<ServiceResponseModel<object>> PullData(string _ScanfromStart)
        {
            try
            {
                SQLiteAsyncConnection db = DataBaseSqlite;

                var ScanfromStart = _ScanfromStart.ToLower() == "true";

                 await db!.PollDataFromServer<Plugin.Model.BasicDataStructures.Category>(ApiRoutes.SyncBasicDataStructures.Category.GetData, ScanfromStart);
                
                return new ServiceResponseModel<object> { Success = true, Message = "Data synchronization completed.", Data = null, CodeStatus = 200 };
            }
            catch (Exception ex)
            {
                ex.SaveError("CAT.LogicModule.InventorySystem");
                return new ServiceResponseModel<object>()
                {
                    CodeStatus = 500,
                    Data = null,
                    Message = ex.Message,
                    Success = false
                };
            }
        }


        
        /*
           this method is called after the application has been initialized and is ready to perform additional setup tasks,
           such as data synchronization or other post-initialization logic.
         */
        public static async Task InitAfter()
        {
            var User = Core.Extension.PreferencesStoreClone.Get<Model.Model.Basic.User>(ReferenceManager.UserObject);
            if (User != null)
            {
                await Task.Run(async () =>
                {
                    SQLiteAsyncConnection? db = DataBaseSqlite;

                    if (db == null) { Debug.WriteLine(false, "Database connection is null."); return; }

                    //----------------------------------------------------------------------------------------------------------------//
                    await PushData();
                    //----------------------------------------------------------------------------------------------------------------//
                    //Upload Data                                                                                                     //
                    await PullData("false"); 
                });
            }
        }
     

    }
}
