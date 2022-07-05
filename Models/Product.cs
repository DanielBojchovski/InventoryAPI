using System.ComponentModel.DataAnnotations;

namespace InventoryAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The product name is required./ Задолжително внесете име на производ.")]
        [MinLength(2, ErrorMessage = "Product name must contain at least 2 characters./ Името на производот мора да содржи барем две букви.")]
        [MaxLength(50, ErrorMessage = "Product name can't contain more than 50 characters./ Името на производот не смее да содржи повеќе од 50 букви.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "The average weekly sales is required./ Задолжително внесете просечна неделна продажба.")]
        [Display(Name = "Average Weekly Sales")]
        [Range(0, 999999999)]
        public int AverageWeeklySales { get; set; }

        [Required(ErrorMessage = "The maximum weekly sales is required./ Задолжително внесете максимална неделна продажба.")]
        [Display(Name = "Maximum Weekly Sales")]
        [Range(0, 999999999)]
        public int MaximumWeeklySales { get; set; }

        [Required(ErrorMessage = "The average delivery time in weeks is required./ Задолжително внесете просечно време за достава во недели.")]
        [Display(Name = "Average Delivery Time In Weeks")]
        [Range(1, 999999999)]
        public int AverageDeliveryTimeInWeeks { get; set; }

        [Required(ErrorMessage = "The maximum delivery time in weeks is required./ Задолжително внесете максимално време за достава во недели.")]
        [Display(Name = "Maximum Delivery Time In Weeks")]
        [Range(1, 999999999)]
        public int MaximumDeliveryTimeInWeeks { get; set; }

        [Display(Name = "Reorder Point")]
        public int _ReorderPoint;

        [Display(Name = "Reorder Quantity")]
        public int _ReorderQuantity;


        public int ReorderPoint
        {
            get { return _ReorderPoint; }
            set
            {
                _ReorderPoint = MaximumWeeklySales * MaximumDeliveryTimeInWeeks;
            }
        }

        public int ReorderQuantity
        {
            get { return _ReorderQuantity; }
            set
            {
                _ReorderQuantity = AverageWeeklySales * AverageDeliveryTimeInWeeks;
            }
        }
    }
}
