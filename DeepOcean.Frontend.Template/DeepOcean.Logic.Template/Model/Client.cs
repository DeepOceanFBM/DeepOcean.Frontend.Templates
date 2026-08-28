using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace LogicProject.Model
{
    // this Class Required to inherit from CAT.Model.SheardData for proper functionality in the DeepOcean framework
    public class Clients : CAT.Model.SheardData 
    {
        //---------------------------------------------
        // Sqlite ID and Server Data
        //---------------------------------------------
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int? SQLite_ID { get; set; } // this in Client
        public int Id { get; set; } // this in Server

        //---------------------------------------------
        // Your Data
        //---------------------------------------------
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        //additional properties you want to store in the database for the Clients table

        //---------------------------------------------
        // Additional Data
        //---------------------------------------------
        public int? CompnayID { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsUpdateToServer { get; set; }
    }
}
