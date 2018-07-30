using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel;

namespace TShirtEmpAdmin.Models
{
    [Table("UserInfo_Employee")]
    public class LogUserInfo
    {
        public double EmpID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PrefName { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string CostCenter { get; set; }
        public string DeptName { get; set; }
        public string Email { get; set; }
        public string Manager { get; set; }
        public string VP { get; set; }
        [Key]
        public int ID { get; set; }


    }
}