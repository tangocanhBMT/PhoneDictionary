using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp1
{
    public class listPhoneDictionary
    {
        private static listPhoneDictionary instance;
        //tạo list danh bạ
        private List<PhoneDictionary> listNumberPhone;

        public List<PhoneDictionary> ListNumberPhone { get => listNumberPhone; set => listNumberPhone = value; }
        public static listPhoneDictionary Instance {
            get {
                if (instance == null)
                {
                    instance = new listPhoneDictionary();
                }
                return instance;
            }
            set => instance = value; }

        //tạo hàm khởi tạo
        private listPhoneDictionary()
        {
            listNumberPhone = new List<PhoneDictionary>();
            listNumberPhone.Add(new PhoneDictionary("Tạ Ngọc Ánh", "0987223344"));
            listNumberPhone.Add(new PhoneDictionary("Leo Teo", "0987255334"));
            listNumberPhone.Add(new PhoneDictionary("Khánh Linh", "09887732890"));
            listNumberPhone.Add(new PhoneDictionary("Khánh", "09887731890"));
            listNumberPhone.Add(new PhoneDictionary(" Linh", "09800732890"));
            listNumberPhone.Add(new PhoneDictionary("Khánh Li", "09887730000"));
        }
    }
}
