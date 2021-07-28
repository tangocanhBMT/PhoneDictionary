using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace WinFormsApp1
{
    public class PhoneDictionary
    {
        private string fullName;
        private string numberPhone;


        //tạo thuộc tính
        public string FullName { get => fullName; set => fullName = value; }

        public string NumberPhone { get => numberPhone; set => numberPhone = value; }

        //hàm khởi tạo

        public PhoneDictionary(string fullName, string numberPhone)
        {
            
            this.fullName = fullName;
            this.numberPhone = numberPhone;
        }

        public PhoneDictionary(DataRow row)
        {
            fullName = row["fullName"].ToString();
            numberPhone = row["PhoneNumber"].ToString();
        }
    }
}
