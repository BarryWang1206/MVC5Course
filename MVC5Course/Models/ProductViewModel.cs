using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "商品名稱必填")] //驗證 必填欄位
        [StringLength(10, ErrorMessage = "商品名稱不得大於10個字元")] //驗證 字元數最大為10
        public string ProductName { get; set; }

        [Required]
        //[DisplayFormat(DataFormatString = "{0:#}")] //資料格式化
        [DisplayFormat(DataFormatString = "{0:N0}")] //資料格式化
        public Nullable<decimal> Price { get; set; }

        [Required]
        public Nullable<decimal> Stock { get; set; }
    }
}