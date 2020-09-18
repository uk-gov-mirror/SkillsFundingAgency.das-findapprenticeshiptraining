using System.Collections.Generic;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Web.Models
{
    public class DeliveryTypesViewModel
    {
        public DeliveryTypesViewModel()
        {
            
        }

        public DeliveryTypesViewModel(DeliveryType deliveryType, ICollection<string> selectedDeliveryTypes)
        {
            Selected = selectedDeliveryTypes?.Contains(deliveryType.Type) ?? false;
            DeliveryType = deliveryType.Type;
        }

        public bool Selected { get; set; }
        public string DeliveryType { get; set; }
    }
}
