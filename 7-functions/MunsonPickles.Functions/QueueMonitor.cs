using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MunsonPickles.Functions
{
    [StorageAccount("PickleStorageConnection")]
    public class QueueMonitor
    {
        [FunctionName("QueueMonitor")]
        [return: Table("reviewimagedata")]
        public ReviewImageInfo Run(
            [QueueTrigger("review-images")]string message,
            ILogger log)
        {
            // split the message name based on the space
            var theParts = message.Split(' ');

            // user id is the first part
            var userId = theParts[0];

            // image name is the second part
            var imageName = theParts[1];

            log.LogInformation($"C# Queue trigger function processed: {message}");

            // write to table storage with information about the blob
            return new ReviewImageInfo {
                BlobName = "{userId}/{imageName}",
                PartitionKey = userId,
                RowKey = Guid.NewGuid().ToString(),
                UploadedDate = DateTime.Now,
                ImageName = imageName
            };
        }
    }

    public class ReviewImageInfo
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string BlobName { get; set; }
        public string ImageName { get; set; } 
        public string UserId { get; set; }
        public DateTime UploadedDate { get; set; }
    }
}
