using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using MimeKit;

namespace RiwiTalent.Utils.MailKit
{
    public class SendFile
    {
        public MimePart  GetFileTerms(string path)
        {
            if(!File.Exists(path))
            {
               throw new FileNotFoundException("The file not found in the specific path", path);
            }

            var attachment = new MimePart("application", "pdf")
            {
                Content = new MimeContent(File.OpenRead(path)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(path)
            };

            return attachment;
        }
    }
}