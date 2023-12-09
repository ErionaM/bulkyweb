using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Protocol.Plugins;

namespace Bulky.Utillity
{
	public static class SD
	{
		//Where will create constant for customer role, add a company role and admin and an employee role
		public const string Role_Customer = "Customer";
		public const string Role_Company = "Company";
		public const string Role_Admin = "Admin";
		public const string Role_Employee = "Employee";


		public const string StatusPending = "Pending";
		public const string StatusApproved = "Approved";
		public const string StatusInProcess = "Processing";
		public const string StatusShipped = "Shipped";
		public const string StatusCancelled = "Cancelled";
		public const string StatusRefunded = "Refunded";
		


		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved= "Approved";
		public const string PaymentStatusDelayedPayment= "ApprovedFroDelayedPayment";
		public const string PaymentStatusRejected= "Rejected";


	}
}
