using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Infrastructure.SQL.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SQL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DemoContext _demoContext;
        public NotificationRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
        }
        public async Task NotificatwAsync(int processId, NotificationDto email)
        {
            var notificationEntity = new NotificationEntity
            {
                To = email.To,
                Cc = email.Cc,
                Bcc = email.Bcc,
                Subject = email.Subject,
                Body = email.Body,
                At = email.At,
                ProcessId = email.ProcessId
            };
            await _demoContext.Notification.AddAsync(notificationEntity);
            await _demoContext.SaveChangesAsync();

        }
    }
}
