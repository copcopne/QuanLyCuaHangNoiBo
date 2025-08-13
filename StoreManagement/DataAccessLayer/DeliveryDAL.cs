using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DataAccessLayer
{
    public class DeliveryDAL
    {
        private readonly salesysdbEntities context;
        public DeliveryDAL(salesysdbEntities context)
        {
            this.context = context;
        }
        public void AddDelivery(Delivery delivery)
        {
            context.Deliveries.Add(delivery);
            context.SaveChanges();
        }
        public void UpdateDelivery(Delivery delivery)
        {
            var existingDelivery = context.Deliveries.FirstOrDefault(d => d.DeliveryID == delivery.DeliveryID);
        }
        public void CancelDelivery(Invoice invoice)
        {
            var delivery = context.Deliveries.FirstOrDefault(d => d.InvoiceID == invoice.InvoiceID);
            if (delivery != null)
            {
                delivery.Status = "Đã hủy";
                context.SaveChanges();
            }
        }
        public List<Delivery> GetDeliveries(string keyword, DateTime? fromDate, DateTime? toDate, string status)
        {
            var query = context.Deliveries.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(d => d.DeliveryAddress.Contains(keyword) ||
                                         d.Employee.FullName.Contains(keyword) ||
                                         d.InvoiceID.ToString().Contains(keyword));
            }
            if (fromDate.HasValue)
            {
                query = query.Where(d => d.DeliveryDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(d => d.DeliveryDate <= toDate.Value);
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(d => d.Status == status);
            }
            return query.ToList();
        }
        public void AutoAssignDelivery()
        {
            List<Employee> staffList = context.Employees.Where(e => e.Position == "delivery").ToList();
            if (!staffList.Any())
                return;

            List<Delivery> pendingAssign = context.Deliveries
                .Where(d => d.Status == "Chưa phân công" && d.Employee == null)
                .ToList();
            foreach (var delivery in pendingAssign)
            {
                var staffLoad = staffList
                    .Select(s => new
                    {
                        Staff = s,
                        ActiveDeliveries = context.Deliveries.Count(d =>
                            d.AssignedStaffID == s.EmployeeID &&
                            (d.Status == "Chưa giao" || d.Status == "Đang giao"))
                    })
                    .OrderBy(x => x.ActiveDeliveries) // nhân viên ít đơn nhất
                    .ThenBy(x => x.Staff.EmployeeID)
                    .FirstOrDefault();
                if (staffLoad != null)
                {
                    delivery.AssignedStaffID = staffLoad.Staff.EmployeeID;
                    delivery.Status = "Chưa giao";
                }
            }
            
            context.SaveChanges();
        }
    }
}
