using MimeKit;

namespace RiwiTalent.Infrastructure.ExternalServices.MailKit
{
    public class SendFile
    {
        public async Task<MimePart> GetFileTermsAsync(string path)
        {
            if(!File.Exists(path))
            {
               throw new FileNotFoundException("The file not found in the specific path", path);
            }

            MimePart attachment;

            using(var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                var content = new MimeContent(stream);
                attachment = new MimePart("application", "pdf")
                {
                    Content = new MimeContent(File.OpenRead(path)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = Path.GetFileName(path)
                };
            }


            return attachment;
        }
    }
}