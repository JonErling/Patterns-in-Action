using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using System.Configuration;
using System.Collections.ObjectModel;

namespace WpfApp.Models
{
    
    // model of the member
    
    public class MemberModel : BaseModel
    {
        private readonly IProvider _provider;

        // private data members

        private int _memberId = 0;
        private string _email;
        private string _companyName;
        private string _city;
        private string _country;

        private ObservableCollection<OrderModel> _orders;

        public MemberModel(IProvider provider)
        {
            this._provider = provider;
        }

        public int Add()
        {
            _provider.AddMember(this); ;
            return 1; 
        }
       
        public int Delete()
        {
            var orders = _provider.GetOrders(MemberId);
            if (orders == null || orders.Count == 0)
            {
                _provider.DeleteMember(MemberId);
                return 1;
            }
            else
                return 0;  // nothing deleted because member has orders.
                
        }

        public int Update()
        {
            _provider.UpdateMember(this);
            return 1; 
        }

        public int MemberId
        {
            get { ConfirmOnUiThread(); return _memberId; }
            set { ConfirmOnUiThread(); if (_memberId != value) { _memberId = value; Notify("MemberId"); } }
        }

        public string Email
        {
            get { ConfirmOnUiThread(); return _email; }
            set { ConfirmOnUiThread(); if (_email != value) { _email = value; Notify("Email"); } }
        }

        public string CompanyName
        {
            get { ConfirmOnUiThread(); return _companyName; }
            set { ConfirmOnUiThread(); if (_companyName != value) { _companyName = value; Notify("CompanyName"); } }
        }

        public string City
        {
            get { ConfirmOnUiThread(); return _city; }
            set { ConfirmOnUiThread(); if (_city != value) { _city = value; Notify("City"); } }
        }

        public string Country
        {
            get { ConfirmOnUiThread(); return _country; }
            set { ConfirmOnUiThread(); if (_country != value) { _country = value; Notify("Country"); } }
        }

        public ObservableCollection<OrderModel> Orders
        {
            get { ConfirmOnUiThread(); LazyloadOrders(); return _orders; }
            set { ConfirmOnUiThread(); _orders = value; Notify("Orders"); }
        }

        // helper that performs lazy loading of orders

        private void LazyloadOrders()
        {
            if (_orders == null) 
            {
                Orders = _provider.GetOrders(MemberId) ?? new ObservableCollection<OrderModel>();
            }
        }
    }
}
