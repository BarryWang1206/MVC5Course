//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC5Course.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Client
    {
    	partial void Init(); //修改EF的tt檔 --> 將部分方法在建構子中執行，若部分方法沒有實作也不會發生例外
    
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.Order = new HashSet<Order>();
    		this.Init(); //修改EF的tt檔 --> 將部分方法在建構子中執行，若部分方法沒有實作也不會發生例外
        }
    
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<double> CreditRating { get; set; }
        public string XCode { get; set; }
        public Nullable<int> OccupationId { get; set; }
        public string TelephoneNumber { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Latitude { get; set; }
        public string Notes { get; set; }
        public string IdNumber { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual Occupation Occupation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
    }
}
