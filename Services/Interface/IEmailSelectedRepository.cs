using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using RiwiTalent.Models.DTOs;

namespace RiwiTalent.Services.Interface
{
    public interface IEmailSelectedRepository
    {
        void SendEmail(MimeMessage message);
        void SendCodersSelected();
    }
}