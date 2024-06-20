using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ConfigValidation
{
    public class KafkaQueueAttribute : ValidationAttribute
    {
        
        public override bool IsValid(object value)
        {
            if (!(value is string topicName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(topicName))
            {
                return false;
            }

            // Check if the topic name adheres to the allowed characters
            if (!Regex.IsMatch(topicName, @"^[a-zA-Z0-9\._-]+$"))
            {
                return false;
            }

            // Check if the topic name does not start or end with a period
            if (topicName.StartsWith(".") || topicName.EndsWith("."))
            {
                return false;
            }

            // Check if the topic name does not exceed 249 characters
            if (topicName.Length > 249)
            {
                return false;
            }

            return true;
        }
    }
}