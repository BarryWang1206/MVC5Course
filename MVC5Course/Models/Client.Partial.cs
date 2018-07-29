
namespace MVC5Course.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MVC5Course.Models.InputValidations;

    [MetadataType(typeof(ClientMetaData))]
    public partial class Client : IValidatableObject
    {	    
       partial void Init() {
            //this.DateOfBirth = DateTime.Now.AddYears(-18);
        }

       public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
       {
            if(this.Longitude.HasValue != this.Latitude.HasValue)
            {
                //小技巧用來判斷目前是進行新增or更新的動作
                if(this.ClientId == 0)
                {
                    //通常Key為0代表著正在進行新增的動作 (練習中之資料庫的Client表因新增時的Key值為DB自行產生之流水號)
                }

                yield return new ValidationResult("經緯度欄位必須一起設定", new string[] { "Longitude", "Latitude" });
            }
       }
    }


    public partial class ClientMetaData
    {
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        [StringLength(40, ErrorMessage="欄位長度不得大於 40 個字元")]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(40, ErrorMessage="欄位長度不得大於 40 個字元")]
        public string MiddleName { get; set; }
        
        [Required]
        [StringLength(40, ErrorMessage="欄位長度不得大於 40 個字元")]
        public string LastName { get; set; }
        
        [Required]
        [StringLength(1, ErrorMessage="欄位長度不得大於 1 個字元")]
        public string Gender { get; set; }

        [DisplayFormat(ApplyFormatInEditMode=true ,DataFormatString ="{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [UIHint("CreditRating")] //指定動態資料用來顯示資料欄位 (範本檢視欄位名稱)
        public Nullable<double> CreditRating { get; set; }
        
        [StringLength(7, ErrorMessage="欄位長度不得大於 7 個字元")]
        public string XCode { get; set; }

        public Nullable<int> OccupationId { get; set; }
        
        [StringLength(20, ErrorMessage="欄位長度不得大於 20 個字元")]
        public string TelephoneNumber { get; set; }
        
        [StringLength(100, ErrorMessage="欄位長度不得大於 100 個字元")]
        public string Street1 { get; set; }
        
        [StringLength(100, ErrorMessage="欄位長度不得大於 100 個字元")]
        public string Street2 { get; set; }
        
        [StringLength(100, ErrorMessage="欄位長度不得大於 100 個字元")]
        public string City { get; set; }
        
        [StringLength(15, ErrorMessage="欄位長度不得大於 15 個字元")]
        public string ZipCode { get; set; }

        public Nullable<double> Longitude { get; set; }

        public Nullable<double> Latitude { get; set; }

        public string Notes { get; set; }

        [身份證字號]
        public string IdNumber { get; set; }

        public virtual Occupation Occupation { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
