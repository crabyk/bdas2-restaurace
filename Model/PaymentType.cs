﻿using System.ComponentModel;

namespace BDAS2_Restaurace.Model
{
    public class PaymentType : ModelBase
    {
        private string name;

        public int ID { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

    }
}
