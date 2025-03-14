//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LopushokApp.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.ShiftSchedule = new HashSet<ShiftSchedule>();
        }
    
        public int ID { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public Nullable<System.DateTime> DataHappy { get; set; }
        public string Passport { get; set; }
        public string BankCard { get; set; }
        public Nullable<int> ID_Type_Employee { get; set; }
        public Nullable<bool> Partner { get; set; }
        public Nullable<int> Children { get; set; }
        public string Health { get; set; }
        public Nullable<int> ID_Specialization { get; set; }
    
        public virtual TypeEmployee TypeEmployee { get; set; }
        public virtual TypeSpec TypeSpec { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShiftSchedule> ShiftSchedule { get; set; }
    }
}
