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
				return new ListViewItem(new[] { Value.ToString(), Ways.Count.ToString(), BetterWaysCount.ToString(), ListToString(Divisors), WaysToString(), ListToString(BetterWays) }) {Tag = this};
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
				BetterFindWays();
			}

			public int Value { get; set; }

			public List<int> Divisors { get; set; } = new List<int>();
			public List<int> BetterWays { get; set; } = new List<int>();
			public List<List<int>> Ways { get; set; } = new List<List<int>>();

			public int BetterWaysCount => BetterWays.Count;

			private void BetterFindWays()
			{
				var divisorIsOdd = false;
				for (int divisor = 2; divisor <= Value / 2 + 1; divisor++)
				{
					var result = Value / divisor;
					var remainder = Value % divisor;
					if (divisorIsOdd && remainder == 0 && result > divisor / 2)
						BetterWays.Add(divisor);
					else if (divisor == remainder * 2 && divisor * remainder < Value)
						BetterWays.Add(divisor);
					divisorIsOdd = !divisorIsOdd;
				}
			}

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

		private void button3_Click(object sender, EventArgs e)
		{
			var wrongOnes = new List<int>();
			foreach (ListViewItem item in listView1.Items)
			{
				var number = item.Tag as Number;
				if (number != null)
				{
					if (number.BetterWaysCount!=number.Ways.Count)
						wrongOnes.Add(number.Value);
				}
			}

			if (wrongOnes.Count > 0)
			{
				MessageBox.Show("These numbers are wrong:" + String.Join(",", wrongOnes));
			}
			else
			{
				MessageBox.Show("Yes, the result is the same for all numbers tested.");
			}
		}
	}
}
