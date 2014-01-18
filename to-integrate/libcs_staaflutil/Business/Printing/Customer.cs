// http://www.devarticles.com/c/a/C-Sharp/Printing-Using-C-sharp/
using System;

namespace Fairweather.Service
{
      //public class Customer_Printer : Print_Composition
      //{
      //      public Customer_Printer(Customer customer) {

      //            Add_Header("Customer: ");

      //            Add_Data("Customer ID", customer.Id.ToString());
      //            Add_Data("Name", "{0} {1}".spf(customer.FirstName, customer.LastName));
      //            Add_Data("Company", customer.Company);
      //            Add_Data("E-mail", customer.Email);
      //            Add_Data("Phone", customer.Phone);

      //            Add_Blank_Line();

      //      }
      //}

      public class Customer
      {
            //public static implicit operator Print_Composition(Customer cust) {
            //      return new Customer_Printer(cust);
            //}
            readonly int m_Id;

            readonly String m_FirstName;

            readonly String m_LastName;

            readonly String m_Company;

            readonly String m_Email;

            readonly String m_Phone;


            public int Id {
                  get {
                        return this.m_Id;
                  }
            }

            public String FirstName {
                  get {
                        return this.m_FirstName;
                  }
            }

            public String LastName {
                  get {
                        return this.m_LastName;
                  }
            }

            public String Company {
                  get {
                        return this.m_Company;
                  }
            }

            public String Email {
                  get {
                        return this.m_Email;
                  }
            }

            public String Phone {
                  get {
                        return this.m_Phone;
                  }
            }


            public Customer(int Id,
                              String FirstName,
                              String LastName,
                              String Company,
                              String Email,
                              String Phone) {
                  this.m_Id = Id;
                  this.m_FirstName = FirstName;
                  this.m_LastName = LastName;
                  this.m_Company = Company;
                  this.m_Email = Email;
                  this.m_Phone = Phone;
            }

      }
}















