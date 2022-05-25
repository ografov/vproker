using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vproker.Models
{
	[Table(nameof(Partners))]
	public partial class Partners
	{
		public string ID { get; set; }

		[Required(ErrorMessage = "Введите наименование партнера")]
		[Display(Name = "Наименование")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Введите размер скидки")]
		[Display(Name = "Партнерская скидка")]
		public float Discount { get; set; }
		public Partners()
		{
			ID = Guid.NewGuid().ToString();
		}

		public Partners(string name, float discount)
		{
			ID = Guid.NewGuid().ToString();
			Name = name;
			Discount = discount;
		}
	}
}
