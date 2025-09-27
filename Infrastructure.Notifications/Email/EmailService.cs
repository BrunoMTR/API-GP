using Domain.Channels;
using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;


namespace Infrastructure.Notifications.Email
{
    public class EmailService : BackgroundService
    {
        private readonly IEmailChannel _emailChannel;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(30);


        public EmailService(IEmailChannel emailChannel, IServiceProvider serviceProvider)
        {
            _emailChannel = emailChannel;
            _serviceProvider = serviceProvider;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var _processRepository = scope.ServiceProvider.GetRequiredService<IProcessRepository>();
            var _notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
            var _historyRepository = scope.ServiceProvider.GetRequiredService<IHistoryRepository>();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    int pageIndex = 0;
                    List<ProcessDto> processes;

                    do
                    {
                        // Pega processos paginados
                        processes = await _processRepository.GetAllAsync(new Query
                        {
                            PageIndex = pageIndex,
                            PageSize = 10
                        });

                        foreach (var process in processes)
                        {
                            // Filtra históricos não notificados
                            var pendingHistories = process.Histories
                                .Where(h => !h.Notified)
                                .ToList();

                            foreach (var history in pendingHistories)
                            {
                                var emailAddress = process.Unit?.Email;
                                if (string.IsNullOrEmpty(emailAddress))
                                    continue;

                                try
                                {
                                    // Monta email
                                    var message = new MimeMessage();
                                    message.From.Add(new MailboxAddress(process.Application?.Abbreviation, process.Application?.ApplicationEmail));
                                    message.To.Add(MailboxAddress.Parse(emailAddress));
                                    message.Subject = $"Notificação Processo {process.Id}";
                                    message.Body = new TextPart("html")
                                    {
                                        Text = $"Olá, há um novo evento no processo {process.Id} referente à sua unidade."
                                    };

                                    // Envia email via MailKit
                                    using var client = new SmtpClient();
                                    await client.ConnectAsync("localhost", 1025, false, stoppingToken);
                                    //await client.AuthenticateAsync("usuario", "senha", stoppingToken);
                                    await client.SendAsync(message, stoppingToken);
                                    await client.DisconnectAsync(true, stoppingToken);

                                    // Atualiza histórico como notificado
                                    history.Notified = true;

                                    // Grava registro da notificação
                                    var notificationDto = new NotificationDto
                                    {
                                        To = emailAddress,
                                        Cc = string.IsNullOrEmpty(process.Application?.ApplicationEmail)
                                            ? new List<string>()
                                            : new List<string> { process.Application.TeamEmail },
                                        Bcc = null,
                                        Subject = message.Subject,
                                        Body = message.HtmlBody,
                                        At = DateTime.UtcNow,
                                        ProcessId = process.Id
                                    };

                                    await _notificationRepository.NotificatwAsync(process.Id, notificationDto);
                                    await _historyRepository.MarkAsNotifiedAsync(history.Id);
                                }
                                catch
                                {
                                    
                                }
                            }
                        }

                        pageIndex++;
                    } while (processes.Count == 10); 
                }
                catch
                {
                    
                }

                await Task.Delay(_pollingInterval, stoppingToken);
            }
        }
    }

}
