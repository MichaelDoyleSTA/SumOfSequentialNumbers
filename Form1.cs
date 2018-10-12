using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SumOrSequentialNumbers
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			listView1.Items.Clear();
			for (var number = Convert.ToInt32(numericUpDown1.Value); number <= Convert.ToInt32(numericUpDown2.Value); number++)
			{


				listView1.Items.Add(new Number(number).AsListViewItem());
			}
		}

		public class Number
		{
			private string ListToString(IEnumerable<int> list)
			{
				return string.Join(",", list.Select(x => x.ToString()));

			}

			public ListViewItem AsListViewItem()
			{
				return new ListViewItem(new[] { Value.ToString(), Ways.Count.ToString(), ListToString(Divisors), WaysToString() });
			}

			private string WaysToString()
			{
				return string.Join(";",
					Ways.Select(way => "{" + way.Count + ":= " + ListToString(way) + "}")
				);
			}

			public Number(int number)
			{
				this.Value = number;
				FindWays();
			}

			public int Value { get; set; }

			public List<int> Divisors { get; set; } = new List<int>();
			public List<List<int>> Ways { get; set; } = new List<List<int>>();

			private void FindWays()
			{

				for (int ii = 2; ii <= Value / 2 + 1; ii++)
				{
					if (Value % ii == 0) Divisors.Add(ii);

					var fraction = Value / ii;
					var smallestStart = Math.Max(1, fraction - ii - 1);
					for (var start = smallestStart; start <= ii; start++)
					{
						if (Ways.Any(existingWay => existingWay.First() == start)) continue;
						var way = new List<int>();
						var sum = 0;
						var num = start;
						while (sum < Value)
						{
							sum += num;
							way.Add(num);
							num++;
						}

						if (sum == Value)
						{
							Ways.Add(way);
						}
					}
				}
			}
		}





		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{

		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
			var sb = new StringBuilder();
			foreach (ListViewItem item in listView1.Items)
			{
				foreach (ListViewItem.ListViewSubItem value in item.SubItems)
				{
					sb.Append(value.Text);
					sb.Append("\t");
				}
				sb.AppendLine();
			}

			Clipboard.SetText(sb.ToString(), TextDataFormat.UnicodeText);
		}
	}
}
